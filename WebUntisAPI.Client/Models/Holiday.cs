using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// A holiday
/// </summary>
[DebuggerDisplay("Name: {Name,nq}")]
public class Holiday : IEquatable<Holiday>, IComparable<Holiday>
{
    /// <summary>
    /// The id of the holiday
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Short name of the holiday
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The start date of the holiday
    /// </summary>
    [JsonProperty("start")] 
    public DateTime StartDate { get; set; }

    /// <summary>
    /// The end date of the holiday
    /// </summary>
    [JsonProperty("end")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Is the holiday bookable
    /// </summary>
    [JsonProperty("bookable")]
    public bool Bookable { get; set; }

    /// <inheritdoc/>
    public int CompareTo(Holiday? other)
    {
        if (other is null)
            return 1;
        return StartDate.CompareTo(other.StartDate);
    }

    /// <inheritdoc/>
    public bool Equals(Holiday? other)
    {
        if (other is null)
            return false;
        return Id == other.Id;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        Equals(obj as Holiday);

    /// <inheritdoc/>
    public override int GetHashCode() =>
        base.GetHashCode();
}