using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages
{
    /// <summary>
    /// An attachment from a message
    /// </summary>
    [DebuggerDisplay("{Name, nq}")]
    public struct Attachment
    {
        /// <summary>
        /// The file name of the attachment
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The attachment id
        /// </summary>
        [JsonProperty("id")]
        internal readonly string _id;

        /// <summary>
        /// Get the content of the attachment as stream
        /// </summary>
        /// <param name="client">The client with them you got this instane (It doesn't have to be the same client but the same account)</param>
        /// <param name="timeout">The time after that the download of the content will cancelled (In miliseconds)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The attachment as stream</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<MemoryStream> DownloadContentAsStreamAsync(WebUntisClient client, int timeout = 2000, CancellationToken ct = default)
        {
            string storageResponseString = await client.MakeAPIGetRequestAsync($"/WebUntis/api/rest/view/v1/messages/{_id}/attachmentstorageurl", ct);

            JObject data = JObject.Parse(storageResponseString);
            JArray headerArray = data.Value<JArray>("additionalHeaders");

            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(data.Value<string>("downloadUrl"))
            };

            // Auth headers
            foreach (JObject jsonHeader in headerArray.Cast<JObject>())
                request.Headers.Add(jsonHeader.Value<string>("key"), jsonHeader.Value<string>("value"));

            // Date header
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            string timeStr = DateTime.Now.ToString("HHmmss");
            request.Headers.Add("x-amz-date", $"{dateStr}T{timeStr}Z");

            using (HttpClient downloadClient = new HttpClient())
            {
                downloadClient.Timeout = TimeSpan.FromMilliseconds(timeout);
                HttpResponseMessage response = await downloadClient.SendAsync(request, ct);

                // Check cancellation token
                if (ct.IsCancellationRequested)
                    return default;

                // Verify response
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    string detail = Regex.Match(await response.Content.ReadAsStringAsync(), @"<Message>([a-zA-z0-9\s]+)</Message>").Groups[1].Value;     // Get the error message
                    throw new UnauthorizedAccessException($"Invalid authentication. Detail: {detail}");
                }

                return (MemoryStream)await response.Content.ReadAsStreamAsync();
            }
        }
    }
}