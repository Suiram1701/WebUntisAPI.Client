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
using WebUntisAPI.Client.Models.NewTimetable.FilterElements;

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

        UriBuilder uriBuilder = new(Session!.ServerUri)
        {
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

        // will be removed later
        //if (!element.CanViewTimetable)
        //    throw new InvalidOperationException($"The current session isn't allowed to view the timetable of {element.Name}");

        UriBuilder uriBuilder = new(Session!.ServerUri)
        {
            Path = "/WebUntis/api/public/timetable/weekly/data",
            Query = $"elementType={(int)element.GetElementType()}&elementId={element.Id}&date={week:yyyy-MM-dd}"
        };
        string responseString = await InternalApiRequestAsync(uriBuilder.Uri, ct);

        JObject dataObj = (JObject)JObject.Parse(responseString)["data"]!;
        if (dataObj["error"] is JObject errorObj)
        {
            string code = errorObj["code"]!.Value<int>().ToString();
            string message = errorObj["data"]!["messageKey"]!.Value<string>()!;

            throw new WebUntisException(new WebUntisError[] { new(code, message) });
        }

        return dataObj["result"]!.ToObject<Models.Timetable.Timetable>()!;
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
    /// <param name="format">The id of the format of the timetable.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The settings</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<TimetableSettings> GetNewTimetableSettingsAsync(int format = 1, CancellationToken ct = default)
    {
        Uri requestUri = new UriBuilder(Session!.ServerUri)
        {
            Path = "/WebUntis/api/rest/view/v1/timetable/entries/settings",
            Query = $"format={format}"
        }.Uri;

        string responseString = await InternalApiRequestAsync(requestUri, ct);
        return JsonConvert.DeserializeObject<TimetableSettings>(responseString)!;
    }

    /// <summary>
    /// Gets the available timetable filters of the specified <paramref name="resourceType"/>.
    /// </summary>
    /// <param name="resourceType">The resource type to get the filters for.</param>
    /// <param name="timetableType">The type of the timetable. <see cref="TimetableType.My_Timetable"/> is only used when the user requests his own timetable but there no differences if also <see cref="TimetableType.Standard"/> is used.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The filters for the specified <paramref name="resourceType"/>.</returns>
    public async Task<TimetableFilters> GetNewTimetableFiltersAsync(ElementType resourceType, TimetableType timetableType = TimetableType.Standard, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(resourceType);
        ArgumentNullException.ThrowIfNull(timetableType);

        Uri requestUri = new UriBuilder(Session!.ServerUri)
        {
            Path = "/WebUntis/api/rest/view/v1/timetable/filter",
            Query = $"resourceType={resourceType.ToString().ToUpperInvariant()}&timetableType={timetableType.ToString().ToUpperInvariant()}"
        }.Uri;
        string responseString = await InternalApiRequestAsync(requestUri, ct);
        return JsonConvert.DeserializeObject<TimetableFilters>(responseString)!;
    }

    /// <summary>
    /// Gets the timetable for a specified filter that were get through <see cref="GetNewTimetableFiltersAsync(ElementType, TimetableType, CancellationToken)"/>.
    /// </summary>
    /// <param name="dateRange">The date range </param>
    /// <param name="filterElement">The timetable element to filter for.</param>
    /// <param name="filters">Period filters to apply. By default every kind of period is requested.</param>
    /// <param name="formatId">The id of the format of the timetable.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A collection containing every day where data were available in the specified date range.</returns>
    public async Task<IEnumerable<TimetableDay>> GetNewTimetableAsync(
        DateRange dateRange,
        ITimetableFilterElement filterElement,
        PeriodType filters = PeriodType.Normal_Teaching_Period | PeriodType.Additional_Period | PeriodType.Event | PeriodType.Stand_By_Period | PeriodType.Office_Hour | PeriodType.Exam | PeriodType.Break_Supervision,
        int formatId = 1,
        CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(dateRange);
        ArgumentNullException.ThrowIfNull(filterElement);
        ArgumentNullException.ThrowIfNull(filters);
        ArgumentNullException.ThrowIfNull(formatId);

        return await GetNewTimetableAsync(dateRange, filterElement.Element, filters, formatId, ct);
    }

    /// <summary>
    /// Gets the timetable for a specified element.
    /// </summary>
    /// <param name="dateRange">The date range </param>
    /// <param name="element">The element to get the timetable for.</param>
    /// <param name="filters">Period filters to apply. By default every kind of period is requested.</param>
    /// <param name="formatId">The id of the format of the timetable.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A collection containing every day where data were available in the specified date range.</returns>
    public async Task<IEnumerable<TimetableDay>> GetNewTimetableAsync(
        DateRange dateRange,
        IElement element,
        PeriodType filters = PeriodType.Normal_Teaching_Period | PeriodType.Additional_Period | PeriodType.Event | PeriodType.Stand_By_Period | PeriodType.Office_Hour | PeriodType.Exam | PeriodType.Break_Supervision,
        int formatId = 1,
        CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(dateRange);
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(filters);
        ArgumentNullException.ThrowIfNull(formatId);

        string resourceType = element.GetElementType().ToString().ToUpperInvariant();
        string periodTypes = filters.ToString().Replace(" ", string.Empty).ToUpperInvariant();
        Uri requestUri = new UriBuilder(Session!.ServerUri)
        {
            Path = "/WebUntis/api/rest/view/v1/timetable/entries",
            Query = $"start={dateRange.Start:yyyy-MM-dd}&end={dateRange.End:yyyy-MM-dd}&resourceType={resourceType}&resources={element.Id}&periodTypes={periodTypes}&format={formatId}" 
        }.Uri;

        string responseString = await InternalApiRequestAsync(requestUri, ct);
        return JObject.Parse(responseString)["days"]!.ToObject<IEnumerable<TimetableDay>>()!;
    }
}