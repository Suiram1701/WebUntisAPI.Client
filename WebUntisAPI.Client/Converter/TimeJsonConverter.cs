using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using WebUntisAPI.Client.Extensions;

namespace WebUntisAPI.Client.Converter
{
    internal class TimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            return new DateTime().FromWebUntisTimeFormat("20200101", token.Value<string>());
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            value.ToWebUntisTimeFormat(out _, out string timeString);
            writer.WriteValue(timeString);
        }
    }
}