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
internal class TimetableTests
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
    public async Task GetHolidaysAsync()
    {
        await Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password);

        IEnumerable<Holiday> holidays = await Client.GetHolidaysAsync();
        Assert.That(holidays.Count(), Is.GreaterThan(0));
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
