using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace WebUntisAPI.Client.Models.Messages.Recipients;

/// <summary>
/// Represents a teacher recipient user.
/// </summary>
[DebuggerDisplay($"{{{nameof(DisplayName)},nq}}")]
public class TeacherRecipient : Recipient
{
    [JsonProperty("userId")]
    [SuppressMessage("CodeQuality", "IDE0051", Justification = "Member used for json deserialization.")]
    private int UserId
    {
        set => Id = value;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// The property contains the const value <c>TEACHER</c>. It isn't possible to change this value.
    /// </remarks>
    public override string? Role { get => "TEACHER"; set { } }
}