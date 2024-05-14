using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.OfficeHours;

namespace API.Test;

[TestFixture]
internal class OfficeHours
{
    [Test]
    public async Task GetSettingsAsync()
    {
        OfficeHoursSettings settings = await SetUp.Client.GetOfficeHoursSettingsAsync();
        Assert.That(settings, Is.Not.Null);
    }

    [Test]
    public async Task GetClassesAsync()
    {
        IEnumerable<OfficeHourClass> classes = await SetUp.Client.GetOfficeHourClassesAsync();

        Assert.That(classes, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(classes.Select(@class => @class.Id), Is.Unique);
            Assert.That(classes.Select(@class => @class.Name), Is.Unique);
        });
    }

    [Test]
    public async Task GetOfficeHoursAsync()
    {
        IEnumerable<OfficeHour> hours = await SetUp.Client.GetOfficeHoursAsync();

        Assert.That(hours, Is.Not.Null);
        Assert.That(hours.Select(hour => hour.Id), Is.Unique);
    }
}
