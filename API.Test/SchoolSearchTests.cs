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
        Task<School[]> schools = SchoolSearch.SearchAsync("Marie-Curie");
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
        Task<School[]> schools = SchoolSearch.SearchAsync("M");
        schools.Wait();
        if (schools.Result == null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Order(3)]
    [Test]
    public void NoSchoolsFound()
    {
        Task<School[]> schools = SchoolSearch.SearchAsync("MMMMMMMMMMMMMMMMMMMMMMMMMMM");
        schools.Wait();
        if (schools.Result.Length == 0)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
