using System.Device.Spi;
using Iot.Device.Ws28xx;

namespace LedMatrix
{
    public class LedController
    {
        // TODO Load these from config
        private const int numPanels = 20;
        private const int ledColumnsPerPanel = 7;
        private const int ledRowsPerPanel = 7;

        private int totalLedCount;
        private Ws2812b ledDriver;

        public LedController()
        {
            this.totalLedCount = numPanels * ledColumnsPerPanel * ledRowsPerPanel;
            this.ledDriver = Connect();
        }

        private Ws2812b Connect()
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
            return new Ws2812b(spi, this.totalLedCount);
        }
    }
}
