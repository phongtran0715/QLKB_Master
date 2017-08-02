using OD.Forms.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class ChangePassword : SecureBaseForm
    {
        public ChangePassword(IPrincipal userPrincipal) :
            base(RolesList.Instance.roleList.ToArray(), userPrincipal)
        {
            InitializeComponent();
            this.txtMKCu.Text = "";
            this.txtMKMoi.Text = "";
            this.txtMKMoi_2.Text = "";
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //check old password
            if (txtMKCu.Text.Trim() != Session.Instance.Password)
            {
                MessageBox.Show("Mật khẩu cũ không đúng, vui lòng nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                //check new password
                if (txtMKMoi.Text.Trim() != txtMKMoi_2.Text.Trim())
                {
                    MessageBox.Show("Mật khẩu mới nhập không giống nhau, vui lòng nhập lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //Update password to DB
                    try
                    {
                        string sqlCommand = "UPDATE UserList SET Password = @pass WHERE UserId = @id;";
                        SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                        mySQL.Parameters.Add("@pass", SqlDbType.NChar).Value = txtMKMoi.Text.Trim();
                        mySQL.Parameters.Add("@id", SqlDbType.Int).Value = Session.Instance.UserId;
                        mySQL.ExecuteNonQuery();
                        string msg = "Thay đổi mật khẩu tài khoản '" + Session.Instance.UserName + "'";
                        Log.Instance.LogMessageToDB(DateTime.Now, Session.Instance.UserId, Session.Instance.UserName, msg);
                        MessageBox.Show("Cập nhập mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Không thể cập nhập cơ sở dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.ToString());
                        Log.Instance.LogMessageToFile(ex.ToString());
                    }
                }
            }
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            lbInfo.Text = "Đổi mật khẩu cho user : <" + Session.Instance.UserName + "> .";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
