using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// An attachment of a message
/// </summary>
[DebuggerDisplay("{Name, nq}")]
public struct Attachment
{
    /// <summary>
    /// The id of the attachment
    /// </summary>
    [JsonProperty("id")]
    [JsonConverter(typeof(GuidJsonConverter))]
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the attachment
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
}