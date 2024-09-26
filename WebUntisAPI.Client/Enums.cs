using Newtonsoft.Json;
using System;

namespace WebUntisAPI.Client;

/// <summary>
/// The gender of a student
/// </summary>
public enum Gender
{
    /// <summary>
    /// Male
    /// </summary>
    Male = 0,

    /// <summary>
    /// Female
    /// </summary>
    Female = 1,
}

/// <summary>
/// All day in a week
/// </summary>
public enum Day
{
    /// <summary>
    /// Sunday
    /// </summary>
    Sunday = 1,

    /// <summary>
    /// Monday
    /// </summary>
    Monday = 2,

    /// <summary>
    /// Tuesday
    /// </summary>
    Tuesday = 3,

    /// <summary>
    /// Wednesday
    /// </summary>
    Wednesday = 4,

    /// <summary>
    /// Thursday
    /// </summary>
    Thursday = 5,

    /// <summary>
    /// Friday
    /// </summary>
    Friday = 6,

    /// <summary>
    /// Saturday
    /// </summary>
    Saturday = 7,
}

/// <summary>
/// All diefrent types of lesons
/// </summary>
public enum LessonType
{
    /// <summary>
    /// Lesson
    /// </summary>
    [JsonProperty("ls")]
    Ls = 0,

    /// <summary>
    /// Office hour
    /// </summary>
    [JsonProperty("oh")]
    Oh = 1,

    /// <summary>
    /// Standby
    /// </summary>
    [JsonProperty("sb")]
    Sb = 2,

    /// <summary>
    /// Break supervision
    /// </summary>
    [JsonProperty("bs")]
    Bs = 3,

    /// <summary>
    /// Examination
    /// </summary>
    [JsonProperty("ex")]
    Ex = 4
}

/// <summary>
/// Various codes for the lessons
/// </summary>
public enum Code
{
    /// <summary>
    /// No code (normal lesson)
    /// </summary>
    [JsonProperty("")]
    None = 0,

    /// <summary>
    /// A cancelled lesson
    /// </summary>
    [JsonProperty("cancelled")]
    Cancelled = 1,

    /// <summary>
    /// An irregular lesson
    /// </summary>
    [JsonProperty("irregular")]
    Irregular = 2,
}

/// <summary>
/// The state of a lesson in the timegrid
/// </summary>
public enum LessonState
{
    /// <summary>
    /// A normal lesson
    /// </summary>
    [JsonProperty("LESSON")]
    Lesson,

    /// <summary>
    /// The lesson is vacant
    /// </summary>
    [JsonProperty("VACANT")]
    Vacant
}

/// <summary>
/// Represents the state of a cell
/// </summary>
public enum CellState
{
    /// <summary>
    /// Stands for a default period without any changes
    /// </summary>
    [JsonProperty("STANDARD")]
    Standard,

    /// <summary>
    /// Stands for a period that was shifted in the timetable
    /// </summary>
    [JsonProperty("SHIFT")]
    Shift,

    /// <summary>
    /// Stands for a period whose room was changed
    /// </summary>
    [JsonProperty("ROOMSUBSTITUTION")]
    RoomSubstitution,

    /// <summary>
    /// Stands for a period where an exam happens
    /// </summary>
    [JsonProperty("EXAM")]
    Exam,

    /// <summary>
    /// Stands for a period that was cancelled
    /// </summary>
    [JsonProperty("CANCEL")]
    Cancel,

    /// <summary>
    /// Stands for a period that was substituted
    /// </summary>
    [JsonProperty("SUBSTITUTION")]
    Substitution,

    /// <summary>
    /// Stands for an additional period
    /// </summary>
    [JsonProperty("ADDITIONAL")]
    Additional 
}


/// <summary>
/// Different types of elements
/// </summary>
public enum ElementType
{
    /// <summary>
    /// A class
    /// </summary>
    Class = 1,

    /// <summary>
    /// A teacher
    /// </summary>
    Teacher = 2,

    /// <summary>
    /// A subject
    /// </summary>
    Subject = 3,

    /// <summary>
    /// A room
    /// </summary>
    Room = 4,

