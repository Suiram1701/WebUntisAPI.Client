using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.Test.AuthentificationTests;
using SixLabors.ImageSharp;
using WebUntisAPI.Client.Models;
using NUnit.Framework;
using WebUntisAPI.Client.Models.Interfaces;

namespace API.Test;

[TestFixture]
internal class ProfileTests
{
    [Test]
    public void GetRecipientProfileImgTest()
    {
        Task<Image> imgRender = SetUp.Client.GetMessagePersonProfileImageAsync(new() { DisplayName = "Test Person" });
        imgRender.Wait();
        imgRender.Result.SaveAsPng("RenderImg.png");

        Task<Image> imgDownload = SetUp.Client.GetMessagePersonProfileImageAsync(new() { ImageUrl = new("https://foundations.projectpythia.org/_images/GitHub-logo.png") });     // A Random non square GitHub image i found
        imgDownload.Wait();
        imgDownload.Result.SaveAsPng("DownloadImg.png");
    }

    [Test]
    public async Task GetSupportedLanguagesAsync()
    {
        IEnumerable<WebUntisLanguage> languages = await SetUp.Client.GetWebUntisLanguagesAsync();

        Assert.Multiple(() =>
        {
            Assert.That(languages.Count(), Is.GreaterThan(1));
            Assert.That(languages.Select(l => l.Key), Is.Unique);
            Assert.That(languages.Select(l => l.Name), Is.Unique);
            Assert.That(languages.Select(l => l.TryGetCulture(out _)), Is.All.True);
        });
    }

    [Test]
    public async Task GetAccountConfigAsync()
    {
        AccountConfig accountConfig = await SetUp.Client.GetAccountConfigAsync();
        Assert.That(accountConfig, Is.Not.Null);
    }

    [Test]
    public async Task GetGeneralInfoAsync()
    {
        GeneralAccountInfo accountInfo = await SetUp.Client.GetGeneralAccountInfoAsync();
        Assert.That(accountInfo, Is.Not.Null);
    }

    [Test]
    public async Task GetContactDetailsAsync()
    {
        IUser user = await SetUp.Client.GetSignedInUserAsync();
        (AccessPermissions permissions, ContactDetails? contactDetails) = await SetUp.Client.GetContactDetailsAsync(user);

        Assert.Multiple(() =>
        {
            Assert.That(permissions.Read);
            Assert.That(permissions.Write);
            Assert.That(contactDetails, Is.Not.Null);
        });
    }

    [Test]
    public void GetOwnProfileImage()
    {
        Task<(Image? image, bool read, bool write)> image = SetUp.Client.GetOwnProfileImageAsync();
        image.Wait();

        image.Result.image.SaveAsPngAsync("ProfileImg.png");       
    }
}
