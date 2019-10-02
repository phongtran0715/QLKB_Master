using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;

namespace PhanMemNoiSoi
{
    class MyBackupRestore
    {
        string databaseName = Properties.Settings.Default.dbName;
        string serverInstance = Properties.Settings.Default.serverName;
        Server srv;
        ServerConnection conn;

        public void connect()
        {
            try
            {
                conn = new ServerConnection();
                conn.ServerInstance = serverInstance;
                srv = new Server(conn);
            }
            catch (Exception)
            {

            }
        }

        public bool backup(string fileName)
        {
            bool exitCode = false;
            Backup bkp = new Backup();
            try
            {
                bkp.Action = BackupActionType.Database;
                bkp.Database = databaseName;
                bkp.Devices.AddDevice(fileName, DeviceType.File);
                bkp.Incremental = false;
                bkp.SqlBackup(srv);
                exitCode = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return exitCode;
        }

        public bool restore(string fileName)
        {
            bool exitCode = false;
            Restore res = new Restore();
            try
            {
                res.Database = databaseName;
                res.Action = RestoreActionType.Database;
                res.Devices.AddDevice(fileName, DeviceType.File);

                res.PercentCompleteNotification = 10;
                res.ReplaceDatabase = true;
                res.SqlRestore(srv);
                exitCode = true;
            }
            catch (SmoException exSMO)
            {
                Console.WriteLine(exSMO.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return exitCode;
        }
    }
}
