using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// A interface that provides all properties a message owns
/// </summary>
public interface IMessage
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
    /// The content of this message
    /// </summary>
    [JsonProperty("content")]
    public string Content { get; set; }

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
    /// Attachments attached to this message
    /// </summary>
    [JsonProperty("storageAttachments")]
    public IEnumerable<Attachment> Attachments { get; set; }
}
