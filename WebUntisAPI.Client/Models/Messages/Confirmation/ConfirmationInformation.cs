using Newtonsoft.Json;
using System;
using WebUntisAPI.Client.Converters;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.Messages.Confirmation;

/// <summary>
/// Information about a confirmation message in the inbox.
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
    /// <remarks>
    /// When null the message weren't confirmed
    /// </remarks>
    [JsonProperty("confirmationDate")]
    public DateTime? ConfirmationDate { get; set; }

    /// <summary>
    /// The name of the user that confirmed the message
    /// </summary>
    /// <remarks>
    /// When null the message weren't confirmed
    /// </remarks>
    [JsonProperty("confirmerUserDisplayName")]
    public string? ConfirmerUserName { get; set; }

    /// <summary>
    /// The id of the user that confirmed the message (this id isn't the id of <see cref="IElement.Id"/>)
    /// </summary>
    /// <remarks>
    /// When null the message weren't confirmed
    /// </remarks>
    [JsonProperty("confirmerUserId")]
    public int? ConfirmerUserId { get; set; }

    /// <summary>
    /// Indicates whether it is allowed to reply the message this instance is assigned to
    /// </summary>
    [JsonProperty("isReplyAllowed")]
    public bool IsReplyAllowed { get; set; }
}
