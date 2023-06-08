using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Converter
{
    /// <summary>
    /// Convert the teachers in webuntis get classes response to int array
    /// </summary>
    public sealed class ClassJsonConverter : JsonConverter<Class>
    {
        ///// <inheritdoc/>
        //public override int[] ReadJson(JsonReader reader, Type objectType, int[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        //{
        //    // Setup
        //    JObject classObject = JObject.Load(reader);
        //    List<int> teachers = new List<int>();

        //    // Read values
        //    int index = 1;
        //    while (classObject["teacher" + index++]?.ToObject<int>() is int teacher)
        //        teachers.Add(teacher);

        //    return teachers.ToArray();
        //}

        ///// <inheritdoc/>
        //public override void WriteJson(JsonWriter writer, int[] value, JsonSerializer serializer)
        //{
        //    JObject teacherObject = new JObject();

        //    for (int i = 0; i < value.Length; i++)
        //        teacherObject.Add("teacher" + (i + 1), value[i]);

        //    teacherObject.WriteTo(writer);
        //}

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
