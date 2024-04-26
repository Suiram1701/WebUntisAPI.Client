using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// A school year
/// </summary>
[DebuggerDisplay("Name: {Name, nq}")]
public class SchoolYear
{
    /// <summary>
    /// The internal id of the school year
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Name of the school year
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The start and end of the school year
    /// </summary>
    [JsonProperty("dateRange")]
    public DateRange Range { get; set; }
}