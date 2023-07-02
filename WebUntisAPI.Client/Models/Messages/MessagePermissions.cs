using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models.Messages
{
    /// <summary>
    /// Information about the permissions you have for messages
    /// </summary>
    public class MessagePermissions
    {
        /// <summary>
        /// To wich person types you could send the message
        /// </summary>
        [JsonProperty("recipientOptions")]
        public string[] RecipientOptions { get; set; }

        /// <summary>
        /// allow request read confirmation
        /// </summary>
        [JsonProperty("allowRequestReadConfirmation")]
        public bool AllowRequestReadConfirmation { get; set; }

        /// <summary>
        /// How much recipient are listed in the search result
        /// </summary>
        [JsonProperty("recipientSearchMaxResult")]
        public int RecipientSearchMaxResult { get; set; }

        /// <summary>
        /// Can you save drafts
        /// </summary>
        [JsonProperty("showDraftsTab")]
        public bool ShowDraftsTab { get; set; }

        /// <summary>
        /// Can you send messages
        /// </summary>
        [JsonProperty("showSentTab")]
        public bool ShowSentTab { get; set; }

        /// <summary>
        /// Can you forbid replies
        /// </summary>
        [JsonProperty("canForbidReplies")]
        public bool CanForbidReplies { get; set; }

        /// <summary>
        /// The maximum size of a file you could attach in bytes
        /// </summary>
        [JsonProperty("maxFileSize")]
        public long MaxFileSize { get; set; }

        /// <summary>
        /// The maximum count of files you could attach
        /// </summary>
        [JsonProperty("maxFileCount")]
        public int MaxFileCount { get; set; }
    }
}

