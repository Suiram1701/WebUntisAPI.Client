using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        /// Get the content of the attachment as stream with a progress report
        /// </summary>
        /// <param name="client">The client with them you got this instane (It doesn't have to be the same client but the same account)</param>
        /// <param name="timeout">The time after that the download of the content will cancelled (In miliseconds)</param>
        /// <param name="progress">The progress of the download in percent</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The attachment as stream</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<Stream> DownloadContentAsStreamAsync(WebUntisClient client, TimeSpan timeout, IProgress<double> progress = null, CancellationToken ct = default)
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
                downloadClient.Timeout = timeout;
                HttpResponseMessage response = await downloadClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

                using (MemoryStream content = new MemoryStream())
                {
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        int totalBytes = (int?)response.Content.Headers.ContentLength ?? -1;
                        int totalRecievedBytes = 0;

                        int bytesRead = 0;
                        byte[] buffer = new byte[8192];
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, ct)) > 0)
                        {
                            if (ct.IsCancellationRequested)
                                return default;

                            content.Write(buffer, 0, bytesRead);
                            totalRecievedBytes += bytesRead;
                            progress.Report((double)totalRecievedBytes / totalBytes * 100d);
                        }
                    }

                    if (ct.IsCancellationRequested)
                        return default;

                    // Verify response
                    if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        string detail = Regex.Match(Encoding.UTF8.GetString(content.ToArray()), @"<Message>([a-zA-z0-9\s]+)</Message>").Groups[1].Value;     // Get the error message
                        throw new UnauthorizedAccessException($"Invalid authentication. Detail: {detail}");
                    }

                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

                    return content;
                }
            }
        }
    }
}