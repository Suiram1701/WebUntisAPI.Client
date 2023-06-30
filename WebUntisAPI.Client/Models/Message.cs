using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// The full message
    /// </summary>
    public class Message
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
        /// The full content of the message
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// The sender of the message
        /// </summary>
        [JsonProperty("sender")]
        public MessageProfile Sender { get; set; }

        /// <summary>
        /// All the recipients for a message
        /// </summary>
        /// <remarks>
        /// When its <see langword="null"/> is the recipient the current user
        /// </remarks>
        [JsonProperty("recipients")]
        public List<MessageProfile> Recipients { get; set; } = null;

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
        /// Is the message revoked
        /// </summary>
        [JsonProperty("isRevoked")]
        public bool IsRevoked { get; set; }

        /// <summary>
        /// The file attachments of the message
        /// </summary>
        [JsonProperty("storageAttachments")]
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// Is the message a reply
        /// </summary>
        [JsonProperty("isReply")]
        public bool IsReply { get; set; } = false;

        /// <summary>
        /// Can you reply the message
        /// </summary>
        [JsonProperty("isReplyAllowed")]
        public bool IsReplyAllowed { get; set; } = true;
        
        /// <summary>
        /// Is this a report message
        /// </summary>
        [JsonProperty("isReportMessage")]
        public bool IsReportMessage { get; set; } = false;

        /// <summary>
        /// Is a reply for this forbidden
        /// </summary>
        [JsonProperty("isReplyForbidden")]
        public bool IsReplyForbidden { get; set; } = false;

        /// <summary>
        /// The history of all replies
        /// </summary>
        [JsonProperty("replyHistory")]
        public List<Message> ReplyHistory { get; set; } = new List<Message>();
    }
}
