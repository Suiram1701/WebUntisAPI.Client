using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WebUntisAPI.Client.Converters
{
    internal class APIDateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            return token.Value<DateTime>();
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            string dateTimeString = value.ToString("yyyy-MM-dd") + "T" + value.ToString("HH:mm:ss");
            writer.WriteValue(dateTimeString);
        }
    }
}
