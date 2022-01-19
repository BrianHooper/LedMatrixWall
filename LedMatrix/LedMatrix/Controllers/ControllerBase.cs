using LedMatrix.Helpers;
using LedMatrix.Models;
using System.Collections.Concurrent;
using System.Drawing;

namespace LedMatrix
{
    public abstract class ControllerBase
    {
        protected bool isActive;

        private FixedSizedQueue<List<Pixel>> frameQueue;

        protected ControllerBase()
        {
            this.frameQueue = new FixedSizedQueue<List<Pixel>>(Constants.PixelBufferQueueLimit);
            var timer = Utility.StartTimer(Constants.MsPerFrame, ProcessFrame);
        }

        protected abstract void SendFrame(List<Pixel> pixels);

        public void ClearPanel()
        {
            var blankPixels = new List<Pixel>();
            for(int i = 0; i < Constants.TotalLeds; i++)
            {
                blankPixels.Add(new Pixel(i, Color.Black));
            }
            this.SendFrame(blankPixels);
        }

        public void ClearFrameQueue()
        {
            this.frameQueue.Clear();
        }

        private void ProcessFrame(Object? source, System.Timers.ElapsedEventArgs e)
        {
            if (this.IsActive() && this.frameQueue?.TryDequeue(out var frame) == true)
            {
                this.SendFrame(frame);
            }
        }

        protected void StopFrameBuffer()
        {
            this.isActive = false;
        }

        public bool Paint(List<Pixel> pixels)
        {
            if (this.isActive)
            {
                this.frameQueue.EnqueueWithLimit(pixels);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PaintWithBufferLimit(List<Pixel> pixels)
        {
            if (this.isActive)
            {
                this.frameQueue.Enqueue(pixels);
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
