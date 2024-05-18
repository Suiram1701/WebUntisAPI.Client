using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models.Timetable;

/// <summary>
/// One school hour
/// </summary>
[DebuggerDisplay("From: {StartTime, nq} to {EndTime, nq}")]
public class SchoolHour
{
    /// <summary>
    /// The number of the period of the day
    /// </summary>
    [JsonProperty("period")]
    public int Period { get; set; }

    /// <summary>
    /// The description of the the hour
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [DebuggerHidden]
    [JsonProperty("label")]
    [SuppressMessage("CodeQuality", "IDE0051", Justification = "Member used for JSON deserialization.")]
    private string Label
    {
        set => Description = value;
    }

    /// <summary>
    /// The start time of the school hour
    /// </summary>
    [JsonProperty("startTime")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// The end time of the school hour
    /// </summary>
    [JsonProperty("endTime")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly EndTime { get; set; }
}