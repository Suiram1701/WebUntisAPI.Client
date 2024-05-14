using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.OfficeHours;

/// <summary>
/// Represents the settings of the office hours view.
/// </summary>
public class OfficeHoursSettings
{
    /// <summary>
    /// Indicates whether the number of the office hour should be shown.
    /// </summary>
    [JsonProperty("showHourNumber")]
    public bool ShowHourNumber { get; set; }

    /// <summary>
    /// Indicates whether the photo should be shown.
    /// </summary>
    [JsonProperty("showPhoto")]
    public bool ShowPhoto { get; set; }

    /// <summary>
    /// Indicates whether the room should be shown.
    /// </summary>
    [JsonProperty("showRoom")]
    public bool ShowRoom { get; set; }

    /// <summary>
    /// Indicates whether the email should be shown.
    /// </summary>
    [JsonProperty("showEmail")]
    public bool ShowEmail { get; set; }

    /// <summary>
    /// Indicates whether the phone number should be shown.
    /// </summary>
    [JsonProperty("showPhone")]
    public bool ShowPhone { get; set; }

    /// <summary>
    /// The message that should be shown on no appointment.
    /// </summary>
    [JsonProperty("noAppointmentMsg")]
    public string NoAppointmentMessage { get; set; } = string.Empty;

    /// <summary>
    /// Custom text
    /// </summary>
    [JsonProperty("customText")]
    public string CustomText { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the school
    /// </summary>
    [JsonProperty("schoolPhone")]
    public string SchoolPhone { get; set; } = string.Empty;

    /// <summary>
    /// idk
    /// </summary>
    [JsonProperty("anonymous")]
    public bool Anonymous { get; set; }

    /// <summary>
    /// Indicates whether its allowed to register a new office hour.
    /// </summary>
    [JsonProperty("allowRegistration")]
    public bool AllowRegistration { get; set; }

    /// <summary>
    /// Indicates whether its allowed to show the registration status of an office hour.
    /// </summary>
    [JsonProperty("showRegistrationStatus")]
    public bool ShowRegistrationStatus { get; set; }
}
