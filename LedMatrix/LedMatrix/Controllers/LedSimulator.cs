using LedMatrix.Helpers;
using LedMatrix.Models;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace LedMatrix.Controllers
{
    public class LedSimulator : IControllerBase
    {
        private BlockingCollection<List<Pixel>> frameQueue;
        private CancellationTokenSource cancellationTokenSource;
        private bool disableProgramStart;

        public LedSimulator(bool disableProgramStart = false)
        {
            this.disableProgramStart = disableProgramStart;
            this.cancellationTokenSource = new CancellationTokenSource();
            this.frameQueue = new BlockingCollection<List<Pixel>>(new ConcurrentBag<List<Pixel>>(), 1000);
            Thread t = new Thread(() => StartSimulator(this.cancellationTokenSource.Token));
            t.Start();
            if (!disableProgramStart)
            {
                Console.WriteLine("Waiting for simulator to start");
                Thread.Sleep(2000);
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Paint(List<Pixel> pixels)
        {
            this.frameQueue.Add(pixels);
        }

        public void Close()
        {
            this.cancellationTokenSource.Cancel();
        }

        private void StartSimulator(CancellationToken ct)
        {
            if (disableProgramStart)
            {
                StartClient(ct);
            }
            else
            {
                using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
                {
                    pProcess.StartInfo.FileName = Constants.ExecutablePath;
                    pProcess.Start();
                    StartClient(ct);
                    pProcess.Close();
                }
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

                            byte[] data = Utility.EncodePixels(frame);

                            //var encodedLength = string.Join(", ", data.Take(4));
                            //Console.WriteLine($"Sending data packet with {frame.Count()} pixels: [{encodedLength}]");
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

