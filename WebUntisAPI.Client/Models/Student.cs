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
    /// A student
    /// </summary>
    [DebuggerDisplay("Name: {ForeName, nq} {LongName, nq}")]
    public class Student : IUser
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
        /// Idk
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// The gender of the student
        /// </summary>
        [JsonProperty("gender")]
        public Gender? Gender { get; set; }
    }
}
