using OD.Forms.Security;
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

        public CreateNewUser(IPrincipal userPrincipal) :
            base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
            this.txtTen.Text = "";
            this.txtMK.Text = "";
            this.txtMK2.Text = "";
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Dispose();
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

            //check password
            if (txtMK.Text.Trim() != txtMK2.Text.Trim())
            {
                MessageBox.Show("Mật khẩu bạn nhập hai lần không giống nhau!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check user type
            if (string.IsNullOrEmpty(cbNhom.Text.Trim()))
            {
                MessageBox.Show("Nhóm user không được để trống!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //update database
            try
            {
                //check user name is exist
                string sqlCommand = "INSERT INTO UserList (UserName, Sex, WorkGroup, Password, USERTYPE) VALUES "
                                                    + "(@UserName, @Sex, @workGroup, @Password, @UserType)";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@UserName", SqlDbType.NChar).Value = txtTen.Text.Trim();
                mySQL.Parameters.Add("@Sex", SqlDbType.NChar).Value = "male";
                mySQL.Parameters.Add("@workGroup", SqlDbType.NChar).Value = getUGroupByNum(cbNhom.SelectedIndex);
                mySQL.Parameters.Add("@Password", SqlDbType.NChar).Value = txtMK.Text.Trim();
                mySQL.Parameters.Add("@UserType", SqlDbType.NChar).Value = cbNhom.Text.Trim();
                mySQL.ExecuteNonQuery();

                this.RefreshDgv();

                MessageBox.Show("Thêm mới tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Tạo tài khoản mới thất bại. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
                this.Close();
            }
        }

        private string getUGroupByNum(int num)
        {
            string uGroup = "";
            string sqlCommand = "SELECT Num FROM UserGroup WHERE Id = @id ;";
            SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
            mySQL.Parameters.Add("@id", SqlDbType.Int).Value = num;
            SqlDataReader rdrInfo = mySQL.ExecuteReader();
            if (rdrInfo.Read())
            {
                uGroup = rdrInfo["Num"].ToString().Trim();
            }
            return uGroup;
        }
    }
}
