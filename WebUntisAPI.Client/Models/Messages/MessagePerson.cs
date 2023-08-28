using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WebUntisAPI.Client.Models.Messages
{
    /// <summary>
    /// The user profile at untis messenges
    /// </summary>
    [DebuggerDisplay("{DisplayName, nq}")]
    public class MessagePerson
    {
        /// <summary>
        /// The name of the class from the user
        /// </summary>
        [JsonProperty("className")]
        public string ClassName { get; set; } = null;

        /// <summary>
        /// Tags of the person
        /// </summary>
        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// The role of the person
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// The displayed name of the user
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The url of the profile image
        /// </summary>
        /// <remarks>
        /// When the value is <see langword="null"/> then the default view is the first letter of the <see cref="DisplayName"/>
        /// </remarks>
        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; } = null;

        /// <summary>
        /// The id of the user
        /// </summary>
        [JsonProperty("userId")]
        public int Id { get; set; }
    }
}
