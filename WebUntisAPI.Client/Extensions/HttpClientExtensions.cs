using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Extensions;

/// <summary>
/// Provides extensions for <see cref="HttpClient"/>
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Sends a GET request at <paramref name="uri"/> and download the content async in <paramref name="stream"/> while the progress were reportet to <paramref name="progress"/> in percent.
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="uri">The source uri</param>
    /// <param name="stream">The target stream</param>
    /// <param name="progress">The instance the progress is reportet to</param>
    /// <param name="writeOnFailure">Indicates whether the response should be written to the <paramref name="stream"/> also when the response isn't a 2xx code</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The response of the server. <see cref="HttpResponseMessage.Content"/> won't be available because the content were already loaded into the stream.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="TaskCanceledException"></exception>
    public static async Task<HttpResponseMessage> GetWithProgressAsync(this HttpClient client, Uri uri, Stream stream, IProgress<double>? progress, bool writeOnFailure = false, CancellationToken ct = default)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        return await client.SendWithProgressReportAsync(request, stream, progress, writeOnFailure, ct);
    }

    /// <summary>
    /// Sends <paramref name="request"/> and download the content async in <paramref name="stream"/> while the progress were reportet to <paramref name="progress"/> in percent.
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="request">The request to send</param>
    /// <param name="stream">The target stream</param>
    /// <param name="progress">The instance the progress is reportet to</param>
    /// <param name="writeOnFailure">Indicates whether the response should be written to the <paramref name="stream"/> also when the response isn't a 2xx code</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The response of the server. <see cref="HttpResponseMessage.Content"/> won't be available because the content were already loaded into the stream.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="TaskCanceledException"></exception>
    public static async Task<HttpResponseMessage> SendWithProgressReportAsync(this HttpClient client, HttpRequestMessage request, Stream stream, IProgress<double>? progress, bool writeOnFailure = false, CancellationToken ct = default)
    {
        if (!stream.CanWrite)
            throw new InvalidOperationException("The stream have to be writable.");

        HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
        if (!response.IsSuccessStatusCode && !writeOnFailure)
            return response;

        long totalBytes = response.Content.Headers.ContentLength ?? -1L;
        long totalReceivedBytes = 0L;

        Stream responseStream = await response.Content.ReadAsStreamAsync(ct);
        Memory<byte> buffer = new byte[4096];

        int bytesRead;
        while ((bytesRead = await responseStream.ReadAsync(buffer, ct)) > 0)
        {
            await stream.WriteAsync(buffer[..bytesRead], ct);

            totalReceivedBytes += bytesRead;
            progress?.Report((double)totalReceivedBytes / totalBytes * 100d);
        }

        return response;
    }
}
