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

namespace WebUntisAPI.Client;

partial class WebUntisClient
{
    /// <summary>
    /// Get the timegrid for the school for the current school year
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The timegrid</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<Timegrid> GetTimegridAsync(CancellationToken ct = default)
    {
        string response = await InternalApiRequestAsync("/WebUntis/api/public/timegrid", ct);
        return JObject.Parse(response)["data"]!.ToObject<Timegrid>()!;
    }

    /// <summary>
    /// Get the timegrid for the school for the specified school year
    /// </summary>
    /// <param name="year">The school year</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The timegrid for all days</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<Timegrid> GetTimegridAsync(SchoolYear year, CancellationToken ct = default)
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

        return JObject.Parse(response)["data"]!.ToObject<Timegrid>()!;
    }

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
    public async Task<Timetable> GetTimetableAsync(IElement element, DateOnly week, CancellationToken ct = default)
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

        return JObject.Parse(responseString)["data"]!["result"]!.ToObject<Timetable>()!;
    }
}