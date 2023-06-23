using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Converter
{
    /// <summary>
    /// A converter for the get status data response
    /// </summary>
    internal class StatusDataJsonConverter : JsonConverter<StatusData>
    {
        public override StatusData ReadJson(JsonReader reader, Type objectType, StatusData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Setup json object
            JObject statusDataObject = JObject.Load(reader);
            IEnumerable<JObject> lessonTypes = statusDataObject["lstypes"].Values<JObject>();
            IEnumerable<JObject> codes = statusDataObject["codes"].Values<JObject>();

            return new StatusData
            {
                LsColors = lessonTypes.First(obj => obj.ContainsKey("ls"))["ls"].ToObject<ForeBackColors>(),
                OhColors = lessonTypes.First(obj => obj.ContainsKey("oh"))["oh"].ToObject<ForeBackColors>(),
                SbColors = lessonTypes.First(obj => obj.ContainsKey("sb"))["sb"].ToObject<ForeBackColors>(),
                BsColors = lessonTypes.First(obj => obj.ContainsKey("bs"))["bs"].ToObject<ForeBackColors>(),
                CancelledLessonColors = codes.First(obj => obj.ContainsKey("cancelled"))["cancelled"].ToObject<ForeBackColors>(),
                IrregularLessonColors = codes.First(obj => obj.ContainsKey("irregular"))["irregular"].ToObject<ForeBackColors>()
            };
        }

        public override void WriteJson(JsonWriter writer, StatusData value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("lstypes");
            writer.WriteStartArray();

            writer.WriteStartObject();
            writer.WritePropertyName("ls");
            serializer.Serialize(writer, value.LsColors);
            writer.WriteEndObject();

            writer.WriteStartObject();
            writer.WritePropertyName("oh");
            serializer.Serialize(writer, value.OhColors);
            writer.WriteEndObject();

            writer.WriteStartObject();
            writer.WritePropertyName("sb");
            serializer.Serialize(writer, value.SbColors);
            writer.WriteEndObject();

            writer.WriteStartObject();
            writer.WritePropertyName("bs");
            serializer.Serialize(writer, value.BsColors);
            writer.WriteEndObject();

            writer.WriteEndArray();

            writer.WritePropertyName("codes");
            writer.WriteStartArray();

            writer.WriteStartObject();
            writer.WritePropertyName("cancelled");
            serializer.Serialize(writer, value.CancelledLessonColors);
            writer.WriteEndObject();

            writer.WriteStartObject();
            writer.WritePropertyName("irregular");
            serializer.Serialize(writer, value.IrregularLessonColors);
            writer.WriteEndObject();

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
