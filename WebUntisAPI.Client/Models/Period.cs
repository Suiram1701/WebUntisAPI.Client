using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using WebUntisAPI.Client.Converters;
using WebUntisAPI.Client.Models.Elements;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// A lesson period
/// </summary>
public class Period
{
    /// <summary>
    /// The id of the period
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The id of the lesson
    /// </summary>
    [JsonProperty("lessonId")]
    public int LessonId { get; set; }

    /// <summary>
    /// The number of the lesson
    /// </summary>
    [JsonProperty("lessonNumber")]
    public int LessonNumber { get; set; }

    /// <summary>
    /// The code of the lesson
    /// </summary>
    [JsonProperty("lessonCode")]
    public string LessonCode { get; set; } = string.Empty;

    /// <summary>
    /// The background color of the lesson
    /// </summary>
    /// <remarks>
    /// When <c>null</c> the default lesson color should used
    /// </remarks>
    [JsonProperty("lessonBackColor")]
    [JsonConverter(typeof(ColorJsonConverter))]
    public Color? LessonBackColor { get; set; }

    /// <summary>
    /// Some informations for the lessons
    /// </summary>
    [JsonProperty("lessonText")]
    public string LessonText { get; set; } = string.Empty;

    /// <summary>
    /// The substitution text of the lesson
    /// </summary>
    [JsonProperty("periodText")]
    public string PeriodText { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether this lesson has an substitution text
    /// </summary>
    [JsonProperty("hasPeriodText")]
    public bool HasPeriodText { get; set; }

    /// <summary>
    /// Informations about the period
    /// </summary>
    [JsonProperty("periodInfo")]
    public string PeriodInfo { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the period has an info
    /// </summary>
    [JsonProperty("hasInfo")]
    public bool HasInfo { get; set; }

    /// <summary>
    /// The text of the substitution
    /// </summary>
    [JsonProperty("subsText")]
    public string SubsText { get; set; } = string.Empty;

    /// <summary>
    /// The date where this lesson happens
    /// </summary>
    [JsonProperty("date")]
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Date { get; set; }

    /// <summary>
    /// The start time of the lesson
    /// </summary>
    [JsonProperty("startTime")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// The end time of the lesson
    /// </summary>
    [JsonProperty("endTime")]
    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// A group of students that take part at the period
    /// </summary>
    [JsonProperty("studentGroup")]
    public string StudentGroup { get; set; } = string.Empty;

    /// <summary>
    /// All elements that are associated with this period
    /// </summary>
    [JsonProperty("elements")]
    public IEnumerable<ElementId> Elements { get; set; } = Array.Empty<ElementId>();

    /// <summary>
    /// The state of the cell
    /// </summary>
    [JsonProperty("cellState")]
    public CellState CellState { get; set; }

    /// <summary>
    /// The priority of the lesson
    /// </summary>
    [JsonProperty("priority")]
    public int Priority { get; set; }

    /// <summary>
    /// The capacity
    /// </summary>
    [JsonProperty("roomCapacity")]
    public int RoomCapacity { get; set; }

    /// <summary>
    /// The count of students
    /// </summary>
    [JsonProperty("studentCount")]
    public int StudentCount { get; set; }

    /// <summary>
    /// Information about a rescheduled period
    /// </summary>
    /// <remarks>
    /// This property is only set when <see cref="CellState"/> is set to <see cref="CellState.Shift"/>
    /// </remarks>
    [JsonProperty("rescheduleInfo")]
    public RescheduleInfo? RescheduleInfo { get; set; }

    /// <summary>
    /// Information about an exam
    /// </summary>
    /// <remarks>
    /// This property is only set when <see cref="CellState"/> is set to <see cref="CellState.Exam"/>
    /// </remarks>
    [JsonProperty("exam")]
    public ExamInfo? ExamInfo { get; set; }
}