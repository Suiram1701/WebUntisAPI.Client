using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A message in Untis
    /// </summary>
    /// <remarks>
    /// That are only previews! You can get the full message with <see cref="GetFullMessageAsync(WebUntisClient, CancellationToken)"/> (It must be the same account with them you got the preview)
    ///</remarks>
    [DebuggerDisplay("Subject: {Subject, nq}, Sender: {Sender, nq}, Send time: {SentTime, nq}")]
    public class MessagePreview
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
        /// The content preview of the message
        /// </summary>
        [JsonProperty("contentPreview")]
        public string ContentPreview { get; set; }

        /// <summary>
        /// The sender of the message
        /// </summary>
        [JsonProperty("sender")]
        public MessagePerson Sender { get; set; }

        /// <summary>
        /// The send time of the message
        /// </summary>
        [JsonProperty("sentDateTime")]
        [JsonConverter(typeof(APIDateTimeJsonConverter))]
        public DateTime SentTime { get; set; }

        /// <summary>
        /// Is allowed to delete the message
        /// </summary>
        [JsonProperty("allowMessageDeletion")]
        public bool AllowMessageDeletion { get; set; }

        /// <summary>
        /// Has the message attachments
        /// </summary>
        [JsonProperty("hasAttachments")]
        public bool HasAttachments { get; set; }

        /// <summary>
        /// Is the message readed
        /// </summary>
        [JsonProperty("isMessageRead")]
        public bool IsMessageRead { get; set; }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        [JsonProperty("isReply")]
        public bool IsReply { get; set; }

        /// <summary>
        /// Can you reply the message
        /// </summary>
        [JsonProperty("isReplyAllowed")]
        public bool IsReplyAllowed { get; set; }

        /// <summary>
        /// Get the full messag of this instance
        /// </summary>
        /// <param name="client">The client with them you got this instane (It doesn't have to be the same client but the same account)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<Message> GetFullMessageAsync(WebUntisClient client, CancellationToken ct = default)
        {
            string responseString = await client.MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages/" + Id, ct);
            return JsonConvert.DeserializeObject<Message>(responseString);
        }
    }
}
