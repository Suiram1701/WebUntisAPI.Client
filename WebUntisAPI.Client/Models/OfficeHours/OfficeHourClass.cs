using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.OfficeHours;

/// <summary>
/// Represents an office hour class.
/// </summary>
public class OfficeHourClass
{
    /// <summary>
    /// The id of this class.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The displayed name of this office hour class.
    /// </summary>
    [JsonProperty("label")]
    public string Name { get; set; } = string.Empty;
}
