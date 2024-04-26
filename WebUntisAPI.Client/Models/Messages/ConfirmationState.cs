using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represent the state of a confirmation of a message
/// </summary>
public class ConfirmationState
{
    /// <summary>
    /// The count of people that confirmed the message
    /// </summary>
    [JsonProperty("confirmedRequestCount")]
    public int ConfirmedRequestCount { get; set; }

    /// <summary>
    /// The count of people that recivied the message
    /// </summary>
    [JsonProperty("totalRequestCount")]
    public int TotalRequestCount { get; set; }
}