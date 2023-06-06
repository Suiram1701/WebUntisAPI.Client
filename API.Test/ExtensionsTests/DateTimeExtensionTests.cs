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
        string E1_Date_Res = "2023-05-31";
        string E1_Time_Res = "2000";
        Assert.Multiple(() =>
        {
            Assert.That(E1_Date, Is.EqualTo(E1_Date_Res));
            Assert.That(E1_Time, Is.EqualTo(E1_Time_Res));
        });

        // Example 2 (True)
        new DateTime(2019, 12, 2, 7, 40, 0).ToWebUntisTimeFormat(out string E2_Date, out string E2_Time);
        string E2_Date_Res = "2019-12-02";
        string E2_Time_Res = "740";
        Assert.Multiple(() =>
        {
            Assert.That(E2_Date, Is.EqualTo(E2_Date_Res));
            Assert.That(E2_Time, Is.EqualTo(E2_Time_Res));
        });
    }

    [Order(2)]
    [Test]
    public static void FromWebUntisTimeFormatTest()
    {
        // Example 1
        string E1_Date = "2023-05-31";
        string E1_Time = "2000";
        DateTime? E1_DateTime = new DateTime().FromWebUntisTimeFormat(E1_Date, E1_Time);
        Assert.That(new DateTime(2023, 5, 31, 20, 0, 0), Is.EqualTo(E1_DateTime));

        // Example 2
        string E2_Date = "2023-06-01";
        string E2_Time = "740";
        DateTime? E2_DateTime = new DateTime().FromWebUntisTimeFormat(E2_Date, E2_Time);
        Assert.That(new DateTime(2023, 6, 1, 7, 40, 0), Is.EqualTo(E2_DateTime));
    }
}
