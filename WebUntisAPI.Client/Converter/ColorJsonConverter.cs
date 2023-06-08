using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebUntisAPI.Client.Extensions;

namespace WebUntisAPI.Client.Converter
{
    /// <summary>
    /// A json converter to convert a <see cref="Color"/> to the Hex color format (RRGGBB)
    /// </summary>
    public sealed class ColorJsonConverter : JsonConverter<Color>
    {
        /// <inheritdoc/>
        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Get value
            JToken jToken = JToken.Load(reader);
            string value = jToken.Value<string>();

            // Convert value
            return new Color().FromHexColorFormat(value);
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer) => serializer.Serialize(writer, value.ToHexColorFormat());
    }
}