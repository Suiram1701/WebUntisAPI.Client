using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;
using System.Net;
using Newtonsoft.Json;

namespace WebUntisAPI.Client
{
    /// <summary>
    /// A help class to search for schools by the name
    /// </summary>
    public static class SchoolSearch
    {
        /// <summary>
        /// The Url for the school search API
        /// </summary>
        private static readonly Uri s_API_Url = new Uri("https://mobile.webuntis.com/ms/schoolquery2");

        /// <summary>
        /// Search for schools by the given name
        /// </summary>
        /// <param name="name">Name to search</param>
        /// <param name="ct">Token to cancel the search request</param>
        /// <param name="id">Name to identifies the request</param>
        /// <returns>All schools found, an empty array when no school found or <see langword="null"/> when too many schools found</returns>
        /// <exception cref="WebUntisException">Throws when the WebUntis API returned an error</exception>
        /// <exception cref="HttpRequestException">Throws when an error happend while request</exception>
        public static async Task<School[]> SearchAsync(string name, string id = "getStudents", CancellationToken ct = default)
        {
            // Write request content
            JSONRPCRequestModel<SchoolSearchModel[]> requestModel = new JSONRPCRequestModel<SchoolSearchModel[]>()
            {
                Id = id,
                Method = "searchSchool",
                Params = new SchoolSearchModel[]
                {
                    new SchoolSearchModel()
                    {
                        Search = name.ToLower()
                    }
                }
            };

            StringContent requestContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

            // Send request
            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
                response = await client.PostAsync(s_API_Url, requestContent, ct);

            JSONRPCResponeModel<SchoolSearchResultModel> responeModel = JsonConvert.DeserializeObject<JSONRPCResponeModel<SchoolSearchResultModel>>(await response.Content.ReadAsStringAsync());

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"The request had an error (Code: {response.StatusCode}).");

            // Check for WebUntis error
            if (responeModel.Error != null)
            {
                if (responeModel.Error.Code == (int)WebUntisException.Codes.TooManyResults)
                    return null;

                throw responeModel.Error;
            }

            // Evaluate response
            return responeModel.Result.Schools;
        }
    }
}
