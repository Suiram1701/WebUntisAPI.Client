using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// Represents a platform application linked with WebUntis.
/// </summary>
[DebuggerDisplay($"{{{nameof(Name)},nq}}")]
public class PlatformApp
{
    /// <summary>
    /// The id of the platform app.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The displayed name of the platform app.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The name of the icon asset.
    /// </summary>
    /// <remarks>
    /// The icon is available under the following url: <![CDATA[https://<server>/assets/platform-app-logos/<asset name>]]>
    /// </remarks>
    [JsonProperty("icon")]
    public string IconAssetName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether this platform app should be opened in a new tab.
    /// </summary>
    [JsonProperty("openInNewTab")]
    public bool OpenInNewTab { get; set; }

    /// <summary>
    /// The url to open when the app should be opened.
    /// </summary>
    [JsonProperty("redirectUrl")]
    public Uri AppUrl { get; set; }

    /// <summary>
    /// The url to call to sign out.
    /// </summary>
    [JsonProperty("logoutUrl")]
    public Uri SignOutUrl { get; set; }
}
