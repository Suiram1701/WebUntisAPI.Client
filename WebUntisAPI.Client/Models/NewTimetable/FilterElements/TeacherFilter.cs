using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.NewTimetable.FilterElements;

/// <summary>
/// Represents a filter for a teacher.
/// </summary>
public class TeacherFilter : ITimetableFilterElement
{
    /// <summary>
    /// The teacher object.
    /// </summary>
    [JsonProperty("teacher")]
    public Teacher Teacher { get; set; } = new();

    /// <summary>
    /// The departments the teacher is a member of.
    /// </summary>
    [JsonProperty("departments")]
    public IEnumerable<object> Department { get; set; } = Enumerable.Empty<object>();

    IElement ITimetableFilterElement.Element => Teacher;
}
