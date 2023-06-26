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

        Task<Subject[]> subjects = Client.GetSubjectsAsync();
        subjects.Wait();
        if (subjects.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetClass()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Class[]> classes = Client.GetClassesAsync(new SchoolYear() { Id = 5});
        classes.Wait();
        if (classes.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetRoom()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Room[]> rooms = Client.GetRoomsAsync();
        rooms.Wait();
        if (rooms.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetDepartment()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Department[]> departments = Client.GetDepartmentsAsync();
        departments.Wait();
        if (departments.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetLatestImportTime()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<DateTime> time = Client.GetLatestImportTimeAsync();
        time.Wait();
        if (time.Result <= DateTime.Now)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetNewsFeed()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<News> news = Client.GetNewsFeedAsync(new DateTime(2023, 06, 23));
        news.Wait();
        if (news.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
