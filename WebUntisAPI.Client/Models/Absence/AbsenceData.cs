namespace WebUntisAPI.Client.Models.Absence;
using Newtonsoft.Json;
using System.Collections.Generic;
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
