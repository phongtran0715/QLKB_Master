using OD.Forms.Security;
using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class EditGlossaryItem : SecureBaseForm
    {
        //Declare delegate for transfer data
        public delegate void UpdateDataHandler(string msg, string code);
        public UpdateDataHandler updateData;
        string content = null;
        string code = null;

        public EditGlossaryItem(IPrincipal userPrincipal, string content, string code) :
            base(RolesList.Instance.roleList.ToArray(), userPrincipal)
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
