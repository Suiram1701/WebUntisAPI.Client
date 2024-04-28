using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Converters;

internal partial class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string _parseRegEx = @"^(\d{4})-?(\d{2})-?(\d{2})$";

#if NET7_0_OR_GREATER
    [GeneratedRegex(_parseRegEx, RegexOptions.Singleline | RegexOptions.CultureInvariant)]
    private static partial Regex ParseRegEx();
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Regex ParseRegEx()
    {
        return new(_parseRegEx);
    }
#endif

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonSerializationException("A string value were expected.");
        }

        string value = reader.ReadAsString() ?? string.Empty;
        Match match = ParseRegEx().Match(value);

        if (!match.Success)
        {
            Exception innerEx = new FormatException($"A string in the format of '{_parseRegEx}' were expected.");
            throw new JsonSerializationException("Unable to deserialize the token.");
        }

        int year = int.Parse(match.Groups[1].Value);
        int month = int.Parse(match.Groups[2].Value);
        int day = int.Parse(match.Groups[3].Value);

        return new(year, month, day);
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        string stringValue = value.ToString("yyyyMMdd");
        writer.WriteValue(stringValue);
    }
}