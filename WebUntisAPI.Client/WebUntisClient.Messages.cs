using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebUntisAPI.Client
{
    public partial class WebUntisClient
    {
        /// <summary>
        /// Get the count of unread messages
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The count of unread messages</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<int> GetUnreadMessagesCountAsync(CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                 throw new UnauthorizedAccessException("You're not logged in");

            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ServerUrl + "/WebUntis/api/rest/view/v1/messages/status")
            };
            SetRequestHeader(request.Headers, true);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return default;

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            return JObject.Parse(await response.Content.ReadAsStringAsync()).Value<int>("unreadMessagesCount");
        }
    }
}
