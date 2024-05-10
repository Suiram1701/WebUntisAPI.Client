using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages.Confirmation;

/// <summary>
/// Represents the confirmation states sent by a recipient of the message.
/// </summary>
/// <remarks>
/// The recipient doesn't have to be a single user it could also be a recipient group with multiple users and confirmation states.
/// </remarks>
[DebuggerDisplay($"Recipient: {{{nameof(DisplayName)},nq}}, is confirmed: {{{nameof(IsConfirmed)}()}}")]
public class RecipientConfirmation
{
    /// <summary>
    /// The id of the recipient.
    /// </summary>
    [JsonProperty("recipientId")]
    public int Id { get; set; }

    /// <summary>
    /// The displayed name of the recipient.
    /// </summary>
    [JsonProperty("recipientDisplayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the class the recipient is a member of.
    /// </summary>
    /// <remarks>
    /// When <c>null</c> the recipient isn't a member of a class.
    /// </remarks>
    [JsonProperty("recipientClassName")]
    public string? ClassName { get; set; }

    /// <summary>
    /// The state of the confirmations.
    /// </summary>
    /// <remarks>
    /// When this collection contains a single item where the <see cref="ConfirmationState.Id"/> is the same than <see cref="Id"/> then this object represents a single user as recipient. Other wise when there are multiple confirmation states with different ids then this object represents a recipient group.
    /// </remarks>
    [JsonProperty("confirmationStates")]
    public IEnumerable<ConfirmationState> ConfirmationStates { get; set; } = Enumerable.Empty<ConfirmationState>();

    /// <summary>
    /// Indicates whether the recipient confirmed the message.
    /// </summary>
    /// <returns>The result</returns>
    public bool IsConfirmed()
    {
        return ConfirmationStates.Any(state => state.ConfirmedAt is not null);
    }
}
