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
        string value = reader.ReadAsString() ?? string.Empty;
        return Guid.Parse(value);
    }

    public override void WriteJson(JsonWriter writer, Guid value, JsonSerializer serializer) =>
        serializer.Serialize(writer, value);
}
