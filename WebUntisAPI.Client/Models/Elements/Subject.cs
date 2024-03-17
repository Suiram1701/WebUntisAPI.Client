using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models.Elements;

/// <summary>
/// A subject
/// </summary>
public class Subject : ElementBase
{
    /// <summary>
    /// Back color of the subject
    /// </summary>
    /// <remarks>
    /// When <c>null</c> the default color should used
    /// </remarks>
    [JsonProperty("backColor")]
    [JsonConverter(typeof(ColorJsonConverter))]
    public Color? BackColor { get; set; }
}