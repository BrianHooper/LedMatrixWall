using System.Drawing;

namespace LedMatrix.Helpers
{
    public class Utility
    {
        public static (int PanelRow, int PanelCol, int NodeRow, int NodeCol) GetPanelLocation(int index)
        {
            var panelIndex = index / Constants.LedsPerNode;
            var panelRow = panelIndex / Constants.NodesPerRow;
            var panelCol = panelIndex % Constants.NodesPerRow;

            var nodeIndex = index % Constants.LedsPerNode;
            var nodeRow = nodeIndex / Constants.LedRowsPerNode;
            var nodeCol = nodeIndex % Constants.LedRowsPerNode;

            return (panelRow, panelCol, nodeRow, nodeCol);
        }

        public static (int X, int Y) GetPixelLocation(int index)
        {
            var panelLocation = GetPanelLocation(index);

            var panelStartX = panelLocation.PanelCol * Constants.PanelPixelWidth;
            var panelStartY = panelLocation.PanelRow * Constants.PanelPixelHeight;

            var x = panelStartX + (panelLocation.NodeCol * Constants.PixelWithSpacing);
            var y = panelStartY + (panelLocation.NodeRow * Constants.PixelWithSpacing);
            return (x, y);
        }

        public static byte[] EncodeColorList(List<Color> colors)
        {
            var colorBytes = colors.SelectMany(c => new byte[] { c.R, c.G, c.B });
            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(colorBytes.Count()).ToList());
            data.AddRange(colorBytes);
            return data.ToArray();
        }

        public static List<Color> DecodeColorList(byte[] data)
        {
            var length = BitConverter.ToInt32(data.Take(4).ToArray());

            var colors = new List<Color>();
            for (int i = 4; i <= length + 2; i += 3)
            {
                var r = data[i];
                var g = data[i + 1];
                var b = data[i + 2];

                var color = Color.FromArgb(r, g, b);
                colors.Add(color);
            }
            

            return colors;
        }
    }
}
