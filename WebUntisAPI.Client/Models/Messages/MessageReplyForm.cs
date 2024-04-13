using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represents the reply form of a message
/// </summary>
public class MessageReplyForm
{
    /// <summary>
    /// The id of the message to reply
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The subject of the message to reply
    /// </summary>
    [JsonProperty("subject")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// The recipient of the reply (the sender of the message to reply)
    /// </summary>
    [JsonProperty("recipient")]
    public MessagePerson Recipient { get; set; } = new();

    /// <summary>
    /// The history of replies
    /// </summary>
    [JsonProperty("replyHistory")]
    public IEnumerable<ReplyMessage> ReplyHistory { get; set; } = Enumerable.Empty<ReplyMessage>();
}
