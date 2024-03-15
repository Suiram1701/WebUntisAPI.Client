using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A lesson period
    /// </summary>
    [DebuggerDisplay("On: {Date.Month, nq}.{Date.Day, nq}.{Date.Year, nq}, From: {StartTime.Hour, nq}:{StartTime.Minute, nq} to {EndTime.Hour, nq}:{EndTime.Minute, nq}, Type = {LessonType, nq}")]
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
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// The time where the period start
        /// </summary>
        /// <remarks>
        /// Date is default set on the 1.1.2020 and only the time is set
        /// </remarks>
        [JsonProperty("startTime")]
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The time where the period ends
        /// </summary>
        /// <remarks>
        /// Date is default set on the 1.1.2020 and only the time is set
        /// </remarks>
        [JsonProperty("endTime")]
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
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
        /// The type of tghe lesson
        /// </summary>
        [JsonProperty("lstype")]
        public LessonType LessonType { get; set; } = LessonType.Ls;

        /// <summary>
        /// Code for a the lesson
        /// </summary>
        [JsonProperty("code")]
        public Code Code { get; set; } = Code.None;

        /// <summary>
        /// Number of the lesson
        /// </summary>
        [JsonProperty("lsnumber")]
        public int LessonNumber { get; set; }

        /// <summary>
        /// Statistical flags of the lesson
        /// </summary>
        [JsonProperty("statflags")]
        public string StatisticalFlags { get; set; } = string.Empty;

        /// <summary>
        /// Informations about the lesson
        /// </summary>
        [JsonProperty("lstext")]
        public string LessonText { get; set; } = string.Empty;

        /// <summary>
        /// Additional informations about the student group
        /// </summary>
        [JsonProperty("sg")]
        public string StudentGroup { get; set; } = string.Empty;

        /// <summary>
        /// Additional text when it is a substitution
        /// </summary>
        [JsonProperty("substText")]
        public string SubstitutionText { get; set; } = string.Empty;

        /// <summary>
        /// Additional informations for the students
        /// </summary>
        [JsonProperty("info")]
        public string Info { get; set; } = string.Empty;

        /// <summary>
        /// The activity type in the lesson
        /// </summary>
        [JsonProperty("activityType")]
        public string ActivityType { get; set; } = string.Empty;
    }
}
