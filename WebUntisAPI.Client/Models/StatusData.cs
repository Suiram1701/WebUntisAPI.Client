using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Information about lesson types and period codes and their colors
    /// </summary>
    [JsonConverter(typeof(StatusDataJsonConverter))]
    public class StatusData
    {
        /// <summary>
        /// Colors for a normal lesson
        /// </summary>
        public ForeBackColors LsColors { get; set; }

        /// <summary>
        /// Colors for a office hour
        /// </summary>
        public ForeBackColors OhColors { get; set; }

        /// <summary>
        /// Colors for a standby
        /// </summary>
        public ForeBackColors SbColors { get; set; }

        /// <summary>
        /// Colors for a break supervision
        /// </summary>
        public ForeBackColors BsColors { get; set; }

        /// <summary>
        /// Colors for a examination
        /// </summary>
        public ForeBackColors ExColors { get; set; }

        /// <summary>
        /// Colors for cancelled lessons
        /// </summary>
        public ForeBackColors CancelledLessonColors { get; set; }

        /// <summary>
        /// Colors for irregular lessons
        /// </summary>
        public ForeBackColors IrregularLessonColors { get; set; }

        /// <summary>
        /// Get the <see cref="ForeBackColors"/> from the lesson type
        /// </summary>
        /// <param name="lessonType">The lesson type</param>
        /// <returns>The colors</returns>
        public ForeBackColors GetLessonTypeColor(LessonType lessonType) =>
            (ForeBackColors)GetType().GetProperties().First(p => p.Name.ToLower() == (lessonType.ToString() + "Colors").ToLower()).GetValue(this);

        /// <summary>
        /// Get the <see cref="ForeBackColors"/> from the lesson type
        /// </summary>
        /// <param name="code">The code</param>
        /// <returns>The colors. <see langword="null"/> when the code was <see cref="Code.None"/></returns>
        public ForeBackColors? GetCodeColor(Code code) =>
            GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == (code.ToString() + "LessonColors").ToLower())?.GetValue(this) as ForeBackColors?;
    }
}
