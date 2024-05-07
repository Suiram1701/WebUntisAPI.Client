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
    /// The recipient option with that this draft was created
    /// </summary>
    [JsonProperty("recipientOption")]
    public string RecipientOption { get; set; } = string.Empty;
}
