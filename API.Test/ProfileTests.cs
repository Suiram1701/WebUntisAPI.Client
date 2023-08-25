using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.Test.AuthentificationTests;
using SixLabors.ImageSharp;
using WebUntisAPI.Client.Models;

namespace API.Test;

[TestFixture]
internal class ProfileTests
{
    [Test]
    public void GetRecipientProfileImgTest()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Image> imgRender = Client.GetMessagePersonProfileImageAsync(new() { DisplayName = "Test Person" });
        imgRender.Wait();
        imgRender.Result.SaveAsPng("RenderImg.png");


        Task<Image> imgDownload = Client.GetMessagePersonProfileImageAsync(new() { ImageUrl = new("https://foundations.projectpythia.org/_images/GitHub-logo.png") });     // A Random non square GitHub image i found
        imgDownload.Wait();
        imgDownload.Result.SaveAsPng("DownloadImg.png");
    }

    [Test]
    public void GetSupportedLanguages()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<Dictionary<string, string>> languages = Client.GetAvailableLanguagesAsync();
        languages.Wait();

        if (languages.Result.Count > 0)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetAccountConfiguration()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<AccountConfig> accountConfig = Client.GetAccountConfigAsync();
        accountConfig.Wait();

        if (accountConfig.Result is not null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetGenerallyInformation()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<GeneralAccount> account = Client.GetGenerallyAccountInformationAsync();
        account.Wait();

        if (account.Result is not null)
            Assert.Pass();
        else
            Assert.Fail();
    }

    [Test]
    public void GetContactDetails()
    {
        Client.LoginAsync(s_Server, s_LoginName, s_UserName, s_Password).Wait();

        Task<(ContactDetails? contact, bool read, bool write)> contact = Client.GetContactDetailsAsync();
        contact.Wait();

        if (contact.Result.read)
            Assert.Pass();
        else
            Assert.Fail();
    }
}
