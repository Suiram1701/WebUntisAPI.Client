using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.NewTimetable;

/// <summary>
/// Represents a period of the time grid.
/// </summary>
[DebuggerDisplay($"Name: {{{nameof(Name)},nq}}")]
public class TimeGridSlot
{
    /// <summary>
    /// The name of the time slot.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The number of the time slot.
    /// </summary>
    [JsonProperty("number")]
    public int? Number { get; set; }

    /// <summary>
    /// The start and end of the time slot.
    /// </summary>
    [JsonProperty("duration")]
    public TimeRange Duration { get; set; }
}
