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
        /// Get all students on the school
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <param name="id">Identifier of the request</param>
        /// <returns>An array of all students on the school</returns>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you don't logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when there was an error while the http request</exception>
        public async Task<Student[]> GetAllStudentsAsync(CancellationToken ct, string id = "getStudents")
        {
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
    }

}