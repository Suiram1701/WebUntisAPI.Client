using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;

namespace API.Test;

[TestFixture]
internal class AuthenticationTests
{
    [Test]
    public async Task AppCredentialsSignInAsync()
    {
        AppCredentials credentials = await SetUp.Client.GetAppCredentialsAsync();
        Assert.That(credentials, Is.Not.Null); 

        IConfigurationSection untisConfig = SetUp.Configuration.GetSection("untis");
        Assert.Multiple(() =>
        {
            Assert.That(credentials.ServerName, Is.EqualTo(untisConfig["serverName"]));
            Assert.That(credentials.School, Is.EqualTo(untisConfig["loginName"]));
            Assert.That(credentials.SchoolId.ToString(), Is.EqualTo(untisConfig["schoolId"])); //ScholId in appsettings needed
            Assert.That(credentials.Username, Is.EqualTo(untisConfig["username"]));
        });

        using WebUntisClient client = new();
        (bool success, MasterData? data) = await client.SignInAsync(credentials, null);

        Assert.That(success, Is.True);
    }

    [Test]
    public async Task ReloadSessionTokenTestAsync()
    {
        bool result = await SetUp.Client.ReloadSessionAsync();
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task FailedSignInAsync()
    {
        using WebUntisClient client = new();

        IConfigurationSection untisConfig = SetUp.Configuration.GetSection("untis");
        string serverName = untisConfig["serverName"]!;
        string schoolName = untisConfig["loginName"]!;

        // test for that the school isn't found
        SignInResult signInResult = await client.SignInAsync(serverName, "abc", "def", "ghi", null);
        Assert.That(signInResult.SchoolNotFound);

        // test for wrong credentials
        SignInResult result = await client.SignInAsync(serverName, schoolName, "abc", "def", null);
        Assert.That(result.Successful, Is.False);
    }
    [Test]
    public void GetSessionIatExp()
    {
        DateTimeOffset iat = SetUp.Client.GetIssuedTime();
        DateTimeOffset exp = SetUp.Client.GetExpiresTime();

        DateTimeOffset current = DateTimeOffset.Now.AddSeconds(5);     // idk why but I have to add some seconds the current tim because the iat value is to large 

        Assert.Multiple(() =>
        {
            Assert.That(iat, Is.LessThanOrEqualTo(current));
            Assert.That(exp, Is.GreaterThan(current));
        });
    }


}
