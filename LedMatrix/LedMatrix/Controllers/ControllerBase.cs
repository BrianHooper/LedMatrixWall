using LedMatrix.Helpers;
using LedMatrix.Models;
using System.Collections.Concurrent;

namespace LedMatrix
{
    public abstract class ControllerBase
    {
        protected BlockingCollection<List<Pixel>> frameBuffer;
        private CancellationTokenSource cancellationTokenSource;
        protected bool isActive;

        protected ControllerBase()
        {
            this.frameBuffer = new BlockingCollection<List<Pixel>>(new ConcurrentBag<List<Pixel>>(), Constants.FrameBufferMaxSize);
            this.cancellationTokenSource = new CancellationTokenSource();
            Thread t2 = new Thread(() => ProcessFrames(this.cancellationTokenSource.Token));
            t2.Start();
        }

        protected abstract void SendFrame(List<Pixel> frame);

        public abstract void Clear();

        private void ProcessFrames(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (this.isActive && this.frameBuffer.TryTake(out var frame, 50, ct))
                    {
                        this.SendFrame(frame);
                        Thread.Sleep(Constants.FrameBufferWaitMs);
                    }
                }
                catch (OperationCanceledException)
                {

                }
            }
        }

        protected void StopFrameBuffer()
        {
            this.cancellationTokenSource.Dispose();
            this.isActive = false;
        }

        public bool Paint(List<Pixel> pixels)
        {
            if (this.isActive)
            {
                this.frameBuffer.Add(pixels);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsActive()
        {
            return this.isActive;
        }
    }
}
