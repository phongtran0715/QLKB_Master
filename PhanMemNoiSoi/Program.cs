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
                Console.WriteLine("Chương trình " + appName + " đang chạy ! Vui lòng thoát chương trình đang chạy trước.");
                Console.ReadKey();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Helper helper = new Helper();

            Splasher.Show(typeof(SplashScreenFrm));
            FingerPrint print = new FingerPrint();
            Splasher.Status = "Đang khởi tạo dữ liệu...";
            //Check key license
            string inputString = print.GenKey(print.cpuId() + print.baseId());
            string keyGen = print.GenKey(helper.RemoveWhitespace(inputString));
            string softwareLicense = Settings.Default.softwareLicense;
            Session.Instance.ActiveLicense = string.Equals(keyGen, softwareLicense);
            Settings.Default.ClientId = inputString;
            Settings.Default.Save();

            Splasher.Status = "Thiết lập kết nối ...";
            Thread.Sleep(500);

            string serverName = Settings.Default.serverName;
            if ((serverName == null) || (serverName == ""))
            {
                Splasher.Close();
                Application.Run(new ConfigDB(true, true));
            }
            else if (!DBConnection.Instance.OpenConnection())
            {
                Splasher.Close();
                Application.Run(new ConfigDB(true, true));
            }
            else
            {
                Application.Run(new Login());
            }
        }
    }
}
