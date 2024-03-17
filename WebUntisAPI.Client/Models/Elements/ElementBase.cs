using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Elements;

/// <summary>
/// The base class for untis internal elements
/// </summary>
[DebuggerDisplay("{ToString(),nq}")]
public abstract class ElementBase
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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The long name of the element
    /// </summary>
    [JsonProperty("longName")]
    public string LongName { get; set; } = string.Empty;

    /// <summary>
    /// The usually displayed name of the element
    /// </summary>
    [JsonProperty("displayname")]
    public string Displayname { get; set; } = string.Empty;

    /// <summary>
    /// The alternative name of the element
    /// </summary>
    [JsonProperty("alternatename")]
    public string Alternatename { get; set; } = string.Empty;

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

    /// <summary>
    /// Determines the <see cref="ElementType"/> that is assigned to this instance
    /// </summary>
    /// <returns>The type</returns>
    public ElementType GetElementType()
    {
        return GetType().Name switch
        {
            nameof(Class) => ElementType.Class,
            nameof(Teacher) => ElementType.Teacher,
            nameof(Subject) => ElementType.Subject,
            nameof(Room) => ElementType.Room,
            nameof(Student) => ElementType.Student,
            _ => throw new NotImplementedException($"The element typ {GetType().Name} isn't implemented.")
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (!string.IsNullOrEmpty(Displayname))
            return Displayname;
        if (!string.IsNullOrEmpty(Alternatename))
            return Alternatename;

        return Name;
    }
}
