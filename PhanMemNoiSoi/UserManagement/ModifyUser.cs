using OD.Forms.Security;
using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class ModifyUser : SecureBaseForm
    {
        public delegate void UpdateDataHandler(string userId, string userName, string uGroup, string descript);
        public UpdateDataHandler updateUserInfo;
        string userId = null;
        public ModifyUser(IPrincipal userPrincipal) :
            base(RolesList.Instance.roleList.ToArray(), userPrincipal)
        {
            InitializeComponent();
        }

        public ModifyUser(IPrincipal userPrincipal, string userId, string username, string group) :
            base(RolesList.Instance.roleList.ToArray(), userPrincipal)
        {
            InitializeComponent();
            txtUserName.Text = username;
            cbGroup.SelectedIndex = getIndexByGroupId(group);
            this.userId = userId;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //check user name 
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                MessageBox.Show("Tên đăng nhập không được trống!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check group
            if (string.IsNullOrEmpty(cbGroup.Text.Trim()))
            {
                MessageBox.Show("Nhóm user không được để trống", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //update database
            string uGroup = getGroupIdByIndex(cbGroup.SelectedIndex);
            updateUserInfo(this.userId,txtUserName.Text.Trim(),
                            getGroupIdByIndex(cbGroup.SelectedIndex), cbGroup.Text.Trim());
            this.Close();
        }

        string getGroupIdByIndex(int index)
        {
            string group = "Doctor";
            switch (index)
            {
                case 0:
                    group = "Doctor";
                    break;
                case 1:
                    group = "Manager";
                    break;
                case 2:
                    group = "Admin";
                    break;
                default:
                    break;
            }
            return group;
        }

        int getIndexByGroupId(string groupId)
        {
            int index;
            if (string.Equals(groupId, "Doctor"))
            {
                index = 0;
            }else if(string.Equals(groupId, "Manager"))
            {
                index = 1;
            }else if (string.Equals(groupId, "Admin"))
            {
                index = 2;
            }else
            {
                index = 0;
            }
            return index;
        }
    }
}
