using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Profile;

/// <summary>
/// Contains data of different ways to access a user's account and more information about it.
/// </summary>
public class AccessData
{
    /// <summary>
    /// App credentials that can be used to access the account.
    /// </summary>
    /// <remarks>
    /// Usually used by Untis Mobile.
    /// </remarks>
    [JsonProperty("appCredentials")]
    public AppCredentials AppCredentials { get; set; } = default!;

    /// <summary>
    /// Totp credentials for the user that are used for 2fa authentication.
    /// </summary>
    [JsonProperty("totpCredentials")]
    public TotpCredentials TotpCredentials { get; set; } = default!;

    /// <summary>
    /// Data to access the account through an iCal publication.
    /// </summary>
    [JsonProperty("iCalPublication")]
    public ICalPublication ICalPublication { get; set; } = default!;

    /// <summary>
    /// Indicates whether the user is allowed to edit his app credentials.
    /// </summary>
    [JsonProperty("canEditAppCredentials")]
    public bool CanEditAppCredentials { get; set; }

    /// <summary>
    /// Indicates whether the user is allowed to use 2fa via totp.
    /// </summary>
    [JsonProperty("canUseTotp")]
    public bool CanUseTotp { get; set; }
}
