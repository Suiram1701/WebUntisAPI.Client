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
}