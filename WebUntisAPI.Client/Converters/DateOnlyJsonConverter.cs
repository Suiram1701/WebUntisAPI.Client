using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Converters;

internal partial class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.Integer)
        {
            throw new JsonSerializationException("An integer value were expected.");
        }

        long value = reader.Value as long? ?? -1L;
        if (reader.Value?.ToString()?.Length != 8 || value < 0L)
        {
            Exception innerEx = new FormatException($"A positive integer value with 8 digits were expected.");
            throw new JsonSerializationException("Unable to deserialize the token.", innerEx);
        }

        // Value is in the format of yyyyMMdd.
        int year = (int)(value / 10000);            // Extract digits 1-4
        int month = (int)(value / 100 % 100);     // Extract digits 5-6
        int day = (int)(value % 100);               // Extract digits 7-8

        return new(year, month, day);
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        int resultValue = value.Year * 10000 + value.Month * 100 + value.Day;     // Bring it in the format of yyyyMMdd.
        writer.WriteValue(resultValue);
    }
}