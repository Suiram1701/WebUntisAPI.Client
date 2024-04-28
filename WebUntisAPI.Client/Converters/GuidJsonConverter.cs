using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Converters;

internal class GuidJsonConverter : JsonConverter<Guid>
{
    public override Guid ReadJson(JsonReader reader, Type objectType, Guid existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonSerializationException("A string value were expected.");
        }

        string value = reader.ReadAsString() ?? string.Empty;
        if (!Guid.TryParse(value, out Guid result))
        {
            Exception innerEx = new FormatException("The string doesn't match the format of a GUID.");
            throw new JsonSerializationException("Unable to deserialize the token.");
        }

        return result;
    }

    public override void WriteJson(JsonWriter writer, Guid value, JsonSerializer serializer)
    {
        string stringValue = value.ToString();
        writer.WriteValue(stringValue);
    }
}
