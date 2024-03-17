using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// A timegrid for a school week
/// </summary>
[JsonConverter(typeof(TimegridJsonConverter))]
public class Timegrid
{
    /// <summary>
    /// The id of the school year this timegrid is assigned to
    /// </summary>
    public int SchoolYearId { get; }

    /// <summary>
    /// Is the timegrid persisted
    /// </summary>
    public bool Persisted { get; }

    /// <summary>
    /// The layout for every school day
    /// </summary>
    public IEnumerable<SchoolHour> Hours { get; }

    /// <summary>
    /// The state of a lesson whether the it is a normal lesson or vacant
    /// </summary>
    /// <remarks>
    /// This contains always 7 items, for every day one. Every item represent a day (starts with monday). The collection inside of every element has the same count of items than <see cref="Hours"/> and contains the <see cref="LessonState"/> specificly for this hour at the day
    /// </remarks>
    public IEnumerable<LessonState>[] LessonStates { get; }

    /// <summary>
    /// Creates a new instance
    /// </summary>
    /// <param name="schoolyear">The school year this timegrid is assigned to</param>
    /// <param name="persisted">Is the timegrid persisted</param>
    /// <param name="hours">The hours for every day</param>
    /// <param name="lessonStates">The state of a lesson whether the it is a normal lesson or vacant</param>
    public Timegrid(int schoolyear, bool persisted, IEnumerable<SchoolHour> hours, IEnumerable<LessonState>[] lessonStates)
    {
        SchoolYearId = schoolyear;
        Persisted = persisted;
        Hours = hours;
        LessonStates = lessonStates;
    }
}