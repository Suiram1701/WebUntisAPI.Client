using Newtonsoft.Json;

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