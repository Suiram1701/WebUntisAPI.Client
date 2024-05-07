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
/// Represents a section that contains multiple recipient groups
/// </summary>
[DebuggerDisplay($"Name: {{{nameof(SectionType)},nq}}")]
public class RecipientSection : IEnumerable<RecipientGroup>
{
    /// <summary>
    /// The internal name of the section.
    /// </summary>
    [JsonProperty("sectionType")]
    public string SectionType { get; } = string.Empty;

    /// <summary>
    /// The groups this section contains.
    /// </summary>
    [JsonProperty("groups")]
    public IEnumerable<RecipientGroup> Groups { get; } = Enumerable.Empty<RecipientGroup>();

    /// <inheritdoc/>
    public IEnumerator<RecipientGroup> GetEnumerator()
    {
        return Groups.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Groups).GetEnumerator();
    }
}
