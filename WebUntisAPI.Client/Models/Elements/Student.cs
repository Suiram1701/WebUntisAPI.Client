using Newtonsoft.Json;
using System.Diagnostics;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Models.Elements;

/// <summary>
/// A student
/// </summary>
public class Student : ElementBase, IUser
{
    /// <inheritdoc/>
    public string ForeName { get; set; } = string.Empty;
}