using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client
{
    public partial class WebUntisClient
    {
        /// <summary>
        /// Get lesson types, periods and their colors
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Get status data</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<StatusData> GetStatusDataAsync(string id = "getStatusData", CancellationToken ct = default)
        {
            StatusData statusData = await MakeRequestAsync<object, StatusData>(id, "getStatusData", new object(), ct);
            return statusData;
        }

        /// <summary>
        /// Get the timegrid for the school
        /// </summary>
        /// <param name="id">Identier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The timegrid for all days</returns>
        /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<Timegrid> GetTimegridAsync(string id = "getTimegrid", CancellationToken ct = default)
        {
            Timegrid timeGrid = await MakeRequestAsync<object, Timegrid>(id, "getTimegridUnits", new object(), ct);
            return timeGrid;
        }
    }
}
