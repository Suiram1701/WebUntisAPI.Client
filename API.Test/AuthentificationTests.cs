using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;

namespace API.Test;

[Order(3)]
[TestFixture]
internal class AuthentificationTests
{
    public static WebUntisClient Client { get; set; } = new("WebUntisAPI_TEST", TimeSpan.FromSeconds(2));

    static AuthentificationTests()
    {
        // Load a file where i saved login data
        using StreamReader str = new("LoginData.txt");
        s_Server = str.ReadLine()!;
        s_LoginName = str.ReadLine()!;
        s_UserName = str.ReadLine()!;
        s_Password = str.ReadLine()!;
    }

    // Login data to test
    public static string s_Server;
    public static string s_LoginName;
    public static string s_UserName;
    public static string s_Password;

    [Test]
    public async Task Authentification()
    {
        try
        {
            using WebUntisClient client = new("WebUntisAPI_TEST", TimeSpan.FromSeconds(2));
            await client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password);
            await client.LogoutAsync();
        }
        catch
        {
            Assert.Fail();
            return;
        }
        Assert.Pass();
    }

    [Test]
    public async Task GetSessionExpiresDateTime()
    {
        using WebUntisClient client = new("WebUntisAPI_TEST", TimeSpan.FromSeconds(2));
        await client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password);
        _ = client.SessionExpires;
        _ = client.SessionBegin;
    }

    [Test]
    public async Task ReloadSessionTokenTest()
    {
        using WebUntisClient client = new("WebUntisAPI_TEST", TimeSpan.FromSeconds(2));
        await client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password);

        await client.ReloadSessionAsync();
        int a = 0;
    }
}
