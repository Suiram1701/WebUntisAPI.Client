using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models.Messages
{
    /// <summary>
    /// The preview for a draft
    /// </summary>
    /// <remarks>
    /// That are only previews! You can get the full message with <see cref="GetFullMessageAsync(WebUntisClient, CancellationToken)"/> (It must be the same account with them you got the preview)
    ///</remarks>
    [DebuggerDisplay("Subject: {Subject, nq}, Sender: {Sender, nq}, Send time: {SentTime, nq}")]
    public class DraftPreview
    {
        /// <summary>
        /// The id of the message
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The subject of the message
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// The sender of the message (only for messages in the inbox)
        /// </summary>
        [JsonProperty("sender")]
        public MessagePerson Sender { get; set; } = null;

        /// <summary>
        /// The send time of the message
        /// </summary>
        [JsonProperty("sentDateTime")]
        [JsonConverter(typeof(APIDateTimeJsonConverter))]
        DateTime SentTime { get; set; }

        /// <summary>
        /// Is allowed to delete the message
        /// </summary>
        [JsonProperty("allowMessageDeletion")]
        public bool AllowMessageDeletion { get; set; }

        /// <summary>
        /// Idk
        /// </summary>
        [JsonProperty("recipientGroups")]
        public List<object> RecipientGroups { get; set; }

        /// <summary>
        /// All the recipients for a message (only for self-sends messages)
        /// </summary>
        /// <remarks>
        /// When its <see langword="null"/> is the recipient the current user
        /// </remarks>
        [JsonProperty("recipientPersons")]
        public List<MessagePerson> Recipients { get; set; } = null;

        /// <summary>
        /// The content preview of the message
        /// </summary>
        [JsonProperty("contentPreview")]
        public string ContentPreview { get; set; }

        /// <summary>
        /// Has the message attachments
        /// </summary>
        [JsonProperty("hasAttachments")]
        public bool HasAttachments { get; set; }

        /// <summary>
        /// The option for the recipient
        /// </summary>
        [JsonProperty("recipientOption")]
        public string RecipientOption { get; set; }

        /// <summary>
        /// Get the full draft of this instance
        /// </summary>
        /// <param name="client">The client with them you got this instane (It doesn't have to be the same client but the same account)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">Thrown when the client instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the client aren't logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<Draft> GetFullMessageAsync(WebUntisClient client, CancellationToken ct = default)
        {
            string responseString = await client.MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages/drafts/" + Id, ct);
            return JsonConvert.DeserializeObject<Draft>(responseString);
        }
    }
}
