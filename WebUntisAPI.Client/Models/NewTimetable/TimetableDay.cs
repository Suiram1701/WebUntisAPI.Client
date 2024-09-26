using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.NewTimetable;

/// <summary>
/// Represents a day in the timetable
/// </summary>
[JsonConverter(typeof(TimetableDayJsonConverter))]
public class TimetableDay
{
    /// <summary>
    /// The date instance is for.
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// The resource this timetable were requested for.
    /// </summary>
    public IElement? Resource { get; init; } = default!;

    /// <summary>
    /// The status of this day.
    /// </summary>
    public PeriodStatus Status { get; init; }

    /// <summary>
    /// day entries
    /// </summary>
    public IEnumerable<object> DayEntries { get; init; } = Enumerable.Empty<object>();

    /// <summary>
    /// Entries of periods in the time grid.
    /// </summary>
    public IEnumerable<Period> GridEntries { get; init; } = Enumerable.Empty<Period>();

    /// <summary>
    /// Entries of other things.
    /// </summary>
    public IEnumerable<BackEntry> BackEntries { get; init; } = Enumerable.Empty<BackEntry>();
}