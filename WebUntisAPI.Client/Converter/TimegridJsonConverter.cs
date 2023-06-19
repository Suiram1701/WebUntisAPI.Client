using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Extensions;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Converter
{
    internal sealed class TimegridJsonConverter : JsonConverter<Timegrid>
    {
        /// <inheritdoc/>
        public override Timegrid ReadJson(JsonReader reader, Type objectType, Timegrid existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray dayTimegrids = JArray.Load(reader);
            Timegrid timegrid = new Timegrid();

            foreach (JObject dayTimegrid in dayTimegrids.Cast<JObject>())
            {
                List<SchoolHour> hours = new List<SchoolHour>();
                foreach (JObject hour in dayTimegrid["timeUnits"].Cast<JObject>())
                    hours.Add(serializer.Deserialize<SchoolHour>(hour.CreateReader()));

                timegrid[(Day)dayTimegrid["day"].Value<int>()] = hours.ToArray();
            }
            return timegrid;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Timegrid value, JsonSerializer serializer)
        {
            writer.WriteStartArray();     // Write the diferent days
            foreach ((Day day, SchoolHour[] schoolHours) in value)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("day");
                writer.WriteValue((int)day);

                writer.WritePropertyName("timeUnits");
                writer.WriteStartArray();     // Write all school hours
                foreach (SchoolHour hour in schoolHours)
                    serializer.Serialize(writer, hour);

                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}
