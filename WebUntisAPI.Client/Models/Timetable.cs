using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;
using WebUntisAPI.Client.Models.Elements;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// A timetable for an element
/// </summary>
[JsonConverter(typeof(TimetableJsonConverter))]
public class Timetable
{
    /// <summary>
    /// The periods that happen in the week
    /// </summary>
    public IEnumerable<Period> Periods { get; }

    /// <summary>
    /// All elements like classes, subjects, teachers and rooms that are used by <see cref="Periods"/>
    /// </summary>
    public IEnumerable<ElementBase> Elements { get; }

    /// <summary>
    /// The timestamp of the last update
    /// </summary>
    public DateTimeOffset LastImportTimestamp { get; }

    /// <summary>
    /// Creates a new instance
    /// </summary>
    /// <param name="periods">The periods</param>
    /// <param name="elements">The elements</param>
    /// <param name="lastImportTimestamp">The last import timestamp</param>
    public Timetable(IEnumerable<Period> periods, IEnumerable<ElementBase> elements, DateTimeOffset lastImportTimestamp)
    {
        Periods = periods;
        Elements = elements;
        LastImportTimestamp = lastImportTimestamp;
    }
}
