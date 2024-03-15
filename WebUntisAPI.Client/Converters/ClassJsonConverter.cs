using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Converter
{
    internal class ClassJsonConverter : JsonConverter<Class>
    {
        /// <inheritdoc/>
        public override Class ReadJson(JsonReader reader, Type objectType, Class existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject classObject = JObject.Load(reader);
            Class @class = new Class()
            {
                Id = classObject["id"].Value<int>(),
                Name = classObject["name"].Value<string>(),
                LongName = classObject["longName"].Value<string>(),
                Active = classObject["active"].Value<bool>()
            };

            int index = 0;
            List<int> teachers = new List<int>();
            while (classObject["teacher" + ++index] is JToken teacher)
                teachers.Add(teacher.ToObject<int>());

            @class.Teachers = teachers.ToArray();
            return @class;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, Class value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("id");     // Write Class.Id
            writer.WriteValue(value.Id);
            writer.WritePropertyName("name");     // Write Class.Name
            writer.WriteValue(value.Name);
            writer.WritePropertyName("longName");     // Wite Class.LongName
            writer.WriteValue(value.LongName);
            writer.WritePropertyName("active");     // Write Class.Active
            writer.WriteValue(value.Active);

            // Write teachers
            for (int i = 0; i < value.Teachers.Length; i++)
            {
                writer.WritePropertyName("teacher" + (i + 1));
                writer.WriteValue(value.Teachers[i]);
            }
        }
    }
}
