﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A teacher
    /// </summary>
    [DebuggerDisplay("Name = {ForeName} {LongName}")]
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