using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A school year
    /// </summary>
    [DebuggerDisplay("Name: {Name, nq}")]
    public class SchoolYear : IEquatable<SchoolYear>, IComparable<SchoolYear>
    {
        /// <summary>
        /// Id of the school year
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the school year
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The date where the school year begins
        /// </summary>
        [JsonProperty("startDate")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The date where the school year ends
        /// </summary>
        [JsonProperty("endDate")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime EndDate { get; set; }

        #region IEquatable<SchoolYear>
        /// <inheritdoc/>
        public bool Equals(SchoolYear other) => Id.Equals(other.Id);
        #endregion

        #region IComparable<SchoolYear>
        /// <inheritdoc/>
        public int CompareTo(SchoolYear other)
        {
            if (StartDate.Year > other.StartDate.Year)
                return 1;
            else if (StartDate.Year < other.StartDate.Year)
                return -1;
            else
                return 0;
        }
        #endregion
    }
}