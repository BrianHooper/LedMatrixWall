using LedMatrix.Helpers;
using System.Drawing;
using System.Text.Json.Serialization;

namespace LedMatrix.Models
{
    public class Pixel
    {
        public int PanelRow { get; set; }
        public int PanelColumn { get; set; }
        public int NodeRow { get; set; }
        public int NodeColumn { get; set; }
        public int Index { get; set; }
        public Color Color { get; set; }

        [JsonConstructor]
        public Pixel(int index, int panelRow, int panelColumn, int nodeRow, int nodeColumn, Color color)
        {
            this.Index = index;
            this.PanelRow = panelRow;
            this.PanelColumn = panelColumn;
            this.NodeRow = nodeRow;
            this.NodeColumn = nodeColumn;
            this.Color = color;
        }
        
        public static Pixel FromIndexAndColor(int index, Color color)
        {
            var pixel = new Pixel(index, 0, 0, 0, 0, color);
            pixel.Init(index);
            return pixel;
        }
        
        public static Pixel FromXYAndColor(int x, int y, Color color)
        {
            var panelRow = y / Constants.LedRowsPerNode;
            var panelColumn = x / Constants.LedColumnsPerNode;
            var nodeRow = y % Constants.LedRowsPerNode;
            var nodeColumn = x % Constants.LedColumnsPerNode;
            return FromPanelLocationAndColor(panelRow, panelColumn, nodeRow, nodeColumn, color);
        }

        public static Pixel FromPanelLocationAndColor(int panelRow, int panelColumn, int nodeRow, int nodeColumn, Color color)
        {
            var pixel = new Pixel(0, panelRow, panelColumn, nodeRow, nodeColumn, color);
            pixel.Init(panelRow, panelColumn, nodeRow, nodeColumn);
            return pixel;
        }

        public static Pixel FromByte(byte[] data, int startIdx)
        {
            var span = data.AsSpan();
            var index = BitConverter.ToInt32(span.Slice(startIdx, 4));
            var color = Color.FromArgb(BitConverter.ToInt32(span.Slice(startIdx + 4, 4)));
            return FromIndexAndColor(index, color);
        }

        private void Init(int index)
        {
            this.Index = index;

            var panelIndex = index / Constants.LedsPerNode;
            this.PanelRow = panelIndex / Constants.NodesPerRow;
            this.PanelColumn = panelIndex % Constants.NodesPerRow;

            var nodeIndex = index % Constants.LedsPerNode;
            this.NodeRow = nodeIndex / Constants.LedRowsPerNode;
            this.NodeColumn = nodeIndex % Constants.LedRowsPerNode;
        }

        private void Init(int panelRow, int panelColumn, int nodeRow, int nodeColumn)
        {
            this.PanelRow = panelRow;
            this.PanelColumn = panelColumn;
            this.NodeRow = nodeRow;
            this.NodeColumn = nodeColumn;
            this.Index = (panelRow * Constants.LedsPerPanelRow) + (panelColumn * Constants.LedsPerNode) + (Constants.LedColumnsPerNode * nodeRow) + nodeColumn;
        }

        public (int X, int Y) GetPixelLocation()
        {
            var panelStartX = this.PanelColumn * Constants.PanelPixelWidth;
            var panelStartY = this.PanelRow * Constants.PanelPixelHeight;

            var x = panelStartX + (this.NodeColumn * Constants.PixelWithSpacing);
            var y = panelStartY + (this.NodeRow * Constants.PixelWithSpacing);
            return (x, y);
        }

        public byte[] EncodeAsByte()
        {
            var indexEncoded = BitConverter.GetBytes(this.Index);
            var colorEncoded = BitConverter.GetBytes(this.Color.ToArgb());
            return indexEncoded.Concat(colorEncoded).ToArray();
        }
    }
}
