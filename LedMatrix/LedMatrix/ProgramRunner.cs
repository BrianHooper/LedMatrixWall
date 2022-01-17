using LedMatrix.Helpers;

namespace LedMatrix
{
    public class ProgramRunner
    {
        private IControllerBase controller;

        public ProgramRunner(IControllerBase controller)
        {
            this.controller = controller;
        }

        public void Run()
        {
            ShowImage();
            //ShowColorLoop();
        }

        private void ShowImage()
        {
            var pixels = Utility.ConvertFromImage(@"C:\Users\brian\Documents\code\LedMatrixWall\LedMatrix\TestImage.png");
            this.controller.Paint(pixels);
        }

        private void ShowColorLoop()
        {
            var colorLoop = new ColorLoop();
            for (int i = 0; i < 100; i++)
            {
                var frame = colorLoop.NextPixelFrame();
                this.controller.Paint(frame);
                Thread.Sleep(50);
            }
        }
    }
}
