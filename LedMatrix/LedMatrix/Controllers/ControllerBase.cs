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
        private System.Timers.Timer frameTimer;

        protected ControllerBase()
        {
            this.frameQueue = new FixedSizedQueue<List<Pixel>>(Constants.PixelBufferQueueLimit);
            
            this.frameTimer = new System.Timers.Timer();
            this.frameTimer.Interval = Constants.MsPerFrame;
            this.frameTimer.Elapsed += ProcessFrame;
            this.frameTimer.Enabled = true;
        }

        protected abstract void SendFrame(List<Pixel> pixels);

        public void Clear()
        {
            var blankPixels = new List<Pixel>();
            for(int i = 0; i < Constants.TotalLeds; i++)
            {
                blankPixels.Add(new Pixel(i, Color.Black));
            }
            this.SendFrame(blankPixels);
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
