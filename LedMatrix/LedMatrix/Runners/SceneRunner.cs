using LedMatrix.Models;

namespace LedMatrix.Runners
{
    public class SceneRunner : RunnerBase
    {
        public SceneRunner(ControllerBase ledController) : base(RunnerType.SandboxRunner, ledController)
        {
            
        }

        protected override void Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
