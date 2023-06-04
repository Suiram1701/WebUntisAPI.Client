using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;
using Newtonsoft.Json;
using WebUntisAPI.Client.Exceptions;
using System.Threading;
using System.ComponentModel;

namespace WebUntisAPI.Client
{
    /// <summary>
    /// A client that connect to WebUntis server to load data
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Please use this class in a using declaration. When you dont use it in a using declaration please call <see cref="LogoutAsync(string, CancellationToken)"/> and <see cref="Dispose()"/> when you don't need the connection.
    ///     </para>
    ///     <para>
    ///     Under no circumstances should 10 req. per sec., more than 1800req. per hr (but in no case more than 3600 req. per hr). If the specifications are exceeded, access to WebUntis is permanently blocked by the WebUntis API.
    ///     </para>
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
        /// Sesson id for requests
        /// </summary>
        private string _sessonId;

        /// <summary>
        /// Initialize a new client
        /// </summary>
        /// <param name="clientName">Unique identifier for the client app</param>
        /// <param name="timeout">The time in milliseconds until requests will be timeouted</param>
        public WebUntisClient(string clientName, int timeout = 500)
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
        /// <param name="school">The school to login (Use only returned instances from <see cref="SchoolSearch.SearchAsync(string, string, CancellationToken)"/>)</param>
        /// <param name="username">Name of the user to login</param>
        /// <param name="password">Password of the user to login</param>
        /// <param name="ct">Cancelationtoken</param>
        /// <param name="id">Identifier for the request</param>
        /// <returns><see langword="true"/> when the login was successfull. <see langword="false"/> when the <paramref name="username"/> or <paramref name="password"/> was invalid</returns>
        /// <exception cref="ArgumentException">The server name is invalid</exception>
        /// <exception cref="HttpRequestException">There was an error while the request</exception>
        /// <exception cref="WebUntisException">The WebUntis server returned an error</exception>
        public async Task<bool> LoginAsync(School school, string username, string password, string id = "getStudents", CancellationToken ct = default) =>
            await LoginAsync(school.Server, school.LoginName, username, password, id, ct);

        /// <summary>
        /// Login as a user in a school to get and write data
        /// </summary>
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
        public async Task<bool> LoginAsync(string server, string loginName, string username, string password, string id = "getStudents", CancellationToken ct = default)
        {
            // Check if you already logged in
            if (LoggedIn)
                return false;

            // Setup server url    
            Match serverName = Regex.Match(server, @"\w+\.webuntis\.com");
            if (!serverName.Success)
                throw new ArgumentException("This isn't a WebUntis server", nameof(server));
            string serverUrl = "https://" + serverName.Value;

            // Make request for login
            JSONRPCRequestModel<LoginModel> requestModel = new JSONRPCRequestModel<LoginModel>()
            {
                Id = id,
                Method = "authenticate",
                Params = new LoginModel()
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

            // Setup data and get default data
            _serverUrl = serverUrl;
            _loginName = loginName;
            _sessonId = responseModel.Result.SessionId;
            _loggedIn = true;

            return true;
        }

        /// <summary>
        /// Logout (You can reuse the client)
        /// </summary>
        /// <param name="id">Identifier for the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Task for the proccess</returns>
        /// <exception cref="HttpRequestException">There was an error while the request</exception>
        public async Task LogoutAsync(string id = "getStudents", CancellationToken ct = default)
        {
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
            requestContent.Headers.Add("jsessionid", _sessonId);

            // Send request
            HttpResponseMessage response = await _client.PostAsync(ServerUrl + "/WebUntis/jsonrpc.do", requestContent, ct);

            // Clear data
            _serverUrl = null;
            _loginName = null;
            _sessonId = null;
            _loggedIn = false;

            // Verify response
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");
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
