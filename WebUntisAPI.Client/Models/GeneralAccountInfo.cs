using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// General information about an account
/// </summary>
public class GeneralAccountInfo
{
    /// <summary>
    /// The name of the user
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The user group of the user
    /// </summary>
    [JsonProperty("userGroup")]
    public string UserGroup { get; set; } = string.Empty;

    /// <summary>
    /// The role of the user
    /// </summary>
    public ElementType UserRole
    {
        get => (ElementType)UserRoleValue;
        set => UserRoleValue = (int)value;
    }
    [JsonProperty("userRoleId")]
    private int UserRoleValue { get; set; }

    /// <summary>
    /// The name of the department (an empty value means the user isn't part of any department)
    /// </summary>
    [JsonProperty("department")]
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// The language code of the language the user uses
    /// </summary>
    /// <remarks>
    /// These code is usualy in the format of ISO 639-1 or a combination of ISO 639-1 and ISO 3166-1. Normally is the combination in the format of 'de-AT' but here is it given as 'deAT'.
    /// This code is equal to a <see cref="WebUntisLanguage.Key"/> of a by <see cref="WebUntisClient.GetWebUntisLanguagesAsync(System.Threading.CancellationToken)"/> returned instance.
    /// </remarks>
    [JsonProperty("languageCode")]
    public string LanguageCode { get; set; } = string.Empty;

    /// <summary>
    /// The email of the user
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The max open bookings
    /// </summary>
    [JsonProperty("effectiveMaxBookings")]
    public int EffectiveMaxBookings { get; set; }

    /// <summary>
    /// The current open bookings
    /// </summary>
    [JsonProperty("openBookings")]
    public int OpenBookings { get; set; }

    /// <summary>
    /// Indicates whether messages should be forwarded to the users <see cref="Email"/>
    /// </summary>
    [JsonProperty("forwardMessageToEmail")]
    public bool ForwardMessageToMail { get; set; }

    /// <summary>
    /// Indicates whether the user should recive notifications from the task and ticket system
    /// </summary>
    [JsonProperty("userTaskNotifications")]
    public bool UserTaskNotifications { get; set; }

    /// <summary>
    /// Indicates whether the user is allowed to change his password by his own
    /// </summary>
    [JsonProperty("pwChangeAllowed")]
    public bool PasswordChangeAllowed { get; set; }

    /// <summary>
    /// Indicates whether system mails should be forwarded to the user
    /// </summary>
    [JsonProperty("systemMailForwarding")]
    public bool SystemMailForwarding { get; set; }

    /// <summary>
    /// The count of items on the start page
    /// </summary>
    [JsonProperty("itemOnStartPage")]
    public int ItemsOnStartPage { get; set; }

    /// <summary>
    /// Indicates whether the lessons of the day should be shown
    /// </summary>
    [JsonProperty("showLessonsOfDay")]
    public bool ShowLessonsOfDay { get; set; }

    /// <summary>
    /// Indicates whether the periods of the next day shoul be shown
    /// </summary>
    [JsonProperty("showNextDayPeriods")]
    public bool ShowNextDayPeriods { get; set; }
}