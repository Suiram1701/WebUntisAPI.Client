using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Extensions;

namespace API.Test.ExtensionsTests;

[Order(1)]
[TestFixture]
internal class DateTimeExtensionTests
{
    [Order(1)]
    [Test]
    public static void ToWebUntisTimeFormatTest()
    {
        // Example 1 (True)
        new DateTime(2023, 5, 31, 20, 0, 0).ToWebUntisTimeFormat(out string E1_Date, out string E1_Time);
        string E1_Date_Res = "20230531";
        string E1_Time_Res = "2000";
        Assert.Multiple(() =>
        {
            Assert.That(E1_Date, Is.EqualTo(E1_Date_Res));
            Assert.That(E1_Time, Is.EqualTo(E1_Time_Res));
        });

        // Example 2 (True)
        new DateTime(2019, 12, 2, 19, 5, 0).ToWebUntisTimeFormat(out string E2_Date, out string E2_Time);
        string E2_Date_Res = "20191202";
        string E2_Time_Res = "1905";
        Assert.Multiple(() =>
        {
            Assert.That(E2_Date, Is.EqualTo(E2_Date_Res));
            Assert.That(E2_Time, Is.EqualTo(E2_Time_Res));
        });

        // Example 3 (False)
        Assert.Throws<FormatException>(() => new DateTime(2022, 12, 31, 24, 0, 0).ToWebUntisTimeFormat(out string E3_Date, out string E3_Time), null);

        // Example 4 (False)
        Assert.Throws<FormatException>(() => new DateTime(2023, 0, 36, 0, 0, 0).ToWebUntisTimeFormat(out string E4_Date, out string E4_Time), null);

        // Example 5
        Assert.Throws<FormatException>(() => new DateTime(2023, 0, 30, 2, 0, 0).ToWebUntisTimeFormat(out string E5_Date, out string E5_Time), null);
    }

    [Order(2)]
    [Test]
    public static void FromWebUntisTimeFormatTest()
    {
        // Example 1
        string E1_Date = "20230531";
        string E1_Time = "2000";
        DateTime E1_DateTime = new DateTime().FromWebUntisTimeFormat(E1_Date, E1_Time);
        Assert.That(new DateTime(2023, 5, 31, 20, 0, 0), Is.EqualTo(E1_DateTime));

        // Example 2
        string E2_Date = "20230601";
        string E2_Time = "1905";
        DateTime E2_DateTime = new DateTime().FromWebUntisTimeFormat(E2_Date, E2_Time);
        Assert.That(new DateTime(2023, 6, 1, 19, 5, 0), Is.EqualTo(E2_DateTime));

        // Example 3
        string E3_Date = "";
        string E3_Time = "";
        DateTime E3_DateTime = new DateTime().FromWebUntisTimeFormat(E3_Date, E3_Time);
        Assert.That(new DateTime(), Is.EqualTo(E3_DateTime));

        // Example 4
        string E4_Date = "";
        string E4_Time = "";
        DateTime E4_DateTime = new DateTime().FromWebUntisTimeFormat(E4_Date, E4_Time);
        Assert.That(new DateTime(), Is.EqualTo(E4_DateTime));

        // Example 5
        string E5_Date = "";
        string E5_Time = "";
        DateTime E5_DateTime = new DateTime().FromWebUntisTimeFormat(E5_Date, E5_Time);
        Assert.That(new DateTime(), Is.EqualTo(E5_DateTime));
    }
}
