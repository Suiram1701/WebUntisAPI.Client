using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models.NewTimetable;

/// <summary>
/// Represents an available time grid definition.
/// </summary>
public class TimeGridDefinition
{
    /// <summary>
    /// The id of the definition.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The name of the definition.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The long name of the definition.
    /// </summary>
    [JsonProperty("longName")]
    public string LongName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the start and end of the time slots should be shown.
    /// </summary>
    [JsonProperty("showStartEndTimeOfSlots")]
    public bool ShowStartEndTimeOfSlots { get; set; }

    /// <summary>
    /// Indicates whether the start and end time should be shown.
    /// </summary>
    [JsonProperty("showStartEndTime")]
    public bool ShowStartEndTime { get; set; }

    /// <summary>
    /// Indicates whether cancellations should be shown.
    /// </summary>
    [JsonProperty("showCancellations")]
    public bool ShowCancellations { get; set; }

    /// <summary>
    /// Indicates whether details should be hidden.
    /// </summary>
    [JsonProperty("hideDetails")]
    public bool HideDetails { get; set; }

    /// <summary>
    /// The start and end time of the whole school day.
    /// </summary>
    [JsonProperty("duration")]
    public TimeRange Duration { get; set; }

    /// <summary>
    /// The type of the grid.
    /// </summary>
    [JsonProperty("timeGridType")]
    public string TimeGridType { get; set; } = string.Empty;

    /// <summary>
    /// The days this time grid is for.
    /// </summary>
    [JsonProperty("timeGridDays", ItemConverterType = typeof(DayOfWeekJsonConverter))]
    public IEnumerable<DayOfWeek> TimeGridDays { get; set; } = Enumerable.Empty<DayOfWeek>();

    /// <summary>
    /// The time grid slots this time grid contains.
    /// </summary>
    [JsonProperty("timeGridSlots")]
    public IEnumerable<TimeGridSlot> TimeGridSlots { get; set; } = Enumerable.Empty<TimeGridSlot>();
}
