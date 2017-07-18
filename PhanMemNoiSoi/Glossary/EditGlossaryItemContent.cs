using System;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class EditGlossaryItemContent : Form
    {
        //Declare delegate for transfer data
        public delegate void UpdateDataHandler(string msg, string code);
        public UpdateDataHandler updateData;
        string content = null;
        string code = null;

        public EditGlossaryItemContent(string content, string code)
        {
            InitializeComponent();
            this.content = content;
            this.code = code;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditGlossary_Load(object sender, EventArgs e)
        {
            this.txtNoiDung.Text = this.content;
            this.lbCode.Text = this.code;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (updateData != null)
            {
                updateData(txtNoiDung.Text.ToString().Trim(), this.code);
            }
            this.Close();
        }
    }
}
