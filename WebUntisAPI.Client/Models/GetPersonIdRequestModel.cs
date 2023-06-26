using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models
{
    internal struct GetPersonIdRequestModel
    {
        [JsonProperty("fn")]
        public string Forename { get; set; }

        [JsonProperty("sn")]
        public string Surname { get; set; }

        [JsonProperty("type")]
        public int UserType { get; set; }

        [JsonProperty("dob")]
        public const int s_Bithdate = 0;
    }
}
