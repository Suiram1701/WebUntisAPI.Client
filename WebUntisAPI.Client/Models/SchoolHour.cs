using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// One school hour
    /// </summary>
    [DebuggerDisplay("Name: {Name, nq}, From: {StartTime.Hour, nq}:{StartTime.Minute, nq} to {EndTime.Hour, nq}:{EndTime.Minute, nq}")]
    public class SchoolHour
    {
        /// <summary>
        /// Name of the school hour
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The start time of the school hour
        /// </summary>
        /// <remarks>
        /// Only <see cref="DateTime.Hour"/> and <see cref="DateTime.Minute"/> is set and the date is always set on the 1.1.2020
        /// </remarks>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The end time of the school hour
        /// </summary>
        /// <remarks>
        /// Only <see cref="DateTime.Hour"/> and <see cref="DateTime.Minute"/> is set and the date is always set on the 1.1.2020
        /// </remarks>
        public DateTime EndTime { get; set; }
    }
}
