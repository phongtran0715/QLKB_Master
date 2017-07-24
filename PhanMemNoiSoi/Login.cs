using System;
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
        }

        private void Login_Load(object sender, EventArgs e)
        {
            init();
            //Load data from data base
            string selectCommand = "SELECT UserId, UserName, WorkGroup, USERTYPE FROM UserList;";
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
            dgUserList.Columns["WorkGroup"].Visible = false;
            dgUserList.Columns["UserName"].HeaderText = "Tên Đăng Nhập";
            dgUserList.Columns["USERTYPE"].HeaderText = "Nhóm";
            dgUserList.Columns["UserName"].Width = dgUserList.Width * 2 / 3;
            dgUserList.Columns["USERTYPE"].Width = dgUserList.Width / 3;
        }

        private void init()
        {
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
                uWorkGroup = dgUserList.Rows[e.RowIndex].Cells["WorkGroup"].Value.ToString().Trim();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
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
                            Main mainFr = new Main();
                            mainFr.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Sai thông tin đăng nhập, vui lòng thử lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập, vui lòng thử lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Instance.LogMessageToFile(ex.ToString());
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
