using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages.Confirmation;

/// <summary>
/// Represents the state of a sent confirmation message.
/// </summary>
public struct SentMessageConfirmationState
{
    /// <summary>
    /// The count of the recipients that confirmed the message.
    /// </summary>
    [JsonProperty("confirmedRequestCount")]
    public int ConfirmedRequestCount { get; set; }

    /// <summary>
    /// The count of people that received the message.
    /// </summary>
    [JsonProperty("totalRequestCount")]
    public int TotalRequestCount { get; set; }
}