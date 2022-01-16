using LedMatrix.Helpers;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace LedMatrix.Controllers
{
    public class LedSimulator : IControllerBase
    {
        private BlockingCollection<List<Color>> frameQueue;
        private CancellationTokenSource cancellationTokenSource;

        public LedSimulator()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.frameQueue = new BlockingCollection<List<Color>>(new ConcurrentBag<List<Color>>(), 1000);
            Thread t = new Thread(() => StartSimulator(this.cancellationTokenSource.Token));
            t.Start();
            Console.WriteLine("Waiting for simulator to start");
            Thread.Sleep(5000);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Paint(List<Color> pixels)
        {
            this.frameQueue.Add(pixels);
        }

        public void Close()
        {
            this.cancellationTokenSource.Cancel();
        }

        private void StartSimulator(CancellationToken ct)
        {
            using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
            {
                pProcess.StartInfo.FileName = Constants.ExecutablePath;
                pProcess.Start();
                StartClient(ct);
                pProcess.Close();
            }
        }

        private void StartClient(CancellationToken ct)
        {

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Constants.ServerPort);
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try

            {
                sender.Connect(remoteEP);

                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        if (this.frameQueue.TryTake(out var frame, 50, ct))
                        {
                            byte[] data = Utility.EncodeColorList(frame);
                            sender.Send(data);
                        }
                    }
                    catch (OperationCanceledException)
                    {

                    }
                }
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }

            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
    }
}

