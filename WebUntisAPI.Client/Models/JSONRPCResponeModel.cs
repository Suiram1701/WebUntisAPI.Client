using Newtonsoft.Json;
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
        [JsonProperty("jsonrpc")]
        public string JSONRPC { get; set; }

        /// <summary>
        /// Identifies the request
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Result of the method
        /// </summary>
        [JsonProperty("result")]
        public TResult Result { get; set; }

        /// <summary>
        /// A error while on the request
        /// </summary>
        [JsonProperty("error")]
        public WebUntisException Error { get; set; } = null;
    }
}