﻿using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;
using WebUntisAPI.Client.Models;
using static API.Test.AuthentificationTests;

namespace API.Test;

[Order(4)]
[TestFixture]
internal class UserTest
{
    private static WebUntisClient Client { get; set; }

    [Order(0)]
    [SetUp]
    public void Setup()
    {
        Client = new("WebUntisAPI_TEST");
    }

    [Test]
    [Order(1)]
    public void GetStudents()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password, CancellationToken.None).Wait();

        Task<Student[]> students = Client.GetAllStudentsAsync(CancellationToken.None);
        students.Wait();
        if (students.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Order(3)]
    [Test]
    public void TearUp()
    {
        Client.LogoutAsync(CancellationToken.None).Wait();
    }
}
