using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A timegrid for a school
    /// </summary>
    [DebuggerDisplay("Days = {SchoolDays}")]
    [JsonConverter(typeof(TimegridJsonConverter))]
    public class Timegrid : IEnumerable<(Day, SchoolHour[])>
    {
        private readonly Dictionary<Day, SchoolHour[]> _schoolDays = new Dictionary<Day, SchoolHour[]>();

        /// <summary>
        /// The count of school hours (mostly 5)
        /// </summary>
        public int SchoolDays => _schoolDays.Count;

        /// <summary>
        /// Get the school hours by the day
        /// </summary>
        /// <param name="day">The day</param>
        /// <returns>The school hours</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the given day not found (mostly <see cref="Day.Saturday"/> and <see cref="Day.Sunday"/>)</exception>
        public SchoolHour[] this[Day day]
        {
            get => _schoolDays[day];
            set
            {
                if (_schoolDays.ContainsKey(day))
                    _schoolDays[day] = value;
                else
                    _schoolDays.Add(day, value);
            }
        }

        #region IEnumerable<SchoolHour[]>
        /// <inheritdoc/>
        public IEnumerator<(Day, SchoolHour[])> GetEnumerator() => _schoolDays.Keys.Zip(_schoolDays.Values, (day, schoolHours) => (day, schoolHours)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}