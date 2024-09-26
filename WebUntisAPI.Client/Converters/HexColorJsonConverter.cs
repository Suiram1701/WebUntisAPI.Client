using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Converters;

internal class HexColorJsonConverter : JsonConverter<Color>
{
    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (!(reader.TokenType is JsonToken.String or JsonToken.Null))
        {
            throw new JsonSerializationException("A string or null value were expected.");
        }

        string? value = reader.Value as string;
        if (value is not null)
        {
            try
            {
                if (!value.StartsWith('#'))
                {
                    value = "#" + value;
                }

                return ColorTranslator.FromHtml(value);
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException("Unable to deserialize the token.", ex);
            }
        }
        else
        {
            return Color.Empty;
        }
    }

    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        if (!value.IsEmpty)
        {
            string colorString = Convert.ToHexString(new[] { value.A, value.R, value.G, value.B });
            writer.WriteValue("#" + colorString);
        }
        else
        {
            writer.WriteNull();
        }
    }
}