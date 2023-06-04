using Newtonsoft.Json;
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
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the method to call
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// Parameters for the method
        /// </summary>
        [JsonProperty("params")]
        public TParam Params { get; set; }

        /// <summary>
        /// Version of json rpc to use
        /// </summary>
        [JsonProperty("jsonrpc")]
        public string JSONRPC = "2.0";
    }
}