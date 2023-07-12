using Newtonsoft.Json;
using System;
using WebUntisAPI.Client.Converter;

namespace WebUntisAPI.Client.Models.Messages
{
    /// <summary>
    /// Informations about a confirmed message
    /// </summary>
    public class ConfirmationInformations
    {
        /// <summary>
        /// Is it allowed to delete the message
        /// </summary>
        [JsonProperty("allowMessageDeletion")]
        public bool AllowMessageDeletion { get; set; }

        /// <summary>
        /// Is it allowed to send a request confirmation
        /// </summary>
        [JsonProperty("allowSendRequestConfirmation")]
        public bool AllowSendRequestConfirmation { get; set; }

        /// <summary>
        /// The datetime where you confirmed the message
        /// </summary>
        [JsonProperty("confirmationDate")]
        [JsonConverter(typeof(APIDateTimeJsonConverter))]
        public DateTime ConfirmationDate { get; set; }

        /// <summary>
        /// The name of the user that confirmed hte message
        /// </summary>
        [JsonProperty("confirmerUserDisplayName")]
        public string ConfirmerUserName { get; set; }

        /// <summary>
        /// The id of the user that confirmed the message (this id isn't the id if from <see cref="IUser.Id"/>)
        /// </summary>
        [JsonProperty("confirmerUserId")]
        public int ConfirmerUserId { get; set; }

        /// <summary>
        /// Is a reply of the message allowed
        /// </summary>
        [JsonProperty("isReplyAllowed")]
        public bool IsReplyAllowed { get; set; }
    }
}
