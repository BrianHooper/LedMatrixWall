using LedMatrix.Helpers;
using System.Drawing;

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

        public Pixel(int index)
        {
            Init(index);
        }

        public Pixel(int x, int y)
        {
            if (x == 14 && y == 10)
            {
                Console.WriteLine();
            }

            var panelRow = y / Constants.LedRowsPerNode;
            var panelColumn = x / Constants.LedColumnsPerNode;
            var nodeRow = y % Constants.LedRowsPerNode;
            var nodeColumn = x % Constants.LedColumnsPerNode;

            Init(panelRow, panelColumn, nodeRow, nodeColumn);
        }

        public Pixel(int panelRow, int panelColumn, int nodeRow, int nodeColumn)
        {
            Init(panelRow, panelColumn, nodeRow, nodeColumn);
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

        public Pixel(byte[] data, int startIdx)
        {
            var span = data.AsSpan();
            this.Index = BitConverter.ToInt32(span.Slice(startIdx, 4));
            this.Color = Color.FromArgb(BitConverter.ToInt32(span.Slice(startIdx + 4, 4)));
            Init(this.Index);
        }

        public byte[] EncodeAsByte()
        {
            var indexEncoded = BitConverter.GetBytes(this.Index);
            var colorEncoded = BitConverter.GetBytes(this.Color.ToArgb());
            return indexEncoded.Concat(colorEncoded).ToArray();
        }
    }
}
