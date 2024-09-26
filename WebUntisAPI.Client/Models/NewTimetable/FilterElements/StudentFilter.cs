using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.NewTimetable.FilterElements;

/// <summary>
/// Represents a filter for a student.
/// </summary>
[DebuggerDisplay($"{{{nameof(Student)}}}")]
public class StudentFilter : ITimetableFilterElement
{
    /// <summary>
    /// The student this filter filters for.
    /// </summary>
    [JsonProperty("student")]
    public Student Student { get; set; } = new();

    /// <summary>
    /// Classes this student is a member of.
    /// </summary>
    /// <remarks>
    /// <c>@class</c> is the class the student is a member of, <c>dateRange</c> is the date range where class exists and <c>department</c> is the department the class is a member of.
    /// </remarks>
    [JsonProperty("classes")]
    public IEnumerable<StudentClass> Classes { get; set; } = Enumerable.Empty<StudentClass>();

    IElement ITimetableFilterElement.Element => Student;
}