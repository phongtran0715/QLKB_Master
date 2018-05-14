using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class ConfigDB : Form
    {
        Helper helper = new Helper();
        string sqlInstancename = DBConnection.Instance.sqlInstanceName;
        string cmdCreateDB = Application.StartupPath + "\\CreateDB\\Create.bat";
        string cmdAttachDB = Application.StartupPath + "\\CreateDB\\Attach.bat";
        string scrCreateDB = Application.StartupPath + "\\CreateDB\\CreateDataBaseQuangHuy.sql";
        string scrAttachDB = Application.StartupPath + "\\CreateDB\\AttachDataBaseQuangHuy.sql";
        private string serverInstance;
        private string dbName;
        private string password;
        private string userName;
        private int checkDbErrorCode = 0;

        public ConfigDB()
        {
            this.InitializeComponent();
        }

        private void ConfigDB_Load(object sender, EventArgs e)
        {
            this.lbStatus.Visible = false;
            this.pbStatus.Visible = false;
            this.cbInstance.Items.Add("");
            string[] strArray = (string[])Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server").GetValue("InstalledInstances");
            if ((strArray != null) && (strArray.Length > 0))
            {
                foreach (string str in strArray)
                {
                    if (str == "MSSQLSERVER")
                    {
                        this.cbInstance.Items.Add(Environment.MachineName);
                    }
                    else
                    {
                        this.cbInstance.Items.Add(Environment.MachineName + @"\" + str);
                    }
                }
            }
            this.txtDbName.Text = PhanMemNoiSoi.Properties.Settings.Default.dbName;
            this.txtUser.Text = PhanMemNoiSoi.Properties.Settings.Default.dbUser;
            this.txtPass.Text = PhanMemNoiSoi.Properties.Settings.Default.dbPassword;
            this.cbInstance.Text = PhanMemNoiSoi.Properties.Settings.Default.serverName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //check server instance name is valid
            if (string.IsNullOrEmpty(cbInstance.Text))
            {
                MessageBox.Show("Tên máy chủ cơ sở dữ liệu không được để trống!","Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtDbName.Text))
            {
                MessageBox.Show("Tên cơ sở dữ liệu không được để trống!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
         
            this.serverInstance = this.cbInstance.Text.Trim();
            this.dbName = this.txtDbName.Text.Trim();
            this.userName = this.txtUser.Text.Trim();
            this.password = this.txtPass.Text.Trim();

            this.pbStatus.Visible = true;
            this.lbStatus.Visible = true;
            this.lbStatus.Text = "Đang khởi tạo cơ sở dữ liệu ...";

            createDBJob();
        }

        private void createDBJob()
        {
            if (this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.CancelAsync();
            }
            else
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
        }


        private bool createDbFromScript(string scriptPath, string serverInstance)
        {
            bool flag = false;
            try
            {
                string connectionString = "Data Source = " + serverInstance + "; Initial Catalog = master; Integrated Security = True";
                string sqlCommand = File.ReadAllText(this.scrCreateDB);
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                Server server = new Server(new ServerConnection(sqlConnection));
                server.ConnectionContext.ExecuteNonQuery(sqlCommand);
                flag = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return flag;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            bool flag = true;
            if (DBConnection.Instance.CheckConnection(this.serverInstance))
            {
                this.createDbFromScript(this.scrCreateDB, this.serverInstance);
                string[] textArray1 = new string[] { "Data Source= ", this.serverInstance, ";Initial Catalog=", this.dbName, ";" };
                string connString = string.Concat(textArray1);
                if (!string.IsNullOrEmpty(this.userName))
                {
                    connString = connString + " User id=" + this.userName + ";";
                    flag = false;
                }
                if (!string.IsNullOrEmpty(this.password))
                {
                    connString = connString + " Password=" + this.password + ";";
                    flag = false;
                }
                if (flag)
                {
                    connString = connString + "Integrated Security=true;MultipleActiveResultSets=true";
                }
                else
                {
                    connString = connString + "MultipleActiveResultSets=true;";
                }
                if (this.helper.DBConnectionStatus(connString))
                {
                    Properties.Settings.Default.serverName = this.serverInstance;
                    Properties.Settings.Default.dbName = this.dbName;
                    Properties.Settings.Default.dbUser = this.userName;
                    Properties.Settings.Default.dbPassword = this.password;
                    Properties.Settings.Default.Save();
                    checkDbErrorCode = 0;
                }
                else
                {
                    checkDbErrorCode = 1;
                }
            }
            else
            {
                this.checkDbErrorCode = 2;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.pbStatus.Visible = false;
            this.lbStatus.Visible = false;
            switch (this.checkDbErrorCode)
            {
                case 0:
                    MessageBox.Show("Khởi tạo cơ sở dữ liệu thành công! Vui lòng khởi động lại phầm mềm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.Hide();
                    Application.Exit();
                    break;

                case 1:
                    MessageBox.Show("Không thể kết nối cơ sở dữ liệu. Vui lòng kiểm tra lại thông số cài đặt", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    break;

                case 2:
                    MessageBox.Show("Tên máy chủ cơ sở dữ liệu không chính xác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    break;
            }
        }

        public void upgradeDB(int dbVersion)
        {
            bool isSuccess = createDbFromScript(this.scrCreateDB, this.serverInstance);
            if(isSuccess == true)
            {
                helper.setDbVersion(dbVersion);
            }
        }
    }
}
