using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Elements;

namespace WebUntisAPI.Client.Models.Interfaces;

/// <summary>
/// An interface for untis internal elements
/// </summary>
public interface IElement
{
    /// <summary>
    /// The id of this element
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The short name of this element
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// The long name of the element
    /// </summary>
    [JsonProperty("longName")]
    public string LongName { get; set; }

    /// <summary>
    /// The usually displayed name of the element
    /// </summary>
    [JsonProperty("displayname")]
    public string Displayname { get; set; }

    /// <summary>
    /// The alternative name of the element
    /// </summary>
    [JsonProperty("alternatename")]
    public string Alternatename { get; set; }

    /// <summary>
    /// Indicates whether the user that requeted this instance is allowed to request the timetable for this element
    /// </summary>
    [JsonProperty("canViewTimetable")]
    public bool CanViewTimetable { get; set; }

    /// <summary>
    /// The capacity
    /// </summary>
    [JsonProperty("roomCapacity")]
    public int RoomCapacity { get; set; }
}
