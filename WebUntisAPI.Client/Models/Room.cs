﻿using Newtonsoft.Json;
using System.Diagnostics;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A Room
    /// </summary>
    [DebuggerDisplay("Name: {LongName, nq}")]
    public class Room
    {
        /// <summary>
        /// Id of the room
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Short name of the room
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Normal name of the room
        /// </summary>
        [JsonProperty("longName")]
        public string LongName { get; set; }

        /// <summary>
        /// Is the room active or not
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }

        /// <summary>
        /// The building where the room is
        /// </summary>
        [JsonProperty("building")]
        public string Building { get; set; }
    }
}