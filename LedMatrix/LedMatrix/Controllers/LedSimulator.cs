using LedMatrix.Helpers;
using LedMatrix.Models;
using System.Net;
using System.Net.Sockets;

namespace LedMatrix.Controllers
{
    public class LedSimulator : ControllerBase
    {
        private bool disableProgramStart;
        private Socket? dataSender;

        public LedSimulator(bool disableProgramStart = false) : base()
        {
            this.disableProgramStart = disableProgramStart;

            if (!disableProgramStart)
            {
                this.StartSimulator();
            }

            this.dataSender = ConnectSocket();

            if (this.dataSender?.Connected == true)
            {
                this.isActive = true;
            }
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        private void StartSimulator()
        {
            if (!disableProgramStart)
            {
                using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
                {
                    pProcess.StartInfo.FileName = Constants.ExecutablePath;
                    pProcess.Start();
                }
            }
        }

        private Socket ConnectSocket()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Constants.ServerPort);
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(remoteEP);
                return sender;
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                return null;
            }
        }

        protected override void SendFrame(List<Pixel> frame)
        {
            if (frame != null && this.dataSender?.Connected == true)
            {
                byte[] data = Utility.EncodePixels(frame);
                try
                {
                    this.dataSender.Send(data);
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                    this.StopFrameBuffer();
                }
            }
        }
    }
}

