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
/// Represents a filter for a subject.
/// </summary>
[DebuggerDisplay($"{{{nameof(Subject)}}}")]
public class SubjectFilter : ITimetableFilterElement
{
    /// <summary>
    /// The subject this filter filteres for.
    /// </summary>
    [JsonProperty("subject")]
    public Subject Subject { get; set; } = default!;

    IElement ITimetableFilterElement.Element => Subject;
}
