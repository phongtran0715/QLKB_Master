using Microsoft.Win32;
using System;
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

        public ConfigDB()
        {
            InitializeComponent();
        }

        private void ConfigDB_Load(object sender, EventArgs e)
        {
            cbInstance.Items.Add("");
            //get all sql server instance
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
            String[] instances = (String[])rk.GetValue("InstalledInstances");
            if (instances.Length > 0)
            {
                foreach (String element in instances)
                {
                    if (element == "MSSQLSERVER")
                        cbInstance.Items.Add(System.Environment.MachineName);
                    else
                        cbInstance.Items.Add(System.Environment.MachineName + @"\" + element);
                }
            }
            //set default value for field
            txtDbName.Text = Properties.Settings.Default.dbName;
            txtUser.Text = Properties.Settings.Default.dbUser;
            txtPass.Text = Properties.Settings.Default.dbPassword;
            cbInstance.Text = Properties.Settings.Default.serverName;
            lbStatus.Visible = false;
            pbStatus.Visible = false;
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
            bool trustedConn = true;
            //check server instance name is valid
            if (string.IsNullOrEmpty(cbInstance.Text))
            {
                MessageBox.Show("Tên máy chủ cơ sở dữ liệu không được để trống!","Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //check database name is valid
            if (string.IsNullOrEmpty(txtDbName.Text))
            {
                MessageBox.Show("Tên cơ sở dữ liệu không được để trống!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check connection 
            bool isConn = DBConnection.Instance.CheckConnection(cbInstance.Text.Trim());
            if (isConn)
            {
                //check database is exist
                bool isDb = DBConnection.Instance.CheckDatabaseExists(cbInstance.Text.Trim(), txtDbName.Text.Trim());
                if (!isDb)
                {
                    MessageBox.Show("Cơ sở dữ liệu '" + txtDbName.Text.Trim() + "' không tồn tại.\nKiểm tra lại thông số cài đặt", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string connetionString = @"Data Source= " + cbInstance.Text.Trim() + ";Initial Catalog= "
                                           + txtDbName.Text.Trim() + ";";
                if (!string.IsNullOrEmpty(txtUser.Text.Trim()))
                {
                    connetionString += " User id=" + txtUser.Text.Trim() + ";";
                    trustedConn = false;
                }
                if (!string.IsNullOrEmpty(txtPass.Text.Trim()))
                {
                    connetionString += " Password=" + txtPass.Text.Trim() + ";";
                    trustedConn = false;
                }
                if (trustedConn)
                {
                    connetionString += "Integrated Security=true;MultipleActiveResultSets=true";
                }
                else
                {
                    connetionString += "MultipleActiveResultSets=true;";
                }

                bool isOK = helper.DBConnectionStatus(connetionString);
                if (isOK)
                {
                    MessageBox.Show("Kết nối dữ liệu thành công!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể kết nối cơ sở dữ liệu. Vui lòng kiểm tra lại thông số cài đặt", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Tên máy chủ cơ sở dữ liệu không chính xác", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }  
    }
}
