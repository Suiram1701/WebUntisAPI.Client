using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models;

namespace API.Test;

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
        string schoolName = SetUp.Configuration.GetSection("untis")["loginName"]!;

        IEnumerable<School>? schools = await _searcher.SearchAsync(schoolName);
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
        IEnumerable<School>? schools = await _searcher.SearchAsync("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        Assert.That(schools, Is.Empty);
    }

    [Test]
    public async Task SearchSchoolByNameAsync()
    {
        string schoolName = SetUp.Configuration.GetSection("untis")["loginName"]!;

        School? school = await _searcher.GetSchoolByNameAsync(schoolName);
        Assert.That(school, Is.Not.Null);
    }

    [Test]
    public async Task GetSchoolByIdAsync()
    {
        string schoolIdString = SetUp.Configuration.GetSection("untis")["schoolId"]!;
        int schoolId = int.Parse(schoolIdString);

        School? school = await _searcher.GetSchoolByIdAsync(schoolId);
        Assert.That(school, Is.Not.Null);
    }
}
