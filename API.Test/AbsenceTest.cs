using NUnit.Framework;
using WebUntisAPI.Client.Exceptions;
using WebUntisAPI.Client.Models.Absence;
using WebUntisAPI.Client.Models.Elements;

namespace API.Test;
[TestFixture]
internal class AbsenceTest
{
    [Test]
    public async Task GetAbsencesAsync()
    {
        AbsenceData absenceData = await SetUp.Client.GetAbsencesAsync(new DateOnly(2024, 8, 19), new DateOnly(2025, 7, 11), await SetUp.Client.GetSignedInUserAsync());
        Assert.That(absenceData, Is.Not.Null);
    }
    [Test]
    public async Task GetAbsencesAsync_Emptydata()
    {
        AbsenceData absenceData = await SetUp.Client.GetAbsencesAsync(new DateOnly(2025, 7, 11), new DateOnly(2024, 8, 19), await SetUp.Client.GetSignedInUserAsync()); // StartDate before endDate -> 100% empty result
        Assert.Multiple(() =>
        {
            Assert.That(absenceData.Absences.Count, Is.EqualTo(0));
            Assert.That(absenceData.AbsenceReasons.Count, Is.EqualTo(0));
            Assert.That(absenceData, Is.Not.Null);
        });
    }
    [Test]
    public void GetAbsencesAsync_InvalidPersonId()
    {
        int invalidPersonId = 1;
        Student student = new Student() { Id = invalidPersonId };
        var exception = Assert.ThrowsAsync<WebUntisException>(async () =>
               await SetUp.Client.GetAbsencesAsync(new DateOnly(2024, 7, 11), new DateOnly(2025, 8, 19), student));

    }

}

