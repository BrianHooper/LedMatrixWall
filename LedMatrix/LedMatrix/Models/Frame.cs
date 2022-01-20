using Newtonsoft.Json;

namespace LedMatrix.Models
{
    public class Frame
    {
        [JsonProperty("Px")]
        public List<Pixel> Pixels { get; set; }

        [JsonProperty("FTS")]
        public int FramesToShow { get; set; }


        [JsonConstructor]
        public Frame(List<Pixel> pixels)
        {
            this.Pixels = pixels;
            this.FramesToShow = 1;
        }

        public List<Pixel> Show()
        {
            this.FramesToShow--;
            return this.Pixels;
        }

        public bool IsShowable()
        {
            return this.FramesToShow > 0;
        }
    }
}
