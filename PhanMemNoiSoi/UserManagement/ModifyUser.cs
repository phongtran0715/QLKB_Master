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
            this.txtTen.Text = username;
            this.cbNhom.SelectedIndex = getIndexByGroupId(group);
            this.userId = userId;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //check user name 
            if (string.IsNullOrEmpty(txtTen.Text.Trim()))
            {
                MessageBox.Show("Tên đăng nhập không được trống!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check group
            if (string.IsNullOrEmpty(cbNhom.Text.Trim()))
            {
                MessageBox.Show("Nhóm user không được để trống", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //check user name is exist
            //TODO : check user name is exist

            //update database
            string uGroup = getGroupByIndex(cbNhom.SelectedIndex);
            updateUserInfo(this.userId,txtTen.Text.Trim(),getGroupByIndex(cbNhom.SelectedIndex), cbNhom.Text.Trim());
            this.Close();
        }

        string getGroupByIndex(int index)
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
