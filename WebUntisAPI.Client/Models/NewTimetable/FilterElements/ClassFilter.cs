using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.NewTimetable.FilterElements;

/// <summary>
/// Represents a filter for a class.
/// </summary>
[DebuggerDisplay($"{{{nameof(Class)}}}")]
public class ClassFilter : ITimetableFilterElement
{
    /// <summary>
    /// The class this filter filters for.
    /// </summary>
    [JsonProperty("class")]
    public Class Class { get; set; } = default!;

    /// <summary>
    /// First class teacher of the class.
    /// </summary>
    /// <remarks>
    /// When <c>null</c> this position isn't occupied.
    /// </remarks>
    [JsonProperty("classTeacher1")]
    public Teacher? ClassTeacher1 { get; set; }

    /// <summary>
    /// Second class teacher of the class.
    /// </summary>
    /// <remarks>
    /// When <c>null</c> this position isn't occupied.
    /// </remarks>
    [JsonProperty("classTeacher2")]
    public Teacher? ClassTeacher2 { get; set; }

    IElement ITimetableFilterElement.Element => Class;
}
