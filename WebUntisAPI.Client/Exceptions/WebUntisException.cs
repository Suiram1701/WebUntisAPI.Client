using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Exceptions
{
    /// <summary>
    /// An error that the WebUntis server returned
    /// </summary>
    [DebuggerDisplay("Message = {Message}, Code = {Code}")]
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
            /// Access denied for the requested timetable
            /// </summary>
            NoRightForTimetable = -8509,

            /// <summary>
            /// The element that you requested was not found
            /// </summary>
            NoSuchElementFound = -7002
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
            this.Code = code;
            this.Message = message;
        }
    }
}