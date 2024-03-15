using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        /// Get all subjects on the school
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>All subjects</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Subject[]> GetSubjectsAsync(string id = "getSubjects", CancellationToken ct = default)
        {
            List<Subject> subjects = (await MakeJSONRPCRequestAsync(id, "getSubjects", null, ct)).ToObject<List<Subject>>();
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
        public async Task<Class[]> GetClassesAsync(string id = "getClasses", CancellationToken ct = default)
        {
            List<Class> classes = (await MakeJSONRPCRequestAsync(id, "getKlassen", null, ct)).ToObject<List<Class>>();
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
            Action<JsonWriter> paramsAction = new Action<JsonWriter>(writer =>
            {
                writer.WriteStartObject();

                writer.WritePropertyName("schoolyearId");
                writer.WriteValue(schoolYear.Id);

                writer.WriteEndObject();
            });
            List<Class> classes = (await MakeJSONRPCRequestAsync(id, "getKlassen", paramsAction, ct)).ToObject<List<Class>>();
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
        public async Task<Room[]> GetRoomsAsync(string id = "getRooms", CancellationToken ct = default)
        {
            List<Room> rooms = (await MakeJSONRPCRequestAsync(id, "getRooms", null, ct)).ToObject<List<Room>>();
            return rooms.ToArray();
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        /// <param name="id">Identifier for request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>All departments</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Department[]> GetDepartmentsAsync(string id = "getDepartments", CancellationToken ct = default)
        {
            List<Department> departments = (await MakeJSONRPCRequestAsync(id, "getDepartments", null, ct)).ToObject<List<Department>>();
            return departments.ToArray();
        }

        /// <summary>
        /// Get the count of unread news
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The unread news count</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<int> GetUnreadNewsCountAsync(CancellationToken ct = default)
        {
            string responseString = await DoAPIRequestAsync("/WebUntis/api/rest/view/v1/dashboard/cards/status", ct);
            return JObject.Parse(responseString).Value<int>("unreadCardsCount");
        }

        /// <summary>
        /// Get all the news for the school as string
        /// </summary>
        /// <param name="date">Date to get the news</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The new at the school for the requested day</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        public async Task<News> GetNewsFeedAsync(DateTime date, CancellationToken ct = default)
        {
            //date.ToWebUntisTimeFormat(out string dateString, out _);
            //string responseString = await DoAPIRequestAsync("/WebUntis/api/public/news/newsWidgetData?date=" + dateString, ct);

            //JToken data = JObject.Parse(responseString).GetValue("data");
            //return data.ToObject<News>();
            throw new NotImplementedException();
        }
    }
}