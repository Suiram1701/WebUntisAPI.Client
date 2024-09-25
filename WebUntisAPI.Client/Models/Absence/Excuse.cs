namespace WebUntisAPI.Client.Models.Absence;

using Newtonsoft.Json;
using System;
using WebUntisAPI.Client.Converters;
/// <summary>
/// Represents the excuse details related to an absence.
/// </summary>
public class Excuse
{
    /// <summary>
    /// Unique identifier for the excuse record.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Additional text or description for the excuse.
    /// </summary>
    [JsonProperty("text")]
    public string Text { get; set; }

    /// <summary>
    /// The date the excuse was created, in Unix timestamp format.
    /// </summary>
    [JsonProperty("excuseDate")]
    [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly ExcuseDate { get; set; }

    /// <summary>
    /// The status of the excuse (e.g., "excused").
    /// </summary>
    [JsonProperty("excuseStatus")]
    public string ExcuseStatus { get; set; }

    /// <summary>
    /// Indicates whether the excuse is valid and the absence is excused.
    /// </summary>
    [JsonProperty("isExcused")]
    public bool IsExcused { get; set; }

    /// <summary>
    /// The user ID of the person who created the excuse record.
    /// </summary>
    [JsonProperty("userId")]
    public int UserId { get; set; }

    /// <summary>
    /// The username of the person who created the excuse record.
    /// </summary>
    [JsonProperty("username")]
    public string Username { get; set; }
}
