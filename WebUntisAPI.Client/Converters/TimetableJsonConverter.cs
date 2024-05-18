using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;
using WebUntisAPI.Client.Models.Timetable;

namespace WebUntisAPI.Client.Converters;

internal class TimetableJsonConverter : JsonConverter<Timetable>
{
    public override Timetable? ReadJson(JsonReader reader, Type objectType, Timetable? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject result = JObject.Load(reader);
        JToken data = result["data"]!;

        int userId = data["elementIds"]!.First!.Value<int>();
        IEnumerable<Period> periods = data["elementPeriods"]![userId.ToString()]!.ToObject<IEnumerable<Period>>()!;

        Collection<IElement> elements = new();
        foreach (JToken elementToken in data["elements"]!)
        {
            ElementType type = (ElementType)elementToken["type"]!.Value<int>()!;
            Type targetType = type switch
            {
                ElementType.Class => typeof(Class),
                ElementType.Teacher => typeof(Teacher),
                ElementType.Subject => typeof(Subject),
                ElementType.Room => typeof(Room),
                ElementType.Student => typeof(Student),
                _ => throw new NotImplementedException($"The element type {type} isn't implemented.")
            };

            elements.Add((IElement)elementToken.ToObject(targetType)!);
        }

        long lastImportTimestamp = result["lastImportTimestamp"]!.Value<long>();
        DateTimeOffset lastImportTimestampOffset = DateTimeOffset.FromUnixTimeMilliseconds(lastImportTimestamp);

        return new(periods, elements, lastImportTimestampOffset);
    }

    public override void WriteJson(JsonWriter writer, Timetable? value, JsonSerializer serializer)
    {
        // JSON contains information about the element the timetable is about.
        throw new NotImplementedException();
    }
}
