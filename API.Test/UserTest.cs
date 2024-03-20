using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;
using WebUntisAPI.Client.Models.Elements;

namespace API.Test;

[TestFixture]
internal class UserTest
{
    [Test]
    [Order(1)]
    public void GetStudents()
    {
        Task<Student[]> students = SetUp.Client.GetStudentsAsync();
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
        Task<Teacher[]> teachers = SetUp.Client.GetTeachersAsync();
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
        //Task<int> personId = SetUp.Client.GetPersonIdAsync(Client.User.ForeName, SetUp.Client.User.LongName, SetUp.Client.UserType ?? UserType.Student);
        //personId.Wait();
        //if (personId.Result == SetUp.Client.User.Id)
        //    Assert.Pass();
        //else
        //    Assert.Fail();
    }
}
