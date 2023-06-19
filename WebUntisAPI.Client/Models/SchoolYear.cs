using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// A school year
    /// </summary>
    [DebuggerDisplay("Name: {Name, nq}")]
    public class SchoolYear : IEquatable<SchoolYear>, IComparable<SchoolYear>
    {
        /// <summary>
        /// Id of the school year
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the school year
        /// </summary>
        /// <remarks>
        /// Example value: 2022/2023
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// The date where the school year begins
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The date where the school year ends
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <inheritdoc/>
        public override string ToString() => Name;

        #region IEquatable<SchoolYear>
        /// <inheritdoc/>
        public bool Equals(SchoolYear other) => Id.Equals(other.Id);
        #endregion

        #region IComparable<SchoolYear>
        /// <inheritdoc/>
        public int CompareTo(SchoolYear other)
        {
            if (StartDate.Year > other.StartDate.Year)
                return 1;
            else if (StartDate.Year < other.StartDate.Year)
                return -1;
            else
                return 0;
        }
        #endregion
    }
}