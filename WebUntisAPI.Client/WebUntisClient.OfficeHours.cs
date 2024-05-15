using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Extensions;
using WebUntisAPI.Client.Models.OfficeHours;

namespace WebUntisAPI.Client;

partial class WebUntisClient
{
    /// <summary>
    /// Gets the settings configured for the office hours view.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The settings</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<OfficeHoursSettings> GetOfficeHoursSettingsAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/public/officehours/settings", ct);
        return JObject.Parse(responseString)["data"]!.ToObject<OfficeHoursSettings>()!;
    }

    /// <summary>
    /// Get the available office hour classes.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The available classes.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<OfficeHourClass>> GetOfficeHourClassesAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/public/officehours/classes", ct);
        return JObject.Parse(responseString)["data"]!.ToObject<IEnumerable<OfficeHourClass>>()!;
    }

    /// <summary>
    /// Get todays office hours.
    /// </summary>
    /// <param name="class">Filters the office hour on the basis of the assigned class. When <c>null</c> the filter won't be applied.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The office hours</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<OfficeHour>> GetOfficeHoursAsync(OfficeHourClass? @class = null, CancellationToken ct = default)
    {
        return await GetOfficeHoursAsync(DateOnly.FromDateTime(DateTime.Now), @class, ct);
    }

    /// <summary>
    /// Get the office hours for the specified date.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="class">Filters the office hour on the basis of the assigned class. When <c>null</c> the filter won't be applied.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The office hours</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<OfficeHour>> GetOfficeHoursAsync(DateOnly date, OfficeHourClass? @class = null, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        Uri requestUri = new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/public/officehours/hours",
            Query = $"date={date:yyyyMMdd}&klasseId={@class?.Id ?? -1}"
        }.Uri;
        string responseString = await InternalApiRequestAsync(requestUri, ct);

        return JObject.Parse(responseString)["data"]!.ToObject<IEnumerable<OfficeHour>>()!;
    }

    /// <summary>
    /// Get the displayed photo of the office hour.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     I suspect this isn't a separate picture, just the assigned teacher's profile picture.
    /// </para>
    /// <para>
    ///     The in the stream written image data will be in one of these formats: .tiff, .jfif, .bmp, .gif, .svg, .png, .webp, .svgz, .jpg, .jpeg, .ico, .xbm, .dib, .pjp, .apng, .tif, .pjpeg or .avif
    /// </para>
    /// </remarks>
    /// <param name="hour">The hour of the photo.</param>
    /// <param name="settings">The settings of this user (These data are required to determine the endpoint of the request).</param>
    /// <param name="stream">The stream to write to</param>
    /// <param name="progress">Provides a functionality to report the download progress of the image</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The Mime type of the returned image. <c>null</c> indicates that the there isn't a image.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<MediaTypeHeaderValue?> GetOfficeHourPhotoAsync(OfficeHour hour, OfficeHoursSettings settings, Stream stream, IProgress<double>? progress = null, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(hour, nameof(hour));
        if (!stream.CanWrite)
        {
            throw new InvalidOperationException("The stream have to be writable.");
        }

        if (hour.PhotoId == -1 || settings.ShowPhoto)
        {
            return null;
        }

        Uri requestUri = new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = settings.Anonymous
                ? "/WebUntis/pimage.do"
                : "/WebUntis/image.do",
            Query = $"cat={2}&id={hour.PhotoId}"
        }.Uri;
        using HttpResponseMessage response = await _client.GetWithProgressAsync(requestUri, stream, progress, ct: ct);
        response.EnsureSuccessStatusCode();

        return response.Content.Headers.ContentType;
    }

    /// <summary>
    /// Get the single time slots of an office hour.
    /// </summary>
    /// <param name="hour">The office hour</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The time slots</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<OfficeHourTimeSlot>> GetOfficeHourTimeSlotsAsync(OfficeHour hour, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(hour, nameof(hour));

        Uri requestUri = new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/public/officehours/registrationdata",
            Query = $"periodId={hour.Id}&teacherId={hour.TeacherId}"
        }.Uri;
        string responseString = await InternalApiRequestAsync(requestUri, ct);

        return JObject.Parse(responseString)["data"]!["timeSlots"]!.ToObject<IEnumerable<OfficeHourTimeSlot>>()!;
    }

    /// <summary>
    /// Signs the user up to this specified time slot of the office hour.
    /// </summary>
    /// <param name="hour">The office hour of the time slot.</param>
    /// <param name="timeSlot">The time slot to sign up.</param>
    /// <param name="userText">An additional text that is supplied to the sign in.</param>
    /// <param name="ct">Cancellation token</param>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task SignUpToTimeSlotAsync(OfficeHour hour, OfficeHourTimeSlot timeSlot, string? userText, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(hour, nameof(hour));
        if (userText?.Length > 255)
        {
            throw new ArgumentException("The parameter can be a maximum of 255 characters long.", nameof(userText));
        }

        string jsonContent = new JObject
        {
            new JProperty("periodId", hour.Id),
            new JProperty("teacherId", hour.TeacherId),
            new JProperty("date", hour.Date.ToString("yyyy-MM-dd")),
            new JProperty("startDate", timeSlot.StartTime.ToString("hh:mm")),
            new JProperty("endTime", timeSlot.EndTime.ToString("hh:mm")),
            new JProperty("userText", userText ?? string.Empty)
        }.ToString();

        using HttpRequestMessage request = new(HttpMethod.Post, new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/public/officehours/registrations"
        }.Uri)
        {
            Content = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json)
        };
        await InternalApiRequestAsync(request, ct);
    }

    /// <summary>
    /// Signs the user off from the specified time slot of the office hour.
    /// </summary>
    /// <param name="hour">The office hour to sign off from.</param>
    /// <param name="userText">An additional text that is supplied to the sign off.</param>
    /// <param name="ct">Cancellation token</param>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task SignOffFromTimeSlotAsync(OfficeHour hour, string? userText, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(hour, nameof(hour));
        if (userText?.Length > 255)
        {
            throw new ArgumentException("The parameter can be a maximum of 255 characters long.", nameof(userText));
        }

        using HttpRequestMessage request = new(HttpMethod.Delete, new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/public/officehours/registrations",
            Query = $"periodId={hour.Id}&teacherId={hour.TeacherId}&userText={userText ?? string.Empty}"
        }.Uri);
        await InternalApiRequestAsync(request, ct);
    }
}
