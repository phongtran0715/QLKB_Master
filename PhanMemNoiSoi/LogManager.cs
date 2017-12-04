using OD.Forms.Security;
using PhanMemNoiSoi.Properties;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class LogManager : SecureBaseForm
    //public partial class LogManager : Form
    {
        SqlDataAdapter dtaLog = new SqlDataAdapter();
        DataTable tbLog = new DataTable();
        BindingSource bsLog = new BindingSource();
        Helper helper = new Helper();
        ComboboxItem cbSelectedItem;
        int MAX_LENGHT = 100;

        public LogManager(IPrincipal userPrincipal) : 
            base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
            cbSelectedItem = new ComboboxItem();
            cbSelectedItem.Text = "";
            cbSelectedItem.Value = 0;
            cbUser.Items.Add(cbSelectedItem);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LogManager_Load(object sender, EventArgs e)
        {
            //load data for combo box
            string sqlQuery = "SELECT UserId, UserName FROM UserList;";
            SqlCommand mySQL = new SqlCommand(sqlQuery, DBConnection.Instance.sqlConn);
            SqlDataReader rdrUser = mySQL.ExecuteReader();
            if (rdrUser.HasRows)
            {
                while (rdrUser.Read())
                {
                    //check password
                    string uId = rdrUser["UserId"].ToString().Trim();
                    string uName = rdrUser["UserName"].ToString().Trim();
                    ComboboxItem item = new ComboboxItem();
                    item.Text = uName;
                    item.Value = uId;
                    cbUser.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Cơ sở dữ liệu trống. Liên hệ với nhân viên quản trị phần mềm ", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dtNgayBatDau.CustomFormat = helper.getDateFormat(Settings.Default.datetimeFormat);
            dtNgayKetThuc.CustomFormat = helper.getDateFormat(Settings.Default.datetimeFormat);
            dtNgayBatDau.Value = DateTime.Now;
            dtNgayKetThuc.Value = dtNgayBatDau.Value;
            //load data for log grid view
            loadDgvLog();
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            //cbUser.Text = "";
            dtNgayBatDau.Checked = false;
            dtNgayKetThuc.Checked = false;
            cbUser.SelectedIndex = 0;
            txtContent.Text = "";
            loadDgvLog();
        }

        private void dgPatient_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn column in dgLogView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < dgLogView.Columns.Count; i++)
            {
                dgLogView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (i == dgLogView.ColumnCount - 1)
                {
                    dgLogView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void rbThang_CheckedChanged(object sender, EventArgs e)
        {
            if (rbThang.Checked == true)
            {
                dtNgayBatDau.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                dtNgayKetThuc.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                                                        DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            }
        }

        private void rbNgay_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNgay.Checked == true)
            {
                dtNgayBatDau.Value = DateTime.Today;
                dtNgayKetThuc.Value = DateTime.Today;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
        }

        private void loadDgvLog()
        {
            // load data for data grid view
            string str = Settings.Default.maxRowDisplay.ToString();
            string query = "SELECT TOP " + str + " Num,TimeLog, UserName, Descript FROM WorkLog ORDER BY TimeLog DESC;";
            dtaLog = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaLog);

            tbLog = new DataTable();
            tbLog.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaLog.Fill(tbLog);
            bsLog.DataSource = tbLog;
            dgLogView.DataSource = bsLog;

            dgLogView.Columns["Num"].Visible = false;
            dgLogView.Columns["TimeLog"].HeaderText = "Thời gian";
            dgLogView.Columns["TimeLog"].DefaultCellStyle.Format = helper.getDateFormat(Settings.Default.datetimeFormat);
            dgLogView.Columns["UserName"].HeaderText = "User";
            dgLogView.Columns["Descript"].HeaderText = "Nội dung";

            dgLogView.Columns["TimeLog"].Width = dgLogView.Width / 5;
            dgLogView.Columns["UserName"].Width = dgLogView.Width / 5;
            dgLogView.Columns["Descript"].Width = dgLogView.Width * 3 / 5;
            helper.setRowNumber(dgLogView);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            //check start date & end date 
            int checkTime = DateTime.Compare(dtNgayBatDau.Value, dtNgayKetThuc.Value);
            if(checkTime > 0)
            {
                MessageBox.Show("Điều kiện tìm kiếm không hợp lệ. \n Ngày bắt đầu phải nhỏ hơn ngày kết thúc", 
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #region comment
            // create sql command
            bool isAnd = false;
            string query = "SELECT " + MAX_LENGHT.ToString() +" Num, TimeLog, UserName, Descript FROM WorkLog ";
            if (!string.IsNullOrEmpty(cbUser.Text.Trim()))
            {
                query += " WHERE UserId = '" + cbSelectedItem.Value + "'";
                isAnd = true;
            }
            if (!string.IsNullOrEmpty(txtContent.Text.Trim()))
            {
                if(isAnd == true)
                {
                    query += " AND Descript LIKE '%" + txtContent.Text.Trim() + "%'";
                }
                else
                {
                    query += " WHERE Descript LIKE '%" + txtContent.Text.Trim() + "%'";
                    isAnd = true;
                }
            }
            if(dtNgayBatDau.Checked == true)
            {
                if(isAnd == true)
                {
                    query += " AND TimeLog >= '" + dtNgayBatDau.Value.ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    query += " WHERE TimeLog >= '" + dtNgayBatDau.Value.ToString("yyyy-MM-dd") + "'";
                    isAnd = true;
                }
            }
            bool timeCondi = string.Equals(dtNgayBatDau.Value.ToShortDateString(), dtNgayKetThuc.Value.ToShortDateString());
            if ((dtNgayKetThuc.Checked == true) && (!timeCondi))
            {
                if(isAnd == true)
                {
                    query += " AND TimeLog <= '" + dtNgayKetThuc.Value.ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    query += " WHERE TimeLog <= '" + dtNgayKetThuc.Value.ToString("yyyy-MM-dd") + "'";
                    isAnd = true;
                }
            }
            query += " ORDER BY TimeLog DESC;";
            Console.WriteLine("query =" + query);
            dtaLog = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaLog);
            tbLog = new DataTable();
            tbLog.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaLog.Fill(tbLog);
            bsLog.DataSource = tbLog;

            // Resize the DataGridView columns to fit the newly loaded content.
            dgLogView.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dgLogView.DataSource = bsLog;
            dgLogView.Columns["Num"].Visible = false;
            dgLogView.Columns["TimeLog"].HeaderText = "Thời gian";
            dgLogView.Columns["UserName"].HeaderText = "User";
            dgLogView.Columns["Descript"].HeaderText = "Nội dung";

            dgLogView.Columns["TimeLog"].Width = dgLogView.Width / 5;
            dgLogView.Columns["UserName"].Width = dgLogView.Width / 5;
            dgLogView.Columns["Descript"].Width = dgLogView.Width * 3 / 5;
            helper.setRowNumber(dgLogView);
            #endregion
        }

        private void LogManager_UserIsAllowed(object sender, EventArgs e)
        {
        }

        private void LogManager_UserIsDenied(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn không có quyền truy cập vào danh mục này. " 
                            + "\n Vui lòng liên hệ với admin.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dgLogView_Resize(object sender, EventArgs e)
        {
            dgLogView.Columns["TimeLog"].Width = dgLogView.Width / 5;
            dgLogView.Columns["UserName"].Width = dgLogView.Width / 5;
            dgLogView.Columns["Descript"].Width = dgLogView.Width * 3 / 5;
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgLogView.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            DialogResult result = MessageBox.Show("Bạn có muốn xóa bản ghi này?", "Thông báo",
                                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int currRowIndexCheck = dgLogView.SelectedRows[selectedRowCount - 1].Index;
                string num = dgLogView.Rows[currRowIndexCheck].Cells["Num"].Value.ToString().Trim();
                //delete from database
                try
                {
                    string sqlCommand = "DELETE FROM WorkLog WHERE Num = @num";
                    SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                    mySQL.Parameters.Add("@num", SqlDbType.Int).Value = num;
                    mySQL.ExecuteReader();
                    // delete data grid view
                    tbLog.Rows.RemoveAt(currRowIndexCheck);
                    bsLog.DataSource = tbLog;
                    dgLogView.DataSource = bsLog;
                    dgLogView.Update();
                    dgLogView.Refresh();
                    helper.setRowNumber(dgLogView);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Không kết nối được đến cơ sở dữ liệu. Vui lòng thử lại sau! \n " + ex.ToString(),
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgLogView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dgLogView.Rows.Count > 4)
            {
                dgLogView.AllowUserToAddRows = false;
            }
            else
            {
                dgLogView.AllowUserToAddRows = true;
            }
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            cbSelectedItem = (ComboboxItem)cmb.SelectedItem;
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
