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
    /// A department
    /// </summary>
    [DebuggerDisplay("Name: {Name, nq}")]
    public class Department
    {
        /// <summary>
        /// Id of the department
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the department
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Long name of the department
        /// </summary>
        [JsonProperty("longName")]
        public string LongName { get; set; }
    }
}
