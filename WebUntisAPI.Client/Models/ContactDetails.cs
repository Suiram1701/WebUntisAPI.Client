using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models
{
    /// <summary>
    /// Contact information about a user
    /// </summary>
    public class ContactDetails
    {
        /// <summary>
        /// The person id
        /// </summary>
        [JsonProperty("personId")]
        public int PersonId { get; set; }

        /// <summary>
        /// The person type id
        /// </summary>
        [JsonProperty("personType")]
        public int PersonType { get; set; }

        /// <summary>
        /// The user id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The user name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// For which person are this information
        /// </summary>
        [JsonProperty("addressType")]
        public string AddressType { get; set; }

        /// <summary>
        /// The id for <see cref="AddressType"/>
        /// </summary>
        [JsonProperty("addressTypeValue")]
        public int AddressTypeValue { get; set; }

        /// <summary>
        /// The street
        /// </summary>
        [JsonProperty("street")]
        public string Street { get; set; }

        /// <summary>
        /// The city
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// The post code
        /// </summary>
        [JsonProperty("postCode")]
        public string PostCode { get; set; }

        /// <summary>
        /// The country
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// The email
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The phone number
        /// </summary>
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The mobile number
        /// </summary>
        [JsonProperty("mobile")]
        public string MobileNumber { get; set; }

        /// <summary>
        /// The fax number
        /// </summary>
        [JsonProperty("fax")]
        public string FaxNumber { get; set; }
    }
}
