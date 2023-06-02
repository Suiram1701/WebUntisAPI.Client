using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;

namespace API.Test;

[Order(2)]
[TestFixture]
internal class SchoolSearchTests
{
    [Order(1)]
    [Test]
    public void NormalSearch()
    {
        Task<SchoolModel[]> schools = SchoolSearch.SearchAsync("Marie-Curie", CancellationToken.None);
        schools.Wait();
        if (schools.Result.Length > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Order(2)]
    [Test]
    public void ToManySchoolsFound()
    {
        bool success;
        try
        {
            SchoolSearch.SearchAsync("M", CancellationToken.None).Wait();
            success = false;
        }
        catch (AggregateException ex) when (ex.InnerException.GetType() == typeof(WebUntisException))
        {
            success = true;
        }

        Assert.IsTrue(success);
    }

    [Order(3)]
    [Test]
    public void NoSchoolsFound()
    {
        Task<SchoolModel[]> schools = SchoolSearch.SearchAsync("MMMMMMMMMMMMMMMMMMMMMMMMMMM", CancellationToken.None);
        schools.Wait();
        if (schools.Result.Length == 0)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
