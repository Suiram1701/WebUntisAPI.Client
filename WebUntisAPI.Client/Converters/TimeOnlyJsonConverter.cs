using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Converters;

internal partial class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string _parseRegEx = @"^T?(\d\d?):?(\d\d)$";

#if NET7_0_OR_GREATER
    [GeneratedRegex(_parseRegEx)]
    private static partial Regex ParseRegEx();
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Regex ParseRegEx()
    {
        return new(_parseRegEx);
    }
#endif

    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonSerializationException("A string value were expected.");
        }

        string tokenString = reader.ReadAsString() ?? string.Empty;
        Match match = ParseRegEx().Match(tokenString);

        if (!match.Success)
        {
            Exception innerEx = new FormatException($"The token have to match the following expression: '{_parseRegEx}'.");
            throw new JsonSerializationException("Unable to deserialize token.", innerEx);
        }

        int hours = int.Parse(match.Groups[1].Value);
        int minutes = int.Parse(match.Groups[2].Value);

        return new(hours, minutes);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        string timeString = value.ToString("hh:mm");
        writer.WriteValue(timeString);
    }
}