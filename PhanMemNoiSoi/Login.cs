﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class Login : Form
    {
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private BindingSource bindingSource = new BindingSource();
        int userId = 0;
        string uWorkGroup = "";

        public Login()
        {
            InitializeComponent();
            Splasher.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            init();
            //Load data from data base
            string selectCommand = "SELECT uList.UserId, uList.UserName, wGroup.WorkGroupId, wGroup.Descript " + 
                                    "FROM UserList uList , WorkGroup wGroup " +
                                    "WHERE uList.WorkGroupId = wGroup.WorkGroupId ;";
            dataAdapter = new SqlDataAdapter(selectCommand, DBConnection.Instance.sqlConn);
            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;

            dataAdapter.Fill(table);

            //remove admin user
            for (int i = table.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = table.Rows[i];
                if (dr["UserName"].ToString().Trim() == "sys")
                {
                    dr.Delete();
                }
            }
            table.AcceptChanges();
            bindingSource.DataSource = table;

            dgUserList.DataSource = bindingSource;
            dgUserList.Columns["UserId"].Visible = false;
            dgUserList.Columns["WorkGroupId"].Visible = false;
            dgUserList.Columns["UserName"].HeaderText = "Tên Đăng Nhập";
            dgUserList.Columns["Descript"].HeaderText = "Nhóm";
            dgUserList.Columns["UserName"].Width = dgUserList.Width * 2 / 3;
            dgUserList.Columns["Descript"].Width = dgUserList.Width / 3;
        }

        private void init()
        {
            //load remember account information
            bool rememberAccount = Properties.Settings.Default.rememberAccount;
            if (rememberAccount)
            {
                cbDefault.Checked = true;
                txtUser.Text = Properties.Settings.Default.lastUserName;
                this.userId = Properties.Settings.Default.lastUserId;
                this.uWorkGroup = Properties.Settings.Default.lastUserType;
            }
            else
            {
                cbDefault.Checked = false;
            }

            txtPass.Text = "";
            this.dgUserList.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11f);
            this.dgUserList.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11f);
        }

        private void dgUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                userId = int.Parse(dgUserList.Rows[e.RowIndex].Cells["UserId"].Value.ToString().Trim());
                txtUser.Text = dgUserList.Rows[e.RowIndex].Cells["UserName"].Value.ToString().Trim();
                uWorkGroup = dgUserList.Rows[e.RowIndex].Cells["WorkGroupId"].Value.ToString().Trim();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool loginSuccessful = false;
            //Check Login information
            try
            {
                //check user
                if (string.Equals(txtUser.Text.Trim(), "sys"))
                {
                    this.userId = 0;
                    this.uWorkGroup = "Admin";
                }
                string sqlCommand = "SELECT * FROM UserList WHERE UserId = @id;";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@id", SqlDbType.Int).Value = this.userId;
                SqlDataReader rdrUser = mySQL.ExecuteReader();
                if (rdrUser.HasRows)
                {
                    while (rdrUser.Read())
                    {
                        //check password
                        string uname = rdrUser["UserName"].ToString().Trim();
                        string pass = rdrUser["Password"].ToString().Trim();

                        if (string.Equals(uname, txtUser.Text.Trim()) 
                            && string.Equals(pass, txtPass.Text.Trim()))
                        {
                            loginSuccessful = true;
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Cơ sở dữ liệu trống. Liên hệ với nhân viên quản trị phần mềm ", "Thông báo", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (loginSuccessful)
                {
                    //save login information
                    if (cbDefault.Checked == true)
                    {
                        Properties.Settings.Default.rememberAccount = true;
                        Properties.Settings.Default.lastUserName = txtUser.Text.Trim();
                        Properties.Settings.Default.lastUserId = this.userId;
                        Properties.Settings.Default.lastUserType = this.uWorkGroup;
                    }
                    else
                    {
                        Properties.Settings.Default.rememberAccount = false;
                    }
                    Properties.Settings.Default.Save();
                    
                    //Update session information
                    this.Hide();
                    Session.Instance.UserName = txtUser.Text.Trim();
                    Session.Instance.Password = txtPass.Text.Trim();
                    Session.Instance.UserId = this.userId;
                    Session.Instance.WorkGroup = this.uWorkGroup;
                    Session.Instance.UserRole = RolesList.Instance.getRoleByUser(this.uWorkGroup, this.userId);

                    string msg = "User '" + Session.Instance.UserName + "' đăng nhập thành công!";
                    Log.Instance.LogMessageToDB(DateTime.Now, Session.Instance.UserId, Session.Instance.UserName, msg);

                    Main mainFr = new Main();
                    mainFr.ShowDialog();
                    this.Close();
                }else
                {
                    MessageBox.Show("Sai thông tin đăng nhập, tên đăng nhập hoặc mật khẩu không đúng.", "Thông báo",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string msg = "User '" + txtUser.Text.Trim() + "' đăng nhập thất bại!";
                    Log.Instance.LogMessageToDB(DateTime.Now, Session.Instance.UserId, Session.Instance.UserName, msg);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckEnter(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnOk_Click(null, null);
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
