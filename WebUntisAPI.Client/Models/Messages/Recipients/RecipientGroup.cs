using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages.Recipients;

/// <summary>
/// Represents a group of message recipients
/// </summary>
[DebuggerDisplay($"Name: {{{nameof(Title)},nq}}")]
public class RecipientGroup
{
    /// <summary>
    /// The id of this group
    /// </summary>
    [JsonProperty("groupId")]
    public int Id { get; set; }

    /// <summary>
    /// The displayed title
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The subtitle of the group
    /// </summary>
    [JsonProperty("subtitle")]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>
    /// Details of this group
    /// </summary>
    [JsonProperty("details")]
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// The ids of members of this group
    /// </summary>
    [JsonProperty("personIds")]
    public IEnumerable<int> MemberIds { get; set; } = Enumerable.Empty<int>();
}
