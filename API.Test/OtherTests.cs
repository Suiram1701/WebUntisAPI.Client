using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;

namespace API.Test;

[TestFixture]
internal class OtherTests
{
    [Test]
    public async Task GetPlatformAppsAsync()
    {
        IEnumerable<PlatformApp> platformApps = await SetUp.Client.GetPlatformApplicationsAsync();

        Assert.That(platformApps, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(platformApps.Select(app => app.Id), Is.Unique);
            Assert.That(platformApps.Select(app => app.Name), Is.Unique);
            Assert.That(platformApps.Select(app => app.AppUrl), Is.Unique);
            Assert.That(platformApps.Select(app => app.SignOutUrl), Is.Unique);
        });
    }
}
