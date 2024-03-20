using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;
using WebUntisAPI.Client.Models.Elements;
using static API.Test.AuthentificationTests;

namespace API.Test;

[TestFixture]
internal class TeachingTests
{
    [Test]
    public void GetSubject()
    {
        Task<Subject[]> subjects = SetUp.Client.GetSubjectsAsync();
        subjects.Wait();
        if (subjects.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetClass()
    {
        Task<Class[]> classes = SetUp.Client.GetClassesAsync();
        classes.Wait();
        if (classes.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetRoom()
    {
        Task<Room[]> rooms = SetUp.Client.GetRoomsAsync();
        rooms.Wait();
        if (rooms.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetDepartment()
    {
        Task<Department[]> departments = SetUp.Client.GetDepartmentsAsync();
        departments.Wait();
        if (departments.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetUnreadNews()
    {
        Task<int> unread = SetUp.Client.GetUnreadNewsCountAsync();
        unread.Wait();
        if (unread.Result == 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetNewsFeed()
    {
        Task<News> news = SetUp.Client.GetNewsFeedAsync(new DateTime(2023, 07, 10));
        news.Wait();
        if (news.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
