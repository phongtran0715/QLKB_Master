using DirectX.Capture;
using PhanMemNoiSoi.Properties;
using PhanMemNoiSoi.SplashScreen;
using System;
using System.Threading;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    static class Program
    {
        private static Mutex mutex = null;
        private const int DB_VERSION = 400;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string appName = "QLKB";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                string message = "Chương trình <" + appName + "> đang chạy ! Vui lòng thoát chương trình đang chạy trước đó.";
                MessageBox.Show(message);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Helper helper = new Helper();

            Splasher.Show(typeof(SplashScreenFrm));
            Splasher.Status = "Đang khởi tạo camera ...";
            initCamera();

            FingerPrint print = new FingerPrint();
            Splasher.Status = "Đang khởi tạo dữ liệu...";
            //Check key license
            string inputString = print.GenKey(print.cpuId() + print.baseId());
            string keyGen = print.GenKey(helper.RemoveWhitespace(inputString));
            string softwareLicense = Settings.Default.softwareLicense;
            Session.Instance.ActiveLicense = string.Equals(keyGen, softwareLicense);
            Settings.Default.ClientId = inputString;
            Settings.Default.Save();

            //check database connection
            if (!DBConnection.Instance.OpenConnection())
            {
                Splasher.Close();
                Application.Run(new ConfigDB());
            }
            else
            {
                int dbVersion = helper.getDbVersion();
                if(dbVersion != DB_VERSION)
                {
                    ConfigDB configDb = new ConfigDB();
                    configDb.upgradeDB(DB_VERSION);
                }                
                Application.Run(new Login());
            }
        }

        static bool initCamera()
        {
            Capture capture = null;
            Filters filters = null; ;
            Capture.HeFrame heFrame = null;
            bool isSuccess = false;
            bool isSucess = false;
            //Init with config param loaded from DB
            try
            {
                filters = new Filters();
                //Load config from DB
                string videoDevice = Settings.Default.vDevice;
                string videoCompressor = Settings.Default.vCompress;
                if ((videoDevice != null) && (videoDevice != ""))
                {
                    capture = new Capture(new Filter(videoDevice));
                    if ((videoCompressor != null) && (videoCompressor != ""))
                    {
                        capture.VideoCompressor = new Filter(videoCompressor);
                    }
                    isSucess = true;
                }
                else
                {
                    isSucess = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                isSucess = false;
            }

            if (isSucess == false)
            {
                //Auto reinit camera with default param
                try
                {
                    if(filters != null)
                    {
                        capture = new Capture(filters.VideoInputDevices[0]);
                        //Save new config param 
                        Settings.Default.vDevice = capture.VideoDevice.MonikerString;
                        if (capture.VideoCompressor != null)
                        {
                            Settings.Default.vCompress = capture.VideoCompressor.MonikerString;
                        }
                        Settings.Default.Save();
                        isSucess = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    isSucess = false;
                }
            }

            //init for capture frame
            if (isSucess)
            {
                try
                {
                    heFrame = new Capture.HeFrame(CaptureComplete);
                    if(capture != null)
                    {
                        capture.FrameEvent2 += heFrame;
                        capture.GrapImg();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return isSuccess;
        }

        static void CaptureComplete(System.Drawing.Bitmap e)
        {
        }
    }
}