    /// <summary>
    /// A student
    /// </summary>
    Student = 5,
}

/// <summary>
/// Different states of elements
/// </summary>
public enum ElementState
{
    /// <summary>
    /// The element is normal
    /// </summary>
    [JsonProperty("REGULAR")]
    Regular,

    /// <summary>
    /// The element is substituted
    /// </summary>
    [JsonProperty("SUBSTITUTED")]
    Substituted,

    /// <summary>
    /// The element is absent
    /// </summary>
    [JsonProperty("ABSENT")]
    Absent
}

/// <summary>
/// Represents different states of the time slot of an office hour.
/// </summary>
public enum TimeSlotState
{
    /// <summary>
    /// Indicates that the signed in user is signed up to the time slot.
    /// </summary>
    SignedUp,

    /// <summary>
    /// Indicates that another user than the signed in one is already signed up to this time slot.
    /// </summary>
    Occupied,

    /// <summary>
    /// Indicates that this time slot is free.
    /// </summary>
    Free,

    /// <summary>
    /// Indicates that the state of time slot couldn't be determined.
    /// </summary>
    None
}

/// <summary>
/// Represents an export file formats.
/// </summary>
public enum ExportFileFormat
{
    /// <summary>
    /// The .pdf format.
    /// </summary>
    Pdf,

    /// <summary>
    /// The .xls format.
    /// </summary>
    Xls,

    /// <summary>
    /// The .csv format.
    /// </summary>
    Csv
}

/// <summary>
/// Flags to enable different types of timetable entries.
/// </summary>
[Flags]
public enum PeriodType
{
    /// <summary>
    /// Shows normal teaching periods
    /// </summary>
    [JsonProperty("NORMAL_TEACHING_PERIOD")]
    Normal_Teaching_Period = 1,

    /// <summary>
    /// Shows additional periods
    /// </summary>
    [JsonProperty("ADDITIONAL_PERIOD")]
    Additional_Period = 2,

    /// <summary>
    /// Shows event periods
    /// </summary>
    [JsonProperty("EVENT")]
    Event = 4,

    /// <summary>
    /// Shows stand by periods
    /// </summary>
    [JsonProperty("STAND_BY_PERIOD")]
    Stand_By_Period = 8,

    /// <summary>
    /// Shows office hours
    /// </summary>
    [JsonProperty("OFFICE_HOUR")]
    Office_Hour = 16,

    /// <summary>
    /// Shows exam periods
    /// </summary>
    [JsonProperty("EXAM")]
    Exam = 32,

    /// <summary>
    /// Shows break supervision periods
    /// </summary>
    [JsonProperty("BREAK_SUPERVISION")]
    Break_Supervision = 64,
}

/// <summary>
/// Represents different statuses of periods.
/// </summary>
public enum PeriodStatus
{
    /// <summary>
    /// The period takes place regular.
    /// </summary>
    [JsonProperty("REGULAR")]
    Regular,

    /// <summary>
    /// Something changed in the period.
    /// </summary>
    [JsonProperty("CHANGED")]
    Changed,

    /// <summary>
    /// The period is cancelled.
    /// </summary>
    [JsonProperty("CANCELLED")]
    Cancelled
}

/// <summary>
/// Represents different status of a period element.
/// </summary>
public enum PeriodElementStatus
{
    /// <summary>
    /// The element is regular.
    /// </summary>
    [JsonProperty("REGULAR")]
    Regular,

    /// <summary>
    /// The element were added.
    /// </summary>
    [JsonProperty("ADDED")]
    Added,

    /// <summary>
    /// The element were removed.
    /// </summary>
    [JsonProperty("REMOVED")]
    Removed
}

/// <summary>
/// Different ways of requesting a timetable.
/// </summary>
public enum TimetableType
{
    /// <summary>
    /// Standard
    /// </summary>
    Standard,

    /// <summary>
    /// My timetable
    /// </summary>
    My_Timetable
}

/// <summary>
/// Different types of a back entry.
/// </summary>
public enum BackEntryType
{
    /// <summary>
    /// A holiday
    /// </summary>
    [JsonProperty("HOLIDAY")]
    Holiday
}