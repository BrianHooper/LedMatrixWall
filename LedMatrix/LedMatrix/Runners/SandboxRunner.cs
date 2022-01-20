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
                Thread.Sleep(5); //Constants.MsPerFrame
            }
        }

        private void ShowSampleSceneLoop(CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(@"C:\Users\brian\Documents\code\LedMatrixWall\LedMatrix\Data\Scenes\TestScene2").OrderBy(f => GetOrder(f)).ToList();
            if (files?.Any() != true)
            {
                Console.WriteLine($"Error: couldn't find frame directory");
                return;
            }

            var idx = 0;
            while (KeepRunning(cancellationToken) && idx < files.Count())
            {
                var filePath = files[idx++];
                var frame = Utility.LoadFrameFromFile(filePath);

                if (frame == null)
                {
                    Console.WriteLine($"Error: couldn't load frame {filePath}");
                }
                else
                {
                    this.Controller.Paint(frame);
                    //Thread.Sleep(5); //Constants.MsPerFrame
                }
            }
        }

        private int GetOrder(string filepath)
        {
            var filename = Path.GetFileNameWithoutExtension(filepath);
            var parts = filename.Split(new char[] { '_' });
            return int.Parse(parts[1]);
        }

        protected override void Run(CancellationToken cancellationToken)
        {
            Console.WriteLine("SandboxRunner is started");
            while (KeepRunning(cancellationToken))
            {
                ShowSampleSceneLoop(cancellationToken);
            }
        }
    }
}
