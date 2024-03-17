using Newtonsoft.Json;

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
    /// Stands for a period that whose room was changed
    /// </summary>
    [JsonProperty("ROOMSUBSTITUTION")]
    RoomSubstitution,

    /// <summary>
    /// Stands for a period where a exam happens
    /// </summary>
    [JsonProperty("EXAM")]
    Exam
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
    Substituted
}