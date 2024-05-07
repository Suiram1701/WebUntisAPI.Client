using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Converters;

internal class RecipientGroupJsonConverter : JsonConverter<string>
{
    private const string _propertyName = "displayName";

    public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            throw new JsonSerializationException("Unable to deserialize.");
        }

        JObject obj = JObject.Load(reader);
        return obj[_propertyName]!.Value<string>()!;
    }

    public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName(_propertyName);
        writer.WriteValue(value);

        writer.WriteEndObject();
    }
}
