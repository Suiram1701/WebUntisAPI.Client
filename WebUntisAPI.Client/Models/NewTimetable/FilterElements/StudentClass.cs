using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Elements;

namespace WebUntisAPI.Client.Models.NewTimetable.FilterElements;

/// <summary>
/// A class a student is a member of.
/// </summary>
[DebuggerDisplay($"{{{nameof(Class)}}}")]
public class StudentClass
{
    /// <summary>
    /// The represented class.
    /// </summary>
    [JsonProperty("class")]
    public Class Class { get; set; } = default!;

    /// <summary>
    /// The date range in that this class is valid.
    /// </summary>
    [JsonProperty("dateRange")]
    public DateRange DateRange { get; set; }

    /// <summary>
    /// The department this class is a member of.
    /// </summary>
    [JsonProperty("department")]
    public object? Department { get; set; }
}
