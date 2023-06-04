﻿using System;
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
    public static bool s_Successfull = false;

    // Login data to test
    public static string s_Server;
    public static string s_LoginName;
    public static string s_UserName;
    public static string s_Password;

    [SetUp]
    public void Setup()
    {
        // Load a file where i saved login data
        using StreamReader str = new("LoginData.txt");
        s_Server = str.ReadLine();
        s_LoginName = str.ReadLine();
        s_UserName = str.ReadLine();
        s_Password = str.ReadLine();
    }

    [Test]
    public void Authentification()
    {
        try
        {
            using WebUntisClient client = new("WebUntisAPI_TEST");
            client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password, CancellationToken.None).Wait();
            client.LogoutAsync().Wait();
        }
        catch
        {
            s_Successfull = false;
            Assert.Fail();
            return;
        }

        s_Successfull = true;
        Assert.Pass();
    }
}
