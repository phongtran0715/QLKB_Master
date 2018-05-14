using System;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class ActiveKeyForm : Form
    {
        FingerPrint fp;
        Helper helper;
        const int KEY_LENGTH = 39;
        string clientKey;
        public ActiveKeyForm()
        {
            InitializeComponent();
            fp = new FingerPrint();
            helper = new Helper();
            txtKey.Text = "";
        }

        private void ActiveKey_Load(object sender, System.EventArgs e)
        {
            //txtClientId.Text = fp.GenKey(fp.cpuId());
            backgroundWorker1.RunWorkerAsync();
        }

        private void btnRegister_Click(object sender, System.EventArgs e)
        {
           string sysKey = fp.GenKey(txtClientId.Text.Trim());
           string userKey = helper.RemoveWhitespace(txtKey.Text.Trim());

            if (string.Equals(userKey, ""))
            {
                MessageBox.Show("Bạn chưa nhập mã bản quyền. Vui lòng nhập mã bản quyền.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (userKey.Length != KEY_LENGTH)
            {
                MessageBox.Show("Mã bản quyền không hợp lệ! Kiểm tra lại mã bản quyền của bạn", "Thông báo",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if(string.Equals(sysKey, userKey))
                {
                    MessageBox.Show("Kích hoạt bản quyền thành công! Vui lòng khởi động lại phần mềm", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Properties.Settings.Default.softwareLicense = sysKey;
                    Properties.Settings.Default.Save();
                    Application.Exit();
                }else
                {
                    MessageBox.Show("Kích hoạt bản quyền thất bại! Mã bản quyền không chính xác.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool validateUserId(string userKey)
        {
            bool exitCode = false;
            if (string.Equals(userKey, ""))
            {
                MessageBox.Show("Bạn chưa nhập mã bản quyền. Vui lòng nhập mã bản quyền.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (helper.RemoveWhitespace(txtKey.Text.Trim()).Length != KEY_LENGTH)
            {
                MessageBox.Show("Mã bản quyền không hợp lệ! Kiểm tra lại mã bản quyền của bạn", "Thông báo",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                exitCode = true;
            }
            return exitCode;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            clientKey = fp.GenKey(fp.cpuId() + fp.baseId());
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            txtClientId.Text = clientKey;
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
