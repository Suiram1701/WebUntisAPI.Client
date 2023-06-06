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
    public sealed class ColorJsonConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType) => objectType == typeof(Color) || objectType == typeof(string);

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Get value
            JToken jToken = JToken.Load(reader);
            string value = jToken.Value<string>();

            // Convert value
            return new Color().FromHexColorFormat(value, false) ?? (object)value;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => ((Color)value).ToHexColorFormat();
    }
}