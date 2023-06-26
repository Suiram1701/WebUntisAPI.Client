﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
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
            List<Subject> subjects = await MakeJSONRPCRequestAsync<object, List<Subject>>(id, "getSubjects", new object(), ct);
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
            List<Class> classes = await MakeJSONRPCRequestAsync<object, List<Class>>(id, "getKlassen", new object(), ct);
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
            List<Class> classes = await MakeJSONRPCRequestAsync<SchoolYearModel, List<Class>>(id, "getKlassen", new SchoolYearModel() { Id = schoolYear.Id }, ct);
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
            List<Room> rooms = await MakeJSONRPCRequestAsync<object, List<Room>>(id, "getRooms", new object(), ct);
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
            List<Department> departments = await MakeJSONRPCRequestAsync<object, List<Department>>(id, "getDepartments", new object(), ct);
            return departments.ToArray();
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
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            date.ToWebUntisTimeFormat(out string dateString, out _);

            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ServerUrl + "/WebUntis/api/public/news/newsWidgetData?date=" + dateString)
            };
            SetRequestHeaders(request.Headers);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return default;

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            JToken data = JObject.Parse(await response.Content.ReadAsStringAsync()).GetValue("data");
            return data.ToObject<News>();
        }
    }
}