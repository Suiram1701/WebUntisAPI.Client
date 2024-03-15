using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;
using static API.Test.AuthentificationTests;

namespace API.Test;

[TestFixture]
internal class TimeTableTests
{
    [Test]
    public void GetStatusData()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<StatusData> status = Client.GetStatusDataAsync();
        status.Wait();
        if (status.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public async Task GetTimeGrid()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Timegrid timegrid = await Client.GetTimegridAsync();
        Assert.Pass();
    }

    [Test]
    public void GetSchoolYears()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<SchoolYear[]> schoolYears = Client.GetSchoolYearsAsync();
        schoolYears.Wait();
        if (schoolYears.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetCurrentSchoolYear()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<SchoolYear> schoolYear = Client.GetCurrentSchoolYearAsync();
        schoolYear.Wait();
        if (schoolYear.Result != null)
            Assert.Pass();
        else
            Assert.Fail();
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
    public void GetTimetable()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Period[]> timetable = Client.GetOwnTimetableAsync(new DateTime(2023, 05, 29), new DateTime(2023, 09, 2));
        timetable.Wait();
        if (timetable.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
