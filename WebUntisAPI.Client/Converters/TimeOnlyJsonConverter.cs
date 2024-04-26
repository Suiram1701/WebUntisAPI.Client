using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Converters;

internal class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string _parseRegEx = @"^T?(\d\d?):?(\d\d)$";

    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);

        string tokenString = token.Value<string>()!;

        Match match = Regex.Match(tokenString, _parseRegEx);
        if (!match.Success)
            throw new FormatException($"The token have to match the following expression: '{_parseRegEx}'.");

        int hours = int.Parse(match.Groups[1].Value);
        int minutes = int.Parse(match.Groups[2].Value);

        return new(hours, minutes);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        // will never get called
        throw new NotImplementedException();
    }
}