using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;
using WebUntisAPI.Client.Models.Elements;

namespace WebUntisAPI.Client.Converters;

internal class TimetableJsonConverter : JsonConverter<Timetable>
{
    public override Timetable? ReadJson(JsonReader reader, Type objectType, Timetable? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject result = JObject.Load(reader);
        JToken data = result["data"]!;

        int userId = data["elementIds"]!.First!.Value<int>();
        IEnumerable<Period> periods = data["elementPeriods"]![userId.ToString()]!.ToObject<IEnumerable<Period>>()!;

        Collection<ElementBase> elements = new();
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

            elements.Add((ElementBase)elementToken.ToObject(targetType)!);
        }

        long lastImportTimestamp = result["lastImportTimestamp"]!.Value<long>();
        DateTimeOffset lastImportTimestampOffset = DateTimeOffset.FromUnixTimeMilliseconds(lastImportTimestamp);

        return new(periods, elements, lastImportTimestampOffset);
    }

    public override void WriteJson(JsonWriter writer, Timetable? value, JsonSerializer serializer)
    {
        // will never get called
        throw new NotImplementedException();
    }
}
