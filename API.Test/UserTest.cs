using NUnit.Framework.Constraints;
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
    [Test]
    [Order(1)]
    public void GetStudents()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Student[]> students = Client.GetStudentsAsync();
        students.Wait();
        if (students.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    [Order(2)]
    public void GetTeachers()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Teacher[]> teachers = Client.GetTeachersAsync();
        teachers.Wait();
        if (teachers.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    [Order(3)]
    public void GetPersonId()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<int> personId = Client.GetPersonIdAsync(Client.User.ForeName, Client.User.LongName, Client.UserType ?? UserType.Student);
        personId.Wait();
        if (personId.Result == Client.User.Id)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
