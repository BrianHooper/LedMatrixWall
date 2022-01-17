using LedMatrix.Helpers;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LedSimulator
{
    internal static class Program
    {
        private const int FPS = 10;


        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var frameListener = new FrameRenderer();

            var communicationMethod = StartListening;

            CancellationTokenSource cts = new CancellationTokenSource();
            Thread t = new Thread(() => communicationMethod(cts.Token, frameListener));
            t.Start();

            Application.Run(frameListener);
            cts.Cancel();
        }

        static void QueueSender(CancellationToken ct, FrameRenderer renderer)
        {
            if (renderer == null)
            {
                return;
            }
            var msPerFrame = 1000 / FPS;

            var colorLoop = new ColorLoop();
            while (!ct.IsCancellationRequested)
            {
                var pixels = colorLoop.NextPixelFrame();
                renderer.SendFrame(pixels);
                Thread.Sleep(msPerFrame);
            }
        }

        public static void StartListening(CancellationToken ct, FrameRenderer renderer)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Constants.ServerPort);
            var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                Debug.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();

                byte[] bytes = new Byte[2 * Constants.DataPacketByteSize];
                List<byte> recievedData = new List<byte>();
                while (!ct.IsCancellationRequested)
                {
                    int bytesRec = handler.Receive(bytes);
                    if (bytesRec > 0)
                    {
                        recievedData.AddRange(bytes);
                        var decodedLength = BitConverter.ToInt32(bytes.Take(4).ToArray());
                        if (decodedLength == -Constants.TotalLeds)
                        {
                            var pixels = Utility.DecodePixels(recievedData.ToArray());
                            if (pixels != null)
                            {
                                renderer.SendFrame(pixels);
                            }
                            recievedData.Clear();
                        }
                    }
                    
                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}