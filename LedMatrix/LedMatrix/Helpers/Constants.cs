using System.Net;

namespace LedMatrix.Helpers
{
    public static class Constants
    {
        public const int NodesPerRow = 5;
        public const int NodesPerColumn = 4;
        public const int LedColumnsPerNode = 7;
        public const int LedRowsPerNode = 7;

        public const int TotalNodes = NodesPerRow * NodesPerColumn;
        public const int LedsPerNode = LedColumnsPerNode * LedRowsPerNode;
        public const int TotalLeds = TotalNodes * LedsPerNode;

        // Simulator
        public const string ExecutablePath = @"C:\Users\brian\Documents\code\LedMatrixWall\LedMatrix\LedSimulator\bin\Release\net6.0-windows\LedSimulator.exe";
        public const int PixelDiameter = 25;
        public const int PixelSpacing = 5;
        public const int PixelWithSpacing = PixelDiameter + PixelSpacing;
        public const int PanelPixelWidth = LedColumnsPerNode * (PixelDiameter + PixelSpacing);
        public const int PanelPixelHeight = LedRowsPerNode * (PixelDiameter + PixelSpacing);
        public const int PixelBufferQueueLimit = 5;
        public const int ServerPort = 11113; 
        public const int FramesPerSecond = 20;
        public const int MsPerFrame = 1000 / FramesPerSecond;
    }
}
