using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A timegrid for a school
    /// </summary>
    [DebuggerDisplay("Days = {SchoolDays, nq}")]
    [JsonConverter(typeof(TimegridJsonConverter))]
    public class Timegrid : IEnumerable<KeyValuePair<Day, SchoolHour[]>>
    {
        /// <summary>
        /// All the school days with their school hours
        /// </summary>
        public Dictionary<Day, SchoolHour[]> SchoolDays { get; set; } = new Dictionary<Day, SchoolHour[]>();

        /// <summary>
        /// The count of school days (mostly 5)
        /// </summary>
        public int SchoolDayCount => SchoolDays.Count;

        #region IEnumerable<SchoolHour[]>
        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<Day, SchoolHour[]>> GetEnumerator() => SchoolDays.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}