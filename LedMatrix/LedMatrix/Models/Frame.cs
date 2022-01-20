namespace LedMatrix.Models
{
    public class Frame
    {
        public List<Pixel> Pixels { get; set; }

        public int FramesToShow { get; set; }

        public Frame(List<Pixel> pixels, int framesToShow = 1)
        {
            this.Pixels = pixels;
            this.FramesToShow = framesToShow;
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
