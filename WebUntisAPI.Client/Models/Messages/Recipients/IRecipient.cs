using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages.Recipients;

/// <summary>
/// Represents basic information about a recipient user.
/// </summary>
public interface IRecipient
{
    /// <summary>
    /// The internal id of the recipient
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The displayed name of the recipient
    /// </summary>
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    /// <summary>
    /// The url of the profile image of the recipient.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     When <c>null</c> the user doesn't own a profile image.
    /// </para>
    /// <para>
    ///     You can get the image with a normal HttpClient without any authorization.
    /// </para>
    /// </remarks>
    [JsonProperty("imageUrl")]
    public Uri? ImageUrl { get; set; }

    /// <summary>
    /// The name of the class this user is a member of.
    /// </summary>
    /// <remarks>
    /// When <c>null</c> the user isn't a member of a class.
    /// </remarks>
    [JsonProperty("className")]
    public string? ClassName { get; set; }
}
