using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Student or teacher
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        [JsonProperty("id")]
        int Id { get; set; }

        /// <summary>
        /// WebUntis internal name of the user
        /// </summary>
        [JsonProperty("name")]
        string Name { get; set; }

        /// <summary>
        /// Fore name of the user
        /// </summary>
        [JsonProperty("foreName")]
        string ForeName { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        [JsonProperty("longName")]
        string LongName { get; set; }
    }
}