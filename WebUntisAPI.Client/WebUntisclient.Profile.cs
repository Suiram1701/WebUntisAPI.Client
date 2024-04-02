using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Messages;
using Newtonsoft.Json.Linq;
using WebUntisAPI.Client.Models;
using WebUntisAPI.Client.Exceptions;
using System.Collections.ObjectModel;
using WebUntisAPI.Client.Models.Interfaces;

#if NET47 || NET481
using System.Drawing.Drawing2D;
using System.Drawing;
#elif NET6_0_OR_GREATER
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
#endif

namespace WebUntisAPI.Client;

public partial class WebUntisClient
{
    /// <summary>
    /// Download the profile image off the message person. When no image is available a image contains the first two start letters of <see cref="MessagePerson.DisplayName"/> will be rendered
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     The downloaded Image will be cropped to a square
    ///     </para>
    ///     <para>
    ///     The rendered image will be rendered to 300px * 300px and a font size of 124
    ///     </para>
    /// </remarks>
    /// <param name="person">The person for the image</param>
    /// <param name="fontName">The font name to render</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
    /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
#if NET47 || NET481
        public async Task<Image> GetMessagePersonProfileImageAsync(MessagePerson person, string fontName = "Segoe UI", CancellationToken ct = default)
#elif NET6_0_OR_GREATER
    public async Task<Image> GetMessagePersonProfileImageAsync(MessagePerson person, string fontName = "Segoe UI", CancellationToken ct = default)
#endif
    {
        // Check for disposing
        if (_disposedValue)
            throw new ObjectDisposedException(GetType().FullName);

        // Check if you logged in
        if (!LoggedIn)
            throw new UnauthorizedAccessException("The client is currently not logged in!");

        // Render the image with the displayed name when no img url available
        if (person.ImageUrl is null)
        {
            const int renderSize = 300;

            string text = string.Empty;
            foreach (char c in person.DisplayName.Split(' ').Take(2).Select(s => s[0]))
                text += c;

#if NET47 || NET481
                Image img = new Bitmap(renderSize, renderSize);
                using (Graphics graphic = Graphics.FromImage(img))
                {
                    graphic.SmoothingMode = SmoothingMode.AntiAlias;
                    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var font = new Font(fontName, 96))
                    using (var brush = new SolidBrush(Color.Black))
                    {
                        SizeF textSize = graphic.MeasureString(text, font);
                        float x = (renderSize - textSize.Width) / 2;
                        float y = (renderSize - textSize.Height) / 2;

                        graphic.DrawString(text, font, brush, x, y);
                    }
                }
#elif NET6_0_OR_GREATER
            Font font = SystemFonts.CreateFont(fontName, 124, FontStyle.Regular);
            RichTextOptions options = new(font)
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Origin = new(renderSize / 2, renderSize / 2)
            };

            Image img = new Image<Rgba32>(renderSize, renderSize);
            img.Mutate(op => op.DrawText(
                textOptions: options,
                text: text,
                color: new Rgba32(0, 0, 0, 255)));
#endif

            return img;
        }

        HttpRequestMessage request = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = person.ImageUrl
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

        HttpResponseMessage response = await _client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        // Check cancellation token
        if (ct.IsCancellationRequested)
            return default;

#if NET47 || NET481
            Image image = Image.FromStream(await response.Content.ReadAsStreamAsync());

            if (image.Height == image.Width)
                return image;

            // Resize the image to a square
            int shorterSide = Math.Min(image.Width, image.Height);
            int xOffset = (image.Width - shorterSide) / 2;
            int yOffset = (image.Height - shorterSide) / 2;

            Bitmap croppedImage = new Bitmap(shorterSide, shorterSide);
            using (Graphics graphics = Graphics.FromImage(croppedImage))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(image, new Rectangle(0, 0, shorterSide, shorterSide), new Rectangle(xOffset, yOffset, shorterSide, shorterSide), GraphicsUnit.Pixel);
            }

            return croppedImage;
#elif NET6_0_OR_GREATER
        Image image = await Image.LoadAsync(await response.Content.ReadAsStreamAsync(ct), ct);

        if (image.Height == image.Width)
            return image;

        // Resize the image to a square
        int shorterSide = Math.Min(image.Height, image.Width);
        int yOffSet = (image.Height - shorterSide) / 2;
        int xOffSet = (image.Width - shorterSide) / 2;

        image.Mutate(op =>
        {
            Rectangle cropRectangle = new(xOffSet, yOffSet, shorterSide, shorterSide);
            op.Crop(cropRectangle);
        });

        return image;
