using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represents a preview of <see cref="DraftMessage"/>
/// </summary>
public class DraftMessagePreview : IMessagePreview
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public string Subject { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string ContentPreview { get; set; } = string.Empty;

    /// <inheritdoc/>
    public DateTime SentDateTime { get; set; }

    /// <inheritdoc/>
    public bool AllowDeletion { get; set; }

    /// <inheritdoc/>
    public bool HasAttachments { get; set; }

    /// <summary>
    /// The recipient option available for this draft
    /// </summary>
    [JsonProperty("recipientOption")]
    public string RecipientOption { get; set; } = string.Empty;

    /// <summary>
    /// The count of recipients
    /// </summary>
    [JsonProperty("numberOfRecipients")]
    public int NumberOfRecipients { get; set; }

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
