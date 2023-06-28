using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
        /// The time in milliseconds until requests will be timeouted
        /// </summary>
        public int Timeout { get; }

        /// <summary>
        /// <see langword="true"/> when the client is currently logged in
        /// </summary>
        public bool LoggedIn => _loggedIn;
        private bool _loggedIn = false;

        /// <summary>
        /// Url of the WebUntis server
        /// </summary>
        public string ServerUrl => _serverUrl;
        private string _serverUrl;

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
        /// Sesson id for json rpc requests
        /// </summary>
        private string _sessonId;

        /// <summary>
        /// Auth token for api requests
        /// </summary>
        private string _bearerToken;

        /// <summary>
        /// Initialize a new client
        /// </summary>
        /// <param name="clientName">Unique identifier for the client app</param>
        /// <param name="timeout">The time in milliseconds until requests will be timeouted</param>
        public WebUntisClient(string clientName, int timeout = 1000)
        {
            ClientName = clientName;
            Timeout = timeout;
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(Timeout)
            };
        }

        /// <summary>
        /// Login as a user in a school to get and write data
        /// </summary>
        /// <remarks>
        /// Information about the user you logged in with is automatically requested
        /// </remarks>
        /// <param name="school">The school to login (Use only returned instances from <see cref="SchoolSearch.SearchAsync(string, string, CancellationToken)"/>)</param>
        /// <param name="username">Name of the user to login</param>
        /// <param name="password">Password of the user to login</param>
        /// <param name="ct">Cancelationtoken</param>
        /// <param name="id">Identifier for the request</param>
        /// <returns><see langword="true"/> when the login was successfull. <see langword="false"/> when the <paramref name="username"/> or <paramref name="password"/> was invalid</returns>
        /// <exception cref="ArgumentException">The server name is invalid</exception>
        /// <exception cref="HttpRequestException">There was an error while the request</exception>
        /// <exception cref="WebUntisException">The WebUntis server returned an error</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task<bool> LoginAsync(School school, string username, string password, string id = "getStudents", CancellationToken ct = default) =>
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
        /// <param name="ct">Cancelationtoken</param>
        /// <param name="id">Identifier for the request</param>
        /// <returns><see langword="true"/> when the login was successfull. <see langword="false"/> when the <paramref name="username"/> or <paramref name="password"/> was invalid</returns>
        /// <exception cref="ArgumentException">The server name is invalid</exception>
        /// <exception cref="HttpRequestException">There was an error while the request</exception>
        /// <exception cref="WebUntisException">The WebUntis server returned an error</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task<bool> LoginAsync(string server, string loginName, string username, string password, string id = "getStudents", CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you already logged in
            if (LoggedIn)
                return false;

            // Setup server url    
            Match serverName = Regex.Match(server, @"\w+\.webuntis\.com");
            if (!serverName.Success)
                throw new ArgumentException("This isn't a valid WebUntis server address", nameof(server));
            string serverUrl = "https://" + serverName.Value;

            // Make request for login
            JSONRPCRequestModel<LoginRequestModel> requestModel = new JSONRPCRequestModel<LoginRequestModel>()
            {
                Id = id,
                Method = "authenticate",
                Params = new LoginRequestModel()
                {
                    User = username,
                    Password = password,
                    Client = ClientName,
                }
            };
            StringContent requestContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

            // Send request
            string schoolName = loginName.ToLower().Replace(' ', '+');
            HttpResponseMessage response = await _client.PostAsync(serverUrl + "/WebUntis/jsonrpc.do?school=" + schoolName, requestContent, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return false;

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            JSONRPCResponeModel<LoginResultModel> responseModel = JsonConvert.DeserializeObject<JSONRPCResponeModel<LoginResultModel>>(await response.Content.ReadAsStringAsync());

            // Check for WebUntis error
            if (responseModel.Error != null)
            {
                if (responseModel.Error.Code == (int)WebUntisException.Codes.BadCredentials)     // Wrong login data
                    return false;

                throw responseModel.Error;
            }

            _serverUrl = serverUrl;
            _loginName = loginName;
            _sessonId = responseModel.Result.SessionId;
            _loggedIn = true;

            // Get the api auth token and the logged in user
            Task<string> bearerTokenTask = GetBearerTokenAsync(ct);
            IUser[] users;
            if ((UserType)responseModel.Result.PersonType == Client.UserType.Student)     // For student and teacher separate tasks
            {
                Task<Student[]> studentTask = GetStudentsAsync("getLoggedInStudent", ct);
                await Task.WhenAll(bearerTokenTask, studentTask);
                users = studentTask.Result;
            }
            else
            {
                Task<Teacher[]> teacherTask = GetTeachersAsync("getLoggedInTeacher", ct);
                await Task.WhenAll(bearerTokenTask, teacherTask);
                users = teacherTask.Result;
            }

            if (ct.IsCancellationRequested)     // Check for cancellation
            {
                _ = LogoutAsync();
                return false;
            }

            _bearerToken = bearerTokenTask.Result;

            _userType = (UserType)responseModel.Result.PersonType;
            _user = users.FirstOrDefault(user => user.Id == responseModel.Result.PersonId);

            return true;
        }

        /// <summary>
        /// Logout (You can reuse the client)
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Task for the proccess</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the object is disposed</exception>
        public async Task LogoutAsync(string id = "Logout", CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                return;

            // Make request for logout
            JSONRPCRequestModel<object> requestModel = new JSONRPCRequestModel<object>()
            {
                Id = id,
                Method = "logout",
                Params = new object()
            };
            StringContent requestContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
            SetRequestHeader(requestContent.Headers);

            // Send request
            _ = await _client.PostAsync(ServerUrl + "/WebUntis/jsonrpc.do", requestContent, ct);

            // Clear data
            _serverUrl = null;
            _loginName = null;
            _sessonId = null;
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
            long timestamp = await MakeJSONRPCRequestAsync<object, long>(id, "getLatestImportTime", new object(), ct);
            return new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc).AddTicks(timestamp * 10000);
        }

        /// <summary>
        /// Make an internal basic request to the WebUntis server
        /// </summary>
        /// <typeparam name="TRequest">Type of the request parameter</typeparam>
        /// <typeparam name="TResult">Type of the returned result</typeparam>
        /// <param name="requestUrl">Path to the API on the server</param>
        /// <param name="id">Identifier of the request</param>
        /// <param name="methodName">Name of the request method</param>
        /// <param name="requestParams">Parameter of the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The result</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the client isn't logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        /// <exception cref="WebUntisException">Thrown when the WebUntis API returned an error</exception>
        private async Task<TResult> MakeJSONRPCRequestAsync<TRequest, TResult>(string id, string methodName, TRequest requestParams, CancellationToken ct, string requestUrl = "/WebUntis/jsonrpc.do")
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            // Make request
            JSONRPCRequestModel<TRequest> requestModel = new JSONRPCRequestModel<TRequest>()
            {
                Id = id,
                Method = methodName,
                Params = requestParams
            };
            StringContent requestContent = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
            SetRequestHeader(requestContent.Headers);

            // Send request
            HttpResponseMessage response = await _client.PostAsync(ServerUrl + requestUrl, requestContent, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return default;

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            JSONRPCResponeModel<TResult> responseModel = JsonConvert.DeserializeObject<JSONRPCResponeModel<TResult>>(await response.Content.ReadAsStringAsync());

            // Check JSON RPC version and request id
            if (requestModel.JSONRPC != responseModel.JSONRPC || requestModel.Id != responseModel.Id)
                throw new HttpRequestException("The WebUntis API returned a wrong result");

            // Check for WebUntis error
            if (responseModel.Error != null)
            {
                if (responseModel.Error.Code == (int)WebUntisException.Codes.NotAuthticated)     // Logout when not authenticated
                    _ = LogoutAsync();

                throw responseModel.Error;
            }

            return responseModel.Result;
        }

        /// <summary>
        /// Get the bearer auth token for the api authentication
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The bearer token</returns>
        /// <exception cref="HttpRequestException">Thrown when an error happend while the http request</exception>
        private async Task<string> GetBearerTokenAsync(CancellationToken ct)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ServerUrl + "/WebUntis/api/token/new")
            };
            SetRequestHeader(request.Headers);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return default;

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            System.Diagnostics.Debug.WriteLine("Bearer");
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Add the default headers to a WebUntis API request
        /// </summary>
        /// <param name="headers">The headers object to add</param>
        /// <param name="setBearer"><see langword="true"/> if the auth header should be set</param>
        private void SetRequestHeader(HttpHeaders headers, bool setBearer = false)
        {
            headers.Add("JSESSIONID", _sessonId);
            if (setBearer)
                (headers as HttpRequestHeaders).Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
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
                // When not manually logged out then logout
                if (LoggedIn)
                    _ = LogoutAsync();

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
