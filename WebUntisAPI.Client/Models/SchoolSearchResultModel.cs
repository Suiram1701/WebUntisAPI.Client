using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A model for the school search result
    /// </summary>
    internal struct SchoolSearchResultModel
    {
        /// <summary>
        /// Idk
        /// </summary>
        public int size;

        /// <summary>
        /// All schools found
        /// </summary>
        public SchoolModel[] schools;
    }
}
