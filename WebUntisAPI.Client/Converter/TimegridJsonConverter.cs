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
                    hours.Add(new SchoolHour()
                    {
                        Name = hour["name"].Value<string>(),
                        StartTime = new DateTime().FromWebUntisTimeFormat("20200101", hour["startTime"].Value<string>()),
                        EndTime = new DateTime().FromWebUntisTimeFormat("20200101", hour["endTime"].Value<string>())
                    });

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
                {
                    writer.WriteStartObject();

                    writer.WritePropertyName("name");
                    writer.WriteValue(hour.Name);

                    hour.StartTime.ToWebUntisTimeFormat(out string _, out string startTimeString);
                    writer.WritePropertyName("startTime");
                    writer.WriteValue(startTimeString);

                    hour.EndTime.ToWebUntisTimeFormat(out string _, out string endTimeString);
                    writer.WritePropertyName("endTime");
                    writer.WriteValue(endTimeString);

                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}
