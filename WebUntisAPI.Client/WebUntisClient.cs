using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client
{
    /// <summary>
    /// A client that connect to WebUntis server to load data
    /// </summary>
    /// <remarks>
    /// Under no circumstances should 10 req. per sec., more than 1800req. per hr (but in no case more than 3600 req. per hr). If the specifications are exceeded, access to WebUntis could permanently blocked by the WebUntis API.
    /// </remarks>
    public partial class WebUntisClient : IDisposable
    {
        /// <summary>
        /// Unique identifier for the client app
        /// </summary>
        public string ClientName { get; }

        /// <summary>
        /// The dateTime where the current session expires
        /// </summary>
        /// <remarks>
        /// You can refresh the session with <see cref="ReloadSessionAsync(CancellationToken)"/>
        /// </remarks>
        public DateTime SessionExpires
        {
            get
            {
                if (_disposedValue)
                    throw new ObjectDisposedException(GetType().FullName);

                if (!LoggedIn)
                    throw new UnauthorizedAccessException("You're not logged in!");

                string tokenString = _bearerToken.Split('.')[1];     // Get the JWT part

                JObject token = JObject.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(tokenString)));
                return new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddSeconds(token["exp"].Value<long>());
            }
        }

        /// <summary>
        /// The date Time off beginning of the current session
        /// </summary>
        public DateTime SessionBegin
        {
            get
            {
                if (_disposedValue)
                    throw new ObjectDisposedException(GetType().FullName);

                if (!LoggedIn)
                    throw new UnauthorizedAccessException("You're not logged in!");

                string tokenString = _bearerToken.Split('.')[1];     // Get the json part of the JWT

                JObject token = JObject.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(tokenString)));
                return new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddSeconds(token["iat"].Value<long>());
            }
        }

        /// <summary>
        /// <see langword="true"/> when the client is currently logged in
        /// </summary>
        public bool LoggedIn => _loggedIn;
        private bool _loggedIn = false;

        /// <summary>
        /// Url of the WebUntis server
        /// </summary>
        public Uri ServerUrl => _serverUrl;
        private Uri _serverUrl;

        /// <summary>
        /// The Untis name of the school
        /// </summary>
        public string LoginName => _loginName;
        private string _loginName;

        /// <summary>
        /// Current client
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Session id for json rpc requests
        /// </summary>
        private string _sessionId;

        /// <summary>
        /// The school name for the session
        /// </summary>
        private string _schoolName;

        /// <summary>
        /// Auth token for api requests
        /// </summary>
        private string _bearerToken;

        /// <summary>
        /// Initialize a new client
        /// </summary>
        /// <param name="clientName">Unique identifier for the client app</param>
        /// <param name="timeout">The time in milliseconds untila requests will be timeout</param>
        public WebUntisClient(string clientName, TimeSpan timeout)
        {
            ClientName = clientName;
            _client = new HttpClient()
            {
                Timeout = timeout
            };
        }

        /// <summary>
        /// Login as a user in a school to get and write data
        /// </summary>
        /// <remarks>
        /// Information about the user you logged in with is automatically requested
        /// </remarks>
        /// <param name="school">The school to login (Use only returned instances from <see cref="SchoolSearcher.SearchAsync(string, string, CancellationToken)"/>)</param>
        /// <param name="username">Name of the user to login</param>
        /// <param name="password">Password of the user to login</param>
        /// <param name="ct">Cancelation Token</param>
        /// <param name="id">Identifier for the request</param>
        /// <returns><see langword="true"/> when the login was successful. <see langword="false"/> when the <paramref name="username"/> or <paramref name="password"/> was invalid</returns>
        /// <exception cref="ArgumentException">The server name is invalid</exception>
        /// <exception cref="HttpRequestException">There was an error while the request</exception>
        /// <exception cref="WebUntisException">The WebUntis server returned an error</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task<bool> LoginAsync(School school, string username, string password, string id = "login", CancellationToken ct = default) =>
            await LoginAsync(school.Server, school.LoginName, username, password, id, ct);

        /// <summary>
        /// Login as a user in a school to get and write data
        /// </summary>
        /// <remarks>
        /// Information about the user you logged in with is automatically requested
        /// </remarks>
        /// <param name="server">server name to login (example: "herakles.webuntis.com")</param>
        /// <param name="loginName">School to login (Not the normal school name but the WebUntis internal one)</param>
        /// <param name="username">Name of the user to login</param>
        /// <param name="password">Password of the user to login</param>
        /// <param name="ct">Cancelation Token</param>
        /// <param name="id">Identifier for the request</param>
        /// <returns><see langword="true"/> when the login was successful. <see langword="false"/> when the <paramref name="username"/> or <paramref name="password"/> was invalid</returns>
        /// <exception cref="ArgumentException">The server name is invalid</exception>
        /// <exception cref="HttpRequestException">There was an error while the request</exception>
        /// <exception cref="WebUntisException">The WebUntis server returned an error</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task<bool> LoginAsync(string server, string loginName, string username, string password, string id = "login", CancellationToken ct = default)
        {
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            if (LoggedIn)
                return false;

            // Setup server url    
            Match serverName = Regex.Match(server, @"\w+\.webuntis\.com");
            if (!serverName.Success)
                throw new ArgumentException("This isn't a valid WebUntis server address", nameof(server));
            string serverUrl = "https://" + serverName.Value;

            // Make request for login
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(id);

                writer.WritePropertyName("method");
                writer.WriteValue("authenticate");

                writer.WritePropertyName("params");
                writer.WriteStartObject();

                writer.WritePropertyName("user");
                writer.WriteValue(username);

                writer.WritePropertyName("password");
                writer.WriteValue(password);

                writer.WritePropertyName("client");
                writer.WriteValue(ClientName);
                writer.WriteEndObject();

                writer.WritePropertyName("jsonrpc");
                writer.WriteValue("2.0");

                writer.WriteEndObject();
            }

            StringContent requestContent = new StringContent(sw.ToString(), Encoding.UTF8, "application/json");

            // Send request
            string schoolName = loginName.Replace(' ', '+');
            HttpResponseMessage response = await _client.PostAsync(serverUrl + "/WebUntis/jsonrpc.do?school=" + schoolName, requestContent, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return false;

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());

            // Check for WebUntis error
            //if (responseObject["error"]?.ToObject(typeof(WebUntisException)) is WebUntisException error)
            //{
            //    if (error.Code == (int)WebUntisException.Codes.BadCredentials)     // Wrong login data
            //        return false;

            //    throw error;
            //}

            string headerValue = response.Headers.First(header => header.Key == "Set-Cookie").Value.ToArray()[1];     // Read additional school name header
            _schoolName = Regex.Match(headerValue, "schoolname=\"(.+)\";").Groups[1].Value;

            _serverUrl = new(serverUrl);
            _loginName = loginName;
            _sessionId = responseObject["result"]["sessionId"].ToObject<string>() ?? throw new InvalidDataException("Session id was expected");
            _loggedIn = true;

            // Get the api auth token and the logged in user
            await ReloadSessionAsync(ct);
            //IUser[] users;
            //if ((UserType)responseObject["result"]["personType"].ToObject<int>() == Client.UserType.Student)     // For student and teacher separate tasks
            //{
            //    Task<Student[]> studentTask = GetStudentsAsync("getLoggedInStudent", ct);
            //    await Task.WhenAll(bearerTokenTask, studentTask);
            //    users = studentTask.Result;
            //}
            //else
            //{
            //    Task<Teacher[]> teacherTask = GetTeachersAsync("getLoggedInTeacher", ct);
            //    await Task.WhenAll(bearerTokenTask, teacherTask);
            //    users = teacherTask.Result;
            //}

            //if (ct.IsCancellationRequested)     // Check for cancellation
            //{
            //    _ = LogoutAsync();
            //    return false;
            //}

            //_userType = (UserType)responseObject["result"]["personType"].ToObject<int>();
            //_user = users.FirstOrDefault(user => user.Id == responseObject["result"]["personId"].ToObject<int>());

            return true;
        }

        /// <summary>
        /// Logout (You can reuse the client)
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Task for the process</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task LogoutAsync(string id = "logout", CancellationToken ct = default)
        {
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            if (!LoggedIn)
                return;

            // Make request for logout
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(id);

                writer.WritePropertyName("method");
                writer.WriteValue("logout");

                writer.WritePropertyName("params");
                writer.WriteStartObject();
                writer.WriteEndObject();

                writer.WritePropertyName("jsonrpc");
                writer.WriteValue("2.0");

                writer.WriteEndObject();
            }

            StringContent requestContent = new StringContent(sw.ToString(), Encoding.UTF8, "application/json");
            requestContent.Headers.Add("JSESSIONID", _sessionId);
            requestContent.Headers.Add("schoolname", _schoolName);

            // Send request
            _ = await _client.PostAsync(ServerUrl + "/WebUntis/jsonrpc.do", requestContent, ct);

            // Clear data
            _serverUrl = null;
            _loginName = null;
            _sessionId = null;
            _schoolName = null;
            _bearerToken = null;
            _loggedIn = false;
        }

        /// <summary>
        /// Get the latest update date of the WebUntis data
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The latest update date in <see cref="DateTimeKind.Utc"/></returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the client isn't logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        public async Task<DateTime> GetLatestImportTimeAsync(string id = "getLatestImportTime", CancellationToken ct = default)
        {
            long timestamp = (await MakeJSONRPCRequestAsync(id, "getLatestImportTime", null, ct)).ToObject<long>();
            return new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddTicks(timestamp * 10000);
        }

        /// <summary>
        /// Make an internal basic request to the WebUntis server
        /// </summary>
        /// <param name="id">Identifier of the request</param>
        /// <param name="methodName">Name of the request method</param>
        /// <param name="paramsWriter">the action to write the params</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the client isn't logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        private async Task<JToken> MakeJSONRPCRequestAsync(string id, string methodName, Action<JsonWriter> paramsWriter, CancellationToken ct)
        {
            throw new NotImplementedException();

            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            if (!LoggedIn)
                throw new UnauthorizedAccessException("The client is currently not logged in!");

            // Make request
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(id);

                writer.WritePropertyName("method");
                writer.WriteValue(methodName);

                writer.WritePropertyName("params");
                if (paramsWriter != null)
                    paramsWriter.Invoke(writer);
                else
                {
                    writer.WriteStartObject();
                    writer.WriteEndObject();
                }

                writer.WritePropertyName("jsonrpc");
                writer.WriteValue("2.0");

                writer.WriteEndObject();
            }

            StringContent requestContent = new StringContent(sw.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(ServerUrl + "/WebUntis/jsonrpc.do", requestContent, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return default;
            //response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            string request = sw.ToString();

            JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());

            // Check for WebUntis error
            if (responseObject["error"]?.ToObject<WebUntisException>() is WebUntisException error)
            {
                throw error;
            }

            return responseObject["result"];
        }

        internal Task<string> InternalAPIRequestAsync(string path, CancellationToken ct)
        {
            string url;
            if (path.StartsWith("http"))
                url = path;
            else
            {
                UriBuilder uriBuilder = new(ServerUrl)
                {
                    Path = path
                };
                url = uriBuilder.ToString();
            }

            HttpRequestMessage request = new(HttpMethod.Get, url);

            return InternalAPIRequestAsync(request, ct);
        }

        internal async Task<string> InternalAPIRequestAsync(HttpRequestMessage request, CancellationToken ct)
        {
            // Check for disposing
#if NET8_0_OR_GREATER
            ObjectDisposedException.ThrowIf(_disposedValue, this);
#else
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);
#endif
            if (!LoggedIn)
                throw new UnauthorizedAccessException("The client is currently not logged in!");

            request.Headers.Authorization = new("Bearer", _bearerToken);
            HttpResponseMessage response = await _client.SendAsync(request, ct);

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

        /// <summary>
        /// Refresh the session
        /// </summary>
        /// <remarks>
        /// Until this action was successfully ended no request should made
        /// </remarks>
        /// <param name="ct">Cancellation token</param>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        public async Task ReloadSessionAsync(CancellationToken ct = default)
        {
            _bearerToken = await InternalAPIRequestAsync("/WebUntis/api/token/new", ct);
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
                    LogoutAsync().Wait();

                if (disposing)
                {
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
}
