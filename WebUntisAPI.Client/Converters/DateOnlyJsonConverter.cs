using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using WebUntisAPI.Client.Extensions;

namespace WebUntisAPI.Client.Converter;

internal class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    const string _format = "yyyy-MM-dd";

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        return DateOnly.ParseExact(obj.Value<string>(), _format);
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        // will never get called
        throw new NotImplementedException();
    }
}