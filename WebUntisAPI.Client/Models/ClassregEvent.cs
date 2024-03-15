using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A classreg event
    /// </summary>
    [DebuggerDisplay("On: {Date, nq}, Student: {Forename, nq} {Surname, nq}")]
    public class ClassregEvent
    {
        /// <summary>
        /// The id of the involved student
        /// </summary>
        [JsonProperty("studentId")]
        public int StudentId { get; set; }

        /// <summary>
        /// The forename of the involved student
        /// </summary>
        [JsonProperty("forename")]
        public string Forename { get; set; }

        /// <summary>
        /// The surname of the involved student
        /// </summary>
        [JsonProperty("surname")]
        public string Surname { get; set; }

        /// <summary>
        /// The date where the event happens
        /// </summary>
        [JsonProperty("date")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// The subject what is involved
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// The reason why this happend
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Additional text to the event
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Id of the category
        /// </summary>
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
    }
}
