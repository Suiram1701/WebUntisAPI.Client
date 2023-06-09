﻿using Newtonsoft.Json;
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
    /// A class
    /// </summary>
    [DebuggerDisplay("Name = {LongName}")]
    [JsonConverter(typeof(ClassJsonConverter))]
    public struct Class
    {
        /// <summary>
        /// Id of the class
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Untis internal name of the class
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the class
        /// </summary>
        public string LongName { get; set; }

        /// <summary>
        /// Is the class active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// An array of all teachers of the class
        /// </summary>
        public int[] Teachers { get; set; }
    }
}