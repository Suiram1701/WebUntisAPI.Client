using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;
using static API.Test.AuthentificationTests;

namespace API.Test;

[Order(5)]
[TestFixture]
internal class TeachingTests
{
    [Test]
    public void GetSubject()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Subject[]> subjects = Client.GetAllSubjectsAsync();
        subjects.Wait();
        if (subjects.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
