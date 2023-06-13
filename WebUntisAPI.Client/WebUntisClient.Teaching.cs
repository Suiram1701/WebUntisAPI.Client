using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;
using WebUntisAPI.Client.Converter;
using WebUntisAPI.Client.Exceptions;

namespace WebUntisAPI.Client
{
    public partial class WebUntisClient
    {
        /// <summary>
        /// Get all subjects on the school
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>All subjects</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Subject[]> GetAllSubjectsAsync(string id = "getSubjects", CancellationToken ct = default)
        {
            List<Subject> subjects = await MakeRequestAsync<object, List<Subject>>(id, "getSubjects", new object(), ct);
            return subjects.ToArray();
        }

        /// <summary>
        /// Get all classes on the school from the current school year
        /// </summary>
        /// <param name="id">Identifier of the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>All classes on the school from current school year</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the hppt request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Class[]> GetAllClassesAsync(string id = "getClasses", CancellationToken ct = default)
        {
            List<Class> classes = await MakeRequestAsync<object, List<Class>>(id, "getKlassen", new object(), ct);
            return classes.ToArray();
        }

        /// <summary>
        /// Get all classes on the school from the selected school year
        /// </summary>
        /// <param name="id">Identifier of the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="schoolYear">The school year from the classes</param>
        /// <returns>All classes on the school for the school year</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the hppt request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Class[]> GetClassesAsync(SchoolYear schoolYear, string id = "getClassesBySchoolYear", CancellationToken ct = default)
        {
            List<Class> classes = await MakeRequestAsync<SchoolYearModel, List<Class>>(id, "getKlassen", new SchoolYearModel() { Id = schoolYear.Id }, ct);
            return classes.ToArray();
        }

        /// <summary>
        /// Get all rooms on the school
        /// </summary>
        /// <param name="id">Identifier of the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>All rooms on the school</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Room[]> GetAllRoomsAsync(string id = "getRooms", CancellationToken ct = default)
        {
            List<Room> rooms = await MakeRequestAsync<object, List<Room>>(id, "getRooms", new object(), ct);
            return rooms.ToArray();
        }
    }
}