﻿using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A subject
    /// </summary>
    [DebuggerDisplay("Name: {LongName, nq}")]
    public class Subject
    {
        /// <summary>
        /// Id of the subject
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Internal name of the subject
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Full name of the subject
        /// </summary>
        [JsonProperty("longName")]
        public string LongName { get; set; }

        /// <summary>
        /// Alternative name of the subject
        /// </summary>
        [JsonProperty("alternateName")]
        public string AlternateName { get; set; }

        /// <summary>
        /// Is the subject active
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Fore color of the subject
        /// </summary>
        [JsonProperty("foreColor")]
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color ForeColor { get; set; }

        /// <summary>
        /// Back color of the subject
        /// </summary>
        [JsonProperty("backColor")]
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color BackColor { get; set; }
    }
}
