using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models.Interfaces;

/// <summary>
/// Student or teacher
/// </summary>
public interface IUser
{
    /// <summary>
    /// Id of the user
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The name of the user
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// the forename of the user
    /// </summary>
    [JsonProperty("forename")]
    public string ForeName { get; set; }

    /// <summary>
    /// The last name of the user
    /// </summary>
    [JsonProperty("longName")]
    public string LongName { get; set; }

    /// <summary>
    /// The displayed name of the user
    /// </summary>
    [JsonProperty("displayname")]
    public string Displayname { get; set; }

    /// <summary>
    /// An alternative name of the user
    /// </summary>
    [JsonProperty("alternatename")]
    public string Alternatename { get; set; }

    /// <summary>
    /// Indicates whether the user that requeted this instance is allowed to request the timetable for this element
    /// </summary>
    [JsonProperty("canViewTimetable")]
    public bool CanViewTimetable { get; set; }
}