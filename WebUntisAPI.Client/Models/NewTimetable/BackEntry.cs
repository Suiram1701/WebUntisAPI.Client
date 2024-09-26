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
/// Represents a back entry of a <see cref="TimetableDay"/>.
/// </summary>
public class BackEntry
{
    /// <summary>
    /// The id of this entry.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The type of this entry.
    /// </summary>
    [JsonProperty("type")]
    public BackEntryType Type { get; set; }

    /// <summary>
    /// The status of this entry.
    /// </summary>
    [JsonProperty("status")]
    public PeriodStatus Status { get; set; }

    /// <summary>
    /// Details of the status.
    /// </summary>
    [JsonProperty("statusDetail")]
    public string? StatusDetail { get; set; }

    /// <summary>
    /// The start and end time this back entry happens.
    /// </summary>
    [JsonProperty("duration")]
    public TimeRange Duration { get; set; }

    /// <summary>
    /// Indicates whether this entry claims the whole day.
    /// </summary>
    [JsonProperty("isFullDay")]
    public bool IsFullDay { get; set; }

    /// <summary>
    /// total duration
    /// </summary>
    [JsonProperty("durationTotal")]
    public TimeRange? DurationTotal { get; set; }

    /// <summary>
    /// The color of this period.
    /// </summary>
    [JsonProperty("color")]
    [JsonConverter(typeof(HexColorJsonConverter))]
    public Color Color { get; set; }

    /// <summary>
    /// The short name of this entry.
    /// </summary>
    [JsonProperty("shortName")]
    public string ShortName { get; set; } = default!;

    /// <summary>
    /// The long name of this entry.
    /// </summary>
    [JsonProperty("longName")]
    public string LongName { get; set; } = default!;

    /// <summary>
    /// All notes of this entry.
    /// </summary>
    [JsonProperty("notesAll")]
    public string? NotesAll { get; set; }
}
