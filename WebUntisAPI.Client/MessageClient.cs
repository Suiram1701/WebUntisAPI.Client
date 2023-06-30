using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client
{
    /// <summary>
    /// A client with them you could load and send untis messages
    /// </summary>
    /// <remarks>
    /// Under no circumstances should 10 req. per sec., more than 1800req. per hr (but in no case more than 3600 req. per hr). If the specifications are exceeded, access to WebUntis could permanently blocked by the WebUntis API.
    /// </remarks>
    public class MessageClient
    {
        private readonly WebUntisClient _client;

        /// <summary>
        /// Create a new message client to access the untis messages
        /// </summary>
        /// <param name="client">The client from which account they want to receive the messages</param>
        public MessageClient(WebUntisClient client)
        {
            _client = client;
        }

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
            string responseString = await _client.MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages/status", ct);
            return JObject.Parse(responseString).Value<int>("unreadMessagesCount");
        }

        /// <summary>
        /// Get the permissions you have to send messages
        /// </summary>
        /// <param name="ct">Cancllation token</param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<MessagePermissions> GetMessagePermissionsAsync(CancellationToken ct = default)
        {
            string responseString = await _client.MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages/permissions", ct);
            return JsonConvert.DeserializeObject<MessagePermissions>(responseString);
        }

        /// <summary>
        /// Get the you message inbox
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The message previews</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<MessagePreview[]> GetMessageInboxAsync(CancellationToken ct = default)
        {
            string responseString = await _client.MakeAPIGetRequestAsync("/WebUntis/api/rest/view/v1/messages", ct);

            JArray jsonMsg = JObject.Parse(responseString).Value<JArray>("incomingMessages");
            return new JsonSerializer().Deserialize<List<MessagePreview>>(jsonMsg.CreateReader()).ToArray();
        }
    }
}