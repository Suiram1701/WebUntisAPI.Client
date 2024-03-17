using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// An webuntis error
/// </summary>
public class WebUntisError
{
    /// <summary>
    /// The code of the error
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// The main title of the error
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Creates a new instance 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="title"></param>
    public WebUntisError(string code, string title)
    {
        Code = code;
        Title = title;
    }
}