using NUnit.Framework;
using WebUntisAPI.Client.Models;

namespace API.Test;
[TestFixture]
internal class AbsenceTest
{
    [Test]
    public async Task GetAbsencesAsync()
    {
        AbsenceData absenceData = await SetUp.Client.GetAbsencesAsync(new DateOnly(2024, 8, 19), new DateOnly(2025, 7, 11));
        Assert.That(absenceData, Is.Not.Null);
    }
    [Test]
    public async Task GetAbsencesAsync_Emptydata()
    {
        AbsenceData absenceData = await SetUp.Client.GetAbsencesAsync(new DateOnly(2025, 7, 11), new DateOnly(2024, 8, 19)); // StartDate before endDate -> 100% empty result
        Assert.That(absenceData.Absences.Count, Is.EqualTo(0));
        Assert.That(absenceData.AbsenceReasons.Count, Is.EqualTo(0));
        Assert.That(absenceData, Is.Not.Null);
    }

}

