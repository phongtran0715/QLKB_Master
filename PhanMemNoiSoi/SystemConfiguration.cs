using OD.Forms.Security;
using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class SystemConfiguration : SecureBaseForm
    {
        public SystemConfiguration(IPrincipal userPrincipal) :
            base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            ConfigDB configDB = new ConfigDB();
            configDB.ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Capture type
            if (rbCapOnly.Checked)
            {
                Properties.Settings.Default.captureType = "capture_only";
            }
            else
            {
                Properties.Settings.Default.captureType = "capture_freeze";
            }

            // image 
            Properties.Settings.Default.imageQuality = cbImgQuality.SelectedIndex;
            Properties.Settings.Default.imageFolder = txtImgPath.Text.Trim();
            Properties.Settings.Default.imgWidth = int.Parse(txtWidth.Text.Trim());
            Properties.Settings.Default.imgHeight = int.Parse(txtHeigh.Text.Trim());

            Properties.Settings.Default.Save();

            this.Close();
        }

        private void rbLocal_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void SystemConfiguration_Load(object sender, EventArgs e)
        {
            //Load data from config file

            //load capture type
            string capType = Properties.Settings.Default.captureType;
            if (string.Equals(capType, "capture_only"))
            {
                rbCapOnly.Checked = true;
            }
            else
            {
                rbCaptureFreeze.Checked = true;
            }

            //Load image quality 
            int imgQuality = Properties.Settings.Default.imageQuality;
            txtImgPath.Text = Properties.Settings.Default.imageFolder;
            cbImgQuality.SelectedIndex = imgQuality;

            //load image size
            txtWidth.Text = Properties.Settings.Default.imgWidth.ToString();
            txtHeigh.Text = Properties.Settings.Default.imgHeight.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            CaptureTest capTest = new CaptureTest();
            capTest.ShowDialog();
            this.Show();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog1.SelectedPath;
                txtImgPath.Text = folderPath;
            }
        }

        private void txtWith_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtHeigh_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void SystemConfiguration_UserIsDenied(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn không có quyền truy cập vào danh mục này.\nVui lòng liên hệ với admin!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
