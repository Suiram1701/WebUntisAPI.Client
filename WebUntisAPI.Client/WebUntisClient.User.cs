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
using WebUntisAPI.Client.Exceptions;

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
        public async Task<Student[]> GetAllStudentsAsync(string id = "getStudents", CancellationToken ct = default)
        {
            List<Student> students = await MakeRequestAsync<object, List<Student>>(id, "getStudents", new object(), ct);
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
        public async Task<Teacher[]> GetAllTeachersAsync(string id = "getTeachers", CancellationToken ct = default)
        {
            List<Teacher> teachers = await MakeRequestAsync<object, List<Teacher>>(id, "getTeachers", new object(), ct);
            return teachers.ToArray();
        }
    }
}