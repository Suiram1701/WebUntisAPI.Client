using Newtonsoft.Json;
using System.Drawing;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Fore- and background color fore something
    /// </summary>
    public struct ForeBackColors
    {
        /// <summary>
        /// Foreground color
        /// </summary>
        [JsonConverter(typeof(ColorJsonConverter))]
        [JsonProperty("foreColor")]
        public Color ForeColor { get; set; }

        /// <summary>
        /// Background color
        /// </summary>
        [JsonConverter(typeof(ColorJsonConverter))]
        [JsonProperty("backColor")]
        public Color BackColor { get; set; }
    }
}
