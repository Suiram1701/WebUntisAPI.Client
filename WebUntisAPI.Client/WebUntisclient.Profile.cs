using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Messages;
using System.Drawing.Imaging;
using Newtonsoft.Json.Linq;
using WebUntisAPI.Client.Models;
using Newtonsoft.Json;
using System.IO;
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

namespace WebUntisAPI.Client
{
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
        /// <param name="fontName">The font size to render</param>
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
                throw new UnauthorizedAccessException("You're not logged in");

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

            request.Headers.Add("JSESSIONID", _sessionId);
            request.Headers.Add("schoolname", _schoolName);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return default;

            // Verify response
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _ = LogoutAsync();
                throw new UnauthorizedAccessException("You're not logged in");
            }

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

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
#elif NET6_0 || NET7_0
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
        /// <returns>The languages (<see cref="KeyValuePair{TKey, TValue}.Key"/> is the WebUntis internal name of the language and <see cref="KeyValuePair{TKey, TValue}.Value"/> is the full name)</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<Dictionary<string, string>> GetAvailableLanguagesAsync(CancellationToken ct = default)
        {
            string responseString = await MakeAPIGetRequestAsync("/WebUntis/api/profile/languages", ct);
            JArray languageObjects = JObject.Parse(responseString)["data"]["languages"].Value<JArray>();

            Dictionary<string, string> languages = new Dictionary<string, string>();
            foreach (JObject language in languageObjects.Cast<JObject>())
                languages.Add(language.Value<string>("key"), language.Value<string>("name"));

            return languages;
        }

        /// <summary>
        /// Get the account configuration
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The account configuration</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the instance was disposed</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when you're logged in</exception>
        /// <exception cref="HttpRequestException">Thrown when an error happened while the http request</exception>
        public async Task<AccountConfig> GetAccountConfigAsync(CancellationToken ct = default)
        {
            string responseString = await MakeAPIGetRequestAsync("/WebUntis/api/profile/config", ct);
            return JObject.Parse(responseString)["data"].ToObject<AccountConfig>();
        }

        /// <summary>
        /// Get the general information about the current account
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The information</returns>
        public async Task<GeneralAccount> GetGenerallyAccountInformationAsync(CancellationToken ct = default)
        {
            string responseString = await MakeAPIGetRequestAsync("/WebUntis/api/profile/general", ct);
            return GeneralAccount.ReadFromJson(JObject.Parse(responseString)["data"].CreateReader());
        }

        /// <summary>
        /// Update the general account information
        /// </summary>
        /// <remarks>
        /// When a value shouldn't change then must you set the current value
        /// </remarks>
        /// <param name="email"><see cref="GeneralAccount.Email"/></param>
        /// <param name="forwardMessageToEmail"><see cref="GeneralAccount.ForwardMessageToMail"/></param>
        /// <param name="itemsOnStartPage"><see cref="GeneralAccount.ItemsOnStartPage"/></param>
        /// <param name="languageCode"><see cref="GeneralAccount.LanguageCode"/></param>
        /// <param name="showLessonOfDay"><see cref="GeneralAccount.ShowLessonsOfDay"/></param>
        /// <param name="showNextDayPeriods"><see cref="GeneralAccount.ShowNextDayPeriods"/></param>
        /// <param name="userTaskNotifications"><see cref="GeneralAccount.UserTaskNotifications"/></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Was the update successful</returns>
        public async Task<bool> UpdateGenerallyAccountInformationAsync(string email, bool forwardMessageToEmail, int itemsOnStartPage, string languageCode, bool showLessonOfDay, bool showNextDayPeriods, bool userTaskNotifications, CancellationToken ct = default)
        {
            // Check for disposing
            if (_disposedValue)
                throw new ObjectDisposedException(GetType().FullName);

            // Check if you logged in
            if (!LoggedIn)
                throw new UnauthorizedAccessException("You're not logged in");

            // Write request
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();

                writer.WritePropertyName("email");
                writer.WriteValue(email);

                writer.WritePropertyName("forwardMessageToEmail");
                writer.WriteValue(forwardMessageToEmail);

                writer.WritePropertyName("itemOnStartPage");
                writer.WriteValue(itemsOnStartPage);

                writer.WritePropertyName("languageCode");
                writer.WriteValue(languageCode);

                writer.WritePropertyName("showLessonsOfDay");
                writer.WriteValue(showLessonOfDay);

                writer.WritePropertyName("showNextDayPeriods");
                writer.WriteValue(showNextDayPeriods);

                writer.WritePropertyName("userTaskNotifications");
                writer.WriteValue(userTaskNotifications);

                writer.WriteEndObject();
            }

            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ServerUrl + "/WebUntis/api/profile/general"),
                Content = new StringContent(sw.ToString())
            };

            request.Headers.Add("JSESSIONID", _sessionId);
            request.Headers.Add("schoolname", _schoolName);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            HttpResponseMessage response = await _client.SendAsync(request, ct);

            // Check cancellation token
            if (ct.IsCancellationRequested)
                return false;

            // Verify response
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _ = LogoutAsync();
                throw new UnauthorizedAccessException("You're not logged in");
            }

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException($"There was an error while the http request (Code: {response.StatusCode}).");

            string responseString = await response.Content.ReadAsStringAsync();
            JObject responseObject = JObject.Parse(responseString);
            return responseObject["data"]["success"].Value<bool>();
        }
    }
}