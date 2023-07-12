using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A news message
    /// </summary>
    public class NewsMessage
    {
        /// <summary>
        /// The id of the message
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The involved subject
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// The normal text of the news (<![CDATA[<br>]]> or \n is used for line breaks)
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
