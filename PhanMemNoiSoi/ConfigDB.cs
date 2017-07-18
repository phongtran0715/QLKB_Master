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
            txtServerName.Text = Properties.Settings.Default.serverName;
            txtDbName.Text = Properties.Settings.Default.dbName;
            txtUser.Text = Properties.Settings.Default.dbUser;
            txtPass.Text = Properties.Settings.Default.dbPassword;
            lbStatus.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.serverName = txtServerName.Text.Trim();
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
            //check connection to sql server is ok
            bool isConn = DBConnection.Instance.CheckConnection(txtServerName.Text.Trim());
            if (isConn)
            {
                //check database is exist
                bool isDb = DBConnection.Instance.CheckDatabaseExists(txtServerName.Text.Trim(), txtDbName.Text.Trim());
                if (!isDb)
                {
                    DialogResult dlgResult = MessageBox.Show("Cơ sở dữ liệu " + txtDbName.Text.Trim() + "không tồn tại.  " +
                                "Bạn có muốn tạo mới cơ sở dữ liệu không?", "Thông báo",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (dlgResult == DialogResult.OK)
                    {
                        string parameter = string.Format("{0} {1}", txtServerName.Text.Trim(), "\"" + scrCreateDB + "\"");
                        int exitCode = helper.ExecuteCommand(cmdCreateDB, parameter);
                    }
                }
                string connetionString = @"Data Source= " + txtServerName.Text.Trim() + ";Initial Catalog= "
                                           + txtDbName.Text.Trim() + ";" + "Integrated Security=True;MultipleActiveResultSets=true";
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
            lbStatus.Visible = false;
        }
    }
}
