using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Converters;

internal class DayOfWeekJsonConverter : JsonConverter<DayOfWeek>
{
    public override DayOfWeek ReadJson(JsonReader reader, Type objectType, DayOfWeek existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonSerializationException("A string value were expected.");
        }

        return reader.Value?.ToString() switch
        {
            "MO" => DayOfWeek.Monday,
            "TU" => DayOfWeek.Tuesday,
            "WE" => DayOfWeek.Wednesday,
            "TH" => DayOfWeek.Thursday,
            "FR" => DayOfWeek.Friday,
            "SA" => DayOfWeek.Saturday,
            "SU" => DayOfWeek.Sunday,
            _ => throw new JsonSerializationException("Unable to convert read value.")
        };
    }

    public override void WriteJson(JsonWriter writer, DayOfWeek value, JsonSerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        string valueString = value.ToString()[..1].ToUpperInvariant();
        writer.WriteValue(valueString);
    }
}
