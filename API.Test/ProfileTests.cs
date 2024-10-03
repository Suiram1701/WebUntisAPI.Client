using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.Test.AuthenticationTests;
using WebUntisAPI.Client.Models;
using NUnit.Framework;
using WebUntisAPI.Client.Models.Interfaces;

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
        ProfileImageInfo info = await SetUp.Client.GetProfileImageAsync(user, Stream.Null);

        Assert.Multiple(() =>
        {
            Assert.That(info.Permissions.Read);
            Assert.That(info.Permissions.Write);
        });
    }
}
