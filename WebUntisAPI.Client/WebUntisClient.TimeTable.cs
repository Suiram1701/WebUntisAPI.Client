using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;
using WebUntisAPI.Client.Models.Elements;

namespace WebUntisAPI.Client;

public partial class WebUntisClient
{
    /// <summary>
    /// Get the timegrid for the school for the current school year
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The timegrid</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<Timegrid> GetTimegridAsync(CancellationToken ct = default)
    {
        string response = await InternalAPIRequestAsync("/WebUntis/api/public/timegrid", ct);

        JObject obj = JObject.Parse(response);
        return obj["data"]!.ToObject<Timegrid>()!;
    }

    /// <summary>
    /// Get the timegrid for the school for the specified school year
    /// </summary>
    /// <param name="year">The school year</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The timegrid for all days</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
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
        string response = await InternalAPIRequestAsync(uriBuilder.ToString(), ct);

        JObject obj = JObject.Parse(response);
        return obj["data"]!.ToObject<Timegrid>()!;
    }

    /// <summary>
    /// Get all available school years
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>All school years</returns>
    /// <exception cref="ObjectDisposedException">Thrown when thew instance was disposed</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when you're not logged in</exception>
    /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
    /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
    public async Task<IEnumerable<SchoolYear>> GetSchoolYearsAsync(CancellationToken ct = default)
    {
        string response = await InternalAPIRequestAsync("/WebUntis/api/rest/view/v1/schoolyears", ct);

        JArray obj = JArray.Parse(response);
        return obj.ToObject<IEnumerable<SchoolYear>>()!;
    }

    /// <summary>
    /// Get the currently active school year
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The school year. When <c>null</c> there is no active school year</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<SchoolYear?> GetCurrentSchoolYearAsync(CancellationToken ct = default)
    {
        string response = await InternalAPIRequestAsync("/WebUntis/api/rest/view/v1/app/data", ct);

        JObject responseObj = JObject.Parse(response);
        return responseObj["currentSchoolYear"]?.ToObject<SchoolYear>();
    }

    /// <summary>
    /// Get all holidays
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>All holidays</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<IEnumerable<Holiday>> GetHolidaysAsync(CancellationToken ct = default)
    {
        string response = await InternalAPIRequestAsync("/WebUntis/api/rest/view/v1/app/data", ct);

        JObject responseObj = JObject.Parse(response);
        return responseObj["holidays"]!.ToObject<IEnumerable<Holiday>>()!;
    }

    /// <summary>
    /// Get the timetable for an element
    /// </summary>
    /// <param name="element">The element of the timetable to get</param>
    /// <param name="week">The first day of the week to get the timetable</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The periods for the class</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    public async Task<Timetable> GetTimetableAsync(ElementBase element, DateOnly week, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        if (!element.CanViewTimetable)
            throw new UnauthorizedAccessException($"The current session isn't allowed to view the timetable of {element.Name}");

        ElementType type = element.GetElementType();
        UriBuilder uriBuilder = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/public/timetable/weekly/data",
            Query = $"elementType={(int)type}&elementId={element.Id}&date={week:yyyy-MM-dd}"
        };

        string responseString = await InternalAPIRequestAsync(uriBuilder.ToString(), ct);
        JToken responseObj = JObject.Parse(responseString)["data"]!["result"]!;

        return responseObj.ToObject<Timetable>()!;
    }
}