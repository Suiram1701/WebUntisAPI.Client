using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages.Recipients;

/// <summary>
/// Represents a group of teacher recipients.
/// </summary>
[JsonObject]
[DebuggerDisplay($"Name: {{{nameof(TypeName)},nq}}")]
public class TeacherRecipientGroup : IEnumerable<TeacherRecipient>
{
    /// <summary>
    /// The internal name of the group.
    /// </summary>
    [JsonProperty("type")]
    public string TypeName { get; set; } = string.Empty;

    /// <summary>
    /// The members of this group.
    /// </summary>
    [JsonProperty("persons")]
    public IEnumerable<TeacherRecipient> Members { get; set; } = Enumerable.Empty<TeacherRecipient>();

    /// <inheritdoc/>
    public IEnumerator<TeacherRecipient> GetEnumerator()
    {
        return Members.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Members).GetEnumerator();
    }
}
