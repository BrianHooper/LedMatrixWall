using LedMatrix.Controllers;
using LedMatrix.Helpers;
using LedMatrix.Models;
using LedMatrix.Runners;

namespace LedMatrix
{
    public class Program
    {
        private Dictionary<RunnerType, RunnerBase> Runners;
        private RunnerBase ActiveRunner;

        static void Main(string[] args)
        {
            new Program().Start();
        }

        private void Start()
        {
            var controller = this.GetController();

            this.Runners = new Dictionary<RunnerType, RunnerBase>() 
            {
                { RunnerType.SandboxRunner, new SandboxRunner(controller) },
                { RunnerType.SceneRunner, new SceneRunner(controller) },
            };

            while(true)
            {
                this.RunnerSelectorHandler();
                Thread.Sleep(5); // Constants.MsPerFrame
            }
        }

        private ControllerBase GetController()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return new LedSimulator();
            }
            else
            {
                return new LedController();
            }
        }
        private void RunnerSelectorHandler()
        {
            var selectorState = GetSelectorState();
            if (!Enum.IsDefined(typeof(RunnerType), selectorState) ) 
            {
                Console.WriteLine($"Error: selector returned invalid runner value {selectorState}");
                return;
            }
            var selectedRunner = (RunnerType)selectorState;
            if (this.ActiveRunner != null && selectedRunner == this.ActiveRunner.RunnerType && this.ActiveRunner.IsRunning())
            {
                return;
            }

            if (this.Runners.TryGetValue(selectedRunner, out var runner))
            {
                if (this.ActiveRunner != null)
                {
                    this.ActiveRunner.Stop();
                }

                this.ActiveRunner = runner;
                if (this.ActiveRunner.HasValidController())
                {
                    this.ActiveRunner.Start();
                }
            }
        }

        private int GetSelectorState()
        {
            return 0;
        }
    }
}