using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PhanMemNoiSoi
{
    class DBConnection
    {
        private static DBConnection instance = null;
        private static readonly object padlock = new object();
        string connetionString = null;
        public SqlConnection sqlConn = null;
        public string sqlInstanceName;
        string dbName;
        int sqlServerType;
        enum SQL_SERVER_TYPE
        {
            MSSQLSERVER = 1,
            SQLEXPRESS
        }

        private DBConnection()
        {
            try
            {
                sqlInstanceName = Properties.Settings.Default.serverName;
                dbName = Properties.Settings.Default.dbName;
                connetionString = @"Data Source= " + sqlInstanceName + ";Initial Catalog= " + dbName + ";" +
                                    "Integrated Security=True;MultipleActiveResultSets=true";

                sqlConn = new SqlConnection(connetionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void getServerName()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
            String[] instances = (String[])rk.GetValue("InstalledInstances");
            if (instances.Length > 0)
            {
                foreach (String element in instances)
                {
                    if (element == "MSSQLSERVER")
                    {
                        sqlServerType = (int)SQL_SERVER_TYPE.MSSQLSERVER;
                    }
                    else
                    {
                        sqlServerType = (int)SQL_SERVER_TYPE.SQLEXPRESS;
                    }
                }
            }

            //create sql server name
            if (sqlServerType == (int)SQL_SERVER_TYPE.MSSQLSERVER)
                sqlInstanceName = System.Environment.MachineName;
            else
                sqlInstanceName = System.Environment.MachineName + @"\SQLEXPRESS";
        }

        public static DBConnection Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DBConnection();
                    }
                    return instance;
                }
            }
        }

        public bool OpenConnection()
        {
            try
            {
                sqlConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR : ");
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
            }
            return false;
        }

        public bool CloseConnection()
        {
            bool exitCode = false;
            if (sqlConn == null)
            {
                exitCode = true;
            }
            else
            {
                try
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                    exitCode = true;
                }
                catch (System.Exception ex)
                {
                    Log.Instance.LogMessageToFile(ex.ToString());
                    Console.Write(ex.ToString());
                }
            }
            return exitCode;
        }

        public void NoExeQuery(string sqlCommand)
        {
            SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
        }

        public bool DBConnectionStatus(string connString)
        {
            bool isOK = false;
            SqlConnection sqlConn = null;
            try
            {
                using (sqlConn =
                    new SqlConnection(connString))
                {
                    sqlConn.Open();
                    if (sqlConn.State == ConnectionState.Open)
                    {
                        isOK = true;
                    }
                }
            }
            catch (SqlException)
            {
                return isOK;
            }
            catch (Exception)
            {
                return isOK;
            }
            finally
            {
                if (isOK)
                {
                    sqlConn.Close();
                }
            }
            return isOK;
        }

        public bool CheckDatabaseExists(string sqlInstanceName, string databaseName)
        {
            bool exitCode = false;
            string connectionString = @"Server = " + sqlInstanceName + ";Trusted_Connection=yes";
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand($"SELECT db_id('{databaseName}')", connection))
                    {
                        connection.Open();
                        exitCode = (command.ExecuteScalar() != DBNull.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
            }
            finally
            {
                if(connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return exitCode;
        }

        public bool CheckConnection(string sqlInstanceName)
        {
            bool exitCode = false;
            string connectionString = @"Server = " + sqlInstanceName + ";Trusted_Connection=yes";
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    exitCode = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return exitCode;
        }

        public bool CheckConnection2(string sqlInstanceName)
        {
            bool exitCode = false;
            try{
                ServerConnection conn = new ServerConnection(sqlInstanceName);
                Server srv = new Server(conn);
                exitCode = true;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return exitCode;
        }
    }
}
