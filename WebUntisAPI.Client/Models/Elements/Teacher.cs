using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.Elements;

/// <summary>
/// A teacher
/// </summary>
public class Teacher : IElement, IUser
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string LongName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string ForeName { get; set; } = string.Empty;
    
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
    /// An extern key
    /// </summary>
    [JsonProperty("externKey")]
    public string ExternKey { get; set; } = string.Empty;
}