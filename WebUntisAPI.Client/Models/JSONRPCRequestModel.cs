using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Basic layout of a json rpc model
    /// </summary>
    [DataContract]
    internal class JSONRPCRequestModel
    {
        /// <summary>
        /// Identifies the request (repeated in response)
        /// </summary>
        [DataMember]
        public string id;

        /// <summary>
        /// Name of the method to call
        /// </summary>
        [DataMember]
        public string method;

        /// <summary>
        /// Parameters for the method
        /// </summary>
        [DataMember]
        public object @params;

        /// <summary>
        /// Version of json rpc to use
        /// </summary>
        [DataMember]
        public string jsonrpc = "2.0";
    }
}