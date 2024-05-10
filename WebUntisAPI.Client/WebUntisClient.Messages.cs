using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Extensions;
using WebUntisAPI.Client.Models.Messages;
using WebUntisAPI.Client.Models.Messages.Confirmation;
using WebUntisAPI.Client.Models.Messages.Recipients;

namespace WebUntisAPI.Client;

partial class WebUntisClient
{
    /// <summary>
    /// Get the count of unread messages
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The count of unread messages</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<int> GetUnreadMessagesCountAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/messages/status", ct);
        return JObject.Parse(responseString)["unreadMessagesCount"]!.Value<int>();
    }

    /// <summary>
    /// Get the permissions you have in context of messages
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The permissions</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<MessagePermissions> GetMessagePermissionsAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/messages/permissions", ct);
        return JsonConvert.DeserializeObject<MessagePermissions>(responseString)!;
    }

    /// <summary>
    /// Get all available teacher recipient groups.
    /// </summary>
    /// <remarks>
    /// This method should used instead of <see cref="ApplyRecipientsFiltersAsync(string, string?, IDictionary{string, IEnumerable{FilterItem}}, CancellationToken)"/> for the recipient option <c>TEACHER</c> and should only used when this recipient option is available.
    /// </remarks>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The groups of teacher recipients</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<IEnumerable<TeacherRecipientGroup>> GetTeacherRecipientsAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/messages/recipients/static/persons", ct);
        return JsonConvert.DeserializeObject<IEnumerable<TeacherRecipientGroup>>(responseString)!;
    }

    /// <summary>
    /// Get all available student recipients and their subdivisions
    /// </summary>
    /// <remarks>
    /// This method is usually used instead of <see cref="ApplyRecipientsFiltersAsync(string, string?, IDictionary{string, IEnumerable{FilterItem}}, CancellationToken)"/> for the recipient option <c>STUDENTS</c>.
    /// </remarks>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The recipients and subdivisions</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<(IEnumerable<StudentRecipient> people, IEnumerable<RecipientSection> sections)> GetStudentRecipientsAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/messages/recipients/STUDENTS", ct);

        JObject responseObj = JObject.Parse(responseString);
        IEnumerable<StudentRecipient> people = responseObj["persons"]!.ToObject<IEnumerable<StudentRecipient>>()!;
        IEnumerable<RecipientSection> sections = responseObj["sections"]!.ToObject<IEnumerable<RecipientSection>>()!;

        return (people, sections);
    }

    /// <summary>
    /// Get all available filters for the specified recipients option
    /// </summary>
    /// <remarks>
    /// When the recipient option doesn't support/use filters then an empty dictionary will get returned.
    /// </remarks>
    /// <param name="recipientOption">The recipient option</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The available filters. The key is the internal name of the option and the values are the available options.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<Dictionary<string, IEnumerable<FilterItem>>> GetRecipientsFiltersAsync(string recipientOption, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(recipientOption, nameof(recipientOption));

        string responseString = await InternalApiRequestAsync($"/WebUntis/api/rest/view/v2/messages/recipients/{recipientOption}/filter", ct);

        Dictionary<string, IEnumerable<FilterItem>> results = new();

        JArray jArray = (JArray)JObject.Parse(responseString)["filters"]!;
        foreach (JToken jToken in jArray)
        {
            string type = jToken["type"]!.Value<string>()!;
            IEnumerable<FilterItem> items = jToken["items"]!.ToObject<IEnumerable<FilterItem>>()!;

            results.Add(type, items);
        }

        return results;
    }

    /// <summary>
    /// Get the filtered recipients by the applied filters and search text.
    /// </summary>
    /// <param name="recipientOption">The recipient option</param>
    /// <param name="searchText">The search text to apply. When <c>null</c> or an empty string will this ignored.</param>
    /// <param name="appliedFilters">The filters to apply. The key is the type of the filter to apply and the value is the actual filter. It is possible to apply multiple filters of the same type</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The filtered recipients.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<IEnumerable<Recipient>> ApplyRecipientsFiltersAsync(string recipientOption, string? searchText, IEnumerable<KeyValuePair<string, FilterItem>> appliedFilters, CancellationToken ct = default)
    {
        Dictionary<string, IEnumerable<FilterItem>> filters = appliedFilters
            .GroupBy(kv => kv.Key, kv => kv.Value)
            .ToDictionary(g => g.Key, g => g.ToArray().AsEnumerable());

        return await ApplyRecipientsFiltersAsync(recipientOption, searchText, filters, ct);
    }

    /// <summary>
    /// Get the filtered recipients by the applied filters and search text.
    /// </summary>
    /// <param name="recipientOption">The recipient option</param>
    /// <param name="searchText">The search text to apply. When <c>null</c> or an empty string will this ignored.</param>
    /// <param name="appliedFilters">The filters to apply. The format is the same that were returned by <see cref="GetRecipientsFiltersAsync(string, CancellationToken)"/> with the differences that you only have to hand over the filter type and the values that you want to apply.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The filtered recipients.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<IEnumerable<Recipient>> ApplyRecipientsFiltersAsync(string recipientOption, string? searchText, IDictionary<string, IEnumerable<FilterItem>> appliedFilters, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(recipientOption, nameof(recipientOption));
        ArgumentNullException.ThrowIfNull(appliedFilters, nameof(appliedFilters));
        if (appliedFilters.Any())
        {
            throw new ArgumentException("At least one filter have to be applied.");
        }

        JObject requestJson = new()
        {
            new JProperty("filters", new JArray(appliedFilters.Select(filterOption => new JObject
            {
                new JProperty("type", filterOption.Key),
                new JProperty("items", new JArray(filterOption.Value))
            }))),
            new JProperty("searchText", searchText ?? string.Empty)
        };

        using HttpRequestMessage request = new(HttpMethod.Post, new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = $"/WebUntis/api/rest/view/v2/messages/recipients/{recipientOption}/filter",
        }.Uri)
        {
            Content = new StringContent(requestJson.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json)
        };
        string responseString = await InternalApiRequestAsync(request, ct);

        return JObject.Parse(responseString)["users"]!.ToObject<IEnumerable<Recipient>>()!;
    }

    /// <summary>
    /// Get all messages of you're inbox
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>All messages in the inbox</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<IEnumerable<InboxMessagePreview>> GetMessageInboxAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/messages", ct);

        JObject responseObj = JObject.Parse(responseString);
        IEnumerable<InboxMessagePreview> inboxMessages = responseObj["incomingMessages"]!.ToObject<IEnumerable<InboxMessagePreview>>()!;
        IEnumerable<InboxMessagePreview> readConfirmationMessages = responseObj["readConfirmationMessages"]!.ToObject<IEnumerable<InboxMessagePreview>>()!;

        return readConfirmationMessages
            .Select(m =>
            {
                m.IsConfirmationRequested = true;     // Set the IsConfirmationRequested property for every readConfirmationMessages true that it is possible to differenciate them from incomingMessages
                return m;
            })
            .Concat(inboxMessages)
            .OrderByDescending(m => m.SentDateTime);
    }

    /// <summary>
    /// Get every message sent by the signed in user
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>All sent messages</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<IEnumerable<SentMessagePreview>> GetSentMessagesAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/messages/sent", ct);
        return JObject.Parse(responseString)["sentMessages"]!.ToObject<IEnumerable<SentMessagePreview>>()!;
    }

    /// <summary>
    /// Get every draft message for the signed in user
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The draft messages saved by the signed in user</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<IEnumerable<DraftMessagePreview>> GetSavedDraftsAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/messages/drafts", ct);
        return JObject.Parse(responseString)["draftMessages"]!.ToObject<IEnumerable<DraftMessagePreview>>()!;
    }

    /// <summary>
    /// Get full message of the of the specified <paramref name="preview"/>
    /// </summary>
    /// <param name="preview">The preview</param>
    /// <param name="contentAsHtml">Indicates whether the <see cref="InboxMessage.Content"/> property should be read as Html</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The full message</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<InboxMessage> GetFullMessageAsync(InboxMessagePreview preview, bool contentAsHtml = false, CancellationToken ct = default)
    {
        IMessage message = await GetFullMessageInternalAsync(preview, contentAsHtml, ct);
        return (InboxMessage)message;
    }

    /// <summary>
    /// Get full message of the of the specified <paramref name="preview"/>
    /// </summary>
    /// <param name="preview">The preview</param>
    /// <param name="contentAsHtml">Indicates whether the <see cref="SentMessage.Content"/> property should be read as Html</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The full message</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<SentMessage> GetFullMessageAsync(SentMessagePreview preview, bool contentAsHtml = false, CancellationToken ct = default)
    {
        IMessage message = await GetFullMessageInternalAsync(preview, contentAsHtml, ct);
        return (SentMessage)message;
    }

    /// <summary>
    /// Get full message of the of the specified <paramref name="preview"/>
    /// </summary>
    /// <param name="preview">The preview</param>
    /// <param name="contentAsHtml">Indicates whether the <see cref="DraftMessage.Content"/> property should be read as Html</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The full message</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<DraftMessage> GetFullMessageAsync(DraftMessagePreview preview, bool contentAsHtml = false, CancellationToken ct = default)
    {
        IMessage message = await GetFullMessageInternalAsync(preview, contentAsHtml, ct);
        return (DraftMessage)message;
    }

    private async Task<IMessage> GetFullMessageInternalAsync(IMessagePreview preview, bool contentAsHtml, CancellationToken ct)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(preview, nameof(preview));

        (string pathExtension, Type responseType) = preview.GetType() switch
        {
            Type t when t == typeof(InboxMessagePreview) => (string.Empty, typeof(InboxMessage)),
            Type t when t == typeof(SentMessagePreview) => ("/sent", typeof(SentMessage)),
            Type t when t == typeof(DraftMessagePreview) => ("/drafts", typeof(DraftMessage)),
            _ => throw new ArgumentException(string.Format("A not build-in implementation of {0} isn't supported by this method.", nameof(IMessagePreview)), nameof(preview))
        };

        UriBuilder uriBuilder = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = $"/WebUntis/api/rest/view/v1/messages{pathExtension}/{preview.Id}",
            Query = $"contentAsHtml={contentAsHtml}"
        };
        string responseString = await InternalApiRequestAsync(uriBuilder.Uri, ct);

        return (IMessage)JsonConvert.DeserializeObject(responseString, responseType)!;
    }

    /// <summary>
    /// Confirms a message in the inbox
    /// </summary>
    /// <remarks>
    /// You should only use this method when the message requires confirmation and isn't already confirmed. Otherwise an <see cref="InvalidOperationException"/> or a <see cref="WebUntisException"/> with the code <c>MESSAGING_READ_CONFIRMATION_ALREADY_CONFIRMED</c> will be thrown.
    /// </remarks>
    /// <param name="message">The message to confirm</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Information about the confirmation</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<ConfirmationInformation> ConfirmMessageAsync(InboxMessage message, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (message.RequestConfirmation is not ConfirmationInformation confirmation)
        {
            throw new InvalidOperationException("A message that doesn't require a read confirmation can't confirmed.");
        }
        else if (confirmation.ConfirmationDate is not null && confirmation.ConfirmerUserId is not null && confirmation.ConfirmerUserName is not null)     // indicators that this message were already confirmed
        {
            throw new InvalidOperationException("A message that were already confirmed can't confirmed again.");
        }
        else if (confirmation.AllowSendConfirmation)
        {
            throw new InvalidOperationException("It isn't allowed to confirm this message.");
        }

        string responseString = await InternalApiRequestAsync($"/WebUntis/api/rest/view/v1/messages/{message.Id}/read-confirmation", ct);
        return JsonConvert.DeserializeObject< ConfirmationInformation>(responseString)!;
    }

    /// <summary>
    /// Get more details about a confirmation requested message.
    /// </summary>
    /// <remarks>
    /// Do only use this method for messages that requires a confirmation otherwise a <see cref="InvalidOperationException"/> will thrown.
    /// </remarks>
    /// <param name="preview">The preview of the confirmation requested message.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Information about the confirmation state of each recipient and recipient user.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<IEnumerable<RecipientConfirmation>> GetConfirmationMessageDetailsAsync(SentMessagePreview preview, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(preview, nameof(preview));

        if (preview.ConfirmationState is null)
        {
            throw new InvalidOperationException("This message doesn't request a confirmation.");
        }

        return await GetConfirmationMessageDetailsInternalAsync(preview.Id, ct);
    }

    /// <summary>
    /// Get more details about a confirmation requested message.
    /// </summary>
    /// <remarks>
    /// Do only use this method for messages that requires a confirmation otherwise a <see cref="InvalidOperationException"/> will thrown.
    /// </remarks>
    /// <param name="message">The confirmation requested message.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Information about the confirmation state of each recipient and recipient user.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<IEnumerable<RecipientConfirmation>> GetConfirmationMessageDetailsAsync(SentMessage message, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (message.ConfirmationState is null)
        {
            throw new InvalidOperationException("This message doesn't request a confirmation.");
        }

        return await GetConfirmationMessageDetailsInternalAsync(message.Id, ct);
    }

    private async Task<IEnumerable<RecipientConfirmation>> GetConfirmationMessageDetailsInternalAsync(int id, CancellationToken ct)
    {
        ThrowWhenNotAvailable();

        string responseString = await InternalApiRequestAsync($"/WebUntis/api/rest/view/v1/messages/sent/{id}/request-confirmation-status", ct);
        return JsonConvert.DeserializeObject<IEnumerable<RecipientConfirmation>>(responseString)!;
    }

    /// <summary>
    /// Downloads a <paramref name="attachment"/> of a message
    /// </summary>
    /// <param name="attachment">The attachment to download</param>
    /// <param name="stream">The stream the attachment should be written to</param>
    /// <param name="progress">The instance the progress should be reported to</param>
    /// <param name="client">The client that should used to download the file. When <c>null</c> same client than for every other request will get used.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A task to await the download</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task DownloadMessageAttachmentAsync(Attachment attachment, Stream stream, IProgress<double>? progress = null, HttpClient? client = null, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        if (!stream.CanWrite)
            throw new InvalidOperationException("The stream have to be writable.");

        string responseString = await InternalApiRequestAsync($"/WebUntis/api/rest/view/v1/messages/{attachment.Id}/attachmentstorageurl", ct);

        JObject responseObj = JObject.Parse(responseString);
        string downloadUrl = responseObj["downloadUrl"]!.Value<string>()!;

        using HttpRequestMessage request = new(HttpMethod.Get, downloadUrl)
        {
            Headers =
            {
                { "x-amz-date", DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ") }     //  Adding the "x-amz-date" header for CORS policy compliance, although the request could function without it.
            }
        };
        foreach (JToken headerToken in responseObj["additionalHeaders"]!)
        {
            string key = headerToken["key"]!.Value<string>()!;
            string value = headerToken["value"]!.Value<string>()!;
            request.Headers.Add(key, value);
        }

        using HttpResponseMessage response = await (client ?? _client).SendWithProgressReportAsync(request, stream, progress, ct: ct);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Sends a message specified by the parameters
    /// </summary>
    /// <param name="subject">The subject of the message</param>
    /// <param name="content">The content of the message (\n is used for line breaks)</param>
    /// <param name="recipientOption">The recipient option that is used for this message. This should be the value that were used</param>
    /// <param name="recipients">The every recipient of the message</param>
    /// <param name="recipientGroups">Groups of recipients</param>
    /// <param name="attachments">Attachments that will be attach to the message</param>
    /// <param name="requestConfirmation">Indicates whether you request a confirmation (you need the permission to do that)</param>
    /// <param name="forbidReply">Indicates whether you forbid the recipients to reply the message</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The state of the sent message.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<SentMessageState> SendMessageAsync(string subject, string content, string recipientOption, IEnumerable<MessagePerson> recipients, IEnumerable<RecipientGroup> recipientGroups, IEnumerable<Tuple<string, Stream>> attachments, bool requestConfirmation, bool forbidReply, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(subject, nameof(subject));
        ArgumentNullException.ThrowIfNull(content, nameof(content));
        ArgumentNullException.ThrowIfNull(recipients, nameof(recipients));
        if (!recipients.Any() && !recipientGroups.Any())
            throw new ArgumentException("The message have to be at least one recipient.", nameof(recipients));
        ArgumentNullException.ThrowIfNull(attachments, nameof(attachments));
        if (attachments.Any(attachment => !attachment.Item2.CanRead))
            throw new InvalidOperationException("Every attachment stream have to be readable.");

        JObject requestJson = new()
        {
            new JProperty("subject", subject),
            new JProperty("content", content),
            new JProperty("recipientOption", recipientOption),
            new JProperty("recipientPersonIds", new JArray(recipients.Select(r => r.Id))),
            new JProperty("recipientPersonIds", recipientGroups.Select(g => g.Id)),
            new JProperty("copyToStudent", false),     // idk what this do
            new JProperty("requestConfirmation", requestConfirmation),
            new JProperty("oneDriveAttachments", new JArray()),
            new JProperty("forbidReply", forbidReply),
        };

        using HttpRequestMessage request = new(HttpMethod.Post, new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/rest/view/v2/messages"
        }.Uri)
        {
            Content = CreateMessageHttpContent(requestJson, attachments)
        };
        string responseString = await InternalApiRequestAsync(request, ct);

        return JsonConvert.DeserializeObject<SentMessageState>(responseString)!;
    }

    /// <summary>
    /// Creates a draft and saves it for the signed in user
    /// </summary>
    /// <param name="subject">The subject of the draft</param>
    /// <param name="content">The content of the draft</param>
    /// <param name="recipientOption">The recipient option that should be used for the draft</param>
    /// <param name="forbidReply">Indicates whether future recipients of this message mustn't reply the message</param>
    /// <param name="requestConfirmation">Indicates whether future recipients of this message have to confirm the message</param>
    /// <param name="copyToStudent">Idk</param>
    /// <param name="attachments">Attachments of the draft</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The preview of the created draft</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<DraftMessagePreview> CreateDraftMessageAsync(string subject, string content, string recipientOption, bool forbidReply, bool requestConfirmation, bool copyToStudent, IEnumerable<Tuple<string, Stream>> attachments, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(subject, nameof(subject));
        ArgumentNullException.ThrowIfNull(content, nameof(content));
        ArgumentNullException.ThrowIfNull(attachments, nameof(attachments));
        if (attachments.Any(attachment => !attachment.Item2.CanRead))
            throw new InvalidOperationException("Every attachment stream have to be readable.");

        JObject requestJson = new()
        {
            new JProperty("subject", subject),
            new JProperty("content", content),
            new JProperty("recipientOption", recipientOption),
            new JProperty("forbidReply", forbidReply),
            new JProperty("requestConfirmation", requestConfirmation),
            new JProperty("copyToStudent", copyToStudent),
            new JProperty("oneDriveAttachments", new JArray())
        };

        using HttpRequestMessage request = new(HttpMethod.Post, new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/rest/view/v2/messages/drafts"
        }.Uri)
        {
            Content = CreateMessageHttpContent(requestJson, attachments)
        };
        string responseString = await InternalApiRequestAsync(request, ct);

        return JsonConvert.DeserializeObject<DraftMessagePreview>(responseString)!;
    }

    /// <summary>
    /// Updates a draft message. To change the draft you have to change the properties of <paramref name="message"/>
    /// </summary>
    /// <param name="message">The draft containing the updated data</param>
    /// <param name="attachmentsToDelete">The attachment elements that should be removed from the draft</param>
    /// <param name="newAttachments">New attachments to attach</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The updated draft</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<DraftMessage> UpdateDraftMessageAsync(DraftMessage message, IEnumerable<Attachment>? attachmentsToDelete, IEnumerable<Tuple<string, Stream>>? newAttachments, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        attachmentsToDelete ??= Enumerable.Empty<Attachment>();
        newAttachments ??= Enumerable.Empty<Tuple<string, Stream>>();

        if (newAttachments.Any(attachment => !attachment.Item2.CanRead))
            throw new InvalidOperationException("Every attachment stream have to be readable.");

        JObject requestJson = new()
        {
            new JProperty("subject", message.Subject),
            new JProperty("content", message.Content),
            new JProperty("recipientOption", message.RecipientOption),
            new JProperty("forbidReply", message.ForbidReply),
            new JProperty("requestConfirmation", message.RequestConfirmation),
            new JProperty("copyToStudent", message.CopyToStudent),
            new JProperty("oneDriveAttachments", new JArray()),
            new JProperty("attachmentIdsToDelete", new JArray(
                attachmentsToDelete.Select(attachment => attachment.Id))
            )
        };

        using HttpRequestMessage request = new(HttpMethod.Put, new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = $"/WebUntis/api/rest/view/v2/messages/drafts/{message.Id}"
        }.Uri)
        {
            Content = CreateMessageHttpContent(requestJson, newAttachments)
        };
        string responseString = await InternalApiRequestAsync(request, ct);

        return JsonConvert.DeserializeObject<DraftMessage>(responseString)!;
    }

    /// <summary>
    /// Replies a message
    /// </summary>
    /// <param name="replyForm">The reply form of the message to reply</param>
    /// <param name="subject">The subject of the reply</param>
    /// <param name="content">The content of the reply</param>
    /// <param name="attachments">Attachments of the reply</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The task to await</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task ReplyMessageAsync(MessageReplyForm replyForm, string subject, string content, IEnumerable<Tuple<string, Stream>> attachments, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(replyForm, nameof(replyForm));
        ArgumentNullException.ThrowIfNull(subject, nameof(subject));
        ArgumentNullException.ThrowIfNull(content, nameof(content));
        ArgumentNullException.ThrowIfNull(attachments, nameof(attachments));
        if (attachments.Any(attachment => !attachment.Item2.CanRead))
            throw new InvalidOperationException("Every attachment stream have to be readable.");

        JObject requestJson = new()
        {
            new JProperty("subject", subject),
            new JProperty("content", content),
            new JProperty("oneDriveAttachments", new JArray())
        };

        using HttpRequestMessage request = new(HttpMethod.Post, new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = $"/WebUntis/api/rest/view/v2/messages/{replyForm.Id}/reply"
        }.Uri)
        {
            Content = CreateMessageHttpContent(requestJson, attachments)
        };

        await InternalApiRequestAsync(request, ct);
    }

    private static MultipartFormDataContent CreateMessageHttpContent(JObject jsonPart, IEnumerable<Tuple<string, Stream>> attachments)
    {
        MultipartFormDataContent content = new()
        {
            {
                new StringContent(jsonPart.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json), "request", "blob"
            }
        };
        foreach (Tuple<string, Stream> attachment in attachments)
        {
            StreamContent streamContent = new(attachment.Item2)
            {
                Headers =
                {
                    { "Content-Type", MediaTypeNames.Application.Octet }
                }
            };
            content.Add(streamContent, "attachments", attachment.Item1);
        }

        return content;
    }

    /// <summary>
    /// Get the reply form of a message
    /// </summary>
    /// <param name="messagePreview">The preview of the message to reply</param>
    /// <param name="contentAsHtml">Indicates whether every content property should be returned as html</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A reply form that can be used to reply the message</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<MessageReplyForm> GetReplyFormAsync(InboxMessagePreview messagePreview, bool contentAsHtml = false, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(messagePreview, nameof(messagePreview));
        if (!messagePreview.IsReplyAllowed)
            throw new InvalidOperationException("It isn't allowed to reply this message.");

        return await GetReplyFormInternalAsync(messagePreview.Id, contentAsHtml, ct);
    }

    /// <summary>
    /// Get the reply form of a message
    /// </summary>
    /// <param name="message">The preview of the message to reply</param>
    /// <param name="contentAsHtml">Indicates whether every content property should be returned as html</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A reply form that can be used to reply the message</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<MessageReplyForm> GetReplyFormAsync(InboxMessage message, bool contentAsHtml = false, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        if (!message.IsReplyAllowed || message.IsReplyForbidden)
            throw new InvalidOperationException("It isn't allowed to reply this message.");
        if (!(message.RequestConfirmation?.IsReplyAllowed ?? true))
            throw new InvalidOperationException("The message have to get confirmed before a reply is created.");

        return await GetReplyFormInternalAsync(message.Id, contentAsHtml, ct);
    }

    private async Task<MessageReplyForm> GetReplyFormInternalAsync(int id, bool contentAsHtml, CancellationToken ct)
    {
        ThrowWhenNotAvailable();

        UriBuilder uriBuilder = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = $"/WebUntis/api/rest/view/v1/messages/{id}/reply-form",
            Query = $"contentAsHtml={contentAsHtml}"
        };
        string responseString = await InternalApiRequestAsync(uriBuilder.Uri, ct);

        MessageReplyForm replyForm = JsonConvert.DeserializeObject<MessageReplyForm>(responseString)!;
        replyForm.Id = id;     // The id isn't provided by the api so I add it here

        return replyForm;
    }

    /// <summary>
    /// Revokes a message
    /// </summary>
    /// <param name="messagePreview">The preview of the message to revoke</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The task to await</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task RevokeMessageAsync(SentMessagePreview messagePreview, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(messagePreview, nameof(messagePreview));

        await RevokeMessageInternalAsync(messagePreview.Id, ct);
    }

    /// <summary>
    /// Revokes a message
    /// </summary>
    /// <param name="message">The message to revoke</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The task to await</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task RevokeMessageAsync(SentMessage message, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        await RevokeMessageInternalAsync(message.Id, ct);
    }

    private async Task RevokeMessageInternalAsync(int id, CancellationToken ct)
    {
        ThrowWhenNotAvailable();

        using HttpRequestMessage request = new(HttpMethod.Post, new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = $"/WebUntis/api/rest/view/v1/messages/{id}/revoke"
        }.Uri);
        await InternalApiRequestAsync(request, ct);
    }

    /// <summary>
    /// Deletes a message
    /// </summary>
    /// <param name="preview">The preview of the message to delete</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The task to await</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task DeleteMessageAsync(IMessagePreview preview, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(preview, nameof(preview));

        if (preview is InboxMessagePreview inboxPreview && !inboxPreview.AllowDeletion)
        {
            throw new InvalidOperationException("It isn't allowed to delete this message.");
        }
        else if (preview is SentMessagePreview sentPreview && !sentPreview.AllowDeletion)
        {
            throw new InvalidOperationException("It isn't allowed to delete this message.");
        }

        await DeleteMessageInternalAsync(preview.Id, ct);
    }

    /// <summary>
    /// Deletes a message
    /// </summary>
    /// <param name="message">The message to delete</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The task to await</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task DeleteMessageAsync(IMessage message, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (message is InboxMessage inboxMessage && !(inboxMessage.AllowDeletion && (inboxMessage.RequestConfirmation?.AllowMessageDeletion ?? true)))
        {
            throw new InvalidOperationException("It isn't allowed to delete this message or you have to confirm this message first.");
        }
        else if (message is SentMessage sentMessage && !sentMessage.AllowDeletion)
        {
            throw new InvalidOperationException("It isn't allowed to delete this message.");
        }

        await DeleteMessageInternalAsync(message.Id, ct);
    }

    private async Task DeleteMessageInternalAsync(int id, CancellationToken ct)
    {
        ThrowWhenNotAvailable();

        using HttpRequestMessage request = new(HttpMethod.Delete, new UriBuilder()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = $"/WebUntis/api/rest/view/v1/messages/{id}"
        }.Uri);
        await InternalApiRequestAsync(request, ct);
    }
}