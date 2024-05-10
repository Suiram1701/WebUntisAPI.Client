using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client;

partial class WebUntisClient
{
    /// <summary>
    /// Gets the available platform applications that are linked to WebUntis.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The available platform apps.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<PlatformApp>> GetPlatformApplicationsAsync(CancellationToken ct = default)
    {
        string responseString = await InternalApiRequestAsync("/WebUntis/api/rest/view/v1/app/platform-application/menus", ct);
        return JsonConvert.DeserializeObject<IEnumerable<PlatformApp>>(responseString)!;
    }
}
