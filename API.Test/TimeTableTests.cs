using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;
using WebUntisAPI.Client.Models;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;
using WebUntisAPI.Client.Models.NewTimetable;
using WebUntisAPI.Client.Models.Timetable;

namespace API.Test;

[TestFixture]
internal class TimetableTests
{
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
    public async Task GetTimeGridAsync()
    {
        WebUntisAPI.Client.Models.Timetable.TimeGrid timeGrid = await SetUp.Client.GetTimeGridAsync();
        Assert.That(timeGrid, Is.Not.Null);
    }

    [Test]
    public async Task GetTimetableAsync()
    {
        IUser user = await SetUp.Client.GetSignedInUserAsync();
        Timetable timetable = await SetUp.Client.GetTimetableAsync(user, new DateOnly(2024, 9, 9));

        Assert.Multiple(() =>
        {
            Assert.That(timetable.Periods.Count(), Is.GreaterThan(0));
            Assert.That(timetable.Elements.Count(), Is.GreaterThan(0));
            Assert.That(timetable.LastImportTimestamp, Is.LessThan(DateTimeOffset.Now));
        });
    }

    [Test]
    public async Task GetNewTimeGridAsync()
    {
        WebUntisAPI.Client.Models.NewTimetable.TimeGrid timeGrid = await SetUp.Client.GetNewTimeGridAsync();

        Assert.That(timeGrid, Is.Not.Null);
        Assert.That(timeGrid.TimeGridDefinitions.Select(tg => tg.Id), Is.Unique);
    }

    [Test]
    public async Task GetNewTimetableSettingsAsync()
    {
        TimetableSettings settings = await SetUp.Client.GetNewTimetableSettingsAsync();
        Assert.That(settings, Is.Not.Null);
    }

    [Test]
    public async Task GetNewTimetableFiltersAsync()
    {
        TimetableFilters filters = await SetUp.Client.GetNewTimetableFiltersAsync(ElementType.Student);     // I use student here because a student and teachers have access to at least one student timetable.
        Assert.That(filters.Students.Count(), Is.GreaterThan(0));
    }

    [Test]
    public async Task GetNewTimetableAsync()
    {
        TimetableFilters filters = await SetUp.Client.GetNewTimetableFiltersAsync(ElementType.Student);     // I use student here because a student and teachers have access to at least one student timetable.

        IEnumerable<TimetableDay> timetable = await SetUp.Client.GetNewTimetableAsync(new DateRange(new(2024, 09, 09), new(2024, 09, 09)), filters.Students.First());
        Assert.That(timetable.Count(), Is.GreaterThan(0));
    }
}
