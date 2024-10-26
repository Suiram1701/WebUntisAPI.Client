using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Messages;
using Newtonsoft.Json.Linq;
using WebUntisAPI.Client.Exceptions;
using System.Collections.ObjectModel;
using WebUntisAPI.Client.Models.Interfaces;
using System.IO;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Extensions;
using System.Net.Mime;
using WebUntisAPI.Client.Models.Profile;

namespace WebUntisAPI.Client;

partial class WebUntisClient
{
    /// <summary>
    /// Get all by WebUntis supported languages
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<WebUntisLanguage>> GetWebUntisLanguagesAsync(CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        string responseString = await InternalApiRequestAsync("/WebUntis/api/profile/languages", ct);
        return JObject.Parse(responseString)["data"]!["languages"]!.ToObject<IEnumerable<WebUntisLanguage>>()!;
    }

    /// <summary>
    /// Get the account configuration for a user (the configuration consist only out of one property)
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account configuration</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<AccountConfig> GetAccountConfigAsync(CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        string responseString = await InternalApiRequestAsync("/WebUntis/api/profile/config", ct);
        return JObject.Parse(responseString)["data"]!.ToObject<AccountConfig>()!;
    }

    /// <summary>
    /// Get the general information about the current signed in account
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The information</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<GeneralAccountInfo> GetGeneralAccountInfoAsync(CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        string responseString = await InternalApiRequestAsync("/WebUntis/api/profile/general", ct);

        JToken dataToken = JObject.Parse(responseString)["data"]!;
        GeneralAccountInfo accountInfo = dataToken["profile"]!.ToObject<GeneralAccountInfo>()!;
        accountInfo.PasswordChangeAllowed |= dataToken["pwChangeAllowed"]!.Value<bool>()!;     // there two properties that indicates whether it is allowed to change the password

        return accountInfo;
    }

    /// <summary>
    /// Get the contact details for the specified account
    /// </summary>
    /// <param name="user">The user whose <see cref="ContactDetails"/> should be requested</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The contact details and the permissions the signed in user has to the details</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<(AccessPermissions permissions, ContactDetails? contactDetails)> GetContactDetailsAsync(IUser user, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        UriBuilder uriBuilder = new(Session!.ServerUri)
        {
            Path = "/WebUntis/api/profile/contactdetails",
            Query = $"personId={user.Id}&isRequestForStudent={false}"     // idk why isRequestForStudent must set to false also when the request where send by a student but when I set it to true I get always 'wrong' data
        };
        string responseString = await InternalApiRequestAsync(uriBuilder.Uri, ct);
        JToken dataToken = JObject.Parse(responseString)["data"]!;

        AccessPermissions permissions = dataToken.ToObject<AccessPermissions>()!;
        if (!permissions.Read)
            return (permissions, null);

        ContactDetails contactDetails = dataToken["address"]!.ToObject<ContactDetails>()!;
        return (permissions, contactDetails);
    }

    /// <summary>
    /// Get the profile image of the specified user.
    /// </summary>
    /// <param name="user">The user of the image to get.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>
    /// Information about the users image.
    /// </returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<ProfileImage> GetProfileImageAsync(IUser user, CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        UriBuilder uriBuilder = new(Session!.ServerUri)
        {
            Path = "/WebUntis/api/profile/image",
            Query = $"type={(int)user.GetElementType()}&id={user.Id}"
        };
        string responseString = await InternalApiRequestAsync(uriBuilder.Uri, ct);

        JToken dataToken = JObject.Parse(responseString)["data"]!;
        int categoryId = dataToken["categoryId"]!.Value<int>()!;
        int imageId = dataToken["imageId"]!.Value<int>()!;
        AccessPermissions permissions = dataToken.ToObject<AccessPermissions>()!;

        if (imageId == -1)     // user has no image
        {
            return new() { Permissions = permissions }; 
        }

        if (!permissions.Read)     // user doesn't have access
        {
            return new() { Permissions = permissions };
        }

        UriBuilder imageUriBuilder = new(Session!.ServerUri)
        {
            Path = "/WebUntis/image.do",
            Query = $"cat={categoryId}&id={imageId}"
        };
        using HttpResponseMessage response = await _client.GetAsync(imageUriBuilder.Uri, ct);
        response.EnsureSuccessStatusCode();

        return new()
        {
            Permissions = permissions,
            ImageMimeType = response.Content.Headers.ContentType,
            ImageStream = await response.Content.ReadAsStreamAsync(ct)
        };
    }

    /// <summary>
    /// Get data for different ways to access the account of the logged in user.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The access data</returns> 
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<AccessData> GetAccessDataAsync(CancellationToken ct = default)
    {
        ThrowWhenNotAvailable();

        string responseString = await InternalApiRequestAsync("/WebUntis/api/profile/access", ct);
        AccessData data = JObject.Parse(responseString)["data"]!.ToObject<AccessData>()!;
        data.TotpCredentials.ServerName = Session!.ServerUri.Host;

        return data;
    }
}