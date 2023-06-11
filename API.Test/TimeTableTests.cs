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
    public void GetTimeGrid()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Timegrid> timegrid = Client.GetTimegridAsync();
        timegrid.Wait();
        if (timegrid.Result.SchoolDays > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
