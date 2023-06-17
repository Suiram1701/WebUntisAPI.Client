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
    /// A lesson period
    /// </summary>
    [DebuggerDisplay("On: {Date.Month, nq}.{Date.Day, nq}.{Date.Year, nq}, From {StartTime.Hour, nq}:{StartTime.Minute, nq} to {EndTime.Hour, nq}:{EndTime.Minute, nq}")]
    public class Period
    {
        /// <summary>
        /// The id of the period
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The date where the period is
        /// </summary>
        [JsonProperty("date")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// The time where the period start
        /// </summary>
        /// <remarks>
        /// Date is default set on the 1.1.2020 and only the time is set
        /// </remarks>
        [JsonProperty("startTime")]
        [JsonConverter(typeof(TimeJsonConverter))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The time where the period ends
        /// </summary>
        /// <remarks>
        /// Date is default set on the 1.1.2020 and only the time is set
        /// </remarks>
        [JsonProperty("endTime")]
        [JsonConverter(typeof(TimeJsonConverter))]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Ids of all involved classes
        /// </summary>
        [JsonProperty("kl")]
        public ObjectId[] ClassIds { get; set; }

        /// <summary>
        /// Ids of all involved teachers
        /// </summary>
        [JsonProperty("te")]
        public ObjectId[] TeacherIds { get; set; }

        /// <summary>
        /// Ids of all involved subjects
        /// </summary>
        [JsonProperty("su")]
        public ObjectId[] SubjectsIds { get; set; }

        /// <summary>
        /// Ids of all involved room
        /// </summary>
        [JsonProperty("ro")]
        public ObjectId[] RoomIds { get; set; }

        /// <summary>
        /// The activity type in the lesson
        /// </summary>
        [JsonProperty("activityType")]
        public string ActivityType { get; set; } = string.Empty;
    }
}
