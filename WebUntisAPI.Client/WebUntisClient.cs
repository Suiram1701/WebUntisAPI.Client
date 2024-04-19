using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client;

/// <summary>
/// A client that connect to a WebUntis server to load data
/// </summary>
public partial class WebUntisClient : IDisposable
{
    /// <summary>
    /// Indicates whether the client is currently logged in
    /// </summary>
    public bool LoggedIn { get; private set; }

    /// <summary>
    /// The host name of the webuntis server
    /// </summary>
    /// <remarks>
    /// <c>null</c> means that the client isn't currently logged in
    /// </remarks>
    public string? ServerName { get; private set; }

    private int? _userType;
    private int? _userId;

    private string? _jwtToken;
    private JObject? _jwtContent;

    private readonly HttpClient _client;
    private readonly bool _disposeClient;

    /// <summary>
    /// Creates a new instance that creates its own <see cref="HttpClient"/> that uses a timeout of 5s and a user agent in the format of WebUntisAPI.Client/{version}
    /// </summary>
    public WebUntisClient() : this(TimeSpan.FromSeconds(5))
    {
    }

    /// <summary>
    /// Creates a new instance that creates its own <see cref="HttpClient"/> that uses the specified timeout and a user agent in the format of WebUntisAPI.Client/{version}
    /// </summary>
    /// <param name="timeout">The timeout for every request</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public WebUntisClient(TimeSpan timeout)
    {
        ArgumentNullException.ThrowIfNull(timeout, nameof(timeout));
        if (timeout <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(timeout), "The timeout have to be larger than 0.");

        _client = new()
        {
            Timeout = timeout
        };

        AssemblyName assemblyName = typeof(WebUntisClient).Assembly.GetName();
        _client.DefaultRequestHeaders.UserAgent.Add(new(assemblyName.Name!, assemblyName.Version?.ToString()));
    }

    /// <summary>
    /// Creates a new instance that uses a provided <see cref="HttpClient"/> instance
    /// </summary>
    /// <param name="client">A client instance to use</param>
    /// <param name="disposeClient">Indicates whether the client should disposed when this instance will be disposed</param>
    /// <exception cref="ArgumentNullException"></exception>
    public WebUntisClient(HttpClient client, bool disposeClient)
    {
        ArgumentNullException.ThrowIfNull(_client, nameof(_client));

        _client = client;
        _disposeClient = disposeClient;
    }

    /// <summary>
    /// Signs in a user
    /// </summary>
    /// <remarks>
    /// A thrown <see cref="WebUntisException"/> that contains an error with the <see cref="WebUntisError.Code"/> <c>SCHOOL_NOT_FOUND</c> means that <paramref name="school"/> is invalid
    /// </remarks>
    /// <param name="school">The school to login</param>
    /// <param name="username">Name of the user to login</param>
    /// <param name="password">Password of the user to login</param>
    /// <param name="id">The identifier of the request. When the param is <c>null</c> then a random GUID will get used.</param>
    /// <param name="clientName">The name of the client. When <c>null</c> the UserAgent of the HttpClient will get used.</param>
    /// <param name="ct">Cancelation Token</param>
    /// <returns><see langword="true"/> when the login was successful. <see langword="false"/> when the <paramref name="username"/> or <paramref name="password"/> was invalid</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public async Task<bool> SignInAsync(School school, string username, string password, string? id = null, string? clientName = null, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(school, nameof(school));

