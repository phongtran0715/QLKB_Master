using OD.Forms.Security;
using PhanMemNoiSoi.Properties;
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
            new ConfigDB(false, false).ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.rbCapOnly.Checked)
            {
                Settings.Default.captureType = "capture_only";
            }
            else
            {
                Settings.Default.captureType = "capture_freeze";
            }
            Settings.Default.imageQuality = this.cbImgQuality.SelectedIndex;
            Settings.Default.imageFolder = this.txtImgPath.Text.Trim();
            if (!string.IsNullOrEmpty(this.txt2Width.Text.Trim()))
            {
                Settings.Default.img2Width = int.Parse(this.txt2Width.Text.Trim());
            }
            if (!string.IsNullOrEmpty(this.txt2Heigh.Text.Trim()))
            {
                Settings.Default.img2Height = int.Parse(this.txt2Heigh.Text.Trim());
            }
            if (!string.IsNullOrEmpty(this.txt4Width.Text.Trim()))
            {
                Settings.Default.img4Width = int.Parse(this.txt4Width.Text.Trim());
            }
            if (!string.IsNullOrEmpty(this.txt4Heigh.Text.Trim()))
            {
                Settings.Default.img4Heigh = int.Parse(this.txt4Heigh.Text.Trim());
            }
            if (!string.IsNullOrEmpty(this.txtMaxDayLog.Text.Trim()))
            {
                Settings.Default.maxDayLog = int.Parse(this.txtMaxDayLog.Text.Trim());
            }
            if (!string.IsNullOrEmpty(this.txtMaxRowDisplay.Text.Trim()))
            {
                Settings.Default.maxRowDisplay = int.Parse(this.txtMaxRowDisplay.Text.Trim());
            }
            Settings.Default.datetimeFormat = cbDateFormat.SelectedIndex;
            Settings.Default.Save();
            base.Close();
        }

        private void SystemConfiguration_Load(object sender, EventArgs e)
        {
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

            int imageQuality = Settings.Default.imageQuality;
            this.txtImgPath.Text = Settings.Default.imageFolder;
            this.cbImgQuality.SelectedIndex = imageQuality;
            this.txt2Width.Text = Settings.Default.img2Width.ToString();
            this.txt2Heigh.Text = Settings.Default.img2Height.ToString();
            this.txt4Width.Text = Settings.Default.img4Width.ToString();
            this.txt4Heigh.Text = Settings.Default.img4Heigh.ToString();
            this.txtMaxDayLog.Text = Settings.Default.maxDayLog.ToString();
            this.txtMaxRowDisplay.Text = Settings.Default.maxRowDisplay.ToString();
            cbDateFormat.SelectedIndex = Settings.Default.datetimeFormat;
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ActiveKeyForm activeFr = new ActiveKeyForm();
            activeFr.ShowDialog();
            this.Close();
        }

        private void txt4Width_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txt4Heigh_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtMaxDayLog_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtMaxRowDisplay_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Settings.Default.captureType = "capture_only";
            Settings.Default.imageQuality = 2;
            Settings.Default.imageFolder = @"D:\QLKB";
            this.cbImgQuality.SelectedIndex = 1;
            Settings.Default.img2Width = 300;
            Settings.Default.img2Height = 220;
            Settings.Default.img4Width = 270;
            Settings.Default.img4Heigh = 150;
            Settings.Default.maxDayLog = 60;
            Settings.Default.maxRowDisplay = 500;
            Settings.Default.datetimeFormat = 0;
            Settings.Default.Save();
            this.SystemConfiguration_Load(null, null);
            MessageBox.Show("Quay lại thiết lập mặc định thành công công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
