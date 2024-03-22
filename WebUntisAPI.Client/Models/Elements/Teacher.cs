using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.Elements;

/// <summary>
/// A teacher
/// </summary>
public class Teacher : ElementBase, IUser
{
    /// <inheritdoc/>
    public string ForeName { get; set; } = string.Empty;

    /// <summary>
    /// An extern key
    /// </summary>
    [JsonProperty("externKey")]
    public string ExternKey { get; set; } = string.Empty;
}