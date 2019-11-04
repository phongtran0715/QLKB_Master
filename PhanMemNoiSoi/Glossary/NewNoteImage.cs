using OD.Forms.Security;
using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    //public partial class NewNoteIamge : SecureBaseForm
    public partial class NewNoteImage : Form
    {
        //Declare delegate for transfer data
        public delegate void UpdateDataHandler(string msg);
        public UpdateDataHandler updateData;

        public NewNoteImage(IPrincipal userPrincipal) 
            //: base(RolesList.Instance.roleList.ToArray(), userPrincipal)
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
            string content = txtNoiDung.Text.ToString().Trim();
            if (content != "")
            {
                if (updateData != null)
                {
                    updateData(txtNoiDung.Text.ToString().Trim());
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Nội dung không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
