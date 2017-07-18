using System;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnKhambenh_Click(object sender, EventArgs e)
        {
            NewCheck ncfr = new NewCheck();
            ncfr.ShowDialog();
        }

        private void btnQuanLyUser_Click(object sender, EventArgs e)
        {
            IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                           Session.Instance.UserRole);
            UserManager userFr = new UserManager(userPrincipal);
            userFr.ShowDialog();
        }

        private void btnQuanLyData_Click(object sender, EventArgs e)
        {
            IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                           Session.Instance.UserRole);
            DataManager dtManaFr = new DataManager(userPrincipal);
            dtManaFr.ShowDialog();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            //TODO : close database connection
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = new Bitmap(PhanMemNoiSoi.Properties.Resources._3);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                           new string[] { RolesList.VIEW_REPORT});
            Search searchFr = new Search(userPrincipal);
            searchFr.ShowDialog();
        }

        private void btnCaiDat_Click(object sender, EventArgs e)
        {
            IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                           new string[] { RolesList.SETUP_CONFIGURATION});
            SystemConfiguration sysConfFr = new SystemConfiguration(userPrincipal);
            sysConfFr.ShowDialog();
        }

        private void btnGioiThieu_Click(object sender, EventArgs e)
        {
            About aboutFr = new About();
            aboutFr.ShowDialog();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show("Bạn có muốn thoát phần mềm?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.OK)
            {
                DBConnection.Instance.CloseConnection();
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Initialize a test Principal 
            IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                           new string[] { RolesList.VIEW_LOG_HISTORY});
            LogManager logFr = new LogManager(userPrincipal);
            logFr.ShowDialog();
        }
    }
}
