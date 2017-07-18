using System;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string dbServerName = Properties.Settings.Default.serverName;
            if (dbServerName == null || dbServerName == "")
            {
                Application.Run(new ConfigDB());
            }
            else
            {
                DBConnection dbConn = DBConnection.Instance;
                bool isConn = dbConn.OpenConnection();
                if (isConn == false)
                {
                    Application.Run(new ConfigDB());
                    return;
                }
                Application.Run(new Login());
            }
        }
    }
}
