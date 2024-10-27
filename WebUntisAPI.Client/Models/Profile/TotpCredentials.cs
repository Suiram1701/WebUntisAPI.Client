using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Profile;

/// <summary>
/// Represents totp credentials for 2fa access to a user's account.
/// </summary>
public class TotpCredentials
{
    /// <summary>
    /// The username of the account whose credentials are represented.
    /// </summary>
    [JsonProperty("user")]
    public string User { get; set; } = default!;

    /// <summary>
    /// The host name of the server.
    /// </summary>
    public string ServerName { get; set; } = default!;

    /// <summary>
    /// A base32 string that is the secret of the totp generation.
    /// </summary>
    [JsonProperty("secret")]
    public string Secret { get; set; } = default!;

    /// <summary>
    /// Indicates whether 2fa is required to use.
    /// </summary>
    [JsonProperty("isTotpRequired")]
    public bool TotpRequired { get; set; }

    /// <summary>
    /// Indicates whether the user has 2fa enabled.
    /// </summary>
    [JsonIgnore]
    public bool IsEnabled => !string.IsNullOrEmpty(Secret);

    /// <summary>
    /// Creates an uri that contains the totp credential of this instance that can be used to create a qr code for an authenticator app.
    /// </summary>
    /// <returns>The uri</returns>
    /// <exception cref="ArgumentException"></exception>
    public Uri CreateQrCodeUri()
    {
        if (Uri.CheckHostName(ServerName) is UriHostNameType.Unknown or UriHostNameType.Basic)
            throw new ArgumentException("The server name have to be a valid host name.", nameof(ServerName));

        string uriString = $"otpauth://totp/{User}@{ServerName}?secret={Secret}";
        return new Uri(uriString, UriKind.Absolute);
    }
}