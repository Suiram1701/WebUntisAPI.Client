using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Converters;

internal class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string _formatRegex = @"^(\d{4})-?(\d{2})-?(\d{2})$";

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken obj = JToken.Load(reader);

        Regex regex = new(_formatRegex, RegexOptions.Singleline | RegexOptions.CultureInvariant);
        Match match = regex.Match(obj.Value<string>()!);

        if (!match.Success)
            throw new FormatException($"A {nameof(DateOnly)} string in the format {_formatRegex} was expected.");

        int year = int.Parse(match.Groups[1].Value);
        int month = int.Parse(match.Groups[2].Value);
        int day = int.Parse(match.Groups[3].Value);

        return new(year, month, day);
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        // will never get called
        throw new NotImplementedException();
    }
}