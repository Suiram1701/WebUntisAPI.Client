using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WebUntisAPI.Client.Converters;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// A holiday
/// </summary>
[DebuggerDisplay($"Name: {{{nameof(Name)},nq}}")]
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
    /// The long name (user displayed) name of the holiday
    /// </summary>
    [JsonProperty("longName")]
    public string LongName { get; set; } = string.Empty;

    /// <summary>
    /// The start date of the holiday
    /// </summary>
    [JsonProperty("startDate")]
    public DateTime StartDate { get; set; }

    [DebuggerHidden]
    [JsonProperty("start")]
    [SuppressMessage("CodeQuality", "IDE0051", Justification = "Member used for JSON deserialization.")]
    private DateTime Start
    {
        set => StartDate = value;
    }

    /// <summary>
    /// The end date of the holiday
    /// </summary>
    [JsonProperty("endDate")]
    public DateTime EndDate { get; set; }

    [DebuggerHidden]
    [JsonProperty("end")]
    [SuppressMessage("CodeQuality", "IDE0051", Justification = "Member used for JSON deserialization.")]
    private DateTime End
    {
        set => EndDate = value;
    }

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