using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using WebUntisAPI.Client.Extensions;

namespace WebUntisAPI.Client.Converter;

internal class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        string tokenString = token.Value<string>();

        int hours = int.Parse(tokenString[..(tokenString.Length - 2)]);
        int minutes = int.Parse(tokenString[(tokenString.Length - 2)..]);

        return new(hours, minutes);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        // will never get called
        throw new NotImplementedException();
    }
}