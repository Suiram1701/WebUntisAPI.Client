using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// A interface that provides a message preview owns
/// </summary>
public interface IMessagePreview
{
    /// <summary>
    /// The internal id of this message
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The subject of this message
    /// </summary>
    [JsonProperty("subject")]
    public string Subject { get; set; }

    /// <summary>
    /// The preview of thís message
    /// </summary>
    [JsonProperty("contentPreview")]
    public string ContentPreview { get; set; }

    /// <summary>
    /// The date time where the message were sent
    /// </summary>
    [JsonProperty("sentDateTime")]
    public DateTime SentDateTime { get; set; }

    /// <summary>
    /// Indicates whether you're allowed to delete this message
    /// </summary>
    [JsonProperty("allowMessageDeletion")]
    public bool AllowDeletion { get; set; }

    /// <summary>
    /// Indicates whether this message have any attachment
    /// </summary>
    [JsonProperty("hasAttachments")]
    public bool HasAttachments { get; set; }
}
