using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Profile;

/// <summary>
/// Represents the data to access the account through an iCal publication.
/// </summary>
public class ICalPublication
{
    /// <summary>
    /// Indicates whether the user is allowed to use iCal publications.
    /// </summary>
    [JsonProperty("isPublicationAllowed")]
    public bool IsPublicationAllowed { get; set; }

    /// <summary>
    /// The iCal url.
    /// </summary>
    [JsonProperty("iCalUrl")]
    public string ICalUrl { get; set; } = default!;

    /// <summary>
    /// The ics formats.
    /// </summary>
    [JsonProperty("icsFormats")]
    public object[] IcsFormats { get; set; } = Array.Empty<object>();

    /// <summary>
    /// The id of the used ics format.
    /// </summary>
    [JsonProperty("icsFormatId")]
    public int IcsFormatId { get; set; } = -1;
}
