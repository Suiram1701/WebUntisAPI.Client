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
    /// Get the count of unread news
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The count of the unread news</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<int> GetUnreadNewsCountAsync(CancellationToken ct = default)
    {
        string responseString = await InternalAPIRequestAsync("/WebUntis/api/rest/view/v1/dashboard/cards/status", ct);
        return JObject.Parse(responseString)["unreadCardsCount"]!.Value<int>();
    }

    /// <summary>
    /// Get all news for of the school for the current date
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The new at the school for the requested day</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public Task<NewsWidget> GetNewsFeedAsync(CancellationToken ct = default) =>
        GetNewsFeedAsync(DateOnly.FromDateTime(DateTime.Now), ct);

    /// <summary>
    /// Get all news for of the school for the day
    /// </summary>
    /// <param name="date">Date to get the news</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The new at the school for the requested day</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<NewsWidget> GetNewsFeedAsync(DateOnly date, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        UriBuilder uriBuilder = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/public/news/newsWidgetData",
            Query = $"date={date:yyyyMMdd}"
        };

        string responseString = await InternalAPIRequestAsync(uriBuilder.ToString(), ct);
        return JObject.Parse(responseString)!.GetValue("data")!.ToObject<NewsWidget>()!;
    }
}