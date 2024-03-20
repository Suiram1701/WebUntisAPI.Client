using Newtonsoft.Json;
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
internal class TimetableTests
{
    [Test]
    public async Task GetTimeGridAsync()
    {
        Timegrid timegrid = await SetUp.Client.GetTimegridAsync();
        Assert.That(timegrid, Is.Not.Null);
    }

    [Test]
    public async Task GetSchoolYearsAsync()
    {
        IEnumerable<SchoolYear> schoolYears = await SetUp.Client.GetSchoolYearsAsync();
        Assert.That(schoolYears.Count(), Is.GreaterThan(0));
    }

    [Test]
    public async Task GetCurrentSchoolYearAsync()
    {
        SchoolYear? schoolYear = await SetUp.Client.GetCurrentSchoolYearAsync();
        Assert.That(schoolYear, Is.Not.Null);
    }

    [Test]
    public async Task GetHolidaysAsync()
    {
        IEnumerable<Holiday> holidays = await SetUp.Client.GetHolidaysAsync();
        Assert.That(holidays.Count(), Is.GreaterThan(0));
    }

    [Test]
    public async Task GetTimetableAsync()
    {
        Timetable timetable = await SetUp.Client.GetTimetableAsync(new Student() { Id = 3299, CanViewTimetable = true }, new DateOnly(2024, 3, 18));
        Assert.Multiple(() =>
        {
            Assert.That(timetable.Periods.Count(), Is.GreaterThan(0));
            Assert.That(timetable.Elements.Count(), Is.GreaterThan(0));
            Assert.That(timetable.LastImportTimestamp, Is.LessThan(DateTimeOffset.Now));
        });
    }
}
