using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebUntisAPI.Client.Converters;

/// <summary>
/// Contains the main data object which includes a list of absences and related information.
/// </summary>
public class AbsenceData
{
    /// <summary>
    /// List of absences related to the student.
    /// </summary>
    [JsonProperty("absences")]
    public List<Absence> Absences { get; set; }

    /// <summary>
    /// List of possible absence reasons.
    /// </summary>
    [JsonProperty("absenceReasons")]
    public List<object> AbsenceReasons { get; set; }

    /// <summary>
    /// Statuses related to excuses.
    /// </summary>
    [JsonProperty("excuseStatuses")]
    public object ExcuseStatuses { get; set; }

    /// <summary>
    /// Indicates whether the absence reason can be changed.
    /// </summary>
    [JsonProperty("showAbsenceReasonChange")]
    public bool ShowAbsenceReasonChange { get; set; }

    /// <summary>
    /// Indicates whether new absence records can be created.
    /// </summary>
    [JsonProperty("showCreateAbsence")]
    public bool ShowCreateAbsence { get; set; }
}

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

/// <summary>
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
