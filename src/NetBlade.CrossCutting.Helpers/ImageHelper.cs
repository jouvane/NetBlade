using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace NetBlade.CrossCutting.Helpers
{
    public static class ImageHelper
    {
        public static byte[] FromBase64String(string img, string imgType = "image/jpeg")
        {
            byte[] contem = Convert.FromBase64String(img.Replace(string.Format("data:{0};base64,", imgType), string.Empty));
            return contem;
        }

        public static byte[] LimitarMaxWidth(byte[] sourceImage, int maxWidth)
        {
            if (sourceImage != null)
            {
                using MemoryStream memoryStream = new MemoryStream(sourceImage);
                using Image image = Image.FromStream(memoryStream);
                return ImageHelper.LimitarMaxWidth(image, maxWidth);
            }

            return null;
        }

        public static byte[] LimitarMaxWidth(Stream sourceImage, int maxWidth)
        {
            if (sourceImage != null)
            {
                using Image image = Image.FromStream(sourceImage);
                return ImageHelper.LimitarMaxWidth(image, maxWidth);
            }

            return null;
        }

        public static byte[] LimitarMaxWidth(Image sourceImage, int maxWidth)
        {
            int width = sourceImage != null ? sourceImage.Width : 0;
            if (width > maxWidth)
            {
                width = maxWidth;
                int height = (int)(sourceImage.Height / (sourceImage.Width / (double)width));

                using Bitmap newImage = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(sourceImage, new Rectangle(0, 0, width, height));
                }

                using MemoryStream memoryStreamThumb = new MemoryStream();
                newImage.Save(memoryStreamThumb, sourceImage.RawFormat);
                return memoryStreamThumb.ToArray();
            }
            else
            {
                using MemoryStream memoryStreamThumb = new MemoryStream();
                sourceImage.Save(memoryStreamThumb, sourceImage.RawFormat);
                return memoryStreamThumb.ToArray();
            }
        }

        public static string ToBase64String(Image sourceImage)
        {
            if (sourceImage != null)
            {
                using MemoryStream m = new MemoryStream();
                sourceImage.Save(m, sourceImage.RawFormat);
                byte[] imageBytes = m.ToArray();

                return ImageHelper.ToBase64String(imageBytes);
            }

            return null;
        }

        public static string ToBase64String(byte[] sourceImage, string imgType = "image/jpeg")
        {
            if (sourceImage != null)
            {
                string base64String = Convert.ToBase64String(sourceImage);
                return string.Format("data:{0};base64,{1}", imgType, base64String);
            }

            return null;
        }
    }
}
