using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using WebUntisAPI.Client.Converters;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.Elements;

/// <summary>
/// A subject
/// </summary>
public class Subject : IElement
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string LongName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string Displayname { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string Alternatename { get; set; } = string.Empty;

    /// <inheritdoc/>
    public bool CanViewTimetable { get; set; }

    [DebuggerHidden]
    [JsonProperty("displayAllowed")]
    [SuppressMessage("CodeQuality", "IDE0051", Justification = "Member used for JSON deserialization.")]
    private bool DisplayAllowed
    {
        set => CanViewTimetable = value;
    }

    /// <inheritdoc/>
    public int RoomCapacity { get; set; }

    /// <summary>
    /// Back color of the subject
    /// </summary>
    /// <remarks>
    /// When <c>null</c> the default color should used
    /// </remarks>
    [JsonProperty("backColor")]
    [JsonConverter(typeof(HexColorJsonConverter))]
    public Color? BackColor { get; set; }
}