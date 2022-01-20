using LedMatrix.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Timers;

namespace LedMatrix.Helpers
{
    public class Utility
    {
        public static byte[] EncodePixels(Frame frame)
        {
            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(-frame.Pixels.Count).ToList());
            data.AddRange(frame.Pixels.SelectMany(p => p.EncodeAsByte()));
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

        public static Frame ConvertFromImage(string filename)
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
            return new Frame(pixels);
        }

        public static System.Timers.Timer StartTimer(int interval, ElapsedEventHandler eventHandler)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = Constants.MsPerFrame;
            timer.Elapsed += eventHandler;
            timer.Enabled = true;
            return timer;
        }
    }
}
