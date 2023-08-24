using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Account configuration details
    /// </summary>
    public class AccountConfig
    {
        /// <summary>
        /// Can display the contact details for this account
        /// </summary>
        [JsonProperty("canReadContactDetail")]
        public bool CanReadContactDetails { get; set; } = false;
    }
}