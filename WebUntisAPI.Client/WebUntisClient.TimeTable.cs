using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Extensions;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client
{
    public partial class WebUntisClient
    {
        /// <summary>
        /// Get lesson types, periods and their colors
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Get status data</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<StatusData> GetStatusDataAsync(string id = "getStatusData", CancellationToken ct = default)
        {
            StatusData statusData = (await MakeJSONRPCRequestAsync(id, "getStatusData", null, ct)).ToObject<StatusData>();
            return statusData;
        }

        /// <summary>
        /// Get the timegrid for the school
        /// </summary>
        /// <param name="id">Identier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The timegrid for all days</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Timegrid> GetTimegridAsync(string id = "getTimegrid", CancellationToken ct = default)
        {
            Timegrid timeGrid = (await MakeJSONRPCRequestAsync(id, "getTimegridUnits", null, ct)).ToObject<Timegrid>();
            return timeGrid;
        }

        /// <summary>
        /// Get all available school years
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>All school years</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<SchoolYear[]> GetSchoolYearsAsync(string id = "getSchoolyears", CancellationToken ct = default)
        {
            List<SchoolYear> schoolYears = (await MakeJSONRPCRequestAsync(id, "getSchoolyears", null, ct)).ToObject<List<SchoolYear>>();
            return schoolYears.ToArray();
        }

        /// <summary>
        /// Get the current school year
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The current school year</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<SchoolYear> GetCurrentSchoolYearAsync(string id = "getCurrentSchoolyear", CancellationToken ct = default)
        {
            SchoolYear schoolYear = (await MakeJSONRPCRequestAsync(id, "getCurrentSchoolyear", null, ct)).ToObject<SchoolYear>();
            return schoolYear;
        }

        /// <summary>
        /// Get all holidays async
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>All holidays</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Holidays[]> GetHolidaysAsync(string id = "getHolidays", CancellationToken ct = default)
        {
            List<Holidays> holidays = (await MakeJSONRPCRequestAsync(id, "getHolidays", null, ct)).ToObject<List<Holidays>>();
            return holidays.ToArray();
        }

        /// <summary>
        /// Get all classreg events
        /// </summary>
        /// <param name="startDate">Start date for the requested events</param>
        /// <param name="endDate">End date for the requested events</param>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellatio token</param>
        /// <returns>All classreg events in the given date range</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<ClassregEvent[]> GetClassregEventsAsync(DateTime startDate, DateTime endDate, string id = "getCLassregEvents", CancellationToken ct = default)
        {
            Action<JsonWriter> paramsAction = new Action<JsonWriter>(writer =>
            {
                writer.WriteStartObject();

                startDate.ToWebUntisTimeFormat(out string startDateString, out _);
                writer.WritePropertyName("startDate");
                writer.WriteValue(startDateString);

                endDate.ToWebUntisTimeFormat(out string endDateString, out _);
                writer.WritePropertyName("endDate");
                writer.WriteValue(endDateString);

                writer.WriteEndObject();
            });
            List<ClassregEvent> classregEvents = (await MakeJSONRPCRequestAsync(id, "getClassregEvents", paramsAction, ct)).ToObject<List<ClassregEvent>>();
            return classregEvents.ToArray();
        }

        #region Timetable
        /// <summary>
        /// Get the timetable the user as their you logged in
        /// </summary>
        /// <param name="startDate">Start date of the timetable (default is the current date)</param>
        /// <param name="endDate">End date of the timetable (default is the current date)</param>
        /// <param name="id">Identier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The periods for the user</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Period[]> GetOwnTimetableAsync(DateTime startDate = default, DateTime endDate = default, string id = "GetOwnTimtable", CancellationToken ct = default)
        {
            if (UserType == Client.UserType.Student)
                return await GetTimetableForStudentAsync((Student)User, startDate, endDate, id, ct);
            else
                return await GetTimetableForTeacherAsync((Teacher)User, startDate, endDate, id, ct);
        }

        /// <summary>
        /// Get the timetable for a class
        /// </summary>
        /// <param name="class">The class from the timetable</param>
        /// <param name="startDate">Start date of the timetable (default is the current date)</param>
        /// <param name="endDate">End date of the timetable (default is the current date)</param>
        /// <param name="id">Identier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The periods for the class</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Period[]> GetTimetableForClassAsync(Class @class, DateTime startDate = default, DateTime endDate = default, string id = "GetTimtableForClass", CancellationToken ct = default)
        {
            return await GetTimetableAsync(@class.Id, 1, startDate, endDate, id, ct);
        }

        /// <summary>
        /// Get the timetable for a teacher
        /// </summary>
        /// <param name="teacher">The teacher from the timetable</param>
        /// <param name="startDate">Start date of the timetable (default is the current date)</param>
        /// <param name="endDate">End date of the timetable (default is the current date)</param>
        /// <param name="id">Identier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The periods for the teacher</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Period[]> GetTimetableForTeacherAsync(Teacher teacher, DateTime startDate = default, DateTime endDate = default, string id = "GetTimtableForTeacher", CancellationToken ct = default)
        {
            return await GetTimetableAsync(teacher.Id, 2, startDate, endDate, id, ct);
        }

        /// <summary>
        /// Get the timetable for a subject
        /// </summary>
        /// <param name="subject">The subject from the timetable</param>
        /// <param name="startDate">Start date of the timetable (default is the current date)</param>
        /// <param name="endDate">End date of the timetable (default is the current date)</param>
        /// <param name="id">Identier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The periods for the subject</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Period[]> GetTimetableForSubjectAsync(Subject subject, DateTime startDate = default, DateTime endDate = default, string id = "GetTimtableForSubject", CancellationToken ct = default)
        {
            return await GetTimetableAsync(subject.Id, 3, startDate, endDate, id, ct);
        }

        /// <summary>
        /// Get the timetable for a room
        /// </summary>
        /// <param name="room">The room from the timetable</param>
        /// <param name="startDate">Start date of the timetable (default is the current date)</param>
        /// <param name="endDate">End date of the timetable (default is the current date)</param>
        /// <param name="id">Identier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The periods for the room</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Period[]> GetTimetableForRoomAsync(Room room, DateTime startDate = default, DateTime endDate = default, string id = "GetTimtableForRoom", CancellationToken ct = default)
        {
            return await GetTimetableAsync(room.Id, 4, startDate, endDate, id, ct);
        }

        /// <summary>
        /// Get the timetable for a student
        /// </summary>
        /// <param name="student">The student from the timetable</param>
        /// <param name="startDate">Start date of the timetable (default is the current date)</param>
        /// <param name="endDate">End date of the timetable (default is the current date)</param>
        /// <param name="id">Identier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The periods for the student</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Period[]> GetTimetableForStudentAsync(Student student, DateTime startDate = default, DateTime endDate = default, string id = "GetTimtableForStudent", CancellationToken ct = default)
        {
            return await GetTimetableAsync(student.Id, 5, startDate, endDate, id, ct);
        }

        private async Task<Period[]> GetTimetableAsync(int objId, int typeId, DateTime startDate, DateTime endDate, string id, CancellationToken ct)
        {
            // Check for defaul time
            if (startDate == default)
                startDate = DateTime.Now;

            if (endDate == default)
                endDate = DateTime.Now;

            Action<JsonWriter> paramsAction = new Action<JsonWriter>(writer =>
            {
                writer.WriteStartObject();

                writer.WritePropertyName("options");
                writer.WriteStartObject();

                writer.WritePropertyName("element");
                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(objId);

                writer.WritePropertyName("type");
                writer.WriteValue(typeId);
                writer.WriteEndObject();

                startDate.ToWebUntisTimeFormat(out string startDateString, out _);
                writer.WritePropertyName("startDate");
                writer.WriteValue(startDateString);

                endDate.ToWebUntisTimeFormat(out string endDateString, out _);
                writer.WritePropertyName("endDate");
                writer.WriteValue(endDateString);

                writer.WritePropertyName("showBooking");
                writer.WriteValue(true);

                writer.WritePropertyName("showInfo");
                writer.WriteValue(true);

                writer.WritePropertyName("showSubstText");
                writer.WriteValue(true);

                writer.WritePropertyName("showLsText");
                writer.WriteValue(true);

                writer.WritePropertyName("showLsNumber");
                writer.WriteValue(true);

                writer.WritePropertyName("showStudentgroup");
                writer.WriteValue(true);

                writer.WriteEndObject();
                writer.WriteEndObject();
            });
            List<Period> timetable = (await MakeJSONRPCRequestAsync(id, "getTimetable", paramsAction, ct)).ToObject<List<Period>>();
            return timetable.ToArray();
        }
        #endregion
    }
}
