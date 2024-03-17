using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models;

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
    public int Period { get; init; }

    /// <summary>
    /// The description of the the hour
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; init; }

    /// <summary>
    /// The start time of the school hour
    /// </summary>
    [JsonProperty("startTime")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly StartTime { get; init; }

    /// <summary>
    /// The end time of the school hour
    /// </summary>
    [JsonProperty("endTime")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly EndTime { get; init; }
}