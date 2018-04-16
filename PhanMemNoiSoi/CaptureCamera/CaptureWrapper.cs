using DirectX.Capture;
using System;
using System.Diagnostics;
using System.Drawing;

namespace PhanMemNoiSoi.CaptureCamera
{
    class CaptureWrapper
    {
        public int vDeviceIndex;
        public int vCompressIndex;
        public int vFrameSizeHeigh;
        public int vFrameSizeWidth;
        public double vFrameRate;
        public Capture capture = null;
        public Filters filters = new Filters();

        private static CaptureWrapper instance = null;

        public static CaptureWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CaptureWrapper();
                }
                return instance;
            }
        }

        private CaptureWrapper()
        {
            syncProperty();
        }

        public bool initTestCamera(System.Windows.Forms.PictureBox pBox)
        {
            bool exitCode = false;
            try
            {
                if (vDeviceIndex >= filters.VideoInputDevices.Count || vDeviceIndex < 0)
                {
                    vDeviceIndex = 0;
                }

                capture = new Capture(filters.VideoInputDevices[vDeviceIndex]);
                capture.CaptureComplete += new EventHandler(OnCaptureComplete);
                capture.PreviewWindow = pBox;
                exitCode = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                exitCode = false;
            }
            return exitCode;
        }

        public bool initCamera(System.Windows.Forms.PictureBox pBox)
        {
            bool exitCode = false;
            try
            {
                if (vDeviceIndex >= filters.VideoInputDevices.Count || vDeviceIndex < 0)
                {
                    vDeviceIndex = 0;
                }

                capture = new Capture(filters.VideoInputDevices[vDeviceIndex]);
                capture.CaptureComplete += new EventHandler(OnCaptureComplete);
                capture.PreviewWindow = pBox;
                //Set capture property
                capture.VideoCompressor = (vCompressIndex > 0 ? filters.VideoCompressors[vCompressIndex - 1] : null);
                capture.FrameRate = vFrameRate;
                Size size = new Size(vFrameSizeWidth, vFrameSizeHeigh);
                capture.FrameSize = size;
                //heFrame = new Capture.HeFrame(CaptureComplete);
                //capture.FrameEvent2 += heFrame;
                //capture.GrapImg();
                
                exitCode = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                exitCode = false;
            }
            return exitCode;
        }
        private void OnCaptureComplete(object sender, EventArgs e)
        {
            // Demonstrate the Capture.CaptureComplete event.
            Debug.WriteLine("Capture complete.");
        }
        public void syncProperty()
        {
            vDeviceIndex    = Properties.Settings.Default.vDeviceIndex;
            vFrameSizeHeigh = Properties.Settings.Default.vFrameSizeHigh;
            vFrameSizeWidth = Properties.Settings.Default.vFrameSizeWidth;
            vFrameRate      = Properties.Settings.Default.vFrameRate;
            vCompressIndex  = Properties.Settings.Default.vCompressIndex;
        }
    }
}
