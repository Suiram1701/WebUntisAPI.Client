using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUntisAPI.Client.Models;

/// <summary>
/// A language supported by WebUntis
/// </summary>
[DebuggerDisplay("{Name,nq}")]
public class WebUntisLanguage
{
    /// <summary>
    /// The short key of the language
    /// </summary>
    /// <remarks>
    /// These code is usualy in the format of ISO 639-1 or a combination of ISO 639-1 and ISO 3166-1. Normally is the combination in the format of 'de-AT' but here is it given as 'deAT'
    /// </remarks>
    [JsonProperty("key")]
    public string Key { get; }

    /// <summary>
    /// The long name of the language in a human readable form in this language
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; }

    /// <summary>
    /// Creates a new instance
    /// </summary>
    /// <param name="key">The short key</param>
    /// <param name="name">The long name</param>
    public WebUntisLanguage(string key, string name)
    {
        Key = key;
        Name = name;
    }

    /// <summary>
    /// Tries to get the <see cref="CultureInfo"/> that is represented by <see cref="Key"/>
    /// </summary>
    /// <param name="culture">The <see cref="CultureInfo"/> that were read</param>
    /// <returns>Indicates whether <paramref name="culture"/> contains a value</returns>
    public bool TryGetCulture(out CultureInfo? culture)
    {
        if (Key.Length == 2)
        {
            culture = new(Key);
            return true;
        }

        if (Key.Length == 4)
        {
            string code = Key.Insert(2, "-");
            culture = new(code);

            return true;
        }

        culture = null;
        return false;
    }

    /// <summary>
    /// Get the <see cref="CultureInfo"/> that is represented by <see cref="Key"/>
    /// </summary>
    /// <returns>The read culture</returns>
    /// <exception cref="FormatException"></exception>
    public CultureInfo GetCulture()
    {
        bool success = TryGetCulture(out CultureInfo? culture);
        if (!success)
            throw new FormatException("The speicifed key isn't in a culture code valid format.");

        return culture!;
    }
}
