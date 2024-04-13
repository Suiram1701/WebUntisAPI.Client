using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represent a message in the inbox
/// </summary>
public class InboxMessage : IMessage
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

    /// <inheritdoc/>
    public DateTime SentDateTime { get; set; }

    /// <inheritdoc/>
    public bool AllowDeletion { get; set; }

    /// <inheritdoc/>
    public IEnumerable<Attachment> Attachments { get; set; } = Enumerable.Empty<Attachment>();

    /// <summary>
    /// Indicates whether this message is a reply
    /// </summary>
    [JsonProperty("isReply")]
    public bool IsReply { get; set; }

    /// <summary>
    /// Indicates whether it is allowed to reply this message
    /// </summary>
    [JsonProperty("isReplyAllowed")]
    public bool IsReplyAllowed { get; set; }

    /// <summary>
    /// Indicates whether this message is a report
    /// </summary>
    [JsonProperty("isReportMessage")]
    public bool IsReportMessage { get; set; }

    /// <summary>
    /// Indicates whether the sender forbid you to reply this message
    /// </summary>
    [JsonProperty("isReplyForbidden")]
    public bool IsReplyForbidden { get; set; }

    /// <summary>
    /// The history of replies
    /// </summary>
    [JsonProperty("replyHistory")]
    public IEnumerable<ReplyMessage> ReplyHistory { get; set; } = Enumerable.Empty<ReplyMessage>();

    /// <summary>
    /// The state of the requests confirmation
    /// </summary>
    /// <remarks>
    /// When <c>null</c> then the sender don't request a confirmation
    /// </remarks>
    [JsonProperty("requestConfirmation")]
    public ConfirmationInformation? RequestConfirmation { get; set; }
}
