using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represents a profile at untis messenges
/// </summary>
[DebuggerDisplay("{DisplayName, nq}")]
public class MessagePerson
{
    /// <summary>
    /// The id of the person
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The name of the class of the person
    /// </summary>
    [JsonProperty("className")]
    public string? ClassName { get; set; } = null;

    /// <summary>
    /// Tags of the person
    /// </summary>
    [JsonProperty("tags")]
    public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();

    /// <summary>
    /// The role of the person
    /// </summary>
    [JsonProperty("role")]
    public string? Role { get; set; }

    /// <summary>
    /// The displayed name of the person
    /// </summary>
    [JsonProperty("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// The url of the profile image
    /// </summary>
    /// <remarks>
    /// When the value is <c>null</c> then the first letter of <see cref="DisplayName"/> will be displayed
    /// </remarks>
    [JsonProperty("imageUrl")]
    public string? ImageUrl { get; set; } = null;

    [JsonProperty("userId")]
    [SuppressMessage("CodeQuality", "IDE0051", Justification = "used for JSON deserialization")]
    private int UserId
    {
        get => Id;
        set => Id = value;
    }
}
