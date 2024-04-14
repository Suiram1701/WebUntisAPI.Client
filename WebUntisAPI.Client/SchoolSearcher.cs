using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client;

/// <summary>
/// A client to search for schools
/// </summary>
public class SchoolSearcher : IDisposable
{
    private readonly HttpClient _client;

    private readonly bool _disposeClient;
    private bool _disposedValue;

    private static readonly Uri _apiUrl = new("https://mobile.webuntis.com/ms/schoolquery2");

    /// <summary>
    /// Creates a new instance
    /// </summary>
    public SchoolSearcher()
    {
        _client = new HttpClient();
        _disposeClient = true;
    }

    /// <summary>
    /// Creates a new instance that uses an existing <see cref="HttpClient"/>
    /// </summary>
    /// <param name="client">The client to use</param>
    /// <param name="dispose">Indicates whether the <paramref name="client"/> should disposed after this instance get disposed</param>
    public SchoolSearcher(HttpClient client, bool dispose)
    {
        _client = client;
        _disposeClient = dispose;
    }

    /// <summary>
    /// Search for schools by the given name
    /// </summary>
    /// <param name="name">Name to search</param>
    /// <param name="id">Identifier for the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Found schools, an empty collection when no school found or <c>null</c> when too many schools were found</returns>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<School>?> SearchAsync(string name, string id = "searchForSchool", CancellationToken ct = default)
    {
        JArray requestParams = new()
        {
            new JObject
            {
                new JProperty("search", name)
            }
        };
        return await InternalSearchAsync(requestParams, id, ct);
    }

    /// <summary>
    /// Get a school by the <see cref="School.LoginName"/> value
    /// </summary>
    /// <param name="schoolName">The <see cref="School.LoginName"/></param>
    /// <param name="id">Identifier for the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The school that was found or <c>null</c> when no school was found</returns>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<School?> GetSchoolByNameAsync(string schoolName, string id = "getSchoolByName", CancellationToken ct = default)
    {
        JArray requestParams = new()
        {
            new JObject
            {
                new JProperty("schoolname", schoolName)
            }
        };
        IEnumerable<School>? schools = await InternalSearchAsync(requestParams, id, ct);

        return schools?.FirstOrDefault();
    }

    /// <summary>
    /// Get a school by the <see cref="School.SchoolId"/>
    /// </summary>
    /// <param name="schoolId">The <see cref="School.SchoolId"/></param>
    /// <param name="id">Identifier for the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The school that was found or <c>null</c> when no school was found</returns>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<School?> GetSchoolByIdAsync(int schoolId, string id = "getSchoolById", CancellationToken ct = default)
    {
        JArray requestParams = new()
        {
            new JObject
            {
                new JProperty("schoolid", schoolId)
            }
        };
        IEnumerable<School>? schools = await InternalSearchAsync(requestParams, id, ct);

        return schools?.FirstOrDefault();
    }

    private async Task<IEnumerable<School>?> InternalSearchAsync(JArray @params, string id, CancellationToken ct)
    {
        // Write a basic JSON RPC 2.0 request that is used here (https://untis-sr.ch/wp-content/uploads/2019/11/2018-09-20-WebUntis_JSON_RPC_API.pdf)
        JObject requestJson = new()
        {
            new JProperty("id", id),
            new JProperty("method", "searchSchool"),
            new JProperty("params", @params),
            new JProperty("jsonrpc", "2.0")
        };

        using StringContent requestContent = new(requestJson.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json);
        using HttpResponseMessage response = await _client.PostAsync(_apiUrl, requestContent, ct);
        string responseString = await response.Content.ReadAsStringAsync(ct);

        JObject responseObject = JObject.Parse(responseString);

        // Check for an error response 
        if (responseObject["error"] is JToken errorToken)
        {
            int errorCode = errorToken["code"]!.Value<int>();
            if (errorCode == -6003)     // -6003 means there to many results
            {
                return null;
            }

            string message = errorToken["message"]!.Value<string>()!;

            WebUntisError[] errors = { new(errorCode.ToString(), message) };
            throw new WebUntisException(errors);
        }

        return responseObject["result"]!["schools"]!.ToObject<IEnumerable<School>>();
    }

#pragma warning disable CS1591
    protected virtual void Dispose(bool disposing)
#pragma warning restore CS1591
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                if (_disposeClient)
                    _client.Dispose();
            }

            _disposedValue = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
