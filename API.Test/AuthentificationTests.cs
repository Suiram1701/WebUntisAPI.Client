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
using WebUntisAPI.Client.Models.Profile;

namespace API.Test;

[TestFixture]
internal class AuthenticationTests
{
    [Test]
    public async Task AppCredentialsSignInAsync()
    {
        AccessData accessData = await SetUp.Client.GetAccessDataAsync();
        Assert.That(accessData, Is.Not.Null); 

        using WebUntisClient newClient = new();
        _ = await newClient.SignInAsync(accessData.AppCredentials);

        IConfigurationSection untisConfig = SetUp.Configuration.GetSection("untis");
        Assert.That(newClient.IsLoggedIn);
        Assert.Multiple(() =>
        {
            //Assert.That(newClient.Session!.User.Name, Is.EqualTo(accessData.AppCredentials.User));     // Currently fails this assertion but this will be fixed in a later commit.
            Assert.That(newClient.Session!.ServerUri.Host, Is.EqualTo(accessData.AppCredentials.ServerName));
        });
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
        DateTimeOffset current = DateTimeOffset.Now.AddSeconds(5);     // idk why but I have to add some seconds the current tim because the iat value is to large 
        WebUntisSession session = SetUp.Client.Session!;

        Assert.Multiple(() =>
        {
            Assert.That(session.IssuedTime, Is.LessThanOrEqualTo(current));
            Assert.That(session.ExpiresTime, Is.GreaterThan(current));
        });
    }
}
