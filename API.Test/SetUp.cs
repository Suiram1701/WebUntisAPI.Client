using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;
using WebUntisAPI.Client.Models;

namespace API.Test;

[SetUpFixture]
internal class SetUp
{
    public static WebUntisClient Client { get; private set; }

    public static IConfiguration Configuration { get; private set; }

    static SetUp()
    {
        Client = new();

        Assembly assembly = typeof(SetUp).Assembly;
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)  
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)  // Ensure appsettings.json is present
            .AddUserSecrets(assembly) 
            .Build();
    }

    [OneTimeSetUp]
    public async Task SetUpAsync()
    {
        IConfigurationSection untisConfig = Configuration.GetSection("untis");
        string serverName = untisConfig["serverName"]!;
        string loginName = untisConfig["loginName"]!;
        string username = untisConfig["username"]!;
        string password = untisConfig["password"]!;
        string? mfaBase32Secret = untisConfig["mfaSecret"]; 

        string? generatetSecret = null;
        if (!string.IsNullOrEmpty(mfaBase32Secret))
        {
            byte[] mfaSecret = Base32Encoding.ToBytes(mfaBase32Secret);
            Totp totp = new(mfaSecret);
            generatetSecret = totp.ComputeTotp();
        }

        SignInResult result = await Client.SignInAsync(serverName, loginName, username, password, generatetSecret);

        if (!result.Successful)
            throw new UnauthorizedAccessException("Could not login the user.");
    }

    [OneTimeTearDown]
    public async Task TearDownAsync()
    {
        await Client.SignOutAsync(null);
        Client.Dispose();
    }
}
