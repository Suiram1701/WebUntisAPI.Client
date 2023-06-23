using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// The parameters for a school search request
    /// </summary>
    internal struct SchoolSearchModel
    {
        /// <summary>
        /// Name to search
        /// </summary>
        [JsonProperty("search")]
        public string Search { get; set; }
    }
}