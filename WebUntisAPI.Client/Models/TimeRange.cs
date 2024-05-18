using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// Represents a range between two <see cref="TimeOnly"/> instances
/// </summary>
[DebuggerDisplay($"{{{nameof(Start)},nq}} - {{{nameof(End)},nq}}")]
public readonly struct TimeRange
{
    /// <summary>
    /// The start time of the range.
    /// </summary>
    [JsonProperty("start")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly Start { get; }

    /// <summary>
    /// The end time of the range.
    /// </summary>
    [JsonProperty("end")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly End { get; }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="start">The start time</param>
    /// <param name="end">The end time</param>
    public TimeRange(TimeOnly start, TimeOnly end)
    {
        Start = start;
        End = end;
    }
}
