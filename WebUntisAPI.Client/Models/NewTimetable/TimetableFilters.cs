using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.NewTimetable.FilterElements;

namespace WebUntisAPI.Client.Models.NewTimetable;

/// <summary>
/// Represents every available filter elements that could be used to request a timetable.
/// </summary>
/// <remarks>
/// Some collections of available filters are <see cref="IEnumerable{Object}"/>. I doesn't know the scheme after that these objects are build.
/// If you have any information about this it would be nice if you create an issue of the repository of this package.
/// </remarks>
public class TimetableFilters
{
    /// <summary>
    /// The resource type that were used to request these data.
    /// </summary>
    [JsonProperty("resourceType")]
    public ElementType ResourceType { get; set; }

    /// <summary>
    /// Buildings that could be filtered for.
    /// </summary>
    /// <remarks>
    /// Not yet implemented.
    /// </remarks>
    [JsonProperty("buildings")]
    public IEnumerable<object> Buildings { get; set; } = Enumerable.Empty<object>();

    /// <summary>
    /// Departments that could be filtered for.
    /// </summary>
    /// <remarks>
    /// Not yet implemented.
    /// </remarks>
    [JsonProperty("departments")]
    public IEnumerable<object> Departments { get; set; } = Enumerable.Empty<object>();

    /// <summary>
    /// Room groups that could be filtered for.
    /// </summary>
    [JsonProperty("roomGroups")]
    public IEnumerable<object> RoomGroups { get; set; } = Enumerable.Empty<object>();

    /// <summary>
    /// Resource types that could be filtered for.
    /// </summary>
    /// <remarks>
    /// Not yet implemented.
    /// </remarks>
    [JsonProperty("resourceTypes")]
    public IEnumerable<object> ResourceTypes { get; set; } = Enumerable.Empty<object>();

    /// <summary>
    /// Assignment groups that could be filtered for.
    /// </summary>
    [JsonProperty("assignmentGroups")]
    public IEnumerable<object> AssignmentGroups { get;set; } = Enumerable.Empty<object>();

    /// <summary>
    /// Classes that could be filtered for.
    /// </summary>
    [JsonProperty("classes")]
    public IEnumerable<ClassFilter> Classes { get; set; } = Enumerable.Empty<ClassFilter>();

    /// <summary>
    /// Resources that could be filtered for.
    /// </summary>
    [JsonProperty("resources")]
    public IEnumerable<object> Resources { get; set; } = Enumerable.Empty<object>();

    /// <summary>
    /// Rooms that could be filtered for.
    /// </summary>
    [JsonProperty("rooms")]
    public IEnumerable<RoomFilter> Rooms { get; set; } = Enumerable.Empty<RoomFilter>();

    /// <summary>
    /// Subjects that could be filtered for.
    /// </summary>
    [JsonProperty("subjects")]
    public IEnumerable<SubjectFilter> Subjects { get; set; } = Enumerable.Empty<SubjectFilter>();

    /// <summary>
    /// Students that could be filtered for.
    /// </summary>
    [JsonProperty("students")]
    public IEnumerable<StudentFilter> Students { get; set; } = Enumerable.Empty<StudentFilter>();

    /// <summary>
    /// Teachers that could filtered for.
    /// </summary>
    [JsonProperty("teachers")]
    public IEnumerable<TeacherFilter> Teachers { get; set; } = Enumerable.Empty<TeacherFilter>();
}
