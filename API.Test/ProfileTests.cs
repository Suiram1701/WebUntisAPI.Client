using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.Test.AuthenticationTests;
using NUnit.Framework;
using WebUntisAPI.Client.Models.Interfaces;
using WebUntisAPI.Client.Models.Profile;
using Microsoft.Extensions.Configuration;

namespace API.Test;

[TestFixture]
internal class ProfileTests
{
    [Test]
    public async Task GetSupportedLanguagesAsync()
    {
        IEnumerable<WebUntisLanguage> languages = await SetUp.Client.GetWebUntisLanguagesAsync();

        Assert.Multiple(() =>
        {
            Assert.That(languages.Count(), Is.GreaterThan(1));
            Assert.That(languages.Select(l => l.Key), Is.Unique);
            Assert.That(languages.Select(l => l.Name), Is.Unique);
            Assert.That(languages.Select(l => l.TryGetCulture(out _)), Is.All.True);
        });
    }

    [Test]
    public async Task GetAccountConfigAsync()
    {
        AccountConfig accountConfig = await SetUp.Client.GetAccountConfigAsync();
        Assert.That(accountConfig, Is.Not.Null);
    }

    [Test]
    public async Task GetGeneralInfoAsync()
    {
        GeneralAccountInfo accountInfo = await SetUp.Client.GetGeneralAccountInfoAsync();
        Assert.That(accountInfo, Is.Not.Null);
    }

    [Test]
    public async Task GetContactDetailsAsync()
    {
        IUser user = SetUp.Client.Session!.User;
        (AccessPermissions permissions, ContactDetails? contactDetails) = await SetUp.Client.GetContactDetailsAsync(user);

        Assert.Multiple(() =>
        {
            Assert.That(permissions.Read);
            Assert.That(permissions.Write);
            Assert.That(contactDetails, Is.Not.Null);
        });
    }

    [Test]
    public async Task GetProfileImageAsync()
    {
        IUser user = SetUp.Client.Session!.User;
        ProfileImage info = await SetUp.Client.GetProfileImageAsync(user);

        Assert.That(info, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(info.Permissions.Read);
            Assert.That(info.Permissions.Write);
        });
    }

    [Test]
    public async Task GetAccessDataAsync()
    {
        AccessData accessData = await SetUp.Client.GetAccessDataAsync();

        IConfigurationSection untisConfig = SetUp.Configuration.GetSection("untis");
        Assert.That(accessData, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(accessData.AppCredentials.ServerName, Is.EqualTo(untisConfig["serverName"]));
            Assert.That(accessData.AppCredentials.School, Is.EqualTo(untisConfig["loginName"]));
            Assert.That(accessData.AppCredentials.SchoolId.ToString(), Is.EqualTo(untisConfig["schoolId"]));
            Assert.That(accessData.AppCredentials.User, Is.EqualTo(untisConfig["username"]));

            Assert.That(accessData.TotpCredentials.ServerName, Is.EqualTo(untisConfig["serverName"]));
            Assert.That(accessData.TotpCredentials.User, Is.EqualTo(untisConfig["username"]));
        });
    }
}
