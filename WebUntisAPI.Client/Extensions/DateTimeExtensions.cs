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
            dateString = dateTime.Year.ToString() + dateTime.Month.ToString() + dateTime.Day.ToString();
            timeString = dateTime.Hour.ToString() + dateTime.Minute.ToString();
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
        /// <returns>The new instance that contains the given time</returns>
        /// <exception cref="FormatException">Thrown when one of the given strings isn't in the right format</exception>
        public static DateTime FromWebUntisTimeFormat(this DateTime dateTime, string dateString, string timeString)
        {
            Regex dateRegex = new Regex(@"^\d{4}(0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])$");     // Regex for the WebUntis date format
            Regex timeRegex = new Regex("^([0-1][0-9]|2[0-3])[0-5][0-9]$");     // Regex for the WebUntis time format

            // Check if the date- and time strings are valid
            bool isDateValid = dateRegex.IsMatch(dateString);
            bool isTimeValid = timeRegex.IsMatch(timeString);

            if (!isDateValid || !isTimeValid)
                throw new FormatException($"The string {(isDateValid ? timeString : dateString)} isn't in the valid format!");

            // Parse the numbers in the string to value
            int year = int.Parse(dateString.Substring(0, 3));
            int month = int.Parse(dateString.Substring(4, 5));
            int day = int.Parse(dateString.Substring(6, 7));

            int hour = int.Parse(timeString.Substring(0, 1));
            int minute = int.Parse(timeString.Substring(2, 3));

            // date and time string to DateTime instance
            return new DateTime(year, month, day, hour, minute, 0);
        }
    }
}
