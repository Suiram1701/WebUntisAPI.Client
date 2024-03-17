using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace WebUntisAPI.Client.Models.Elements;

/// <summary>
/// A teacher
/// </summary>
public class Teacher : ElementBase, IUser
{
    /// <summary>
    /// An extern key
    /// </summary>
    [JsonProperty("externKey")]
    public string ExternKey { get; set; } = string.Empty;
}