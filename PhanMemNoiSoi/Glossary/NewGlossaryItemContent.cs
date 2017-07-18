using System;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class NewGlossaryItemContent : Form
    {
        //Declare delegate for transfer data
        public delegate void UpdateDataHandler(string msg);
        public UpdateDataHandler updateData;

        public NewGlossaryItemContent()
        {
            InitializeComponent();

        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditGlossary_Load(object sender, EventArgs e)
        {
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (updateData != null)
            {
                updateData(txtNoiDung.Text.ToString().Trim());
            }
            this.Close();
        }
    }
}
