using System.Windows.Forms;
using System.Collections.Concurrent;
using LedMatrix.Helpers;
using System.Diagnostics;

namespace LedSimulator
{
    public partial class FrameRenderer : Form
    {
        private FixedSizedQueue<List<Color>> pixelQueue { get; set; }
        private List<Color> pixels;
        private StringFormat stringFormat;

        public FrameRenderer()
        {
            InitializeComponent();
            this.Show();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.pixelQueue = new FixedSizedQueue<List<Color>>(Constants.PixelBufferQueueLimit);
            this.pixels = new List<Color>();

            this.stringFormat = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            };

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = Constants.MsPerFrame;
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }

        public void SendFrame(List<Color> pixels)
        {
            this.pixelQueue.Enqueue(pixels);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (pixels?.Any() != true)
            {
                return;
            }
            using (SolidBrush t = new SolidBrush(Color.White))
            {
                using (SolidBrush f = new SolidBrush(Color.Black))
                {
                    for (int i = 0; i < pixels.Count; i++)
                    {
                        var color = pixels[i];
                        var location = Utility.GetPixelLocation(i);
                        var boundingRectangle = new Rectangle(location.X, location.Y, Constants.PixelDiameter, Constants.PixelDiameter);
                        f.Color = pixels[i];
                        e.Graphics.FillRectangle(f, boundingRectangle);
                        //e.Graphics.DrawString((i + 1).ToString(), Font, t, boundingRectangle, stringFormat);
                    }
                }
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (pixelQueue.TryDequeue(out var updatedPixels))
            {
                this.pixels = updatedPixels;
                this.Invalidate();
            }
        }
    }
}