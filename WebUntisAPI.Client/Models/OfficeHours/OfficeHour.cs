using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.OfficeHours;

/// <summary>
/// Represents an office hour.
/// </summary>
public class OfficeHour
{
    /// <summary>
    /// The id of this period.
    /// </summary>
    [JsonProperty("periodId")]
    public int Id { get; set; }

    /// <summary>
    /// The id of the photo of the teacher.
    /// </summary>
    [JsonProperty("photoId")]
    public int PhotoId { get; set; }

    /// <summary>
    /// The id of the assigned teacher.
    /// </summary>
    [JsonProperty("teacherId")]
    public int TeacherId { get; set; }

    /// <summary>
    /// The teacher assigned to this office hour.
    /// </summary>
    [JsonProperty("teacher")]
    public string Teacher { get; set; } = string.Empty;

    /// <summary>
    /// The date time were the office hour takes place.
    /// </summary>
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    /// <summary>
    /// The start time of the office hour.
    /// </summary>
    [JsonProperty("startTime")]
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// The end time of the office hour.
    /// </summary>
    [JsonProperty("endTime")]
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// The period where the office hour takes place.
    /// </summary>
    [JsonProperty("hour")]
    public int Hour { get; set; }

    /// <summary>
    /// The email address of the assigned teacher.
    /// </summary>
    /// <remarks>
    /// When <c>null</c> this value isn't specified.
    /// </remarks>
    [JsonProperty("email")]
    public string? Email { get; set; }

    /// <summary>
    /// The phone number of the assigned teacher.
    /// </summary>
    /// <remarks>
    /// When <c>null</c> this value isn't specified.
    /// </remarks>
    public string? Phone { get; set; }

    /// <summary>
    /// The room where the office hour takes place.
    /// </summary>
    [JsonProperty("rooms")]
    public string? Room { get; set; }

    /// <summary>
    /// Indicates whether this office hour is available.
    /// </summary>
    [JsonProperty("available")]
    public bool Available { get; set; }
}
