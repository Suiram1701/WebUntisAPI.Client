using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Messages;
using System.Net.Http.Headers;
using System.Net;
using System.IO;

namespace WebUntisAPI.Client
{
    public partial class WebUntisClient
    {
        /// <summary>
        /// Get the count of unread messages
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The count of unread messages</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<int> GetUnreadMessagesCountAsync(CancellationToken ct = default)
        {
            string responseString = await MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages/status", ct);
            return JObject.Parse(responseString).Value<int>("unreadMessagesCount");
        }

        /// <summary>
        /// Get the permissions you have to send messages
        /// </summary>
        /// <param name="ct">Cancllation token</param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<MessagePermissions> GetMessagePermissionsAsync(CancellationToken ct = default)
        {
            string responseString = await MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages/permissions", ct);
            return JsonConvert.DeserializeObject<MessagePermissions>(responseString);
        }

        /// <summary>
        /// Get all available reception people
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The people (<see cref="KeyValuePair{TKey, TValue}.Key"/> is the type of people that are contained in <see cref="KeyValuePair{TKey, TValue}.Value"/>)</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<KeyValuePair<string, MessagePerson[]>[]> GetMessagePeopleAsync(CancellationToken ct = default)
        {
            string responseString = await MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages/recipients/static/persons", ct);

            List<KeyValuePair<string, MessagePerson[]>> personTypes = new List<KeyValuePair<string, MessagePerson[]>>();
            JArray types = JArray.Parse(responseString);
            foreach (JObject personType in types.Cast<JObject>())
                personTypes.Add(new KeyValuePair<string, MessagePerson[]>(personType.Value<string>("type"),
                    new JsonSerializer().Deserialize<List<MessagePerson>>(personType.Value<JArray>("persons").CreateReader()).ToArray()));
            return personTypes.ToArray();
        }

        /// <summary>
        /// Get the you message inbox
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The message previews</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<MessagePreview[]> GetMessageInboxAsync(CancellationToken ct = default)
        {
            string responseString = await MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages", ct);

            JArray jsonMsg = JObject.Parse(responseString).Value<JArray>("incomingMessages");
            return new JsonSerializer().Deserialize<List<MessagePreview>>(jsonMsg.CreateReader()).ToArray();
        }

        /// <summary>
        /// Send a draft
        /// </summary>
        /// <param name="draft">The draft that you want to send</param>
        /// <param name="recipients">The recipients for the message</param>
        /// <param name="timeout">The time out for the attachment download (when the draft had attachments they must be downloaded to send them)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The preview of the sent message</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<MessagePreview> SendMessageAsync(Draft draft, MessagePerson[] recipients, int timeout = 2000, CancellationToken ct = default)
        {
            Tuple<string, MemoryStream>[] attachments = new Tuple<string, MemoryStream>[0];
            if (draft.Attachments.Count > 0)
            {
                Dictionary<string, Task<MemoryStream>> attachmentTasks = new Dictionary<string, Task<MemoryStream>>();

                foreach (Attachment attachment in draft.Attachments)
                    attachmentTasks.Add(attachment.Name, attachment.DownloadContentAsStreamAsync(this, timeout, ct));

                await Task.WhenAll(attachmentTasks.Values);
                attachments = attachmentTasks.Select(attachment => new Tuple<string, MemoryStream>(attachment.Key, attachment.Value.Result)).ToArray();
            }

            return await SendMessageAsync(draft.Subject, draft.Content, recipients, draft.ForbidReply, attachments, ct);
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="subject">The subject</param>
        /// <param name="content">The content (use <![CDATA[<br>]]> for line breaks</param>
        /// <param name="recipients">The recipients for the message</param>
        /// <param name="forbidReply">Is a reply forbidden (it need a permission to to that)</param>
        /// <param name="attachments">The attachments to send (Item1 is the name and Item2 the content)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The preview of the sent message</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<MessagePreview> SendMessageAsync(string subject, string content, MessagePerson[] recipients, bool forbidReply, Tuple<string, MemoryStream>[] attachments = null, CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            MultipartFormDataContent requestContent = new MultipartFormDataContent();

            // Json part
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("subject");
                writer.WriteValue(subject);

                writer.WritePropertyName("content");
                writer.WriteValue(content);

                writer.WritePropertyName("requestConfirmation");
                writer.WriteValue(false);

                writer.WritePropertyName("recipientUserIds");
                writer.WriteStartArray();
                foreach (MessagePerson recipient in recipients)
                    writer.WriteValue(recipient.Id);
                writer.WriteEndArray();

                writer.WritePropertyName("oneDriveAttachments");
                writer.WriteStartArray();
                writer.WriteEndArray();

                writer.WritePropertyName("forbidReply");
                writer.WriteValue(forbidReply);

                writer.WriteEndObject();

                StringContent jsonContent = new StringContent(sw.GetStringBuilder().ToString(), Encoding.UTF8, "application/json");
                requestContent.Add(jsonContent, "request", "blob");
            }

