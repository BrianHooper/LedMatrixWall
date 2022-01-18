using LedMatrix.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace LedMatrix.Helpers
{
    public class Utility
    {
        public static byte[] EncodePixels(List<Pixel> pixels)
        {
            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(-pixels.Count()).ToList());
            data.AddRange(pixels.SelectMany(p => p.EncodeAsByte()));
            return data.ToArray();
        }

        public static List<Pixel> DecodePixels(byte[] data)
        {
            var length = -BitConverter.ToInt32(data.Take(4).ToArray());
            var pixels = new List<Pixel>();
            var dataSize = length * 8 + 4;
            for (int i = 4; i < dataSize; i += 8)
            {
                pixels.Add(new Pixel(data, i));
            }
            return pixels;
        }

        public static byte[] EncodeColorList(List<Color> colors)
        {
            var colorBytes = colors.SelectMany(c => new byte[] { c.R, c.G, c.B });
            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(colorBytes.Count()).ToList());
            data.AddRange(colorBytes);
            return data.ToArray();
        }

        public static List<Color> DecodeColorList(byte[] data)
        {
            var length = BitConverter.ToInt32(data.Take(4).ToArray());

            var colors = new List<Color>();
            for (int i = 4; i <= length + 2; i += 3)
            {
                var r = data[i];
                var g = data[i + 1];
                var b = data[i + 2];

                var color = Color.FromArgb(r, g, b);
                colors.Add(color);
            }
            

            return colors;
        }

        public static List<Pixel> ConvertFromImage(string filename)
        {
            var image = Image.FromFile(filename);
            var width = Constants.LedsPerRow;
            var height = Constants.LedsPerColumn;

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            var pixels = new List<Pixel>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var pixel = new Pixel(x, y, destImage.GetPixel(x, y));
                    pixels.Add(pixel);
                }
            }
            return pixels;
        }
    }
}
