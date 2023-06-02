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
    /// <typeparam name="TParam">Type of the parameter</typeparam>
    internal class JSONRPCRequestModel<TParam>
    {
        /// <summary>
        /// Identifies the request (repeated in response)
        /// </summary>
        public string id;

        /// <summary>
        /// Name of the method to call
        /// </summary>
        public string method;

        /// <summary>
        /// Parameters for the method
        /// </summary>
        public TParam @params;

        /// <summary>
        /// Version of json rpc to use
        /// </summary>
        public string jsonrpc = "2.0";
    }
}