using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;
using WebUntisAPI.Client.Models.Messages.Recipients;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represents a preview of <see cref="SentMessage"/>
/// </summary>
public class SentMessagePreview : IMessagePreview
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
    /// Indicates whether this message is a reply
    /// </summary>
    [JsonProperty("isReply")]
    public bool IsReply { get; set; }

    /// <summary>
    /// The count of recipients
    /// </summary>
    [JsonProperty("numberOfRecipients")]
    public int NumberOfRecipients { get; set; }

    /// <summary>
    /// A collection that contains the displayed names of every recipient group.
    /// </summary>
    [JsonProperty("recipientGroups", ItemConverterType = typeof(RecipientGroupJsonConverter))]
    public IEnumerable<string> RecipientGroups { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// The recipients of this message
    /// </summary>
    [JsonProperty("recipientPersons")]
    public IEnumerable<MessagePerson> RecipientPeople { get; set; } = Enumerable.Empty<MessagePerson>();

    /// <summary>
    /// The confirmation state of this message
    /// </summary>
    /// <remarks>
    /// When <c>null</c> weren't a confirmation requested
    /// </remarks>
    [JsonProperty("requestConfirmationStatus")]
    public ConfirmationState? RequestConfirmationState { get; set; }
}
