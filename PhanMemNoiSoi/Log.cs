namespace PhanMemNoiSoi
{
    class Log
    {
        private static Log instance = null;
        private static readonly object padlock = new object();

        private Log()
        {
        }

        public static Log Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Log();
                    }
                    return instance;
                }
            }
        }

        public string GetTempPath()
        {
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }
        bool isLog = false;
        //TODO set condition to write log file
        public void LogMessageToFile(string msg)
        {
            if (isLog)
            {
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                GetTempPath() + "QuangHuyMedicalLog.txt");
                try
                {
                    string logLine = System.String.Format(
                        "{0:G}: {1}.", System.DateTime.Now, msg);
                    sw.WriteLine(logLine);
                }
                finally
                {
                    sw.Close();
                }
            }
        }
    }
}
