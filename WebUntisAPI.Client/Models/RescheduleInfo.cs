using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// Information about a rescheduled period
/// </summary>
public class RescheduleInfo
{
    /// <summary>
    /// The date
    /// </summary>
    [JsonProperty("date")]
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Date { get; set; }

    /// <summary>
    /// The start time
    /// </summary>
    [JsonProperty("startTime")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// The end time
    /// </summary>
    [JsonProperty("endTime")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// Indicates whether these time information are the source values
    /// </summary>
    [JsonProperty("isSource")]
    public bool IsSource { get; set; }
}
