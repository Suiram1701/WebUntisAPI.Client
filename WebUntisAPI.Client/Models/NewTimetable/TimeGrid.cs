using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models.NewTimetable;

/// <summary>
/// Information about time grid formats and different time grid definitions.
/// </summary>
public class TimeGrid
{
    /// <summary>
    /// The first day of a week.
    /// </summary>
    [JsonProperty("firstDayOfWeek")]
    [JsonConverter(typeof(DayOfWeekJsonConverter))]
    public DayOfWeek FirstDayOfWeek { get; set; }

    /// <summary>
    /// idk
    /// </summary>
    [JsonProperty("studentFormat")]
    public int? StudentFormat { get; set; }

    /// <summary>
    /// idk
    /// </summary>
    [JsonProperty("classFormat")]
    public int? ClassFormat { get; set; }

    /// <summary>
    /// idk
    /// </summary>
    [JsonProperty("subjectFormat")]
    public int? SubjectFormat { get; set; }

    /// <summary>
    /// idk
    /// </summary>
    [JsonProperty("teacherFormat")]
    public int? TeacherFormat { get; set; }

    /// <summary>
    /// idk
    /// </summary>
    [JsonProperty("roomFormat")]
    public int? RoomFormat { get; set; }

    /// <summary>
    /// idk
    /// </summary>
    [JsonProperty("resourceFormat")]
    public int ResourceFormat { get; set; }

    /// <summary>
    /// All available time grid definitions.
    /// </summary>
    [JsonProperty("formatDefinitions")]
    public IEnumerable<TimeGridDefinition> TimeGridDefinitions { get; set; } = Enumerable.Empty<TimeGridDefinition>();
}