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
            JToken data = JObject.Load(reader);

            bool persisted = data["persisted"].Value<bool>();

            IEnumerable<SchoolHour> hours = data["rows"].ToObject<IEnumerable<SchoolHour>>();
            IEnumerable<LessonState>[] lessonStates = data["units"].Values<JProperty>()
                .Select(jp => jp.Values().Select(jt => jt["state"].ToObject<LessonState>()))
                .ToArray();

            return new()
            {
                Hours = hours,
                LessonStates = lessonStates,
                Persisted = persisted
            };
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Timegrid value, JsonSerializer serializer)
        {
            // will never get called
            throw new NotImplementedException();
        }
    }
}
