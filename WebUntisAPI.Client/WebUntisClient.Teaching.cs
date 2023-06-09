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
        /// Get all classes on the school
        /// </summary>
        /// <param name="id">Identifier of the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>All classes on the school</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the hppt request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Class[]> GetAllClassesAsync(string id = "getClasses", CancellationToken ct = default)
        {
            //TODO: School year overload

            List<Class> classes = await MakeRequestAsync<object, List<Class>>(id, "getKlassen", new object(), ct);
            return classes.ToArray();
        }

            // Make request
            JSONRPCRequestModel<object> requestModel = new JSONRPCRequestModel<object>()
            {
                Id = id,
                Method = "getKlassen",
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

            JSONRPCResponeModel<List<Class>> responseModel = JsonConvert.DeserializeObject<JSONRPCResponeModel<List<Class>>>(await response.Content.ReadAsStringAsync());

            // Check for WebUntis error
            if (responseModel.Error != null)
                throw responseModel.Error;

            return responseModel.Result.ToArray();
        }
    }

}