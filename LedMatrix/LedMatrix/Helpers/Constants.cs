using System.Net;

namespace LedMatrix.Helpers
{
    public static class Constants
    {
        public const int NodesPerRow = 5;
        public const int NodesPerColumn = 4;
        public const int LedColumnsPerNode = 7;
        public const int LedRowsPerNode = 7;

        public const int TotalNodes = NodesPerRow * NodesPerColumn; // 20
        public const int LedsPerNode = LedColumnsPerNode * LedRowsPerNode; // 49
        public const int LedsPerPanelRow = LedsPerNode * NodesPerRow; // 245
        public const int LedsPerRow = LedColumnsPerNode * NodesPerRow; // 35
        public const int LedsPerColumn = LedRowsPerNode * NodesPerColumn; // 28

        public const int TotalLeds = TotalNodes * LedsPerNode; // 980

        public const int FrameBufferMaxSize = 1000;
        public const int FrameBufferWaitMs = 30;
        public const int RunnerCancelTimeoutMs = 1000;
        public const int RealTimeQueueLimit = 5;
        public const int DeferredQueueLimit = 100;

        public const string ScenesDirectory = @"..\..\..\..\Data\Scenes";

        // Simulator
        public const string ExecutablePath = @"C:\Users\brian\Documents\code\LedMatrixWall\LedMatrix\LedSimulator\bin\Debug\net6.0-windows\LedSimulator.exe";
        public const int PixelDiameter = 20;
        public const int PixelSpacing = 7;
        public const int PixelWithSpacing = PixelDiameter + PixelSpacing;
        public const int PanelPixelWidth = LedColumnsPerNode * (PixelDiameter + PixelSpacing);
        public const int PanelPixelHeight = LedRowsPerNode * (PixelDiameter + PixelSpacing);
        public const int FrameBufferQueueLimit = 5;
        public const int ServerPort = 11113; 
        public const int FramesPerSecond = 30;
        public const int MsPerFrame = 1000 / FramesPerSecond;
        public const int DataPacketByteSize = 8192;
    }
}
