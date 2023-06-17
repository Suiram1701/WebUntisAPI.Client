using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client
{
    /// <summary>
    /// Types of users
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// A Untis teacher
        /// </summary>
        Teacher = 2,

        /// <summary>
        /// A Untis student
        /// </summary>
        Student = 5
    }

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
        /// No code
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
}