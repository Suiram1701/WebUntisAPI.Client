using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Converters;

internal class ColorJsonConverter : JsonConverter<Color>
{
    /// <inheritdoc/>
    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken jToken = JToken.Load(reader);
        string value = jToken.Value<string>()!;

        byte[] argb = Convert.FromHexString(value.TrimStart('#'));
        if (argb.Length == 3)
            return Color.FromArgb(255, argb[0], argb[1], argb[2]);
        else if (argb.Length == 4)
            return Color.FromArgb(argb[0], argb[1], argb[2], argb[3]);
        else
            throw new FormatException("The specified color string have to be a valie hex color string.");
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        // will never get called
        throw new NotImplementedException();
    }
}