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
    private SchoolSearcher _searcher;

    [OneTimeSetUp]
    public void Setup()
    {
        _searcher = new();
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        _searcher.Dispose();
    }

    [Test]
    public async Task NormalSearchAsync()
    {
        IEnumerable<School>? schools = await _searcher.SearchAsync("Marie-Curie");
        Assert.That(schools?.Count(), Is.GreaterThan(0));
    }

    [Test]
    public async Task ToManySchoolsFoundAsync()
    {
        IEnumerable<School>? schools = await _searcher.SearchAsync("M");
        Assert.That(schools, Is.Null);
    }

    [Test]
    public async Task NoSchoolsFoundAsync()
    {
        IEnumerable<School>? schools = await _searcher.SearchAsync("MMMMMMMMMMMMMMMMMMMMMMMMMMM");
        Assert.That(schools, Is.Empty);
    }

    [Test]
    public async Task SearchSchoolByNameAsync()
    {
        School? school = await _searcher.GetSchoolByNameAsync("Marie-Curie Gym");
        Assert.That(school, Is.Not.Null);
    }

    [Test]
    public async Task GetSchoolByIdAsync()
    {
        School? school = await _searcher.GetSchoolByIdAsync(2382100);
        Assert.That(school, Is.Not.Null);
    }
}
