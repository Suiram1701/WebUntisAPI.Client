using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Converters;

internal class UnixMillisJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);

        if (token.Type != JTokenType.Integer)
            throw new JsonReaderException("Unexpected token type (integer expected).");

        long value = token.Value<long>()!;
        return DateTimeOffset.FromUnixTimeMilliseconds(value);
    }

    public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
    {
        // never get called
        throw new NotImplementedException();
    }
}
