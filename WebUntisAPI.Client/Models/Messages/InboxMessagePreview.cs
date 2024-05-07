using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Messages.Recipients;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represents a preview of <see cref="InboxMessage"/>
/// </summary>
public class InboxMessagePreview : IMessagePreview
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public string Subject { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string ContentPreview { get; set; } = string.Empty;

    /// <summary>
    /// The sender of this message
    /// </summary>
    [JsonProperty("sender")]
    public MessagePerson Sender { get; set; } = new();

    /// <inheritdoc/>
    public DateTime SentDateTime { get; set; }

    /// <inheritdoc/>
    public bool AllowDeletion { get; set; }

    /// <inheritdoc/>
    public bool HasAttachments { get; set; }

    /// <summary>
    /// Indicates whether this message is read
    /// </summary>
    [JsonProperty("isMessageRead")]
    public bool IsMessageRead { get; set; }

    /// <summary>
    /// Indicates whether this message is a reply
    /// </summary>
    [JsonProperty("isReply")]
    public bool IsReply { get; set; }

    /// <summary>
    /// Indicates whether 
    /// </summary>
    [JsonProperty("isReplyAllowed")]
    public bool IsReplyAllowed { get; set; }

    /// <summary>
    /// Indicates whether the sender of this message requested a confirmation
    /// </summary>
    public bool IsConfirmationRequested { get; set; }
}
