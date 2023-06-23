using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Model for the param of the login
    /// </summary>
    internal struct LoginModel
    {
        /// <summary>
        /// Username to login
        /// </summary>
        [JsonProperty("user")]
        public string User { get; set; }

        /// <summary>
        /// Password for login
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Unique identifier for the client app
        /// </summary>
        [JsonProperty("client")]
        public string Client { get; set; }
    }
}
