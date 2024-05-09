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

internal class MasterDataSchoolYearJsonConverter : JsonConverter<SchoolYear>
{
    public override SchoolYear? ReadJson(JsonReader reader, Type objectType, SchoolYear? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            throw new JsonSerializationException("A json object were expected.");
        }

        JObject obj = JObject.Load(reader);
        SchoolYear schoolYear = obj.ToObject<SchoolYear>()!;

        DateOnly start = obj["startDate"]!.ToObject<DateOnly>();
        DateOnly end = obj["endDate"]!.ToObject<DateOnly>();
        schoolYear.Range = new(start, end);

        return schoolYear;
    }

    public override void WriteJson(JsonWriter writer, SchoolYear? value, JsonSerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        writer.WriteStartObject();

        // Every property is written manually because when it were serialized with JsonSerializer also DateRange will serialized.
        writer.WritePropertyName("id");
        writer.WriteValue(value.Id);

        writer.WritePropertyName("name");
        writer.WriteValue(value.Name);

        writer.WritePropertyName("startDate");
        writer.WriteValue(value.Range.Start.ToString("yyyy-MM-dd"));

        writer.WritePropertyName("endDate");
        writer.WriteValue(value.Range.End.ToString("yyyy-MM-dd"));

        writer.WriteEndObject();
    }
}
