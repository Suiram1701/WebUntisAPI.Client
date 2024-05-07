using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models.Messages;

/// <summary>
/// Represents a report of a sent message
/// </summary>
public readonly struct SentMessageState
{
    /// <summary>
    /// The count of recipients
    /// </summary>
    public int NumberOfRecipients { get; }

    /// <summary>
    /// The count of CC recipients
    /// </summary>
    /// <remarks>
    /// When <c>null</c> are no CC recipients available
    /// </remarks>
    public int? NumberOfCCRecipients { get; }

    /// <summary>
    /// Creates a new instance
    /// </summary>
    /// <param name="numberOfRecipients">The count of recipients</param>
    /// <param name="numberOfCCRecipients">The count of cc recipients</param>
    [JsonConstructor]
    public SentMessageState(int numberOfRecipients, int? numberOfCCRecipients)
    {
        NumberOfRecipients = numberOfRecipients;
        NumberOfCCRecipients = numberOfCCRecipients;
    }
}
