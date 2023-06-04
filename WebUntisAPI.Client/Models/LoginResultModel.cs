using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// The result model of the authenticate method
    /// </summary>
    internal struct LoginResultModel
    {
        /// <summary>
        /// Sesson of the login
        /// </summary>
        public string sessionId;

        /// <summary>
        /// Type of the person
        /// </summary>
        public int personType;

        /// <summary>
        /// Is of the person
        /// </summary>
        public int personId;
    }
}