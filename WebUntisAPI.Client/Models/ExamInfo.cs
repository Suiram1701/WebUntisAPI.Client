using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// Represents information about an exam
/// </summary>
public class ExamInfo
{
    /// <summary>
    /// The id of the exam
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The name of the exam
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The date of the exam
    /// </summary>
    [JsonProperty("date")]
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Date { get; set; }

    /// <summary>
    /// Idk
    /// </summary>
    [JsonProperty("markSchemaId")]
    public int MarkSchemaId { get; set; }
}
