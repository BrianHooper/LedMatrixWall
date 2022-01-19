using LedMatrix.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedMatrix.Helpers
{
    public class DataUtility
    {
        public DataUtility()
        {
            //JsonSerializer serializer = new JsonSerializer();
            //serializer.NullValueHandling = NullValueHandling.Ignore;
            //using (StreamWriter sw = new StreamWriter(@"SerializedTestFrame.json"))
            //{
            //    sw.Write("");
            //    JsonConvert.Ser
            //    //serializer.Serialize(writer, frame);
            //}
        }

        public List<Pixel> LoadFrameFromFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void WriteFrameToFile(List<Pixel> frame, string filePath)
        {

        }

        public static void GetImageFromMp4(string path)
        {
            // _env is the injected IWebHostEnvironment
            // _tempPath is temporary file storage
            //var ffmpegPath = Path.Combine(_env.ContentRootPath, "<path-to-ffmpeg.exe>");

            //var mediaToolkitService = MediaToolkitService.CreateInstance(ffmpegPath);
            //var metadataTask = new FfTaskGetMetadata(_tempFile);
            //var metadata = await mediaToolkitService.ExecuteAsync(metadataTask);

            //var i = 0;
            //while (i < metadata.Metadata.Streams.First().DurationTs)
            //{
            //    var outputFile = string.Format("{0}\\image-{1:0000}.jpeg", _imageDir, i);
            //    var thumbTask = new FfTaskSaveThumbnail(_tempFile, outputFile, TimeSpan.FromSeconds(i));
            //    _ = await mediaToolkitService.ExecuteAsync(thumbTask);
            //    i++;
            //}
        }

    }
}
