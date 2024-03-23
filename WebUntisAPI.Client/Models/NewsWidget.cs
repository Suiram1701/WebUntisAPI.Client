using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// All news for a day
/// </summary>
public class NewsWidget
{
    /// <summary>
    /// A system message
    /// </summary>
    [JsonProperty("systemMessage")]
    public NewsMessage? SystemMessage { get; set; }

    /// <summary>
    /// All messages for the day
    /// </summary>
    [JsonProperty("messagesOfDay")]
    public IEnumerable<NewsMessage> MessagesOfTheDay { get; set; } = Array.Empty<NewsMessage>();

    /// <summary>
    /// The relative url where the RSS is available
    /// </summary>
    [JsonProperty("rssUrl")]
    public string RssUrl { get; set; } = string.Empty;
}