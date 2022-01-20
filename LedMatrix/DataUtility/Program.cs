using LedMatrix.Helpers;
using LedMatrix.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;

namespace LedMatrix
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sourceFramesFolder = @"C:\Users\brian\Documents\code\LedMatrixWall\LedMatrix\Data\CountdownFrames";
            var destinationFolderName = "TestScene2";
            ConvertListOfFrameImages(sourceFramesFolder, destinationFolderName);

            Console.WriteLine("Done");
        }

        public static void ConvertListOfFrameImages(string sourceFolder, string destinationFolderName)
        {
            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine($"Error: unable to find source directory {sourceFolder}");
                return;
            }

            var destinationFolder = Path.Combine(Constants.ScenesDirectory, destinationFolderName);
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            if (!Directory.Exists(destinationFolder))
            {
                Console.WriteLine($"Error: unable to create directory {destinationFolder}");
                return;
            }

            foreach(var framePath in Directory.GetFiles(sourceFolder))
            {
                var frame = Utility.ConvertFromImage(framePath);
                var frameName = Path.GetFileNameWithoutExtension(framePath) + ".frm";
                var destination = Path.Combine(destinationFolder, frameName);
                Utility.WriteFrameToFile(frame, destination);
            }
        }

        private static void Benchmark(Action action)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
        }

        private static void Create5000Frames()
        {
            for (int i = 0; i < 5000; i++)
            {
                var frame = new List<Pixel>();
                for (int j = 0; j < Constants.TotalLeds; j++)
                {
                    frame.Add(Pixel.FromIndexAndColor(j, Color.Black));
                }
            }
        }

        public static List<Pixel> DeserializeFrame()
        {
            return null;
        }

        public static string SerializeFrame(List<Pixel> frame)
        {
            return JsonConvert.SerializeObject(frame);
        }
    }
}