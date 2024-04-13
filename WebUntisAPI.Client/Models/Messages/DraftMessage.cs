using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represents a draft message
/// </summary>
public class DraftMessage : IMessage
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public string Subject { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string Content { get; set; } = string.Empty;

    /// <inheritdoc/>
    /// <remarks>
    /// This value is always set to 1970-01-01 00:00:00 for drafts
    /// </remarks>
    public DateTime SentDateTime { get; set; }

    /// <inheritdoc/>
    public bool AllowDeletion { get; set; }

    /// <inheritdoc/>
    public IEnumerable<Attachment> Attachments { get; set; } = Enumerable.Empty<Attachment>();

    /// <summary>
    /// The recipient option available for this draft
    /// </summary>
    [JsonProperty("recipientOption")]
    public string RecipientOption { get; set; } = string.Empty;

    /// <summary>
    /// Idk
    /// </summary>
    [JsonProperty("copyToStudents")]
    public bool CopyToStudent { get; set; }

    /// <summary>
    /// Indicates whether recipients are allowed 
    /// </summary>
    [JsonProperty("forbidReply")]
    public bool ForbidReply { get; set; }

    /// <summary>
    /// Indicates whether the recipients have to confirm this message
    /// </summary>
    [JsonProperty("requestConfirmation")]
    public bool RequestConfirmation { get; set; }

    /// <summary>
    /// The recipient groups (for drafts this is always empty)
    /// </summary>
    [JsonProperty("recipientGroups")]
    public IEnumerable<string> RecipientGroups { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// The recipient people (for drafts this is always empty)
    /// </summary>
    [JsonProperty("recipientPersons")]
    public IEnumerable<MessagePerson> RecipientPeople { get; set; } = Enumerable.Empty<MessagePerson>();
}
