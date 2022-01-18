using System.Device.Spi;
using System.Drawing;
using Iot.Device.Graphics;
using Iot.Device.Ws28xx;
using LedMatrix.Helpers;
using LedMatrix.Models;

namespace LedMatrix
{
    public class LedController : ControllerBase
    {
        private Ws2812b ledDevice;
        private BitmapImage deviceImage;

        public LedController() : base()
        {
            var settings = new SpiConnectionSettings(0, 0)
            {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };

            // TODO SpiDevice.Create method checks Environment.OSVersion.Platform == PlatformID.Win32NT to determine what spi device to instantiate
            // Need to make sure when compiling for arm/raspi that it is creating a UnixSpiDevice 
            // https://github.com/dotnet/iot/blob/e0dbd643875189804da3f090fd26227f9b01a13c/src/System.Device.Gpio/System/Device/Spi/SpiDevice.cs
            var spi = SpiDevice.Create(settings);

            this.ledDevice = new Ws2812b(spi, Constants.TotalLeds);
            this.deviceImage = this.ledDevice.Image;
            this.isActive = true;
        }

        protected override void SendFrame(List<Pixel> frame)
        {
            if (frame?.Count() != Constants.TotalLeds)
            {
                throw new ArgumentException($"Error, expected {Constants.TotalLeds} pixels, got {frame?.Count()}");
            }

            foreach (var pixel in frame)
            {
                this.deviceImage.SetPixel(pixel.Index, 0, pixel.Color);
            }
            this.ledDevice.Update();
        }
    }
}
