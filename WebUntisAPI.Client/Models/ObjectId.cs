using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// The id of an object and when its irregular the original id and the current id
    /// </summary>
    public struct ObjectId
    {
        /// <summary>
        /// The id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The original id 
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("orgid")]
        public int? OriginalId { get; set; }
    }
}

