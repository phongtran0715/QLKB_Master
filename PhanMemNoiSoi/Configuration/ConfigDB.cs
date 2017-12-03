using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
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
        private bool isRunMainApp;
        private bool isCreateDb;
        private int checkDbErrorCode = 0;

        public ConfigDB(bool isRunMainApp, bool isCreateDb)
        {
            this.InitializeComponent();
            this.isRunMainApp = isRunMainApp;
            this.isCreateDb = isCreateDb;
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

        private void button1_Click(object sender, EventArgs e)
        {
            //save setting value
            Properties.Settings.Default.serverName = cbInstance.Text.Trim();
            Properties.Settings.Default.dbName = txtDbName.Text.Trim();
            Properties.Settings.Default.dbUser = txtUser.Text.Trim();
            Properties.Settings.Default.dbPassword = txtPass.Text.Trim();
            Properties.Settings.Default.Save();
            MessageBox.Show("Cập nhập thông tin thành công", "Thông báo", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (this.backgroundWorker1.IsBusy)
            {
                this.backgroundWorker1.CancelAsync();
            }
            else
            {
                this.pbStatus.Visible = true;
                this.lbStatus.Visible = true;
                this.lbStatus.Text = "Đang kiểm tra ...";
                this.btnOk.Enabled = false;
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
                if (this.isCreateDb)
                {
                    this.createDbFromScript(this.scrCreateDB, this.serverInstance);
                }
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
                    PhanMemNoiSoi.Properties.Settings.Default.serverName = this.serverInstance;
                    PhanMemNoiSoi.Properties.Settings.Default.dbName = this.dbName;
                    PhanMemNoiSoi.Properties.Settings.Default.dbUser = this.userName;
                    PhanMemNoiSoi.Properties.Settings.Default.dbPassword = this.password;
                    PhanMemNoiSoi.Properties.Settings.Default.Save();
                    this.checkDbErrorCode = 0;
                }
                else
                {
                    this.checkDbErrorCode = 1;
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
            this.btnOk.Enabled = true;
            switch (this.checkDbErrorCode)
            {
                case 0:
                    MessageBox.Show("Kết nối dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    if (this.isRunMainApp)
                    {
                        Process.Start(Application.ExecutablePath);
                        Application.Exit();
                    }
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
    }
}
