using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages.Recipients;

/// <summary>
/// Represents a sender/recipient user of a sent message.
/// </summary>
[DebuggerDisplay($"{{{nameof(DisplayName)},nq}}")]
public class MessagePerson : IRecipient
{
    /// <inheritdoc/>
    [JsonProperty("userId")]
    public int Id { get; set; }

    /// <inheritdoc/>
    public Uri? ImageUrl { get; set; }

    /// <inheritdoc/>
    public string DisplayName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string? ClassName { get; set; }
}
