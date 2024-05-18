using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Extensions;
using WebUntisAPI.Client.Models;
using WebUntisAPI.Client.Models.Interfaces;
using WebUntisAPI.Client.Models.NewTimetable;

namespace WebUntisAPI.Client;

partial class WebUntisClient
{
    /// <summary>
    /// Get all available school years
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>All school years</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<SchoolYear>> GetSchoolYearsAsync(CancellationToken ct = default)
    {
        string response = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/schoolyears", ct);
        return JsonConvert.DeserializeObject<IEnumerable<SchoolYear>>(response)!;
    }

    /// <summary>
    /// Get the currently active school year
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The school year. When <c>null</c> there is no active school year</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<SchoolYear?> GetCurrentSchoolYearAsync(CancellationToken ct = default)
    {
        string response = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/app/data", ct);
        return JObject.Parse(response)["currentSchoolYear"]?.ToObject<SchoolYear>();
    }

    /// <summary>
    /// Get all holidays
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>All holidays</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<Holiday>> GetHolidaysAsync(CancellationToken ct = default)
    {
        string response = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/app/data", ct);
        return JObject.Parse(response)["holidays"]!.ToObject<IEnumerable<Holiday>>()!;
    }

    /// <summary>
    /// Get the time grid for the school for the current school year
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The time grid</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<Models.Timetable.TimeGrid> GetTimeGridAsync(CancellationToken ct = default)
    {
        string response = await InternalApiRequestAsync("/WebUntis/api/public/timegrid", ct);
        return JObject.Parse(response)["data"]!.ToObject<Models.Timetable.TimeGrid>()!;
    }

    /// <summary>
    /// Get the time grid for the school for the specified school year
    /// </summary>
    /// <param name="year">The school year</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The time grid for all days</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<Models.Timetable.TimeGrid> GetTimeGridAsync(SchoolYear year, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        UriBuilder uriBuilder = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/public/timegrid",
            Query = "schoolyearId=" + year.Id
        };
        string response = await InternalApiRequestAsync(uriBuilder.Uri, ct);

        return JObject.Parse(response)["data"]!.ToObject<Models.Timetable.TimeGrid>()!;
    }

    /// <summary>
    /// Get the timetable for an element
    /// </summary>
    /// <param name="element">The element of the timetable to get</param>
    /// <param name="week">The first day of the week to get the timetable</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The timetable for the element</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<Models.Timetable.Timetable> GetTimetableAsync(IElement element, DateOnly week, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(element, nameof(element));

        if (!element.CanViewTimetable)
            throw new InvalidOperationException($"The current session isn't allowed to view the timetable of {element.Name}");

        UriBuilder uriBuilder = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/public/timetable/weekly/data",
            Query = $"elementType={(int)element.GetElementType()}&elementId={element.Id}&date={week:yyyy-MM-dd}"
        };
        string responseString = await InternalApiRequestAsync(uriBuilder.Uri, ct);

        return JObject.Parse(responseString)["data"]!["result"]!.ToObject<Models.Timetable.Timetable>()!;
    }

    /// <summary>
    /// Get the time grid for the school for the current school year.
    /// </summary>
    /// <remarks>
    /// At development time this is on the website marked as BETA feature. When data you received from this endpoint are incorrect or unexpected exceptions were thrown please create an issue on the GitHub page of this package.
    /// </remarks>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The time grid</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<TimeGrid> GetNewTimeGridAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/timetable/grid", ct);
        return JsonConvert.DeserializeObject<Models.NewTimetable.TimeGrid>(responseString)!;
    }

    /// <summary>
    /// Get the settings how the timetable should be displayed.
    /// </summary>
    /// <remarks>
    /// At development time this is on the website marked as BETA feature. When data you received from this endpoint are incorrect or unexpected exceptions were thrown please create an issue on the GitHub page of this package.
    /// </remarks>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The settings</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<TimetableSettings> GetNewTimetableSettingsAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/timetable/entries/settings", ct);
        return JsonConvert.DeserializeObject<TimetableSettings>(responseString)!;
    }
}