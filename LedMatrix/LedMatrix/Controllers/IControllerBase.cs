using System.Drawing;

namespace LedMatrix
{
    public interface IControllerBase
    {
        public abstract void Clear();

        public abstract void Paint(List<Color> pixels);
    }
}
