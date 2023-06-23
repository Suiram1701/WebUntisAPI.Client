using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A teacher
    /// </summary>
    [DebuggerDisplay("Name: {ForeName, nq} {LongName, nq}")]
    public class Teacher : IUser
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string ForeName { get; set; }

        /// <inheritdoc/>
        public string LongName { get; set; }

        /// <summary>
        /// I think a special title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Is the teacher active
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Idk
        /// </summary>
        [JsonProperty("dids")]
        public List<object> Dids { get; set; }
    }
}