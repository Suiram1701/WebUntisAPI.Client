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
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            // Make request
            JSONRPCRequestModel<object> requestModel = new JSONRPCRequestModel<object>()
            {
                Id = id,
                Method = "getStudents",
                Params = new object()
            };
            StringContent requestContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

            // Send request
            HttpResponseMessage response = await _client.PostAsync(ServerUrl + "/WebUntis/jsonrpc.do", requestContent, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return null;

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");
 
            JSONRPCResponeModel<List<Student>> responseModel = JsonConvert.DeserializeObject<JSONRPCResponeModel<List<Student>>>(await response.Content.ReadAsStringAsync());

            // Check for WebUntis error
            if (responseModel.Error != null)
                throw responseModel.Error;

            return responseModel.Result.ToArray();
        }

        /// <summary>
        /// Get all teachers on the school
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>An array of all teachers</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when you don't logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when there was an error while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task<Teacher[]> GetAllTeachersAsync(string id = "getTeachers", CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            // Make request
            JSONRPCRequestModel<object> requestModel = new JSONRPCRequestModel<object>()
            {
                Id = id,
                Method = "getTeachers",
                Params = new object()
            };
            StringContent requestContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

            // Send request
            HttpResponseMessage response = await _client.PostAsync(ServerUrl + "/WebUntis/jsonrpc.do", requestContent, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return null;

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            JSONRPCResponeModel<List<Teacher>> responseModel = JsonConvert.DeserializeObject<JSONRPCResponeModel<List<Teacher>>>(await response.Content.ReadAsStringAsync());

            // Check for WebUntis error
            if (responseModel.Error != null)
                throw responseModel.Error;

            return responseModel.Result.ToArray();
        }
    }
}