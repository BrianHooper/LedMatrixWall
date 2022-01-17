using LedMatrix.Models;

namespace LedMatrix
{
    public interface IControllerBase
    {
        public abstract void Clear();

        public abstract void Paint(List<Pixel> pixels);
    }
}
