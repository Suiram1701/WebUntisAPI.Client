using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client.Exceptions;

/// <summary>
/// An error that the WebUntis server returned
/// </summary>
public class WebUntisException : Exception
{
    /// <summary>
    /// The errors returned by the server
    /// </summary>
    public IEnumerable<WebUntisError> Errors { get; }

    /// <summary>
    /// A new <see cref="WebUntisException"/>
    /// </summary>
    /// <param name="errors">A collection of the errors</param>
    public WebUntisException(IEnumerable<WebUntisError> errors) : base(BuildMessageFromErrors(errors))
    {
        Errors = errors;
    }

    internal WebUntisException(JArray errorArray)
    {
        Collection<WebUntisError> errors = new();
        foreach (JToken token in errorArray)
        {
            string code = token["code"]!.Value<string>()!;
            string title = token["title"]!.Value<string>()!;

            errors.Add(new(code, title));
        }

        Errors = errors;
    }

    private static string BuildMessageFromErrors(IEnumerable<WebUntisError> errors)
    {
        StringBuilder builder = new();
        foreach (WebUntisError error in errors)
        {
            builder.Append(error.Code);
            builder.Append(": ");
            builder.AppendLine(error.Title);
        }

        return builder.ToString();
    }
}