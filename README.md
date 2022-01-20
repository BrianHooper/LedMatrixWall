# Relevant documentation:
	https://jamesnaylor.dev/Posts/Read/pi-ws2812-with-net-core
	https://github.com/dotnet/iot#how-to-install
	https://github.com/dotnet/core/blob/main/samples/RaspberryPiInstructions.md
	https://learn.adafruit.com/retro-gaming-with-raspberry-pi/adding-controls-hardware
	https://www.raspberryconnect.com/projects/65-raspberrypi-hotspot-accesspoints/158-raspberry-pi-auto-wifi-hotspot-switch-direct-connection

# Installed the following packages from azure nuget feed:
    System.Device.Gpio v1.5.0
    Iot.Device.Bindings v1.5.0

# Convert video to list of frames
	ffmpeg.exe -i "C:\Users\brian\Documents\code\LedMatrixWall\LedMatrix\Data\BackBeautifulEider-mobile.mp4" frame_%0d.png
