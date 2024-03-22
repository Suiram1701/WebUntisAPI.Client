using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;

namespace API.Test;

[TestFixture]
internal class UserTest
{
    [Test]
    public async Task GetCurrentPersonAsync()
    {
        IUser user = await SetUp.Client.GetSignedInUserAsync();

        string realUsername = SetUp.Configuration.GetSection("untis")["username"]!;
        Assert.That(user.Name, Is.EqualTo(realUsername));
    }
}
