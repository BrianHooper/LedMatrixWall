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
            var frame = new List<Pixel>();
            for (int i = 0; i < Constants.TotalLeds; i++)
            {
                frame.Add(new Pixel(i, Color.FromArgb(255, 219, 42, 42)));
            }

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (StreamWriter sw = new StreamWriter(@"SerializedTestFrame.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, frame);
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
                    frame.Add(new Pixel(j, Color.Black));
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