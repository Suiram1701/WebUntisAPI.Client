using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Information about the permissions you have in context of messages
/// </summary>
public class MessagePermissions
{
    /// <summary>
    /// Represents the types of users you're allowed to send messages to
    /// </summary>
    [JsonProperty("recipientOptions")]
    public IEnumerable<string> RecipientOptions { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Indicates whether you're allowed to request a read confirmation from the recipients
    /// </summary>
    [JsonProperty("allowRequestReadConfirmation")]
    public bool AllowRequestReadConfirmation { get; set; }

    /// <summary>
    /// The count of recipients listed in the search list
    /// </summary>
    [JsonProperty("recipientSearchMaxResult")]
    public int RecipientSearchMaxResult { get; set; }

    /// <summary>
    /// Indicates whether you're allowed to see the drafts tab
    /// </summary>
    [JsonProperty("showDraftsTab")]
    public bool ShowDraftsTab { get; set; }

    /// <summary>
    /// Indicates whether you're allowed to see the sent tab
    /// </summary>
    [JsonProperty("showSentTab")]
    public bool ShowSentTab { get; set; }

    /// <summary>
    /// Indicates whether you're allowed to forbid the recipients to reply the message
    /// </summary>
    [JsonProperty("canForbidReplies")]
    public bool CanForbidReplies { get; set; }

    /// <summary>
    /// The maximum size in bytes of each file attached to a message
    /// </summary>
    [JsonProperty("maxFileSize")]
    public long MaxFileSize { get; set; }

    /// <summary>
    /// The maximum count of files you're allowed to attach to a message
    /// </summary>
    [JsonProperty("maxFileCount")]
    public int MaxFileCount { get; set; }
}