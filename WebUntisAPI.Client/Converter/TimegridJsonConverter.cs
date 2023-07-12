using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Converter
{
    internal class TimegridJsonConverter : JsonConverter<Timegrid>
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

                timegrid.SchoolDays.Add((Day)dayTimegrid["day"].Value<int>(), hours.ToArray());
            }
            return timegrid;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Timegrid value, JsonSerializer serializer)
        {
            writer.WriteStartArray();     // Write the diferent days
            foreach (KeyValuePair<Day, SchoolHour[]> day in value)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("day");
                writer.WriteValue((int)day.Key);

                writer.WritePropertyName("timeUnits");
                writer.WriteStartArray();     // Write all school hours
                foreach (SchoolHour hour in day.Value)
                    serializer.Serialize(writer, hour);

                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}
