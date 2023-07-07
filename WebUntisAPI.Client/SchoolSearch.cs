using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;

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
        /// <param name="id">Identifier for the request</param>
        /// <returns>All schools found, an empty array when no school found or <see langword="null"/> when too many schools found</returns>
        /// <exception cref="WebUntisException">Throws when the WebUntis API returned an error</exception>
        /// <exception cref="HttpRequestException">Throws when an error happend while request</exception>
        public static async Task<School[]> SearchAsync(string name, string id = "getStudents", CancellationToken ct = default)
        {
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(id);

                writer.WritePropertyName("method");
                writer.WriteValue("searchSchool");

                writer.WritePropertyName("params");
                writer.WriteStartArray();
                writer.WriteStartObject();
                writer.WritePropertyName("search");
                writer.WriteValue(name);
                writer.WriteEndObject();
                writer.WriteEndArray();

                writer.WritePropertyName("jsonrpc");
                writer.WriteValue("2.0");

                writer.WriteEndObject();
            }

            StringContent requestContent = new StringContent(sw.ToString(), Encoding.UTF8, "application/json");

            // Send request
            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
                response = await client.PostAsync(s_API_Url, requestContent, ct);

            if (ct.IsCancellationRequested)
                return Array.Empty<School>();

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"The request had an error (Code: {response.StatusCode}).");

            JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());

            // Check for WebUntis error
            if (responseObject["error"]?.ToObject<WebUntisException>() is WebUntisException error)
            {
                if (error.Code == (int)WebUntisException.Codes.TooManyResults)
                    return null;

                throw error;
            }

            return responseObject["result"]["schools"].ToObject<School[]>();
        }
    }
}
