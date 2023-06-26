using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// All news for a day
    /// </summary>
    public class News
    {
        /// <summary>
        /// A system message
        /// </summary>
        [JsonProperty("systemMessage")]
        public NewsMessage SystemMessage { get; set; }

        /// <summary>
        /// All messages for the day
        /// </summary>
        [JsonProperty("messagesOfDay")]
        public List<NewsMessage> Messages { get; set; }
    }
}
