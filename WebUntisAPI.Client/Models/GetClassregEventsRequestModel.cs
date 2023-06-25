using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    internal struct GetClassregEventsRequestModel
    {
        [JsonProperty("startDate")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime EndDate { get; set; }
    }
}
