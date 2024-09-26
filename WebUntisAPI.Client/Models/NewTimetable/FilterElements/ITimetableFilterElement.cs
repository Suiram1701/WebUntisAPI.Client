using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.NewTimetable.FilterElements;

/// <summary>
/// An interface that provides the functionality to filter for an element.
/// </summary>
public interface ITimetableFilterElement
{
    /// <summary>
    /// The element the to filter for.
    /// </summary>
    [JsonIgnore]
    [DebuggerHidden]
    IElement Element { get; }
}
