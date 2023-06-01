using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Exceptions
{
    /// <summary>
    /// An error that the WebUntis server returned
    /// </summary>
    [DataContract]
    public class WebUntisException : Exception
    {
        /// <summary>
        /// Code of  the error
        /// </summary>
        [DataMember]
        public int code { get; set; }

        /// <summary>
        /// A message that describes the error
        /// </summary>
        [DataMember]
        public string message { get; set; }

        /// <summary>
        /// A new <see cref="WebUntisException"/>
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="message">Message</param>
        public WebUntisException(int code, string message) : base(message)
        {
            this.code = code;
            this.message = message;
        }
    }
}