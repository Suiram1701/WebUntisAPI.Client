using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    internal struct SchoolYearModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
