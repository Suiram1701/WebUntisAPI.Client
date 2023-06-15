using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A holiday
    /// </summary>
    [DebuggerDisplay("Name = {LongName}")]
    public struct Holidays
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
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date of the holiday
        /// </summary>
        [JsonProperty("endDate")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime EndDate { get; set; }
    }
}
