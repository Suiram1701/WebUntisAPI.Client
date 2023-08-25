using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models.Messages
{
    /// <summary>
    /// A filter item that can applied to a staff search
    /// </summary>
    public struct FilterItem
    {
        /// <summary>
        /// The id of this filter item
        /// </summary>
        [JsonProperty("referenceId")]
        public int ReferenceId { get; set; }

        /// <summary>
        /// The displayed name of this filter item
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}