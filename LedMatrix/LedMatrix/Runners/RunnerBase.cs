using LedMatrix.Helpers;

namespace LedMatrix.Runners
{
    public abstract class RunnerBase
    {
        protected ControllerBase Controller { get; private set; }
        public RunnerType RunnerType { get; private set; }

        private CancellationTokenSource? CancellationTokenSource { get; set; }
        private Thread? RunnerThread { get; set; }

        protected int FrameQueueSizeLimit { get; private set; }


        public RunnerBase(RunnerType runnerType, ControllerBase ledController, int frameQueueSizeLimit)
        {
            this.RunnerType = runnerType;
            this.Controller = ledController;
            this.FrameQueueSizeLimit = frameQueueSizeLimit;
        }

        public bool IsRunning()
        {
            return this.RunnerThread?.IsAlive == true;
        }

        public void Start()
        {
            if (this.RunnerThread?.IsAlive == true)
            {
                Console.WriteLine($"Error: Can't start runner process for {this.RunnerType}, previous runner thread is still alive.");
                return;
            }

            this.CancellationTokenSource = new CancellationTokenSource();
            this.RunnerThread = new Thread(() => Run(this.CancellationTokenSource.Token));
            this.RunnerThread.Start();
        }

        public void Stop()
        {
            if (this.RunnerThread == null || !this.RunnerThread.IsAlive)
            {
                if (this.CancellationTokenSource != null)
                {
                    this.CancellationTokenSource.Dispose();
                    this.CancellationTokenSource = null;
                }
                return;
            }

            if (this.CancellationTokenSource == null)
            {
                this.RunnerThread.Join(Constants.RunnerCancelTimeoutMs);
                Console.WriteLine($"Error attempting to stop {this.RunnerType} runner, thread was active but cancellationToken was invalid");
                return;
            }

            this.CancellationTokenSource.Cancel();
            this.RunnerThread.Join(Constants.RunnerCancelTimeoutMs);
            this.CancellationTokenSource.Dispose();
            this.CancellationTokenSource = null;
            
            if (this.RunnerThread.IsAlive)
            {
                Console.WriteLine($"Error attempting to stop {this.RunnerType} runner, runner is still alive after timeout");
                return;
            }
        }

        public bool HasValidController()
        {
            return this.Controller.IsActive();
        }

        protected bool KeepRunning(CancellationToken cancellationToken)
        {
            return !cancellationToken.IsCancellationRequested && this.Controller.IsActive();
        }

        protected abstract void Run(CancellationToken cancellationToken);
    }
}
