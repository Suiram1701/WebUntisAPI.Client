using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// General information about a account
    /// </summary>
    public class GeneralAccount
    {
        /// <summary>
        /// The name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user group of the user
        /// </summary>
        public string UserGroup { get; set; }

        /// <summary>
        /// The id of the role of the user
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// The name of the department (An empty value means the user has not department)
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// The language code of the user
        /// </summary>
        /// <remarks>
        /// That are no normal language codes. You must use the values from <see cref="WebUntisClient.GetAvailableLanguagesAsync(System.Threading.CancellationToken)"/>
        /// </remarks>
        public string LanguageCode { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The max open bookings
        /// </summary>
        public int EffectiveMaxBookings { get; set; }

        /// <summary>
        /// The current open bookings
        /// </summary>
        public int OpenBookings { get; set; }

        /// <summary>
        /// Forward the messages to the users <see cref="Email"/>
        /// </summary>
        public bool ForwardMessageToMail { get; set; }

        /// <summary>
        /// Notifications from the task and ticket system
        /// </summary>
        public bool UserTaskNotifications { get; set; }

        /// <summary>
        /// Is a password change allowed
        /// </summary>
        public bool PasswordChangeAllowed { get; set; }

        /// <summary>
        /// Forward system mails
        /// </summary>
        public bool SystemMailForwarding { get; set; }

        /// <summary>
        /// The gender of the user
        /// </summary>
        public Gender UserGender { get; set; }

        /// <summary>
        /// The count of items on the start page
        /// </summary>
        public int ItemsOnStartPage { get; set; }

        /// <summary>
        /// Show the lessons of the day
        /// </summary>
        public bool ShowLessonsOfDay { get; set; }

        /// <summary>
        /// Show the next day periods
        /// </summary>
        public bool ShowNextDayPeriods { get; set; }

        internal static GeneralAccount ReadFromJson(JsonReader reader)
        {
            JObject data = JObject.Load(reader);
            JObject profile = data["profile"].Value<JObject>();

            return new GeneralAccount
            {
                Name = profile["name"].Value<string>(),
                UserGroup = profile["userGroup"].Value<string>(),
                RoleId = profile["userRoleId"].Value<int>(),
                Department = profile["department"].Value<string>(),
                LanguageCode = profile["languageCode"].Value<string>(),
                Email = profile["email"].Value<string>(),
                EffectiveMaxBookings = profile["effectiveMaxBookings"].Value<int>(),
                OpenBookings = profile["openBookings"].Value<int>(),
                ForwardMessageToMail = profile["forwardMessageToEmail"].Value<bool>(),
                UserTaskNotifications = profile["userTaskNotifications"].Value<bool>(),
                PasswordChangeAllowed = data["pwChangeAllowed"]?.Value<bool>() ?? profile["pwChangeAllowed"].Value<bool>(),
                SystemMailForwarding = profile["systemMailForwarding"].Value<bool>(),
                UserGender = profile["gender"].ToObject<Gender>(),
                ItemsOnStartPage = profile["itemOnStartPage"].Value<int>(),
                ShowLessonsOfDay = profile["showLessonsOfDay"].Value<bool>(),
                ShowNextDayPeriods = profile["showNextDayPeriods"].Value<bool>()
            };
        }

        /// <summary>
        /// A user gender
        /// </summary>
        public class Gender
        {
            /// <summary>
            /// The gender id
            /// </summary>
            [JsonProperty("id")]
            public int Id { get; set; }

            /// <summary>
            /// The long label
            /// </summary>
            [JsonProperty("longLabel")]
            public string LongLabel { get; set; }

            /// <summary>
            /// the short label
            /// </summary>
            [JsonProperty("shortLabel")]
            public string ShortMale { get; set; }

            /// <summary>
            /// The icon
            /// </summary>
            [JsonProperty("icon")]
            public string Icon { get; set; }
        }
    }
}