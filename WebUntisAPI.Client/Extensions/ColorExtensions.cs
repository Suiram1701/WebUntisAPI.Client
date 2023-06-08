using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WebUntisAPI.Client.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="Color"/> struct
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Convert a color instance to the Hexadeciaml format: RRGGBB
        /// </summary>
        /// <param name="color"></param>
        /// <returns>The color string</returns>
        public static string ToHexColorFormat(this Color color)
        {
            return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        /// <summary>
        /// Convert a color string to the Hexadecimal format: RRGGBB
        /// </summary>
        /// <param name="color"></param>
        /// <param name="colorString">The string to convert</param>
        /// <returns>The created color</returns>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="colorString"/> isn't in the correct format</exception>
        public static Color FromHexColorFormat(this Color color, string colorString)
        {
            if (!Regex.IsMatch(colorString, @"[\da-fA-F]{6}"))
                throw new ArgumentException("The color string isn't in a correct format", nameof(colorString));
                
            // Convert the hex string to the single color channels
            int red = Convert.ToByte(colorString.Substring(0, 2), 16);
            int green = Convert.ToByte(colorString.Substring(2, 2), 16);
            int blue = Convert.ToByte(colorString.Substring(4, 2), 16);

            return Color.FromArgb(red, green, blue);
        }
    }

}
