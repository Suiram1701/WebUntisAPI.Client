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

// JsonConverter is instead of JsonConverter<T> used here because the generic one doesn't work (idk why).
internal class MasterDataSchoolYearsJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(IEnumerable<SchoolYear>).IsAssignableFrom(objectType);
    }

    public override IEnumerable<SchoolYear>? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        JToken elementArray = JToken.Load(reader);
        if (elementArray.Type != JTokenType.Array)
        {
            throw new JsonReaderException("An array were expected.");
        }

        foreach (JToken element in elementArray)
        {
            if (element.Type != JTokenType.Object)
            {
                throw new JsonSerializationException("An object were expected.");
            }

            DateOnly startDate = element["startDate"]!.ToObject<DateOnly>();
            DateOnly endDate = element["endDate"]!.ToObject<DateOnly>()!;

            SchoolYear schoolYear = element.ToObject<SchoolYear>()!;
            schoolYear.Range = new(startDate, endDate);

            yield return schoolYear;
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not IEnumerable<SchoolYear> schoolYears)
        {
            throw new JsonSerializationException($"This converter can't convert other objects than {nameof(IEnumerable<SchoolYear>)}.");
        }

        writer.WriteStartArray();
        foreach (SchoolYear schoolYear in schoolYears)
        {
            // Every property have to written manually because the serializer would also serialize the Range property.
            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(schoolYear.Id);

            writer.WritePropertyName("name");
            writer.WriteValue(schoolYear.Name);

            writer.WritePropertyName("startDate");
            writer.WriteValue(schoolYear.Range.Start.ToString("yyyy-MM-dd"));

            writer.WritePropertyName("endDate");
            writer.WriteValue(schoolYear.Range.End.ToString("yyyy-MM-dd"));

            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }
}
