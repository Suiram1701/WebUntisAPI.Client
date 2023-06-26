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
        /// The type of the user as that you currently logged in (Student or teacher)
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> when you're not logged in
        /// </remarks>
        public UserType? UserType => _userType;
        private UserType? _userType = null;

        /// <summary>
        /// The user as that you currently logged in
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> when you're not logged in
        /// </remarks>
        public IUser User => _user;
        private IUser _user = null;

        /// <summary>
        /// Get all students on the school
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <param name="id">Identifier of the request</param>
        /// <returns>An array of all students on the school</returns>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you don't logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when there was an error while the http request</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task<Student[]> GetStudentsAsync(string id = "getStudents", CancellationToken ct = default)
        {
            List<Student> students = await MakeJSONRPCRequestAsync<object, List<Student>>(id, "getStudents", new object(), ct);
            return students.ToArray();
        }

        /// <summary>
        /// Get all teachers on the school
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>An array of all teachers</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when there was an error while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task<Teacher[]> GetTeachersAsync(string id = "getTeachers", CancellationToken ct = default)
        {
            List<Teacher> teachers = await MakeJSONRPCRequestAsync<object, List<Teacher>>(id, "getTeachers", new object(), ct);
            return teachers.ToArray();
        }

        /// <summary>
        /// Search for the id of a user where you know fore- and surname
        /// </summary>
        /// <param name="forename">Forename of the user (It doesn't matter if it is upper or lower case)</param>
        /// <param name="surname">Surname of the user (It doesn't matter if it is upper or lower case)</param>
        /// <param name="type">Type of the user</param>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The <see cref="IUser.Id"/> of the requested user. 0 if the user isn't found</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when there was an error while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task<int> GetPersonIdAsync(string forename, string surname, UserType type, string id = "getPersonId", CancellationToken ct = default)
        {
            GetPersonIdRequestModel model = new GetPersonIdRequestModel()
            {
                Forename = forename,
                Surname = surname,
                UserType = (int)type
            };
            return await MakeJSONRPCRequestAsync<GetPersonIdRequestModel, int>(id, "getPersonId", model, ct);
        }
    }
}