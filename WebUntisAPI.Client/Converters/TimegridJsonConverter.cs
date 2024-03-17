using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Converters;

internal class TimegridJsonConverter : JsonConverter<Timegrid>
{
    public override Timegrid ReadJson(JsonReader reader, Type objectType, Timegrid? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken data = JObject.Load(reader);

        int schoolyear = data["schoolyearId"]!.Value<int>();
        bool persisted = data["persisted"]!.Value<bool>();

        IEnumerable<SchoolHour> hours = data["rows"]!.ToObject<IEnumerable<SchoolHour>>()!;
        IEnumerable<LessonState>[] lessonStates = data["units"]!.Values<JProperty>()
            .Select(jp => jp!.Values().Select(jt => jt["state"]!.ToObject<LessonState>()))
            .ToArray();

        return new(schoolyear, persisted, hours, lessonStates);
    }

    public override void WriteJson(JsonWriter writer, Timegrid? value, JsonSerializer serializer)
    {
        // will never get called
        throw new NotImplementedException();
    }
}