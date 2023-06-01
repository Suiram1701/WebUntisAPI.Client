using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Basic layout of a json rpc response
    /// </summary>
    [DataContract]
    internal class JSONRPCResponeModel
    {
        /// <summary>
        /// Version of json rpc to use
        /// </summary>
        [DataMember]
        public string jsonrpc;

        /// <summary>
        /// Identifies the request
        /// </summary>
        [DataMember]
        public string id;

        /// <summary>
        /// Result of the method
        /// </summary>
        [DataMember]
        public object result;

        /// <summary>
        /// A error while on the request
        /// </summary>
        [DataMember]
        public WebUntisException error = null;
    }
}