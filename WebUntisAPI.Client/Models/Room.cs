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
    /// A Room
    /// </summary>
    [DebuggerDisplay("Name = {LongName}")]
    public struct Room
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