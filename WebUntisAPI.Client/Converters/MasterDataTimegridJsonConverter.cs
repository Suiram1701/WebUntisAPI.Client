using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Converters;

internal class MasterDataTimegridJsonConverter : JsonConverter<ReadOnlyDictionary<DayOfWeek, IEnumerable<SchoolHour>>>
{
    public override ReadOnlyDictionary<DayOfWeek, IEnumerable<SchoolHour>>? ReadJson(JsonReader reader, Type objectType, ReadOnlyDictionary<DayOfWeek, IEnumerable<SchoolHour>>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JArray days = (JArray)JObject.Load(reader)["days"]!;

        Dictionary<DayOfWeek, IEnumerable<SchoolHour>> schoolDays = new();
        foreach (JToken day in days)
        {
            DayOfWeek dayOfWeek = day["day"]!.Value<string>() switch
            {
                "MON" => DayOfWeek.Monday,
                "TUE" => DayOfWeek.Tuesday,
                "WED" => DayOfWeek.Wednesday,
                "THU" => DayOfWeek.Thursday,
                "FRI" => DayOfWeek.Friday,
                "SAT" => DayOfWeek.Saturday,
                "SUN" => DayOfWeek.Sunday,
                _ => throw new Exception("Could not determine day value.")
            };

            IEnumerable<SchoolHour> schoolHours = day["units"]!.ToObject<IEnumerable<SchoolHour>>()!;
            IEnumerable<int> counter = Enumerable.Range(0, schoolHours.Count());     // the json doesn't contain ids for the school hours

            schoolDays.Add(dayOfWeek, schoolHours.Zip(counter, (schoolHour, i) =>
            {
                schoolHour.Period = i;
                return schoolHour;
            }).ToArray());
        }

        return new(schoolDays);
    }

    public override void WriteJson(JsonWriter writer, ReadOnlyDictionary<DayOfWeek, IEnumerable<SchoolHour>>? value, JsonSerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        writer.WriteStartObject();

        writer.WritePropertyName("days");
        writer.WriteStartArray();
        foreach ((DayOfWeek day, IEnumerable<SchoolHour> hours) in value)
        {
            string dayString = day switch
            {
                DayOfWeek.Monday => "MON",
                DayOfWeek.Tuesday => "TUE",
                DayOfWeek.Wednesday => "WED",
                DayOfWeek.Thursday => "THU",
                DayOfWeek.Friday => "FRI",
                DayOfWeek.Saturday => "SAT",
                DayOfWeek.Sunday => "SUN",
                _ => throw new NotImplementedException()
            };
            writer.WritePropertyName("day");
            writer.WriteValue(dayString);

            writer.WritePropertyName("units");
            writer.WriteStartArray();
            foreach (SchoolHour hour in hours)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("label");
                writer.WriteValue(string.Empty);

                writer.WritePropertyName("startTime");
                writer.WriteValue("T" + hour.StartTime.ToString("hh:mm"));

                writer.WritePropertyName("endTime");
                writer.WriteValue("T" + hour.EndTime.ToString("hh:mm"));

                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
