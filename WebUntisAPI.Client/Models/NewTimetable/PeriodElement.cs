using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.NewTimetable;

/// <summary>
/// An element of a period.
/// </summary>
[DebuggerDisplay($"{{{nameof(LongName)},nq}}")]
public class PeriodElement
{
    /// <summary>
    /// The type of this element.
    /// </summary>
    [JsonProperty("type")]
    public ElementType Type { get; set; }

    /// <summary>
    /// The status of this element.
    /// </summary>
    [JsonProperty("status")]
    public Client.PeriodElementStatus Status { get; set; }

    /// <summary>
    /// The short name.
    /// </summary>
    [JsonProperty("shortName")]
    public string ShortName { get; set; } = string.Empty;

    /// <summary>
    /// The long name.
    /// </summary>
    [JsonProperty("longName")]
    public string LongName { get; set; } = string.Empty;
}
