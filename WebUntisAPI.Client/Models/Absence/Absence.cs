namespace WebUntisAPI.Client.Models.Absence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebUntisAPI.Client.Converters;
///<summary>
/// Represents an absence record for a student.
/// </summary>
public class Absence
{
    /// <summary>
    /// Unique identifier for the absence record.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The start date of the absence.
    /// </summary>
    [JsonProperty("startDate")]
    [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// The end date of the absence.
    /// </summary>
    [JsonProperty("endDate")]
    [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// The start time of the absence (in HH:mm format).
    /// </summary>
    [JsonProperty("startTime")]
    [Newtonsoft.Json.JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// The end time of the absence (in HH:mm format).
    /// </summary>
    [JsonProperty("endTime")]
    [Newtonsoft.Json.JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// The creation date of the absence record, in Unix timestamp format.
    /// </summary>
    [JsonProperty("createDate")]
    [Newtonsoft.Json.JsonConverter(typeof(UnixMillisJsonConverter))]
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// The last update date of the absence record, in Unix timestamp format.
    /// </summary>
    [JsonProperty("lastUpdate")]
    [Newtonsoft.Json.JsonConverter(typeof(UnixMillisJsonConverter))]
    public DateTimeOffset LastUpdate { get; set; }

    /// <summary>
    /// The user who created the absence record.
    /// </summary>
    [JsonProperty("createdUser")]
    public string CreatedUser { get; set; }

    /// <summary>
    /// The user who last updated the absence record.
    /// </summary>
    [JsonProperty("updatedUser")]
    public string UpdatedUser { get; set; }

    /// <summary>
    /// The reason ID for the absence.
    /// </summary>
    [JsonProperty("reasonId")]
    public int ReasonId { get; set; }

    /// <summary>
    /// The reason description for the absence.
    /// </summary>
    [JsonProperty("reason")]
    public string Reason { get; set; }

    /// <summary>
    /// Additional text or description for the absence.
    /// </summary>
    [JsonProperty("text")]
    public string Text { get; set; }

    /// <summary>
    /// List of interruptions related to the absence.
    /// </summary>
    [JsonProperty("interruptions")]
    public List<object> Interruptions { get; set; }

    /// <summary>
    /// Indicates whether the absence record can be edited.
    /// </summary>
    [JsonProperty("canEdit")]
    public bool CanEdit { get; set; }

    /// <summary>
    /// The name of the student associated with the absence.
    /// </summary>
    [JsonProperty("studentName")]
    public string StudentName { get; set; }

    /// <summary>
    /// The status of the excuse (e.g., "excused" or null).
    /// </summary>
    [JsonProperty("excuseStatus")]
    public string ExcuseStatus { get; set; }

    /// <summary>
    /// Indicates whether the absence is excused.
    /// </summary>
    [JsonProperty("isExcused")]
    public bool IsExcused { get; set; }

    /// <summary>
    /// The excuse object related to the absence.
    /// </summary>
    [JsonProperty("excuse")]
    public Excuse Excuse { get; set; }
}
