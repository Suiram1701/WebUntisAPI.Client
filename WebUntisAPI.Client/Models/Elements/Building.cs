using Newtonsoft.Json;
using System;
using WebUntisAPI.Client.Models.Interfaces;
using WebUntisAPI.Client.Models.NewTimetable.FilterElements;

namespace WebUntisAPI.Client.Models.Elements;

/// <summary>
/// Represents a building.
/// </summary>
public class Building
{
    /// <summary>
    /// The id of this building.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The short name of this building.
    /// </summary>
    [JsonProperty("shortName")]
    public string ShortName { get; set; } = string.Empty;

    /// <summary>
    /// The name of this building.
    /// </summary>
    [JsonProperty("longName")]
    public string LongName { get; set; } = string.Empty;

    /// <summary>
    /// The usually shown name of this building.
    /// </summary>
    [JsonProperty("displayName")]
    public string DisplayName { get; set; } = string.Empty;
}