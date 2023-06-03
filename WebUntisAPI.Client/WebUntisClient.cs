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

namespace WebUntisAPI.Client
{
    /// <summary>
    /// A client that connect to WebUntis server to load data
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Please use this class in a using declaration. When you dont use it in a using declaration please call <see cref="Logout()"/> and <see cref="Dispose()"/> when you don't need the connection.
    ///     </para>
    ///     <para>
    ///     Under no circumstances should 10 req. per sec., more than 1800req. per hr (but in no case more than 3600 req. per hr). If the specifications are exceeded, access to WebUntis is permanently blocked by the WebUntis API.
    ///     </para>
    /// </remarks>
    public class WebUntisClient : IDisposable
    {
        /// <summary>
        /// The id with that the client make requests
        /// </summary>
        public string RequestId { get; }

        /// <summary>
        /// Url of the WebUntis server
        /// </summary>
        public string ServerUrl { get; }

        private HttpClient _client;

        /// <summary>
        /// Connect to the WebUntis server
        /// </summary>
        /// <param name="school">School to connect</param>
        /// <param name="id">Identifier for the requests</param>
        /// <param name="timeout">The time in milliseconds until requests will be timeouted</param>
        /// <exception cref="PingException">Thrown when the WebUntis server ins't reachable</exception>
        public WebUntisClient(SchoolModel school, string id = "WebUntisAPI", int timeout = 500) : this(school.server, school.loginName, id, timeout)
        {
        }

        /// <summary>
        /// Connect to the WebUntis server
        /// </summary>
        /// <param name="server">Server name of the WebUntis server</param>
        /// <param name="loginName">Name of the school in WebUntis system</param>
        /// <param name="id">Identifier for the requests</param>
        /// <exception cref="PingException">Thrown when the WebUntis server ins't reachable</exception>
        public WebUntisClient(string server, string loginName, string id = "WebUntisAPI", int timeout = 500)
        {
            // Test if the WebUntis server is reachable
            string basicServerUrl = $"https://{server}/WebUntis/?school={loginName}";
            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> response = client.GetAsync(basicServerUrl);
                response.Wait();
                if (!response.Result.IsSuccessStatusCode)
                {
                    client.Dispose();
                    Dispose();
                    throw new PingException("Can't connect to WebUntis server");
                }
            }

            RequestId = id;
            ServerUrl = $"https://{server}";

            // Setup http client
            _client = new HttpClient
            {
                BaseAddress = new Uri(ServerUrl),
                Timeout = TimeSpan.FromMilliseconds(timeout)
            };
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
                if (disposing)
                {
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
