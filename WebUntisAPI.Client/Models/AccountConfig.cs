using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// Account configuration details
/// </summary>
public class AccountConfig
{
    /// <summary>
    /// Indicates whether the user is authorized to read his contact details
    /// </summary>
    [JsonProperty("canReadContactDetail")]
    public bool CanReadContactDetails { get; set; } = false;
}