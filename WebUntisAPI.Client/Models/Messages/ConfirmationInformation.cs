using Newtonsoft.Json;
using System;
using WebUntisAPI.Client.Converters;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Informations about a confirmation message
/// </summary>
public class ConfirmationInformation
{
    /// <summary>
    /// Indicates whether it is allowed to delete this message before confirmation
    /// </summary>
    [JsonProperty("allowMessageDeletion")]
    public bool AllowMessageDeletion { get; set; }

    /// <summary>
    /// Indicates whether you're allowed to send a confirmation
    /// </summary>
    [JsonProperty("allowSendRequestConfirmation")]
    public bool AllowSendConfirmation { get; set; }

    /// <summary>
    /// The datetime where the message where confirmed the message
    /// </summary>
    [JsonProperty("confirmationDate")]
    public DateTime ConfirmationDate { get; set; }

    /// <summary>
    /// The name of the user that confirmed the message
    /// </summary>
    [JsonProperty("confirmerUserDisplayName")]
    public string ConfirmerUserName { get; set; } = string.Empty;

    /// <summary>
    /// The id of the user that confirmed the message (this id isn't the id of <see cref="IElement.Id"/>)
    /// </summary>
    [JsonProperty("confirmerUserId")]
    public int ConfirmerUserId { get; set; }

    /// <summary>
    /// Indicates whether it is allowed to reply the message this instance is assigned to
    /// </summary>
    [JsonProperty("isReplyAllowed")]
    public bool IsReplyAllowed { get; set; }
}
