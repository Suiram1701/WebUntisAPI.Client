using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    [JsonConverter(typeof(TimetableRequestModelJsonConverter))]
    internal struct TimetableRequestModel
    {
        /// <summary>
        /// Id of that what you want too request
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The type of what you want to request
        /// </summary>
        /// <remarks>
        /// The meanings:
        ///     <list type="bullet">
        ///         <item>1 = Class</item>
        ///         <item>2 = teacher</item>
        ///         <item>3 = subject</item>
        ///         <item>4 = room</item>
        ///         <item>5 = student</item>
        ///     </list>
        /// </remarks>
        public int Type { get; set; }

        /// <summary>
        /// Start date of requested informations
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of the requested informations
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
