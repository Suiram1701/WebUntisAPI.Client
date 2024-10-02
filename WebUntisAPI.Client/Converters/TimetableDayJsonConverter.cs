using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WebUntisAPI.Client.Extensions;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;
using WebUntisAPI.Client.Models.NewTimetable;

namespace WebUntisAPI.Client.Converters;

internal class TimetableDayJsonConverter : JsonConverter<TimetableDay>
{
    public override TimetableDay? ReadJson(JsonReader reader, Type objectType, TimetableDay? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);

        PeriodStatus status = obj["status"]!.ToObject<PeriodStatus>();
        if (status == PeriodStatus.NotAllowed)
        {
            return new() { Status = status };
        }

        IElement? element = null;
        if (!string.IsNullOrEmpty(obj["resourceType"]!.ToString()) && obj["resource"] != null)
        {
            ElementType elementType = Enum.Parse<ElementType>(obj["resourceType"]!.Value<string>()!, ignoreCase: true);
            element = elementType switch
            {
                ElementType.Class => obj["resource"]!.ToObject<Class>()!,
                ElementType.Teacher => obj["resource"]!.ToObject<Teacher>()!,
                ElementType.Subject => obj["resource"]!.ToObject<Subject>()!,
                ElementType.Room => obj["resource"]!.ToObject<Room>()!,
                ElementType.Student => obj["resource"]!.ToObject<Student>()!,
                _ => throw new InvalidOperationException($"Unable to parse resource of type {elementType}.")
            };
        }

        DateOnly date = obj["date"]!.ToObject<DateOnly>();
        IEnumerable<Period> periods = obj["gridEntries"]!.ToObject<IEnumerable<Period>>() ?? Enumerable.Empty<Period>();
        IEnumerable<BackEntry> backEntries = obj["backEntries"]!.ToObject<IEnumerable<BackEntry>>() ?? Enumerable.Empty<BackEntry>();

        return new()
        {
            Date = date,
            Resource = element,
            Status = status,
            GridEntries = periods,
            BackEntries = backEntries
        };
    }

    public override void WriteJson(JsonWriter writer, TimetableDay? value, JsonSerializer serializer)
    {
        if (value is not null)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("date");
            writer.WriteValue(value.Date.ToString("s"));

            writer.WritePropertyName("resourceType");
            if (value.Resource is not null)
            {
                writer.WriteValue(value.Resource.GetElementType().ToString().ToUpperInvariant());
            }
            else
            {
                writer.WriteNull();
            }

            writer.WritePropertyName("resource");
            if (value.Resource is not null)
            {
                new JObject(value.Resource).WriteTo(writer);
            }
            else
            {
                writer.WriteNull();
            }

            writer.WritePropertyName("status");
            writer.WriteValue(value.Status.ToString().ToUpperInvariant());

            writer.WritePropertyName("dayEntries");
            new JArray(value.DayEntries.ToArray()).WriteTo(writer);

            writer.WritePropertyName("gridEntries");
            new JArray(value.GridEntries.ToArray()).WriteTo(writer);

            writer.WritePropertyName("backEntries");
            new JArray(value.BackEntries.ToArray()).WriteTo(writer);

            writer.WriteEndObject();
        }
        else
        {
            writer.WriteNull();
        }
    }
}
