using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace WebUntisAPI.Client.Models.Elements;

/// <summary>
/// Represents a reference to an element
/// </summary>
[DebuggerDisplay("{ToString(), nq}")]
public struct ElementId
{
    /// <summary>
    /// The element type this instance refers to
    /// </summary>
    [JsonProperty("type")]
    public ElementType Type { get; set; }

    /// <summary>
    /// The id
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The original id 
    /// </summary>
    /// <remarks>
    /// 0 means there is no orginal id
    /// </remarks>
    [JsonProperty("orgId")]
    public int OriginalId { get; set; }

    /// <summary>
    /// Indicates whether this element is missing
    /// </summary>
    [JsonProperty("missing")]
    public bool Missing { get; set; }

    /// <summary>
    /// The state of the element
    /// </summary>
    [JsonProperty("state")]
    public ElementState State { get; set; }

    /// <inheritdoc/>
    public override readonly string ToString()
    {
        StringBuilder builder = new();
        builder.Append(Id);

        if (OriginalId != 0)
        {
            builder.Append(" (");
            builder.Append(OriginalId);
            builder.Append(')');
        }

        if (Missing)
        {
            // Do a strike through like it is displayed on the website
            for (int i = 1; i < builder.Length; i += 2)
                builder.Insert(i, '\u0335');
        }

        return builder.ToString();
    }
}