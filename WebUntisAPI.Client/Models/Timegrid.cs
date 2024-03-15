using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// A timegrid for a school week
/// </summary>
[JsonConverter(typeof(TimegridJsonConverter))]
public class Timegrid
{
    /// <summary>
    /// The layout for every school day
    /// </summary>
    public IEnumerable<SchoolHour> Hours { get; init; }

    /// <summary>
    /// The state of a lesson whether the it is a normal lesson or vacant.
    /// </summary>
    /// <remarks>
    /// This contains always 7 items, for every day one. Every item represent a day (starts with monday). The collection inside of every element has the same count of items than <see cref="Hours"/> and contains the <see cref="LessonState"/> specificly for this hour at the day
    /// </remarks>
    public IEnumerable<LessonState>[] LessonStates { get; init; }

    /// <summary>
    /// Is the timegrid persisted
    /// </summary>
    public bool Persisted { get; init; }
}