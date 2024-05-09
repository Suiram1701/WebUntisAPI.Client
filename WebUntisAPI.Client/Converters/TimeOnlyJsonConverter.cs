using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Converters;

internal partial class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return reader.TokenType switch
        {
            JsonToken.String => ReadStringJson(reader),
            JsonToken.Integer => ReadIntegerJson(reader),
            _ => throw new JsonSerializationException("A string or integer value were expected.")
        };
    }

    private static TimeOnly ReadStringJson(JsonReader reader)
    {
        string value = reader.Value?.ToString()?.TrimStart('T') ?? string.Empty;
        if (!TimeOnly.TryParse(value, out TimeOnly timeOnly))
        {
            throw new JsonSerializationException("The string have be in the format of hh:mm.");
        }

        return timeOnly;
    }

    private static TimeOnly ReadIntegerJson(JsonReader reader)
    {
        long value = reader.Value as long? ?? -1L;
        if (!(reader.Value?.ToString()?.Length is 3 or 4) || value < 0L)
        {
            throw new JsonSerializationException("A positive integer value with 4 digits were expected");
        }

        // Value is in the format of hhmm.
        int hours = (int)(value / 100);       // Extract digits 1-2
        int minutes = (int)(value % 100);     // Extract digits 3-4

        return new(hours, minutes);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        int resultValue = (value.Hour * 100) + value.Minute;
        writer.WriteValue(resultValue);
    }
}