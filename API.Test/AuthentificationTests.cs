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

        // test for 'SCHOOL_NOT_FOUND' exception
        WebUntisException? wuEx = Assert.ThrowsAsync<WebUntisException>(async () =>
        {
            await client.LoginAsync(serverName, string.Empty, string.Empty, string.Empty);
        });
        Assert.That(wuEx.Errors.First().Code, Is.EqualTo("SCHOOL_NOT_FOUND"));

        // test for wrong credentials
        bool success = await client.LoginAsync(serverName, schoolName, string.Empty, string.Empty);
        Assert.That(success, Is.False);
    }

    [Test]
    public void GetSessionExpiresDateTime()
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
