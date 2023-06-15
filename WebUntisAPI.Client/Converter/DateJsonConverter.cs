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
    internal class DateJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            return new DateTime().FromWebUntisTimeFormat(token.Value<string>(), "000");
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            value.ToWebUntisTimeFormat(out string dateString, out _);
            writer.WriteValue(dateString);
        }
    }
}
