using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.NewTimetable;

/// <summary>
/// Represents the status of a period element.
/// </summary>
public class PeriodElementStatus
{
    /// <summary>
    /// The current element.
    /// </summary>
    [JsonProperty("current")]
    public PeriodElement? Current { get; set; }

    /// <summary>
    /// The removed element.
    /// </summary>
    [JsonProperty("removed")]
    public PeriodElement? Removed { get; set; }
}
