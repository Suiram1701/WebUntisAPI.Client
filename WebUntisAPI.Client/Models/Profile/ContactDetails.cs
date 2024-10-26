using Newtonsoft.Json;

namespace WebUntisAPI.Client.Models.Profile;

/// <summary>
/// Contact information about a user
/// </summary>
public class ContactDetails
{
    /// <summary>
    /// The person id (not the same user id that you get from <see cref="WebUntisSession.User"/>)
    /// </summary>
    [JsonProperty("personId")]
    public int PersonId { get; set; }

    /// <summary>
    /// The person type id (not the same id that you get from <see cref="WebUntisSession.User"/>)
    /// </summary>
    [JsonProperty("personType")]
    public int PersonType { get; set; }

    /// <summary>
    /// The id of this instance
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// The name
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The addressant these information are for
    /// </summary>
    [JsonProperty("addressType")]
    public string AddressType { get; set; } = string.Empty;

    /// <summary>
    /// Maybe the id for <see cref="AddressType"/>
    /// </summary>
    [JsonProperty("addressTypeValue")]
    public int AddressTypeValue { get; set; }

    /// <summary>
    /// The street
    /// </summary>
    [JsonProperty("street")]
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// The city
    /// </summary>
    [JsonProperty("city")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// The post code
    /// </summary>
    [JsonProperty("postCode")]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// The country
    /// </summary>
    [JsonProperty("country")]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// The email
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The phone number
    /// </summary>
    [JsonProperty("phone")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// The mobile number
    /// </summary>
    [JsonProperty("mobile")]
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    /// The fax number
    /// </summary>
    [JsonProperty("fax")]
    public string FaxNumber { get; set; } = string.Empty;
}
