using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Model for the param of the login
    /// </summary>
    internal struct LoginModel
    {
        /// <summary>
        /// Username to login
        /// </summary>
        public string user;

        /// <summary>
        /// Password for login
        /// </summary>
        public string password;

        /// <summary>
        /// Unique identifier for the client app
        /// </summary>
        public string client;
    }
}
