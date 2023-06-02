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
    /// <typeparam name="TResult">Result type</typeparam>
    internal class JSONRPCResponeModel<TResult>
    {
        /// <summary>
        /// Version of json rpc to use
        /// </summary>
        public string jsonrpc;

        /// <summary>
        /// Identifies the request
        /// </summary>
        public string id;

        /// <summary>
        /// Result of the method
        /// </summary>
        public TResult result;

        /// <summary>
        /// A error while on the request
        /// </summary>
        public WebUntisException error = null;
    }
}