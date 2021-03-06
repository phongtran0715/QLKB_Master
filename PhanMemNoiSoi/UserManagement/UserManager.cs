﻿using OD.Forms.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class UserManager : SecureBaseForm
    {
        private SqlDataAdapter dta = new SqlDataAdapter();
        private BindingSource bSource = new BindingSource();
        DataTable table = new DataTable();
        IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                           Session.Instance.UserRole);
        Helper helper;

        public UserManager(IPrincipal userPrincipal) 
            : base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
            InitDataGrid();
            helper = new Helper();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThayMK_Click(object sender, EventArgs e)
        {
            ChangePassword changPassFr = new ChangePassword(userPrincipal);
            changPassFr.ShowDialog();
        }

        private void UserManagerForm_Load(object sender, EventArgs e)
        {
            loadDgvUser();
            //Hide tab control if user is not admin
            if(Session.Instance.WorkGroup != "Admin")
            {
                tabControl1.TabPages.RemoveAt(1);
            }
        }

        private void loadDgvUser()
        {
            //Load data from data base
            table = new DataTable();
            bSource = new BindingSource();
            dta = new SqlDataAdapter();

            string sqlQuery = "SELECT DISTINCT uList.UserId, uList.UserName, wGroup.WorkGroupId, wGroup.Descript " + 
                                    " FROM UserList uList , WorkGroup wGroup WHERE uList.WorkGroupId = wGroup.WorkGroupId ;";
            dta = new SqlDataAdapter(sqlQuery, DBConnection.Instance.sqlConn);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dta);
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dta.Fill(table);

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
            bSource.DataSource = table;

            dgvUserList.DataSource = bSource;
            dgvUserList.Columns["UserId"].Visible = false;
            dgvUserList.Columns["WorkGroupId"].Visible = false;
            dgvUserList.Columns["UserName"].HeaderText = "Tên Đăng Nhập";
            dgvUserList.Columns["Descript"].HeaderText = "Nhóm";
            dgvUserList.Columns["UserName"].Width = dgvUserList.Width / 2;
            dgvUserList.Columns["Descript"].Width = dgvUserList.Width / 2;
        }

        private void InitDataGrid()
        {
            this.dgvUserList.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11f);
            this.dgvUserList.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11f);
        }

        private void btnXoaMK_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvUserList.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int rowIndex = dgvUserList.SelectedRows[selectedRowCount - 1].Index;
            string userId = dgvUserList.Rows[rowIndex].Cells["UserId"].Value.ToString().Trim();
            string userName = dgvUserList.Rows[rowIndex].Cells["UserName"].Value.ToString().Trim();
            DialogResult dlResult = MessageBox.Show("Bạn có chắc muốn xóa mật khẩu tài khoản <" + userName + "> không?",
                                    "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dlResult == DialogResult.OK)
            {
                //Clear password
                try
                {
                    string newPass = helper.ComputeHash("", "SHA512", null);
                    string sqlCommand = "UPDATE UserList SET Password = @pass WHERE UserId = @id ;" ;
                    SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                    mySQL.Parameters.Add("@pass", SqlDbType.NChar).Value = newPass;
                    mySQL.Parameters.Add("@id", SqlDbType.Int).Value = int.Parse(userId);
                    mySQL.ExecuteNonQuery();
                    string msg = "Xóa mật khẩu tài khoản '" + userName + "'";
                    Log.Instance.LogMessageToDB(DateTime.Now, Session.Instance.UserId, Session.Instance.UserName, msg);
                    MessageBox.Show("Xóa mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Xóa mật khẩu thất bại. Không thể cập nhập cơ sở dữ liệu. Vui lòng thử lại sau!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvUserList.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int rowIndex = dgvUserList.SelectedRows[selectedRowCount - 1].Index;
            string userId = dgvUserList.Rows[rowIndex].Cells["UserId"].Value.ToString().Trim();
            string userName = dgvUserList.Rows[rowIndex].Cells["UserName"].Value.ToString().Trim();
            if(int.Parse(userId) == Session.Instance.UserId)
            {
                MessageBox.Show("Tài khoản đang đăng nhập. Bạn không thể xóa tài khoản này!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult dlResult = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản <" + userName + "> không?", "Thông báo",
                                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dlResult == DialogResult.OK)
            {
                //Delete user from DB and grdview
                try
                {
                    string sqlCommand = "DELETE FROM UserList WHERE UserId = @id";
                    SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                    mySQL.Parameters.Add("@id", SqlDbType.Int).Value = int.Parse(userId);
                    mySQL.ExecuteNonQuery();
                    // update grid view
                    dgvUserList.Rows.RemoveAt(rowIndex);
                    string msg = "Xóa tài khoản '" + userName + "'";
                    Log.Instance.LogMessageToDB(DateTime.Now, Session.Instance.UserId, Session.Instance.UserName, msg);

                    MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Exception)
                {
                    MessageBox.Show("Xóa tài khoản thất bại. Không cập nhập được cơ sở dữ liệu. Vui lòng thử lại sau!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //auto select first row
            if (dgvUserList.Rows.Count > 0)
            {
                dgvUserList.Rows[0].Selected = true;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvUserList.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int rowIndex = dgvUserList.SelectedRows[selectedRowCount - 1].Index;
            string userId = dgvUserList.Rows[rowIndex].Cells["UserId"].Value.ToString().Trim();
            string userName = dgvUserList.Rows[rowIndex].Cells["UserName"].Value.ToString().Trim();
            string group = dgvUserList.Rows[rowIndex].Cells["WorkGroupId"].Value.ToString().Trim();
            ModifyUser mUserFr = new ModifyUser(userPrincipal, userId, userName, group);
            mUserFr.updateUserInfo += updateUserInfo;
            mUserFr.ShowDialog();
        }

        private void updateUserInfo(string userId, string userName, string uGroupId, string descript)
        {
            //update database
            try
            {
                string sqlCommand = "UPDATE UserList SET UserName = @name, WorkGroupId = @groupId WHERE UserId = @id;";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@name", SqlDbType.NChar).Value = userName;
                mySQL.Parameters.Add("@groupId", SqlDbType.NChar).Value = uGroupId;
                mySQL.Parameters.Add("@id", SqlDbType.Int).Value = int.Parse(userId);
                string msg = "Sửa đổi thông tin tài khoản '" + userName + "'";
                Log.Instance.LogMessageToDB(DateTime.Now, Session.Instance.UserId, Session.Instance.UserName, msg);
                mySQL.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Sửa dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau",
                                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                return;
            }
            //update dgv
            int selectedRowCount =
            dgvUserList.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int rowIndex = dgvUserList.SelectedRows[selectedRowCount - 1].Index;
            dgvUserList.Rows[rowIndex].Cells["UserName"].Value = userName;
            dgvUserList.Rows[rowIndex].Cells["WorkGroupId"].Value = uGroupId;
            dgvUserList.Rows[rowIndex].Cells["Descript"].Value = descript;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            CreateNewUser createUFr = new CreateNewUser(userPrincipal);
            createUFr.RefreshDgv += addNewItemOnDg;
            createUFr.ShowDialog();
        }

        public void addNewItemOnDg()
        {
            loadDgvUser();
        }

        private void UserManager_UserIsDenied(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn không có quyền truy cập vào danh mục này.\nVui lòng liên hệ với admin!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void loadDgvRoleList()
        {
            //Load data from data base
            DataTable table = new DataTable();
            BindingSource bSource = new BindingSource();
            SqlDataAdapter dta = new SqlDataAdapter();
            try
            {
                string selectCommand = "SELECT Num, RoleName FROM RolesList;";
                dta = new SqlDataAdapter(selectCommand, DBConnection.Instance.sqlConn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dta);
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dta.Fill(table);
                bSource.DataSource = table;

                dgvRoleList.DataSource = bSource;
                dgvRoleList.Columns["Num"].Visible = false;
                dgvRoleList.Columns["RoleName"].HeaderText = "Danh mục quyền";
                dgvRoleList.Columns["RoleName"].Width = dgvRoleList.Width;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void loadUserGroup()
        {
            //Load data from data base
            DataTable utb = new DataTable();
            BindingSource ubs = new BindingSource();
            SqlDataAdapter udta = new SqlDataAdapter();
            try
            {
                string selectCommand = "SELECT DISTINCT WorkGroupId, Descript FROM WorkGroup;";
                udta = new SqlDataAdapter(selectCommand, DBConnection.Instance.sqlConn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(udta);
                utb.Locale = System.Globalization.CultureInfo.InvariantCulture;
                udta.Fill(utb);
                ubs.DataSource = utb;

                dgvWorkGroup.DataSource = ubs;
                dgvWorkGroup.Columns["WorkGroupId"].Visible = false;
                dgvWorkGroup.Columns["Descript"].HeaderText = "Nhóm";
                dgvWorkGroup.Rows[0].Selected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void getRoleByUser(string groupId)
        {
            //reset role list
            foreach (DataGridViewRow row in dgvRoleList.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["X"];
                chk.Value = chk.FalseValue;
            }

            //update role list
            // check item selected
            string query = "SELECT DISTINCT RoleId FROM UserGroupRole WHERE GroupId = @id";
            SqlCommand sqlCom = new SqlCommand(query, DBConnection.Instance.sqlConn);
            sqlCom.Parameters.Add("@id", SqlDbType.NChar).Value = groupId;
            SqlDataReader sqlDtaReader = sqlCom.ExecuteReader();

            while (sqlDtaReader.Read())
            {
                string roleId = sqlDtaReader["RoleId"].ToString().Trim();
                // update selected value on data grid
                foreach (DataGridViewRow row in dgvRoleList.Rows)
                {
                    if (string.Equals(row.Cells["Num"].Value.ToString().Trim(), roleId))
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["X"];
                        chk.Value = chk.TrueValue;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // get user group
            int selectedRowCount =
            dgvWorkGroup.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int currRowIndexCheck = dgvWorkGroup.SelectedRows[selectedRowCount - 1].Index;
            string groupId = dgvWorkGroup.Rows[currRowIndexCheck].Cells["WorkGroupId"].Value.ToString().Trim();

            for (int i = 0; i < dgvRoleList.Rows.Count; i++)
            {
                string roleId = dgvRoleList.Rows[i].Cells["Num"].Value.ToString().Trim();
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dgvRoleList.Rows[i].Cells["X"];
                if (chk.Value == chk.TrueValue)
                {
                    try
                    {
                        //check content exist in DB
                        string query = "SELECT COUNT(*) FROM UserGroupRole WHERE RoleId = @roleId "
                                        + " AND GroupId = @gId";
                        SqlCommand mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                        mySQL.Parameters.Add("@roleId", SqlDbType.NChar).Value = roleId;
                        mySQL.Parameters.Add("@gId", SqlDbType.NChar).Value = groupId;
                        int numRow = int.Parse(mySQL.ExecuteScalar().ToString());
                        if (numRow == 0)
                        {
                            //Insert record to DB
                            query = "INSERT INTO UserGroupRole (RoleId, GroupId) " +
                                            " VALUES ( @roleId, @gId); ";
                            mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                            mySQL.Parameters.Add("@roleId", SqlDbType.NChar).Value = roleId;
                            mySQL.Parameters.Add("@gId", SqlDbType.NChar).Value = groupId;
                            mySQL.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.ToString());
                        return;
                    }
                }
                else
                {
                    try
                    {
                        //check content exist in DB
                        string query = "SELECT COUNT(*) FROM UserGroupRole WHERE RoleId = @roleId "
                                        + " AND GroupId = @gId ;";
                        SqlCommand mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                        mySQL.Parameters.Add("@roleId", SqlDbType.NChar).Value = roleId;
                        mySQL.Parameters.Add("@gId", SqlDbType.NChar).Value = groupId;
                        int numRow = int.Parse(mySQL.ExecuteScalar().ToString());
                        if (numRow != 0)
                        {
                            //delete record from DB
                            query = "DELETE FROM UserGroupRole WHERE RoleId = @roleId "
                                    + " AND GroupId = @gId ;";
                            mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                            mySQL.Parameters.Add("@roleId", SqlDbType.NChar).Value = roleId;
                            mySQL.Parameters.Add("@gId", SqlDbType.NChar).Value = groupId;
                            mySQL.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.ToString());
                        return;
                    }
                }
            }
            //update user role
            Session.Instance.UserRole = RolesList.Instance.getRoleByUser(Session.Instance.WorkGroup, 
                                                                         Session.Instance.UserId);
            MessageBox.Show("Lưu cài đặt thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UserManager_UserIsAllowed(object sender, EventArgs e)
        {
            btnXoaMK.Enabled = ValidatedUserRoles.Contains(RolesList.CHANGE_PASSWORD);
            btnThem.Enabled = ValidatedUserRoles.Contains(RolesList.ADD_NEW_USER);
            btnSua.Enabled = ValidatedUserRoles.Contains(RolesList.MODIFY_USER);
            btnXoa.Enabled = ValidatedUserRoles.Contains(RolesList.DELETE_USER);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                this.BeginInvoke(new Action(() => updateRoleList()));
            }
        }

        private void updateRoleList()
        {
            loadUserGroup();
            loadDgvRoleList();
            //update role list
            // check item selected
            string query = "SELECT RoleId FROM UserGroupRole WHERE GroupId = @id";
            SqlCommand sqlCom = new SqlCommand(query, DBConnection.Instance.sqlConn);
            sqlCom.Parameters.Add("@id", SqlDbType.NChar).Value = "Doctor";
            SqlDataReader sqlDtaReader = sqlCom.ExecuteReader();

            while (sqlDtaReader.Read())
            {
                string roleId = sqlDtaReader["RoleId"].ToString().Trim();
                // update selected value on data grid
                foreach (DataGridViewRow row in dgvRoleList.Rows)
                {
                    if (string.Equals(row.Cells["Num"].Value.ToString().Trim(), roleId))
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["X"];
                        chk.Value = chk.TrueValue;
                        break;
                    }
                }
            }
        }
        private void dgvWorkGroup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                string groupId = dgvWorkGroup.Rows[e.RowIndex].Cells["WorkGroupId"].Value.ToString().Trim();
                getRoleByUser(groupId);
            }
        }
    }
}
