using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.Test.AuthentificationTests;
using SixLabors.ImageSharp;

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
}
