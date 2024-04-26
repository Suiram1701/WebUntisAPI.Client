using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Converters;

internal class MasterDataSchoolYearsJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(IEnumerable<SchoolYear>).IsAssignableFrom(objectType);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JToken elementArray = JToken.Load(reader);

        if (elementArray.Type != JTokenType.Array)
            throw new JsonReaderException("An array were expected.");

        Collection<SchoolYear> years = new();
        foreach (JToken element in elementArray)
        {
            DateOnly startDate = element["startDate"]!.ToObject<DateOnly>();
            DateOnly endDate = element["endDate"]!.ToObject<DateOnly>()!;

            SchoolYear schoolYear = element.ToObject<SchoolYear>()!;
            schoolYear.Range = new(startDate, endDate);

            years.Add(schoolYear);
        }

        return years;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        // never get called
        throw new NotImplementedException();
    }
}
