using System;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="DateTime"/> struct.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert a <see cref="DateTime"/> to the WebUntis date and time string format.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     The date format is YYYYMMDD (Year Month Day).
        ///     </para>
        ///     <para>
        ///     The time format is HHMM (Hour Minute).
        ///     </para>
        /// </remarks>
        /// <param name="dateTime">Instance</param>
        /// <param name="dateString">Date string</param>
        /// <param name="timeString">Time string</param>
        public static void ToWebUntisTimeFormat(this DateTime dateTime, out string dateString, out string timeString)
        {
            dateString = dateTime.ToString("yyyyMMdd");
            timeString = dateTime.Hour + dateTime.ToString("mm");
        }

        /// <summary>
        /// Convert the WebUntis date and time string format to the <see cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     The date format is YYYYMMDD (Year Month Day).
        ///     </para>
        ///     <para>
        ///     The time format is HHMM (Hour Minute).
        ///     </para>
        /// </remarks>
        /// <param name="_">Instance</param>
        /// <param name="dateString">Date string</param>
        /// <param name="timeString">Time string</param>
        /// <returns>The new instance that contains the given time. When not to throw on exception and an exception happened is the return value <see langword="null"/></returns>
        /// <exception cref="FormatException">Thrown when one of the given strings isn't in the right format</exception>
        public static DateTime FromWebUntisTimeFormat(this DateTime _, string dateString, string timeString)
        {
            Match dateMatch = Regex.Match(dateString, @"^(\d{4})(0\d|1[0-2])(0[1-9]|[1-2]\d|3[0-1])$");
            Match timeMatch = Regex.Match(timeString, @"^(\d|1\d|2[0-3])([0-5]\d)$");

            if (!dateMatch.Success || !timeMatch.Success)
                throw new FormatException("A valid format was expected");

            int year = int.Parse(dateMatch.Groups[1].Value);
            int month = int.Parse(dateMatch.Groups[2].Value);
            int day = int.Parse(dateMatch.Groups[3].Value);

            int hour = int.Parse(timeMatch.Groups[1].Value);
            int minute = int.Parse(timeMatch.Groups[2].Value);

            // date and time string to DateTime instance
            return new DateTime(year, month, day, hour, minute, 0);
        }
    }
}
