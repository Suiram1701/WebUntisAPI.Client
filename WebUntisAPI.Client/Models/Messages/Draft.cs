using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models.Messages
{
    /// <summary>
    /// A draft
    /// </summary>
    [DebuggerDisplay("Subject: {Subject, nq}, Sender: {Sender, nq}, Send time: {SentTime, nq}")]
    public class Draft
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
        /// The full content of the draft (\n is used for line breaks)
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// The file attachments of the draft
        /// </summary>
        [JsonProperty("storageAttachments")]
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();

        /// <summary>
        /// The option for the recipient
        /// </summary>
        [JsonProperty("recipientOption")]
        public string RecipientOption { get; set; }

        /// <summary>
        /// Forbid the reply (you need the permission to do that)
        /// </summary>
        [JsonProperty("forbidReply")]
        public bool ForbidReply { get; set; }

        /// <summary>
        /// Idk
        /// </summary>
        [JsonProperty("copyToStudents")]
        public bool CopyToStudents { get; set; }
    }
}
