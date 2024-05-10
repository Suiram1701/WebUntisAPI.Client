using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// Provides additional information to the stream of a profile image
/// </summary>
public class ProfileImageInfo
{
    /// <summary>
    /// The access permissions the signed in user has to the image
    /// </summary>
    public AccessPermissions Permissions { get; set; }

    /// <summary>
    /// Indicates whether the user has an image
    /// </summary>
    public bool HasImage { get; set; }

    /// <summary>
    /// The MIME type of the image
    /// </summary>
    /// <remarks>
    /// <c>null</c> when the user doesn't have an image
    /// </remarks>
    public MediaTypeHeaderValue? ImageMimeType { get; set; }
}
