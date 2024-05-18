using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUntisAPI.Client.Models.Timetable;

namespace WebUntisAPI.Client.Converters;

internal class TimeGridJsonConverter : JsonConverter<TimeGrid>
{
    public override TimeGrid ReadJson(JsonReader reader, Type objectType, TimeGrid? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            throw new JsonSerializationException("An object were expected.");
        }

        JToken data = JObject.Load(reader);

        int schoolyear = data["schoolyearId"]!.Value<int>();
        bool persisted = data["persisted"]!.Value<bool>();

        IEnumerable<SchoolHour> hours = data["rows"]!.ToObject<IEnumerable<SchoolHour>>()!;
        IEnumerable<LessonState>[] lessonStates = data["units"]!.Values<JProperty>()
            .Select(jp => jp!.Values().Select(jt => jt["state"]!.ToObject<LessonState>()))
            .ToArray();

        return new(schoolyear, persisted, hours, lessonStates);
    }

    public override void WriteJson(JsonWriter writer, TimeGrid? value, JsonSerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        writer.WriteStartObject();

        writer.WritePropertyName("schoolyearId");
        writer.WriteValue(value.SchoolYearId);

        writer.WritePropertyName("persisted");
        writer.WriteValue(value.Persisted);

        writer.WritePropertyName("rows");
        writer.WriteStartArray();
        foreach (SchoolHour hour in value.Hours)
        {
            serializer.Serialize(writer, hour);
        }
        writer.WriteEndArray();

        writer.WritePropertyName("units");
        writer.WriteStartObject();
        for (int i = 0; i < value.LessonStates.Length; i++)
        {
            writer.WritePropertyName(i.ToString());
            writer.WriteStartArray();

            foreach (LessonState state in value.LessonStates[i])
            {
                writer.WriteStartObject();

                writer.WritePropertyName("state");
                writer.WriteValue(state);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
        writer.WriteEndObject();

        writer.WriteEndObject();
    }
}