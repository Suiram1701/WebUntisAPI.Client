using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// Represents a range between two <see cref="DateOnly"/> instances
/// </summary>
[DebuggerDisplay("{Start,nq} - {End,nq}")]
public readonly struct DateRange
{
    /// <summary>
    /// The start of the range
    /// </summary>
    [JsonProperty("start")]
    public DateOnly Start { get; }

    /// <summary>
    /// Rge end of the range
    /// </summary>
    [JsonProperty("end")]
    public DateOnly End { get; }

    /// <summary>
    /// Creates a new instance
    /// </summary>
    /// <param name="start">The start</param>
    /// <param name="end">The end</param>
    public DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }
}
