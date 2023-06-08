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
            dateString = dateTime.ToString("yyyy-MM-dd");
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
        /// <param name="dateTime">Instance</param>
        /// <param name="dateString">Date string</param>
        /// <param name="timeString">Time string</param>
        /// <returns>The new instance that contains the given time. When not to throw on exception and an exception happened is the return value <see langword="null"/></returns>
        /// <exception cref="FormatException">Thrown when one of the given strings isn't in the right format</exception>
        public static DateTime FromWebUntisTimeFormat(this DateTime dateTime, string dateString, string timeString)
        {
            Regex dateRegex = new Regex(@"^\d{4}-(0\d|1[0-2])-(0[1-9]|[1-2]\d|3[0-1])$");     // Regex for the WebUntis date format
            Regex timeRegex = new Regex(@"^(\d|1\d|2[0-3])[0-5]\d$");     // Regex for the WebUntis time format

            // Check if the date- and time strings are valid
            bool isDateValid = dateRegex.IsMatch(dateString);
            bool isTimeValid = timeRegex.IsMatch(timeString);

            if (!isDateValid || !isTimeValid)
                throw new FormatException($"The string {(isDateValid ? timeString : dateString)} isn't in the valid format!");
                

            // Parse the numbers in the string to value
            int year = int.Parse(dateString.Substring(0, 4));
            int month = int.Parse(dateString.Substring(5, 2));
            int day = int.Parse(dateString.Substring(8, 2));

            bool is4Letters = timeString.Length == 4;
            int hour = int.Parse(is4Letters ? timeString.Substring(0, 2) : timeString[0].ToString());
            int minute = int.Parse(is4Letters ? timeString.Substring(2, 2) : timeString.Substring(1, 2));

            // date and time string to DateTime instance
            return new DateTime(year, month, day, hour, minute, 0);
        }
    }
}
