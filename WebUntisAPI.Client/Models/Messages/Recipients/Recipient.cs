using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages.Recipients;

/// <summary>
/// Represents a normal recipient user.
/// </summary>
[DebuggerDisplay($"{{{nameof(DisplayName)},nq}}")]
public class Recipient : IRecipient
{
    /// <inheritdoc/>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <inheritdoc/>
    public string DisplayName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Uri? ImageUrl { get; set; }

    /// <inheritdoc/>
    public string? ClassName { get; set; }

    /// <summary>
    /// The role of the user
    /// </summary>
    [JsonProperty("role")]
    public virtual string? Role { get; set; }

    /// <summary>
    /// Tags of the user.
    /// </summary>
    [JsonProperty("tags")]
    public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
}