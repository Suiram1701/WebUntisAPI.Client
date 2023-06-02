using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Represent a Untis school
    /// </summary>
    public struct SchoolModel
    {
        /// <summary>
        /// Real name of the school
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// Id of the school
        /// </summary>
        public long schoolId { get; set; }

        /// <summary>
        /// Login name of the school
        /// </summary>
        public string loginName { get; set; }

        /// <summary>
        /// Real address of the school
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// Server of the school
        /// </summary>
        public string server { get; set; }

        /// <summary>
        /// Server url of the school
        /// </summary>
        public string serverUrl { get; set; }
    }
}