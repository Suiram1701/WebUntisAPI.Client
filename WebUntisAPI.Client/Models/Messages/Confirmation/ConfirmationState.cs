using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages.Confirmation;

/// <summary>
/// Represents the confirmation state of a user.
/// </summary>
[DebuggerDisplay($"User: {{{nameof(DisplayName)},nq}}, Confirmed at: {{{nameof(ConfirmedAt)}}}")]
public class ConfirmationState
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    [JsonProperty("userId")]
    public int Id { get; set; }

    /// <summary>
    /// The displayed name of the user.
    /// </summary>
    [JsonProperty("userDisplayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// The date time where the confirmation were sent.
    /// </summary>
    /// <remarks>
    /// When <c>null</c> the user doesn't confirmed the message yet.
    /// </remarks>
    [JsonProperty("date")]
    public DateTime? ConfirmedAt { get; set; }
}
