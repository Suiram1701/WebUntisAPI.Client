using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A model for the school search result
    /// </summary>
    internal struct SchoolSearchResultModel
    {
        /// <summary>
        /// All schools found
        /// </summary>
        [JsonProperty("schools")]
        public School[] Schools { get; set; }
    }
}
