using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// The result model of the authenticate method
    /// </summary>
    internal struct LoginResultModel
    {
        /// <summary>
        /// Sesson of the login
        /// </summary>
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        /// <summary>
        /// Type of the person
        /// </summary>
        [JsonProperty("personType")]
        public int PersonType { get; set; }

        /// <summary>
        /// Id of the person
        /// </summary>
        [JsonProperty("personId")]
        public int PersonId { get; set; }
    }
}