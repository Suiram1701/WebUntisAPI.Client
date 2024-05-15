using Newtonsoft.Json;
using System;

namespace WebUntisAPI.Client.Models.OfficeHours;

/// <summary>
/// Represents a time slot of an office hour.
/// </summary>
public class OfficeHourTimeSlot
{
    /// <summary>
    /// The availability of this time slot.
    /// </summary>
    public TimeSlotState State => StateString switch
    {
        "SELF" => TimeSlotState.SignedUp,
        "OTHER" => TimeSlotState.Occupied,
        "FREE "=> TimeSlotState.Free,
        _ => TimeSlotState.None
    };

    [JsonProperty("state")]
    private string StateString { get; set; } = string.Empty;

    /// <summary>
    /// The start time of this time slot.
    /// </summary>
    [JsonProperty("startTime")]
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// The end time of this time slot.
    /// </summary>
    [JsonProperty("endTime")]
    public TimeOnly EndTime { get; set; }
}