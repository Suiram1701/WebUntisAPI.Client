using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using WebUntisAPI.Client.Models.Messages.Recipients;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represents a message in a reply history
/// </summary>
public class ReplyMessage : IMessage
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public string Subject { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// The sender of this message
    /// </summary>
    [JsonProperty("sender")]
    public MessagePerson Sender { get; set; } = new();

    /// <summary>
    /// The recipients of this message
    /// </summary>
    [JsonProperty("recipients")]
    public IEnumerable<MessagePerson> Recipients { get; set; } = Enumerable.Empty<MessagePerson>();

    /// <inheritdoc/>
    public DateTime SentDateTime { get; set; }

    /// <summary>
    /// Indicates whether this message were revoked
    /// </summary>
    [JsonProperty("isRevoked")]
    public bool IsRevoked { get; set; }

    /// <inheritdoc/>
    public bool AllowDeletion { get; set; }

    /// <inheritdoc/>
    public IEnumerable<Attachment> Attachments { get; set; } = Enumerable.Empty<Attachment>();
}