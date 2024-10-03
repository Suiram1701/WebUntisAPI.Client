using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client;

/// <summary>
/// A WebUntis session
/// </summary>
public class WebUntisSession
{
    /// <summary>
    /// The user that's account is associated with this session.
    /// </summary>
    public IUser User => _user;
    internal IUser _user = default!;

    /// <summary>
    /// The date time where this session was initially created.
    /// </summary>
    /// <remarks>
    /// <see cref="IssuedTime"/> and <see cref="ExpiresTime"/> will update on session renew.
    /// This value will persist through all session updates.
    /// </remarks>
    public DateTime CreatedTime { get; init; }

    /// <summary>
    /// The date time where this session was issued.
    /// </summary>
    public DateTimeOffset IssuedTime => _issuedTime;
    private DateTimeOffset _issuedTime;

    /// <summary>
    /// The date time where this session will expire.
    /// </summary>
    public DateTimeOffset ExpiresTime => _expiresTime;
    private DateTimeOffset _expiresTime;

    /// <summary>
    /// The url this server of this session has.
    /// </summary>
    public Uri ServerUri { get; init; } = default!;

    internal string AuthorizationBearer
    {
        get => _authorizationBearer;
        init => _authorizationBearer = value;
    }
    private string _authorizationBearer = default!;

    internal bool UpdateJwtBearer(string bearer)
    {
        if (string.IsNullOrWhiteSpace(bearer))
        {
            throw new ArgumentNullException(nameof(bearer));
        }

        // determine whether a new jwt was returned
        string[] jwtParts = bearer.Split('.');
        bool isJwt = jwtParts.Length == 3 && jwtParts.Take(2).All(p => p.StartsWith("ey"));

        if (isJwt)
        {
            _authorizationBearer = bearer;
            byte[] jwtContentPartB;
            try
            {
                jwtContentPartB = Convert.FromBase64String(bearer.Split('.')[1]);

            }
            catch (FormatException)
            {
                string base64Url = bearer.Split('.')[1];

                //Base64Url -> Base64
                string base64 = base64Url.Replace('-', '+').Replace('_', '/');

                // Add Padding if needed
                switch (base64.Length % 4)
                {
                    case 2:
                        base64 += "==";
                        break;
                    case 3:
                        base64 += "=";
                        break;
                }

                jwtContentPartB = Convert.FromBase64String(base64);
            }

            // Parse the returned jwt for iss and exp
            string jwtContentPart = Encoding.UTF8.GetString(jwtContentPartB);
            JObject obj = JObject.Parse(jwtContentPart);

            long iatSeconds = obj["iat"]!.Value<long>();
            _issuedTime = DateTimeOffset.FromUnixTimeSeconds(iatSeconds);

            long expSeconds = obj["exp"]!.Value<long>();
            _expiresTime = DateTimeOffset.FromUnixTimeSeconds(expSeconds);
        }

        return isJwt;
    }
}
