using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
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
            StatusData statusData = await MakeRequestAsync<object, StatusData>(id, "getStatusData", new object(), ct);
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
            Timegrid timeGrid = await MakeRequestAsync<object, Timegrid>(id, "getTimegridUnits", new object(), ct);
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
            List<SchoolYear> schoolYears = await MakeRequestAsync<object, List<SchoolYear>>(id, "getSchoolyears", new object(), ct);
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
            SchoolYear schoolYear = await MakeRequestAsync<object, SchoolYear>(id, "getCurrentSchoolyear", new object(), ct);
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
            List<Holidays> holidays = await MakeRequestAsync<object, List<Holidays>>(id, "getHolidays", new object(), ct);
            return holidays.ToArray();
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
            // Check for defaul time
            if (startDate == default)
                startDate = DateTime.Now;

            if (endDate == default)
                endDate = DateTime.Now;

            TimetableRequestModel requestModel = new TimetableRequestModel()
            {
                Id = @class.Id,
                Type = 1,
                StartDate = startDate,
                EndDate = endDate
            };
            List<Period> timetable = await MakeRequestAsync<TimetableRequestModel, List<Period>>(id, "getTimetable", requestModel, ct);
            return timetable.ToArray();
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
            // Check for defaul time
            if (startDate == default)
                startDate = DateTime.Now;

            if (endDate == default)
                endDate = DateTime.Now;

            TimetableRequestModel requestModel = new TimetableRequestModel()
            {
                Id = teacher.Id,
                Type = 2,
                StartDate = startDate,
                EndDate = endDate
            };
            List<Period> timetable = await MakeRequestAsync<TimetableRequestModel, List<Period>>(id, "getTimetable", requestModel, ct);
            return timetable.ToArray();
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
            // Check for defaul time
            if (startDate == default)
                startDate = DateTime.Now;

            if (endDate == default)
                endDate = DateTime.Now;

            TimetableRequestModel requestModel = new TimetableRequestModel()
            {
                Id = subject.Id,
                Type = 3,
                StartDate = startDate,
                EndDate = endDate
            };
            List<Period> timetable = await MakeRequestAsync<TimetableRequestModel, List<Period>>(id, "getTimetable", requestModel, ct);
            return timetable.ToArray();
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
            // Check for defaul time
            if (startDate == default)
                startDate = DateTime.Now;

            if (endDate == default)
                endDate = DateTime.Now;

            TimetableRequestModel requestModel = new TimetableRequestModel()
            {
                Id = room.Id,
                Type = 4,
                StartDate = startDate,
                EndDate = endDate
            };
            List<Period> timetable = await MakeRequestAsync<TimetableRequestModel, List<Period>>(id, "getTimetable", requestModel, ct);
            return timetable.ToArray();
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
            // Check for defaul time
            if (startDate == default)
                startDate = DateTime.Now;

            if (endDate == default)
                endDate = DateTime.Now;

            TimetableRequestModel requestModel = new TimetableRequestModel()
            {
                Id = student.Id,
                Type = 5,
                StartDate = startDate,
                EndDate = endDate
            };
            List<Period> timetable = await MakeRequestAsync<TimetableRequestModel, List<Period>>(id, "getTimetable", requestModel, ct);
            return timetable.ToArray();
        }
        #endregion
    }
}
