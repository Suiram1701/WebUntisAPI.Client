using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Converters;

internal class HexColorJsonConverter : JsonConverter<Color>
{
    /// <inheritdoc/>
    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        string colorString = token.Value<string>() ?? string.Empty;

        if (token.Type == JTokenType.String)
        {
            return ColorTranslator.FromHtml(colorString);
        }
        else if (token.Type == JTokenType.Null)
        {
            return Color.Empty;
        }
        throw new JsonSerializationException("Invalid color format.");
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        // will never get called
        throw new NotImplementedException();
    }
}