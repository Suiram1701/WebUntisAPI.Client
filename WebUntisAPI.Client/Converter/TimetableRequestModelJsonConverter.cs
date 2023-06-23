using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using WebUntisAPI.Client.Extensions;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Converter
{
    internal class TimetableRequestModelJsonConverter : JsonConverter<TimetableRequestModel>
    {
        public override TimetableRequestModel ReadJson(JsonReader reader, Type objectType, TimetableRequestModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject options = JObject.Load(reader).Value<JObject>("options");
            return new TimetableRequestModel()
            {
                Id = options.Value<JObject>("element").Value<int>("id"),
                Type = options.Value<JObject>("element").Value<int>("type"),
                StartDate = new DateTime().FromWebUntisTimeFormat(options.Value<string>("startDate"), "000"),
                EndDate = new DateTime().FromWebUntisTimeFormat(options.Value<string>("endDate"), "000")
            };
        }

        public override void WriteJson(JsonWriter writer, TimetableRequestModel value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("options");
            writer.WriteStartObject();

            writer.WritePropertyName("element");
            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(value.Id);

            writer.WritePropertyName("type");
            writer.WriteValue(value.Type);

            writer.WriteEndObject();

            value.StartDate.ToWebUntisTimeFormat(out string startDateString, out _);
            writer.WritePropertyName("startDate");
            writer.WriteValue(startDateString);

            value.EndDate.ToWebUntisTimeFormat(out string endDateString, out _);
            writer.WritePropertyName("endDate");
            writer.WriteValue(endDateString);

            writer.WritePropertyName("showBooking");
            writer.WriteValue(true);

            writer.WritePropertyName("showInfo");
            writer.WriteValue(true);

            writer.WritePropertyName("showSubstText");
            writer.WriteValue(true);

            writer.WritePropertyName("showLsText");
            writer.WriteValue(true);

            writer.WritePropertyName("showLsNumber");
            writer.WriteValue(true);

            writer.WritePropertyName("showStudentgroup");
            writer.WriteValue(true);

            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}