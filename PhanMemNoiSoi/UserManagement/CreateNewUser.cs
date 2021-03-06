﻿using OD.Forms.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class CreateNewUser : SecureBaseForm
    {
        public delegate void DoEvent();
        public event DoEvent RefreshDgv;
        Helper helper;

        public CreateNewUser(IPrincipal userPrincipal) :
            base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
            txtUserName.Text = "";
            txtPass.Text = "";
            txtPassConfirm.Text = "";
            helper = new Helper();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //check user name 
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                MessageBox.Show("Tên đăng nhập không được để trống!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check password
            if (txtPass.Text.Trim() != txtPassConfirm.Text.Trim())
            {
                MessageBox.Show("Mật khẩu bạn nhập hai lần không giống nhau!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check user type
            if (string.IsNullOrEmpty(cbGroup.Text.Trim()))
            {
                MessageBox.Show("Nhóm user không được để trống!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //update database
            try
            {
                //check user name is exist
                string sqlCommand = "INSERT INTO UserList (UserName, Sex, Password, WorkGroupId) VALUES "
                                                    + "(@UserName, @Sex, @Password, @groupId)";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@UserName", SqlDbType.NChar).Value = txtUserName.Text.Trim();
                mySQL.Parameters.Add("@Sex", SqlDbType.NChar).Value = "male";
                //Encrypt password
                string hashPassword = helper.ComputeHash(txtPass.Text.Trim(), "SHA512", null);
                mySQL.Parameters.Add("@Password", SqlDbType.NChar).Value = hashPassword;
                mySQL.Parameters.Add("@groupId", SqlDbType.NChar).Value = getUGroupByNum(cbGroup.SelectedIndex);
                mySQL.ExecuteNonQuery();

                this.RefreshDgv();
                string msg = "Thêm mới tài khoản '" + txtUserName.Text.Trim() + "'";
                Log.Instance.LogMessageToDB(DateTime.Now, Session.Instance.UserId, Session.Instance.UserName, msg);

                MessageBox.Show("Thêm mới tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Tạo tài khoản mới thất bại. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Get Group by combo box selected index
        /// Bác sỹ ~ Doctor ~ Id = 0
        /// Quản lý ~ Manager ~ Id = 1
        /// Admin ~ Admin ~ Id = 2 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string getUGroupByNum(int num)
        {
            string wGroup = "";
            switch (num)
            {
                case 0:
                    wGroup = "Doctor";
                    break;
                case 1:
                    wGroup = "Manager";
                    break;
                case 2:
                    wGroup = "Admin";
                    break;
                default:
                    wGroup = "Doctor";
                    break;
            }
            return wGroup;
        }
    }
}
