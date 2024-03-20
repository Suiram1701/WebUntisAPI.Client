using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.Test.AuthentificationTests;
using SixLabors.ImageSharp;
using WebUntisAPI.Client.Models;
using NUnit.Framework;

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
    public void GetSupportedLanguages()
    {
        Task<Dictionary<string, string>> languages = SetUp.Client.GetAvailableLanguagesAsync();
        languages.Wait();

        if (languages.Result.Count > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetAccountConfiguration()
    {
        Task<AccountConfig> accountConfig = SetUp.Client.GetAccountConfigAsync();
        accountConfig.Wait();

        if (accountConfig.Result is not null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetGenerallyInformation()
    {
        Task<GeneralAccount> account = SetUp.Client.GetGenerallyAccountInformationAsync();
        account.Wait();

        if (account.Result is not null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetContactDetails()
    {
        Task<(ContactDetails? contact, bool read, bool write)> contact = SetUp.Client.GetContactDetailsAsync();
        contact.Wait();

        if (contact.Result.read)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetOwnProfileImage()
    {
        Task<(Image? image, bool read, bool write)> image = SetUp.Client.GetOwnProfileImageAsync();
        image.Wait();

        image.Result.image.SaveAsPngAsync("ProfileImg.png");       
    }
}
