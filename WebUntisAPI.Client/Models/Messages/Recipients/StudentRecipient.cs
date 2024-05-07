using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages.Recipients;

/// <summary>
/// Represents a student recipient user.
/// </summary>
[DebuggerDisplay($"{{{nameof(DisplayName)},nq}}")]
public class StudentRecipient : IRecipient
{
    /// <inheritdoc/>
    [JsonProperty("id")]
    public int Id { get; set; }

    [DebuggerHidden]
    [JsonProperty("personId")]
    [SuppressMessage("CodeQuality", "IDE0051", Justification = "Member used for json deserialization.")]
    private int PersonId
    {
        set => Id = value;
    }

    /// <inheritdoc/>
    public string DisplayName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public Uri? ImageUrl { get; set; }

    /// <inheritdoc/>
    public string? ClassName { get; set; }
}
