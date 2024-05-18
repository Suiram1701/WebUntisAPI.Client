using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.NewTimetable;

/// <summary>
/// Represents settings how the timetable should be displayed.
/// </summary>
public class TimetableSettings
{
    /// <summary>
    /// Indicates whether symbols should be shown.
    /// </summary>
    [JsonProperty("showSymbols")]
    public bool ShowSymbols { get; set; }

    /// <summary>
    /// Indicates whether the absences of teachers should be shown
    /// </summary>
    [JsonProperty("showTeacherAbsences")]
    public bool ShowTeacherAbsences { get; set; }

    /// <summary>
    /// Indicates whether the absences of students should be shown.
    /// </summary>
    [JsonProperty("showStudentAbsences")]
    public bool ShowStudentAbsences { get; set; }

    /// <summary>
    /// Indicates whether room locks should be shown.
    /// </summary>
    [JsonProperty("showRoomLocks")]
    public bool ShowRoomLocks { get; set; }

    /// <summary>
    /// Indicates whether resource locks should be shown.
    /// </summary>
    [JsonProperty("showResourceLocks")]
    public bool ShowResourceLocks { get; set; }

    /// <summary>
    /// Indicates whether the ICal address of the timetable should be shown.
    /// </summary>
    [JsonProperty("showICal")]
    public bool ShowICal { get; set; }

    /// <summary>
    /// Indicates whether changes of periods should be highlighted.
    /// </summary>
    [JsonProperty("highlightChanges")]
    public bool HighlightChanges { get; set; }

    /// <summary>
    /// Indicates whether periods where an exam take place should be highlighted.
    /// </summary>
    [JsonProperty("highlightExams")]
    public bool HighlightExams { get; set; }

    /// <summary>
    /// Indicates whether cancelled periods should be highlighted.
    /// </summary>
    [JsonProperty("highlightCancellations")]
    public bool HighlightCancellations { get; set; }

    /// <summary>
    /// Indicates whether external timetable entries should be highlighted.
    /// </summary>
    [JsonProperty("highlightExternalEntries")]
    public bool HighlightExternalEntries { get; set; }
}