            // Attachment part
            foreach (Tuple<string, MemoryStream> attachment in attachments)
            {
                ByteArrayContent fileContent = new ByteArrayContent(attachment.Item2.ToArray());
                fileContent.Headers.Add("Content-Type", "application/x-msdownload");
                requestContent.Add(fileContent, "attachments", attachment.Item1);
            }

            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ServerUrl + "/WebUntis/api/rest/view/v2/messages/users"),
                Content = requestContent
            };
            request.Headers.Add("JSESSIONID", _sessonId);
            request.Headers.Add("schoolname", _schoolName);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Verify response
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _ = LogoutAsync();
                throw new UnauthorizedAccessException("You're not logged in");
            }

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            return JsonConvert.DeserializeObject<MessagePreview>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Revoke a message (move back into drafts)(only for self-sent messages!)
        /// </summary>
        /// <param name="message">The message to revoke</param>
        /// <param name="ct">Cancellation token</param>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task RevokeMessageAsync(MessagePreview message, CancellationToken ct = default)
        {
            await RevokeMessageAsync(new Message() { Id = message.Id }, ct);
        }

        /// <summary>
        /// Revoke a message (move back into drafts)(only for self-sent messages!)
        /// </summary>
        /// <param name="message">The message to revoke</param>
        /// <param name="ct">Cancellation token</param>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task RevokeMessageAsync(Message message, CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ServerUrl + $"/WebUntis/api/rest/view/v1/messages/{message.Id}/revoke"),
            };
            request.Headers.Add("JSESSIONID", _sessonId);
            request.Headers.Add("schoolname", _schoolName);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Verify response
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _ = LogoutAsync();
                throw new UnauthorizedAccessException("You're not logged in");
            }

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");
        }

        /// <summary>
        /// Delete a message
        /// </summary>
        /// <param name="message">Message to delete</param>
        /// <param name="ct">Cancellation token</param>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task DeleteMessageAsync(MessagePreview message, CancellationToken ct = default)
        {
            await DeleteDraftAsync(new Draft() { Id = message.Id }, ct);
        }

        /// <summary>
        /// Delete a message
        /// </summary>
        /// <param name="message">Message to delete</param>
        /// <param name="ct">Cancellation token</param>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task DeleteMessageAsync(Message message, CancellationToken ct = default)
        {
            await DeleteDraftAsync(new Draft() { Id = message.Id }, ct);
        }

        /// <summary>
        /// Get all your saved drafts
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The previews of the drafts</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<DraftPreview[]> GetSavedDraftsAsync(CancellationToken ct = default)
        {
            string responseString = await MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages/drafts", ct);

            JArray drafts = JObject.Parse(responseString).Value<JArray>("draftMessages");
            return new JsonSerializer().Deserialize<List<DraftPreview>>(drafts.CreateReader()).ToArray();
        }

        /// <summary>
        /// Create a new draft and save it in webuntis
        /// </summary>
        /// <param name="subject">Subject of the draft</param>
        /// <param name="content">String content of the draft (<![CDATA[<br>]]> is a line break)</param>
        /// <param name="recipientOption">Recipient option</param>
        /// <param name="forbidReply">Forbid Reply</param>
        /// <param name="copyToStudent">Idk</param>
        /// <param name="attachments">The attachments (Item1 is the file name and Item2 the content)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The preview of the created draft</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<DraftPreview> CreateDraftAsync(string subject, string content, string recipientOption, bool forbidReply, bool copyToStudent, Tuple<string, MemoryStream>[] attachments = null, CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            MultipartFormDataContent requestContent = new MultipartFormDataContent();

            // Json part
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("copyToStudent");
                writer.WriteValue(copyToStudent);

                writer.WritePropertyName("content");
                writer.WriteValue(content);

                writer.WritePropertyName("recipientOption");
                writer.WriteValue(recipientOption);

                writer.WritePropertyName("subject");
                writer.WriteValue(subject);

                writer.WritePropertyName("requestConfirmation");
                writer.WriteValue(false);

                writer.WritePropertyName("oneDriveAttachments");
                writer.WriteStartArray();
                writer.WriteEndArray();


                writer.WritePropertyName("forbidReply");
                writer.WriteValue(forbidReply);

                writer.WriteEndObject();

                StringContent jsonContent = new StringContent(sw.GetStringBuilder().ToString(), Encoding.UTF8, "application/json");
                requestContent.Add(jsonContent, "request", "blob");
            }

            // Attachment part
            foreach (Tuple<string, MemoryStream> attachment in attachments)
            {
                ByteArrayContent fileContent = new ByteArrayContent(attachment.Item2.ToArray());
                fileContent.Headers.Add("Content-Type", "application/x-msdownload");
                requestContent.Add(fileContent, "attachments", attachment.Item1);
            }

            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ServerUrl + "/WebUntis/api/rest/view/v2/messages/drafts"),
                Content = requestContent
            };
            request.Headers.Add("JSESSIONID", _sessonId);
            request.Headers.Add("schoolname", _schoolName);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Verify response
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _ = LogoutAsync();
                throw new UnauthorizedAccessException("You're not logged in");
            }

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            return JsonConvert.DeserializeObject<DraftPreview>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Update the draft and save it in webuntis
        /// </summary>
        /// <remarks>Change all in the <paramref name="draft"/> excepted the attachments</remarks>
        /// <param name="draft">The draft to update</param>
        /// <param name="newAttachments">The new attachments (Item1 is the file name and Item2 the content)</param>
        /// <param name="attachmentToDelete">The attachments from the draft you want to delete</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The preview of the created draft</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<DraftPreview> UpdateDraftAsync(Draft draft, Tuple<string, MemoryStream>[] newAttachments = null, Attachment[] attachmentToDelete = null, CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            MultipartFormDataContent requestContent = new MultipartFormDataContent();

            // Json part
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("copyToStudent");
                writer.WriteValue(draft.CopyToStudents);

                writer.WritePropertyName("content");
                writer.WriteValue(draft.Content);

                writer.WritePropertyName("recipientOption");
                writer.WriteValue(draft.RecipientOption);

                writer.WritePropertyName("subject");
                writer.WriteValue(draft.Subject);

                writer.WritePropertyName("requestConfirmation");
                writer.WriteValue(false);

                writer.WritePropertyName("oneDriveAttachments");
                writer.WriteStartArray();
                writer.WriteEndArray();

                writer.WritePropertyName("attachmentIdsToDelete");
                writer.WriteStartArray();
                foreach (Attachment attachment in attachmentToDelete ?? new Attachment[0])
                    writer.WriteValue(attachment._id);
                writer.WriteEndArray();

                writer.WritePropertyName("forbidReply");
                writer.WriteValue(draft.ForbidReply);

                writer.WriteEndObject();

                StringContent jsonContent = new StringContent(sw.GetStringBuilder().ToString(), Encoding.UTF8, "application/json");
                requestContent.Add(jsonContent, "request", "blob");
            }

            // Attachment part
            foreach (Tuple<string, MemoryStream> attachment in newAttachments ?? new Tuple<string, MemoryStream>[0])
            {
                ByteArrayContent fileContent = new ByteArrayContent(attachment.Item2.ToArray());
                fileContent.Headers.Add("Content-Type", "application/x-msdownload");
                requestContent.Add(fileContent, "attachments", attachment.Item1);
            }

            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(ServerUrl + "/WebUntis/api/rest/view/v2/messages/drafts/" + draft.Id),
                Content = requestContent
            };
            request.Headers.Add("JSESSIONID", _sessonId);
            request.Headers.Add("schoolname", _schoolName);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Verify response
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _ = LogoutAsync();
                throw new UnauthorizedAccessException("You're not logged in");
            }

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            return JsonConvert.DeserializeObject<DraftPreview>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Delete the given draft
        /// </summary>
        /// <param name="draft">The draft</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Task</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task DeleteDraftAsync(DraftPreview draft, CancellationToken ct = default)
        {
            await DeleteDraftAsync(new Draft() { Id = draft.Id }, ct);
        }

        /// <summary>
        /// Delete the given draft
        /// </summary>
        /// <param name="draft">The draft</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Task</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task DeleteDraftAsync(Draft draft, CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(ServerUrl + "/WebUntis/api/rest/view/v1/messages/" + draft.Id)
            };
            request.Headers.Add("JSESSIONID", _sessonId);
            request.Headers.Add("schoolname", _schoolName);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Verify response
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _ = LogoutAsync();
                throw new UnauthorizedAccessException("You're not logged in");
            }

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");
        }
    }
}