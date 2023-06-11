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
        public ForeBackColors LessonColors { get; set; }

        /// <summary>
        /// idk
        /// </summary>
        public ForeBackColors OhColors { get; set; }

        /// <summary>
        /// idk
        /// </summary>
        public ForeBackColors SbColors { get; set; }

        /// <summary>
        /// idk
        /// </summary>
        public ForeBackColors BsColors { get; set; }

        /// <summary>
        /// Colors for cancelled lessons
        /// </summary>
        public ForeBackColors CancelledLessonColors { get; set; }

        /// <summary>
        /// Colors for irregular lessons
        /// </summary>
        public ForeBackColors IrregularLessonColors { get; set; }
  
    }
}
