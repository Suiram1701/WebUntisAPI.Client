using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represents a message sent by you
/// </summary>
public class SentMessage : IMessage
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public string Subject { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string Content { get; set; } = string.Empty;

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
    public bool Reply { get; set; }

    /// <summary>
    /// Indicates whether it is forbidden to reply this message
    /// </summary>
    [JsonProperty("isReplyForbidden")]
    public bool IsReplyForbidden { get; set; }

    /// <summary>
    /// The groups of recipients
    /// </summary>
    [JsonProperty("recipientGroups")]
    public IEnumerable<string> RecipientGroups { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// The recipients of this message
    /// </summary>
    [JsonProperty("recipientPersons")]
    public IEnumerable<MessagePerson> RecipientPeople { get; set; } = Enumerable.Empty<MessagePerson>();

    /// <summary>
    /// The history of replies
    /// </summary>
    [JsonProperty("replyHistory")]
    public IEnumerable<ReplyMessage> ReplyHistory { get; set; } = Enumerable.Empty<ReplyMessage>();

    /// <summary>
    /// The confirmation state of this message
    /// </summary>
    /// <remarks>
    /// When <c>null</c> weren't a confirmation requested
    /// </remarks>
    [JsonProperty("requestConfirmationStatus")]
    public ConfirmationState? RequestConfirmationState { get; set; }
}
