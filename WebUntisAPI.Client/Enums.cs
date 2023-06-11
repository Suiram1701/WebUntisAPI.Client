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
}