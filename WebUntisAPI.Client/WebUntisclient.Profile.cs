﻿using System;
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
#if NET47 || NET481
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
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


    }
}