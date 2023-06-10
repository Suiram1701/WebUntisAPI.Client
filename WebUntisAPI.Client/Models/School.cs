using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Represent a Untis school
    /// </summary>
    [DebuggerDisplay("Name = {DisplayName}, Id = {SchoolId}")]
    public class School
    {
        /// <summary>
        /// Real name of the school
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Id of the school
        /// </summary>
        [JsonProperty("schoolId")]
        public long SchoolId { get; set; }

        /// <summary>
        /// Login name of the school
        /// </summary>
        [JsonProperty("loginName")]
        public string LoginName { get; set; }

        /// <summary>
        /// Real address of the school
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Server of the school
        /// </summary>
        [JsonProperty("server")]
        public string Server { get; set; }

        /// <summary>
        /// Server url of the school
        /// </summary>
        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }
    }
}