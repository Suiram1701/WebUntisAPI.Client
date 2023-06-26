using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace WebUntisAPI.Client.Exceptions
{
    /// <summary>
    /// An error that the WebUntis server returned
    /// </summary>
    [DebuggerDisplay("Message = {Message, nq}, Code = {Code, nq}")]
    public class WebUntisException : Exception
    {
        /// <summary>
        /// Some error codes that the webuntis server can return
        /// </summary>
        public enum Codes
        {
            /// <summary>
            /// Too many results in a search
            /// </summary>
            TooManyResults = -6003,

            /// <summary>
            /// Wrong data
            /// </summary>
            BadCredentials = -8504,

            /// <summary>
            /// Json rpc method not found
            /// </summary>
            MethodNotFound = -32601,

            /// <summary>
            /// The element that you requested was not found
            /// </summary>
            NoSuchElementFound = -7002,

            /// <summary>
            /// Access dinied for the method
            /// </summary>
            NoRightForMethod = -8509,

            /// <summary>
            /// You're not authticated
            /// </summary>
            NotAuthticated = -8520
        }

        /// <summary>
        /// Code of  the error
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// A message that describes the error
        /// </summary>
        [JsonProperty("message")]
        public override string Message { get; }

        /// <summary>
        /// A new <see cref="WebUntisException"/>
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Message</param>
        public WebUntisException(int code, string message) : base(message)
        {
            Code = code;
            Message = message;
        }
    }
}