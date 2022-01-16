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
                var pixels = new List<Color>();
                for (int i = 0; i < Constants.TotalLeds; i++)
                {
                    pixels.Add(colorLoop.Next());
                }
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

            byte[] bytes = new Byte[4096];

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                Debug.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();

                while (!ct.IsCancellationRequested)
                {
                    var data = string.Empty;
                    while (!ct.IsCancellationRequested)
                    {
                        int bytesRec = handler.Receive(bytes);
                        if (bytesRec > 0)
                        {
                            var pixels = Utility.DecodeColorList(bytes);
                            if (pixels != null)
                            {
                                renderer.SendFrame(pixels);
                            }
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