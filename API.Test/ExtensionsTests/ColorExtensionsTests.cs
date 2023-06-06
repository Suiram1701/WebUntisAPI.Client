using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Extensions;

namespace API.Test.ExtensionsTests;

[TestFixture]
internal class ColorExtensionsTests
{
    [Test]
    public void ToHexColorFormat()
    {
        // Example 1
        Color c1 = Color.FromArgb(200, 150, 255);
        Assert.That(c1.ToHexColorFormat(), Is.EqualTo("C896FF"));

        // Example 2
        Color c2 = Color.FromArgb(10, 16, 0);
        Assert.That(c2.ToHexColorFormat(), Is.EqualTo("0A1000"));
    }

    [Test]
    public void FromHexColorFormat()
    {
        // Example 1
        Color c1 = Color.FromArgb(200, 150, 255);
        Assert.That(new Color().FromHexColorFormat("C896FF"), Is.EqualTo(c1));

        // Example 2
        Color c2 = Color.FromArgb(10, 16, 0);
        Assert.That(new Color().FromHexColorFormat("0A1000"), Is.EqualTo(c2));
    }
}
