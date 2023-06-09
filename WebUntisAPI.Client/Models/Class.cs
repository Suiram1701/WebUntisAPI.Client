using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;
using WebUntisAPI.Client.Exceptions;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A class
    /// </summary>
    [DebuggerDisplay("Name = {LongName}")]
    [JsonConverter(typeof(ClassJsonConverter))]
    public struct Class
    {
        /// <summary>
        /// Id of the class
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Untis internal name of the class
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the class
        /// </summary>
        public string LongName { get; set; }

        /// <summary>
        /// Is the class active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// An array of all teachers of the class
        /// </summary>
        public int[] Teachers { get; set; }

        /// <summary>
        /// Get all taechers of this class
        /// </summary>
        /// <param name="client">The client for the request (This should be the same client with that you requested the class)</param>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The teachers of the class</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the client aren't not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when there was an error while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Teacher[]> GetTeachersAsync(WebUntisClient client, string id = "getClassTeachers", CancellationToken ct = default)
        {
            int[] classTeachers = Teachers;
            return (await client.GetAllTeachersAsync(id, ct)).Where(t => classTeachers.Contains(t.Id)).ToArray();
        }
    }
}