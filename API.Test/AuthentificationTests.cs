using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;

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
