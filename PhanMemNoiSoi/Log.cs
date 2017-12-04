using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    class Log
    {
        private static Log instance = null;
        private static readonly object padlock = new object();
        bool isLog = false;

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
        
        public void LogMessageToDB(DateTime datetime, int uId, string uName, string msg)
        {
            try
            {
                //delete data older than 60 days
                string sqlQuery = "DELETE FROM WorkLog WHERE[dbo].[WorkLog].[TimeLog] < GETDATE() - 60;";
                SqlCommand mySQL = new SqlCommand(sqlQuery, DBConnection.Instance.sqlConn);
                mySQL.ExecuteNonQuery();
                //insert new record
                sqlQuery = "INSERT INTO WorkLog(TimeLog, UserId, UserName, Descript) VALUES (@datetime, @id, @name, @msg)";
                mySQL = new SqlCommand(sqlQuery, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@datetime", SqlDbType.DateTime).Value = String.Format("{0:g}", datetime);
                mySQL.Parameters.Add("@id", SqlDbType.Int).Value = uId;
                mySQL.Parameters.Add("@name", SqlDbType.NChar).Value = uName;
                mySQL.Parameters.Add("@msg", SqlDbType.NChar).Value = msg;
                mySQL.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Không ghi được log chương trình. Không kết nối được đến CSDL \n" + ex.Message);
                MessageBox.Show("Không ghi được log hoạt động của chương trình \n" + ex.ToString());
                return;
            }
        }
    }
}
