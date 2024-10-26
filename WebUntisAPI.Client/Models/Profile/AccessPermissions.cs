using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Profile;

/// <summary>
/// Represents access permissions a user has to a specified resource
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public struct AccessPermissions
{
    /// <summary>
    /// Indicates whether read access is available
    /// </summary>
    [JsonProperty("read")]
    public bool Read { get; set; }

    /// <summary>
    /// Indicates whether write access is available
    /// </summary>
    [JsonProperty("write")]
    public bool Write { get; set; }

    private readonly string GetDebuggerDisplay()
    {
        if (Read && Write)
            return "R/W";
        else if (Read)
            return "R";
        else if (Write)
            return "W";
        else
            return "None";
    }
}
