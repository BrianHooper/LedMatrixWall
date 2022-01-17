using LedMatrix.Helpers;

namespace LedMatrix
{
    public class ProgramRunner
    {
        private ControllerBase controller;

        public ProgramRunner(ControllerBase controller)
        {
            this.controller = controller;
        }

        public void Run()
        {
            //ShowImage();
            ShowColorLoop();
        }

        private void ShowImage()
        {
            var pixels = Utility.ConvertFromImage(@"C:\Users\brian\Documents\code\LedMatrixWall\LedMatrix\TestImage.png");
            this.controller.Paint(pixels);
        }

        private void ShowColorLoop()
        {
            var colorLoop = new ColorLoop();
            while (controller.IsActive())
            {
                var frame = colorLoop.NextPixelFrame();
                this.controller.Paint(frame);
                Thread.Sleep(50);
            }
        }
    }
}
