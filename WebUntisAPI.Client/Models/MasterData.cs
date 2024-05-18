using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converters;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;
using WebUntisAPI.Client.Models.Timetable;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// A collection general purpose data get with in a sign in
/// </summary>
public class MasterData
{
    /// <summary>
    /// The timestamp of the time where these data were requested
    /// </summary>
    [JsonProperty("timeStamp")]
    [JsonConverter(typeof(UnixMillisJsonConverter))]
    public DateTimeOffset TimeStamp { get; set; }

    /// <summary>
    /// The holidays of the school
    /// </summary>
    [JsonProperty("holidays")]
    public IEnumerable<Holiday> Holidays { get; set; } = Enumerable.Empty<Holiday>();

    /// <summary>
    /// The classes of the school
    /// </summary>
    [JsonProperty("klassen")]
    public IEnumerable<Class> Classes { get; set; } = Enumerable.Empty<Class>();

    /// <summary>
    /// The rooms of the school
    /// </summary>
    [JsonProperty("rooms")]
    public IEnumerable<Room> Rooms { get; set; } = Enumerable.Empty<Room>();

    /// <summary>
    /// The subjects of the school
    /// </summary>
    [JsonProperty("subjects")] 
    public IEnumerable<Subject> Subjects { get; set; } = Enumerable.Empty<Subject>();

    /// <summary>
    /// The teachers of the school
    /// </summary>
    [JsonProperty("teachers")]
    public IEnumerable<Teacher> Teachers { get; set; } = Enumerable.Empty<Teacher>();

    /// <summary>
    /// The schoolyears of the school
    /// </summary>
    [JsonProperty("schoolyears", ItemConverterType = typeof(MasterDataSchoolYearJsonConverter))]
    public IEnumerable<SchoolYear> SchoolYears { get; set; } = Enumerable.Empty<SchoolYear>();

    /// <summary>
    /// The timegrid used for the school
    /// </summary>
    /// <remarks>
    /// The property layout of this object is kindly different than the data returned from <see cref="WebUntisClient.GetTimeGridAsync(CancellationToken)"/> but both represent the same data.
    /// </remarks>
    [JsonProperty("timeGrid")]
    [JsonConverter(typeof(MasterDataTimegridJsonConverter))]
    public ReadOnlyDictionary<DayOfWeek, IEnumerable<SchoolHour>> Timegrid { get; set; } = new(new Dictionary<DayOfWeek, IEnumerable<SchoolHour>>());
}