#endif
    }

    /// <summary>
    /// Get all by WebUntis supported languages
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<IEnumerable<WebUntisLanguage>> GetWebUntisLanguagesAsync(CancellationToken ct = default)
    {
        string responseString = await InternalAPIRequestAsync("/WebUntis/api/profile/languages", ct);

        JObject responseObj = JObject.Parse(responseString);
        return responseObj["data"]!["languages"]!.ToObject<IEnumerable<WebUntisLanguage>>()!;
    }

    /// <summary>
    /// Get the account configuration for a user (the configuration consist only out of one property)
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account configuration</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<AccountConfig> GetAccountConfigAsync(CancellationToken ct = default)
    {
        string responseString = await InternalAPIRequestAsync("/WebUntis/api/profile/config", ct);

        JToken dataToken = JObject.Parse(responseString)["data"]!;
        return dataToken.ToObject<AccountConfig>()!;
    }

    /// <summary>
    /// Get the general information about the current signed in account
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The information</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<GeneralAccountInfo> GetGeneralAccountInfoAsync(CancellationToken ct = default)
    {
        string responseString = await InternalAPIRequestAsync("/WebUntis/api/profile/general", ct);

        JToken dataToken = JObject.Parse(responseString)["data"]!;
        GeneralAccountInfo accountInfo = dataToken["profile"]!.ToObject<GeneralAccountInfo>()!;
        accountInfo.PasswordChangeAllowed |= dataToken["pwChangeAllowed"]!.Value<bool>()!;

        return accountInfo;
    }

    /// <summary>
    /// Get the contact details for the specified account
    /// </summary>
    /// <param name="user">The user whose <see cref="ContactDetails"/> should be requested</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The contact details and the permissions the signed in user has to the details</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="WebUntisException"></exception>
    /// <exception cref="HttpRequestException"></exception>
    public async Task<(AccessPermissions permissions, ContactDetails? contactDetails)> GetContactDetailsAsync(IUser user, CancellationToken ct = default)
    {
        UriBuilder uriBuilder = new()
        {
            Scheme = Uri.UriSchemeHttps,
            Host = ServerName,
            Path = "/WebUntis/api/profile/contactdetails",
            Query = $"personId={user.Id}&isRequestForStudent={false}"     // idk why isRequestForStudent must set to false also when the request where send by a student but when I set it to true I get always 'wrong' data
        };
        string responseString = await InternalAPIRequestAsync(uriBuilder.ToString(), ct);
        JToken dataToken = JObject.Parse(responseString)["data"]!;

        AccessPermissions permissions = dataToken.ToObject<AccessPermissions>()!;
        if (!permissions.Read)
            return (permissions, null);

        ContactDetails contactDetails = dataToken["address"]!.ToObject<ContactDetails>()!;
        return (permissions, contactDetails);
    }

    /// <summary>
    /// Get your own profile image
    /// </summary>
    /// <remarks>
    /// If you have not specified a profile picture, the image would be null
    /// </remarks>
    /// <param name="ct">Cancellation token</param>
    /// <returns>If canRead is false, the image is <see langword="null"/></returns>
    /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
    /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
#if NET47 || NET481
        public async Task<(Image image, bool canRead, bool canWrite)> GetOwnProfileImageAsync(CancellationToken ct = default)
#elif NET6_0_OR_GREATER
    public async Task<(Image image, bool canRead, bool canWrite)> GetOwnProfileImageAsync(CancellationToken ct = default)
#endif
    {
        string responseString = await InternalAPIRequestAsync("/WebUntis/api/profile/image?type={(int)UserType}&id={User.Id}", ct);
        JObject data = JObject.Parse(responseString)["data"].Value<JObject>();

        if (!data["read"].Value<bool>())     // Return null when you do not have a read permission
            return (null, false, data["write"].Value<bool>());

        HttpRequestMessage request = new HttpRequestMessage { Method = HttpMethod.Get };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

        if (data["imageId"].Value<int>() < 0)     // Return the default image when the image isn't set
            return (null, true, data["write"].Value<bool>());

        request.RequestUri = new Uri(ServerName + $"/WebUntis/image.do?cat={data["categoryId"].Value<int>()}&id={data["imageId"].Value<int>()}");
        HttpResponseMessage response = await _client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        // Check cancellation token
        if (ct.IsCancellationRequested)
            return default;
#if NET47 || NET481
            Image image = Image.FromStream(await response.Content.ReadAsStreamAsync());
#elif NET6_0_OR_GREATER
        Image image = await Image.LoadAsync(await response.Content.ReadAsStreamAsync(ct), ct);
#endif
        return (image, true, data["write"].Value<bool>());
    }
}