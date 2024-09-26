using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models.NewTimetable;

/// <summary>
/// Represents a period of the timetable.
/// </summary>
public class Period
{
    /// <summary>
    /// The ids of single time slots this period takes place at.
    /// </summary>
    /// <remarks>
    /// This means that for example a double lesson also takes two ids.
    /// </remarks>
    [JsonProperty("ids")]
    public IEnumerable<int> Ids { get; set; } = Enumerable.Empty<int>();

    /// <summary>
    /// The start and end time this period takes place at.
    /// </summary>
    [JsonProperty("duration")]
    public TimeRange Duration { get; set; }

    /// <summary>
    /// The type of this period.
    /// </summary>
    [JsonProperty("type")]
    public PeriodType Type { get; set; }

    /// <summary>
    /// The status of this period.
    /// </summary>
    [JsonProperty("status")]
    public PeriodStatus Status { get; set; }

    /// <summary>
    /// Additional details to the status.
    /// </summary>
    [JsonProperty("statusDetail")]
    public string? StatusDetails { get; set; }

    /// <summary>
    /// The name of the period.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The color of this period.
    /// </summary>
    [JsonProperty("color")]
    [JsonConverter(typeof(HexColorJsonConverter))]
    public Color Color { get; set; }

    /// <summary>
    /// Notes of this period.
    /// </summary>
    [JsonProperty("notesAll")]
    public string? NotesAll { get; set; }

    /// <summary>
    /// The first position to display data.
    /// </summary>
    [JsonProperty("position1")]
    public IEnumerable<PeriodElementStatus> Position1 { get; set; } = Enumerable.Empty<PeriodElementStatus>();

    /// <summary>
    /// The second position to display data.
    /// </summary>
    [JsonProperty("position2")]
    public IEnumerable<PeriodElementStatus> Position2 { get; set; } = Enumerable.Empty<PeriodElementStatus>();

    /// <summary>
    /// The third position to display data.
    /// </summary>
    [JsonProperty("position3")]
    public IEnumerable<PeriodElementStatus> Position3 { get; set; } = Enumerable.Empty<PeriodElementStatus>();

    /// <summary>
    /// The fourth position to display data.
    /// </summary>
    [JsonProperty("position4")]
    public IEnumerable<PeriodElementStatus> Position4 { get; set; } = Enumerable.Empty<PeriodElementStatus>();

    /// <summary>
    /// The fifth position to display data.
    /// </summary>
    [JsonProperty("position5")]
    public IEnumerable<PeriodElementStatus> Position5 { get; set; } = Enumerable.Empty<PeriodElementStatus>();

    /// <summary>
    /// The lesson text
    /// </summary>
    [JsonProperty("lessonText")]
    public string? LessonText { get; set; }

    /// <summary>
    /// The lesson info
    /// </summary>
    [JsonProperty("lessonInfo")]
    public string? LessonInfo { get; set; }

    /// <summary>
    /// The substitution text
    /// </summary>
    [JsonProperty("substitutionText")]
    public string? SubstitutionText { get; set; }

    /// <summary>
    /// Moved
    /// </summary>
    [JsonProperty("moved")]
    public object? Moved { get; set; }

    /// <summary>
    /// total duration
    /// </summary>
    [JsonProperty("durationTotal")]
    public TimeRange? DurationTotal { get; set; }

    /// <summary>
    /// link
    /// </summary>
    [JsonProperty("link")]
    public object? Link { get; set; }
}
