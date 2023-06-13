using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Extensions;

namespace WebUntisAPI.Client.Converter
{
    internal class SchoolYearJsonConverter : JsonConverter<SchoolYear>
    {
        public override SchoolYear ReadJson(JsonReader reader, Type objectType, SchoolYear existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            return new SchoolYear()
            {
                Id = obj["id"].Value<int>(),
                Name = obj["name"].Value<string>(),
                StartDate = new DateTime().FromWebUntisTimeFormat(obj["startDate"].Value<string>(), "000"),
                EndDate = new DateTime().FromWebUntisTimeFormat(obj["endDate"].Value<string>(), "000"),
            };
        }

        public override void WriteJson(JsonWriter writer, SchoolYear value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(value.Id);

            writer.WritePropertyName("name");
            writer.WriteValue(value.Name);

            value.StartDate.ToWebUntisTimeFormat(out string startDateString, out _);
            writer.WritePropertyName("startDate");
            writer.WriteValue(startDateString);

            value.StartDate.ToWebUntisTimeFormat(out string endDateString, out _);
            writer.WritePropertyName("endDate");
            writer.WriteValue(endDateString);

            writer.WriteEndObject();
        }
    }
}

