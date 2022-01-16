using LedMatrix.Controllers;
using LedMatrix.Helpers;

namespace LedMatrix
{
    public class Program
    {
        private IControllerBase ledController;

        private void ShowColorLoop()
        {
            var colorLoop = new ColorLoop();
            for (int i = 0; i < 100; i++)
            {
                var frame = colorLoop.NextFrame();
                this.ledController.Paint(frame);
                Thread.Sleep(50);
            }
        }

        public void Run()
        {
            this.ledController = new LedSimulator();
            ShowColorLoop();
            Thread.Sleep(10000);
            ShowColorLoop();
        }

        static void Main(string[] args)
        {
            new Program().Run();
        }
    }
}