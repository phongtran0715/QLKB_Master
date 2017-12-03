using OD.Forms.Security;
using System;
using System.Windows.Forms;
using System.Security.Principal;

namespace PhanMemNoiSoi
{
    public partial class DataManager : SecureBaseForm
    {
        public DataManager(IPrincipal userPrincipal) :
            base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
        }

        private void btnDanhMuc_Click(object sender, EventArgs e)
        {
            this.Hide();
            IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                            Session.Instance.UserRole);
            GlossaryMainteance glMaFr = new GlossaryMainteance(userPrincipal);
            glMaFr.ShowDialog();
            this.Show();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                           new string[] { RolesList.BACKUP_RESTORE_DATA });
            BackupAndRestore barfr = new BackupAndRestore(userPrincipal);
            barfr.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng đang được hoàn thiện. Vui lòng quay lại sau!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DataManager_UserIsDenied(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn không có quyền truy cập vào danh mục này.\nVui lòng liên hệ với admin!",
                             "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
