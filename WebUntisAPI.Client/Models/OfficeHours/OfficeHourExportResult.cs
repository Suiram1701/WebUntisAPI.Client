using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.OfficeHours;

/// <summary>
/// Represents the state result of an office hour export.
/// </summary>
public class OfficeHourExportResult
{
    /// <summary>
    /// Indicates whether the file is public accessible.
    /// </summary>
    [JsonProperty("isPublic")]
    public bool IsPublic { get; set; }

    /// <summary>
    /// Indicates whether the file is ready to get downloaded.
    /// </summary>
    [JsonProperty("finished")]
    public bool IsFinished { get; set; }

    /// <summary>
    /// Indicates whether an error happened while preparing the file.
    /// </summary>
    [JsonProperty("error")]
    public bool Error { get; set; }

    /// <summary>
    /// The displayed name of the file (without file extension).
    /// </summary>
    [JsonProperty("reportName")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The file extension of the file.
    /// </summary>
    [JsonProperty("format")]
    public string Format {  get; set; } = string.Empty;
}
