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
/// Represents a filter for a room.
/// </summary>
[DebuggerDisplay($"{{{nameof(Room)}}}")]
public class RoomFilter : ITimetableFilterElement
{
    /// <summary>
    /// The room this filter filters for.
    /// </summary>
    public Room Room { get; set; } = default!;

    /// <summary>
    /// The capacity of this room.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// The room groups this room is a part of.
    /// </summary>
    public IEnumerable<object> RoomGroups { get; set; } = Enumerable.Empty<object>();

    /// <summary>
    /// The building this room is in.
    /// </summary>
    public Building Building { get; set; } = default!;

    /// <summary>
    /// The department this room is a member of.
    /// </summary>
    public object? Department { get; set; }

    IElement ITimetableFilterElement.Element => Room;
}
