using LedMatrix.Helpers;

namespace LedMatrix.Runners
{
    public class SandboxRunner : RunnerBase
    {
        public SandboxRunner(ControllerBase ledController) : base(RunnerType.SandboxRunner, ledController, Constants.DeferredQueueLimit)
        {
        }

        private void ShowImage()
        {
            var frame = Utility.ConvertFromImage(@"C:\Users\brian\Pictures\DaddyIssues.jpg");
            this.Controller.Paint(frame);
        }

        private void ShowColorLoop(CancellationToken cancellationToken)
        {
            var colorLoop = new ColorLoop();
            while (KeepRunning(cancellationToken))
            {
                var frame = colorLoop.NextPixelFrame();
                this.Controller.Paint(frame);
                Thread.Sleep(Constants.MsPerFrame);
            }
        }

        protected override void Run(CancellationToken cancellationToken)
        {
            Console.WriteLine("SandboxRunner is started");
            while (KeepRunning(cancellationToken))
            {
                ShowColorLoop(cancellationToken);
            }
        }
    }
}
