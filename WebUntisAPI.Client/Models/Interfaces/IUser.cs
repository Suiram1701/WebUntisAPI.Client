using Newtonsoft.Json;
using WebUntisAPI.Client.Models.Elements;

namespace WebUntisAPI.Client.Models.Interfaces;

/// <summary>
/// A user
/// </summary>
public interface IUser : IElement
{
    /// <summary>
    /// the forename of the user
    /// </summary>
    [JsonProperty("forename")]
    public string ForeName { get; set; }
}