        return await SignInAsync(school.Server, school.LoginName, username, password, id, clientName, ct);
    }

    /// <summary>
    /// Signs in a user
    /// </summary>
    /// <remarks>
    /// A thrown <see cref="WebUntisException"/> that contains an error with the <see cref="WebUntisError.Code"/> <c>-8500</c> means that <paramref name="loginName"/> is invalid
    /// </remarks>
    /// <param name="server">server name to login (example: <c>herakles.webuntis.com</c>)</param>
    /// <param name="loginName">School to login (<see cref="School.LoginName"/>)</param>
    /// <param name="username">Name of the user to login</param>
    /// <param name="password">Password of the user to login</param>
    /// <param name="id">The identifier of the request. When the param is <c>null</c> then a random GUID will get used.</param>
    /// <param name="clientName">The name of the client. When <c>null</c> the UserAgent of the HttpClient will get used.</param>
    /// <param name="ct">Cancelation Token</param>
    /// <returns><c>true</c> when the login was successful. <c>false</c> when the <paramref name="username"/> or <paramref name="password"/> was invalid or the client were already logged in</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public async Task<bool> SignInAsync(string server, string loginName, string username, string password, string? id, string? clientName = null, CancellationToken ct = default)
    {
        // Check for disposing
#if NET8_0_OR_GREATER
            ObjectDisposedException.ThrowIf(_disposedValue, this);
#else
        if (_disposedValue)
            throw new ObjectDisposedException(GetType().FullName);
#endif
        ArgumentNullException.ThrowIfNull(server, nameof(server));
        ArgumentNullException.ThrowIfNull(loginName, nameof(loginName));
        ArgumentNullException.ThrowIfNull(username, nameof(username));
        ArgumentNullException.ThrowIfNull(password, nameof(password));

        clientName ??= _client.DefaultRequestHeaders.UserAgent.ToString();

        if (LoggedIn)
            return false;

        JObject @params = new()
        {
            new JProperty("user", username),
            new JProperty("password", password),
            new JProperty("client", clientName)
        };
        Uri requestUri = new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = server,
            Path = "WebUntis/jsonrpc.do",
            Query = $"school={loginName}"
        }.Uri;

        try
        {
            JObject result = (await InternalJsonRpcRequestAsync(@params, "authenticate", id, requestUri, ct: ct))!;

            _userType = result["personType"]!.Value<int>()!;
            _userId = result["personId"]!.Value<int>()!;
        }
        catch (WebUntisException ex)
        {
            if (ex.Errors.Any(e => e.Code.Equals((-8504).ToString())))     // code -8504 indicates that the credentials were wrong
                return false;
            throw;
        }

        try
        {
            LoggedIn = true;
            ServerName = server;

            bool result = await ReloadSessionAsync(ct);
            if (!result)
                throw new Exception("An error happened while reloading the login session.");
        }
        catch
        {
            ClearSession();
            throw;
        }

        return true;
    }

    /// <summary>
    /// igns out the user (You can reuse the client)
    /// </summary>
    /// <exception cref="ObjectDisposedException"></exception>
    public async Task SignOutAsync(string? id, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        await InternalJsonRpcRequestAsync(new JObject(), "logout", id, ct: ct);
        ClearSession();
    }

    /// <summary>
    /// Get the currently signed in user
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The user</returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public async Task<IUser> GetSignedInUserAsync(CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        // Determine the type of the user by in the jwt saved 'roles' property
        string userRoles = _jwtContent!["roles"]!.Value<string>()!;
        ElementType userType = userRoles.Contains("STUDENT")
            ? ElementType.Student
            : ElementType.Teacher;

        UriBuilder uriBuilder = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/public/timetable/weekly/pageconfig",
            Query = $"type={(int)userType}"
        };
        string response = await InternalApiRequestAsync(uriBuilder.Uri, ct);

        JToken responseElement = JObject.Parse(response)["data"]!["elements"]![0]!;
        Type tUser = responseElement["type"]!.Value<int>() switch
        {
            (int)ElementType.Teacher => typeof(Teacher),
            (int)ElementType.Student => typeof(Student),
            _ => throw new Exception("The in the response specified element type isn't a user.")
        };

        IUser user = (IUser)responseElement.ToObject(tUser)!;
        user.CanViewTimetable = true;

        return user;
    }

    /// <summary>
    /// Refresh the session
    /// </summary>
    /// <remarks>
    /// Until this action was successfully ended no request should made
    /// </remarks>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A value that indicates whether the reload was successful (when <see langword="false"/> it isn't possible to determine the specific error)</returns>
    /// <exception cref="HttpRequestException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public async Task<bool> ReloadSessionAsync(CancellationToken ct = default)
    {
        string response = await InternalApiRequestAsync("/WebUntis/api/token/new", ct);

        // determine whether a new jwt was returned
        string[] jwtParts = response.Split('.');
        bool result = jwtParts.Length == 3
            && jwtParts.Take(2).All(p => p.StartsWith("ey"));

        if (result)
        {
            _jwtToken = response;

            // Parse the returned jwt in preparation for other methods
            byte[] jwtContentPartB = Convert.FromBase64String(_jwtToken!.Split('.')[1]);
            string jwtContentPart = Encoding.UTF8.GetString(jwtContentPartB);
            _jwtContent = JObject.Parse(jwtContentPart);
        }
        return result;
    }

    /// <summary>
    /// Get the time where the current session were issued
    /// </summary>
    /// <returns>The date time where the current session was issued</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public DateTimeOffset GetIssuedTime()
    {
        ThrowWhenNotAvailable();

        long iat = _jwtContent!["iat"]!.Value<long>();
        return DateTimeOffset.FromUnixTimeSeconds(iat);
    }

    /// <summary>
    /// Get the time where the current session will be expired
    /// </summary>
    /// <remarks>
    /// You have to reload the session with <see cref="ReloadSessionAsync(CancellationToken)"/> before the returned date time
    /// </remarks>
    /// <returns>The date time where the current session will be expired</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public DateTimeOffset GetExpiresTime()
    {
        ThrowWhenNotAvailable();

        long exp = _jwtContent!["exp"]!.Value<long>();
        return DateTimeOffset.FromUnixTimeSeconds(exp);
    }

    private async Task<string> InternalApiRequestAsync(string path, CancellationToken ct)
    {
        ThrowWhenNotAvailable();

        UriBuilder uriBuilder = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = path
        };
        using HttpRequestMessage request = new(HttpMethod.Get, uriBuilder.Uri);

        return await InternalApiRequestAsync(request, ct);
    }

    private async Task<string> InternalApiRequestAsync(Uri uri, CancellationToken ct)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        return await InternalApiRequestAsync(request, ct);
    }

    private async Task<string> InternalApiRequestAsync(HttpRequestMessage request, CancellationToken ct)
    {
        ThrowWhenNotAvailable();

        request.Headers.Authorization = new("Bearer", _jwtToken);
        using HttpResponseMessage response = await _client.SendAsync(request, ct);

        string responseString = await response.Content.ReadAsStringAsync(ct);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            JObject obj = JObject.Parse(responseString);
            if (obj.ContainsKey("errors"))
            {
                JArray errorArray = (JArray)obj!["errors"]!;
                throw new WebUntisException(errorArray);
            }

            string code = obj["errorCode"]!.Value<string>()!;
            string message = obj["errorMessage"]!.Value<string>()!;

            IEnumerable<WebUntisError> errors = new[] { new WebUntisError(code, message) };
            throw new WebUntisException(errors);
        }

        return responseString;
    }

    private async Task<JObject?> InternalJsonRpcRequestAsync(JToken @params, string method, string? id, string requestPath = "WebUntis/jsonrpc.do", CancellationToken ct = default)
    {
        return await InternalJsonRpcRequestAsync(@params, method, id, new UriBuilder
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = requestPath
        }.Uri, ct);
    }

    private async Task<JObject?> InternalJsonRpcRequestAsync(JToken @params, string method, string? id, Uri requestrUri, CancellationToken ct = default)
    {
        id ??= Guid.NewGuid().ToString();

        JObject requestJson = new()
        {
            new JProperty("id", id),
            new JProperty("method", method),
            new JProperty("params", @params),
            new JProperty("jsonrpc", "2.0")
        };

        using HttpContent content = new StringContent(requestJson.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json);
        using HttpResponseMessage response = await _client.PostAsync(requestrUri, content, ct);

        string responseString = await response.Content.ReadAsStringAsync(ct);
        JObject responseJson = JObject.Parse(responseString);

        if (responseJson["error"] is JObject error)
        {
            int code = error["code"]!.Value<int>()!;
            string message = error["message"]!.Value<string>()!;

            IEnumerable<WebUntisError> errors = new[] { new WebUntisError(code.ToString(), message) };
            throw new WebUntisException(errors);
        }

        return responseJson["result"] as JObject;
    }

    private void ThrowWhenNotAvailable()
    {
        // Check for disposing
#if NET8_0_OR_GREATER
            ObjectDisposedException.ThrowIf(_disposedValue, this);
#else
        if (_disposedValue)
            throw new ObjectDisposedException(GetType().FullName);
#endif
        if (!LoggedIn)
            throw new InvalidOperationException("The client is currently not signed in!");
    }

    private void ClearSession()
    {
        string servername = ServerName!;

        _userType = null;
        _userId = null;
        LoggedIn = false;
        ServerName = null;
        _jwtToken = null;
        _jwtContent = null;

        // Clear the cookies of the webuntis domain when its possible
        FieldInfo iHandler = typeof(HttpMessageInvoker).GetField("_handler", BindingFlags.NonPublic | BindingFlags.Instance)!;
        HttpMessageHandler? handler = iHandler.GetValue(_client) as HttpMessageHandler;

        if (handler is HttpClientHandler clientHandler)
        {
            CookieContainer cookieContainer = clientHandler.CookieContainer;
            UriBuilder builder = new()
            {
                Scheme = Uri.UriSchemeHttps,
                Host = servername
            };

            // Remove every cookie
            foreach (Cookie cookie in cookieContainer.GetCookies(builder.Uri).Cast<Cookie>())
                cookie.Expired = true;
        }
    }

    #region IDisposable
    private bool _disposedValue;

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            _client.CancelPendingRequests();

            // When not manually logged out then logout
            if (LoggedIn)
                ClearSession();

            if (disposing)
            {
                if (_disposeClient)
                    _client.Dispose();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Finalizer
    /// </summary>
    ~WebUntisClient()
    {
        Dispose(disposing: false);
    }
    #endregion
}