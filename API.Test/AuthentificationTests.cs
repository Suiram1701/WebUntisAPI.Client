using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;
using WebUntisAPI.Client.Exceptions;

namespace API.Test;

[TestFixture]
internal class AuthentificationTests
{
    [Test]
    public async Task ReloadSessionTokenTestAsync()
    {
        bool result = await SetUp.Client.ReloadSessionAsync();
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task FailedLoginAsync()
    {
        using WebUntisClient client = new();

        IConfigurationSection untisConfig = SetUp.Configuration.GetSection("untis");
        string serverName = untisConfig["serverName"]!;
        string schoolName = untisConfig["loginName"]!;

        // test for error code -8500 that means school not found
        WebUntisException? wuEx = Assert.ThrowsAsync<WebUntisException>(async () =>
        {
            await client.SignInAsync(serverName, "abc", "def", "ghi", null);
        });
        Assert.That(wuEx.Errors.First().Code, Is.EqualTo((-8500).ToString()));

        // test for wrong credentials
        bool success = await client.SignInAsync(serverName, schoolName, "abc", "def", null);
        Assert.That(success, Is.False);
    }

    [Test]
    public void GetSessionIatExp()
    {
        DateTimeOffset iat = SetUp.Client.GetIssuedTime();
        DateTimeOffset exp = SetUp.Client.GetExpiresTime();

        DateTimeOffset current = DateTimeOffset.Now;

        Assert.Multiple(() =>
        {
            Assert.That(iat, Is.LessThan(current));
            Assert.That(exp, Is.GreaterThan(current));
        });
    }
}
