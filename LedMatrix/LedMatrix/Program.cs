using LedMatrix.Controllers;
using LedMatrix.Helpers;
using LedMatrix.Models;

namespace LedMatrix
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var pixel = new Pixel(3, 4, 6, 6);
            Start();
        }

        private static void Start()
        {
            IControllerBase controller = Environment.OSVersion.Platform == PlatformID.Win32NT ? new LedSimulator(false) : new LedController();
            new ProgramRunner(controller).Run();
        }
    }
}