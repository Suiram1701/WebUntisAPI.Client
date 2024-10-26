using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// Provides additional information to the stream of a profile image
/// </summary>
public class ProfileImage : IDisposable
{
    private bool disposedValue;

    /// <summary>
    /// The access permissions the signed in user has to the image
    /// </summary>
    public AccessPermissions Permissions { get; init; }

    /// <summary>
    /// The MIME type of the image
    /// </summary>
    /// <remarks>
    /// <c>null</c> when the user doesn't have an image
    /// </remarks>
    public MediaTypeHeaderValue? ImageMimeType { get; init; }

    /// <summary>
    /// The stream that contains the image data.
    /// </summary>
    /// <remarks>
    /// <c>null</c> when the user doesn't have an image <br />
    /// The image data will be in one of these formats: .tiff, .jfif, .bmp, .gif, .svg, .png, .webp, .svgz, .jpg, .jpeg, .ico, .xbm, .dib, .pjp, .apng, .tif, .pjpeg or .avif.
    /// </remarks>
    public Stream? ImageStream { get; init; }

    /// <summary>
    /// Indicates whether the user has a profile image.
    /// </summary>
    /// <returns>The result</returns>
    public bool HasImage() => ImageMimeType is not null && ImageStream is not null;

    /// <summary>
    /// Disposes the current object.
    /// </summary>
    /// <param name="disposing">Dispose managed objects</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                ImageStream?.Dispose();
            }

            disposedValue = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
