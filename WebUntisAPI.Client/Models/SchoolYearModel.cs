using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models
{
    internal struct SchoolYearModel
    {
        [JsonProperty("schoolyearId")]
        public int Id { get; set; }
    }
}
