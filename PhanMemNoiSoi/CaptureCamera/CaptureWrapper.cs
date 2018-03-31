namespace PhanMemNoiSoi.CaptureCamera
{
    class CaptureWrapper
    {
        public int vDeviceIndex;
        public int vCompressIndex;
        public int vSourceIndex;
        public int vFrameSizeX;
        public int vFrameSizeY;
        public double vFrameRate;

        public CaptureWrapper()
        {
            vDeviceIndex    = Properties.Settings.Default.vDeviceIndex;
            vFrameSizeX     = Properties.Settings.Default.vFrameSizeX;
            vFrameSizeY     = Properties.Settings.Default.vFrameSizeY;
            vFrameRate      = Properties.Settings.Default.vFrameRate;
            vCompressIndex  = Properties.Settings.Default.vCompressIndex;
            vSourceIndex    = Properties.Settings.Default.vSourceIndex;
        }
    }
}
