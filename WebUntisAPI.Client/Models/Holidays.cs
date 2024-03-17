using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A holiday
    /// </summary>
    [DebuggerDisplay("Name: {LongName, nq}")]
    public class Holidays
    {
        /// <summary>
        /// The id of the holiday
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Short name of the holiday
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The normal name of the holiday
        /// </summary>
        [JsonProperty("longName")]
        public string LongName { get; set; }

        /// <summary>
        /// The start date of the holiday
        /// </summary>
        [JsonProperty("startDate")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date of the holiday
        /// </summary>
        [JsonProperty("endDate")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime EndDate { get; set; }
    }
}
