using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QLKBKeyGen
{
    /*
     * Client ID Example : B2B8-C7F4-7724-0D13-A323-A16B-6E17-95E7
     */
    public partial class Form1 : Form
    {
        FingerPrint fp;
        const int CLIENT_ID_LENGTH = 39;
        public Form1()
        {
            InitializeComponent();
            txtClientId.Text = "";
            txtKey.Text = "";
            fp = new FingerPrint();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private bool validateClientId(string clientId)
        {
            bool exitCode = false;
            if (string.Equals(clientId, "")){
                MessageBox.Show("Bạn chưa nhập mã thiết bị. Vui lòng nhập mã thiết bị.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtKey.Text = "";
            }else if(RemoveWhitespace(txtClientId.Text.Trim()).Length != CLIENT_ID_LENGTH)
            {
                MessageBox.Show("Mã thiết bị không hợp lệ! Kiểm tra lại mã thiết bị của bạn", "Thông báo",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtKey.Text = "";
            }
            else
            {
                exitCode = true;
            }
            return exitCode;
        }

        private string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        private void btnGenKey_Click_1(object sender, EventArgs e)
        {
            bool checkId = validateClientId(txtClientId.Text.Trim());
            if (checkId)
            {
                txtKey.Text = fp.GenKey(RemoveWhitespace(txtClientId.Text.Trim()));
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
