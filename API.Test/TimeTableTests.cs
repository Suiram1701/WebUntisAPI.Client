using Newtonsoft.Json;
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
internal class TimeTableTests
{
    [Test]
    public async Task GetTimeGridAsync()
    {
        await Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password);

        Timegrid timegrid = await Client.GetTimegridAsync();
        Assert.That(timegrid, Is.Not.Null);
    }

    [Test]
    public async Task GetSchoolYearsAsync()
    {
        await Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password);

        IEnumerable<SchoolYear> schoolYears = await Client.GetSchoolYearsAsync();
        Assert.That(schoolYears.Count(), Is.GreaterThan(0));
    }

    [Test]
    public async Task GetCurrentSchoolYearAsync()
    {
        await Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password);

        SchoolYear? schoolYear = await Client.GetCurrentSchoolYearAsync();
        Assert.That(schoolYear, Is.Not.Null);
    }

    [Test]
    public void GetHolidays()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Holidays[]> holidays = Client.GetHolidaysAsync();
        holidays.Wait();
        if (holidays.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public async Task GetTimetableAsync()
    {
        await Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password);

        Timetable timetable = await Client.GetTimetableAsync(new Student() { Id = 3299, CanViewTimetable = true }, new DateOnly(2024, 3, 18));
        Assert.Multiple(() =>
        {
            Assert.That(timetable.Periods.Count(), Is.GreaterThan(0));
            Assert.That(timetable.Elements.Count(), Is.GreaterThan(0));
            Assert.That(timetable.LastImportTimestamp, Is.LessThan(DateTimeOffset.Now));
        });
    }
}
