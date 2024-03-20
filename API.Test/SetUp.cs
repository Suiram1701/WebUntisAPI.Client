using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;

namespace API.Test;

[SetUpFixture]
internal class SetUp
{
    public static WebUntisClient Client { get; private set; }

    private readonly IConfiguration _configuration;

    public SetUp()
    {
        Assembly assembly = typeof(SetUp).Assembly;
        _configuration = new ConfigurationBuilder()
            .AddUserSecrets(assembly)
            .Build();
    }

    [OneTimeSetUp]
    public async Task SetUpAsync()
    {
        IConfigurationSection untisConfig = _configuration.GetSection("untis");
        string serverName = untisConfig["serverName"]!;
        string loginName = untisConfig["loginName"]!;
        string username = untisConfig["username"]!;
        string password = untisConfig["password"]!;

        Client = new(TimeSpan.FromSeconds(5));
        bool success = await Client.LoginAsync(serverName, loginName, username, password);

        if (!success)
            throw new UnauthorizedAccessException("Could not login the user.");
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        SetUp.Client.Dispose();
    }
}
