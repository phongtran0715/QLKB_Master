using OD.Forms.Security;
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
        SqlDataAdapter dtaPatient = new SqlDataAdapter();
        DataTable tbPatient = new DataTable();
        BindingSource bsPatient = new BindingSource();
        Helper helper = new Helper();

        public LogManager(IPrincipal userPrincipal) : 
            base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Search_Load(object sender, EventArgs e)
        {

        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            // load data for data grid view
            string query = "SELECT SickNum,SickName, Age, Birthday, Createtime FROM SickData;";
            dtaPatient = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaPatient);

            tbPatient = new DataTable();
            tbPatient.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaPatient.Fill(tbPatient);
            bsPatient.DataSource = tbPatient;

            // Resize the DataGridView columns to fit the newly loaded content.
            dgLogView.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dgLogView.DataSource = bsPatient;
            dgLogView.Columns[0].Visible = false;
            dgLogView.Columns[1].HeaderText = "Tên";
            dgLogView.Columns[2].HeaderText = "Tuổi";
            dgLogView.Columns[3].HeaderText = "Ngày sinh";
            dgLogView.Columns[4].HeaderText = "Ngày khám";
            helper.setRowNumber(dgLogView);
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

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            #region comment
            /*
            // create sql command
            bool isAnd = false;
            string query = "SELECT SickNum,SickName, Age, Birthday, Createtime FROM SickData ";
            if (!string.IsNullOrEmpty(txtTenSearch.Text.Trim()))
            {
                query += " WHERE SickName LIKE '%" + txtTenSearch.Text.Trim() + "%'";
                isAnd = true;
            }
            if (!string.IsNullOrEmpty(txtCmndSearch.Text.Trim()))
            {
                if(isAnd == true)
                {
                    query += " AND IDCode LIKE '%" + txtTenSearch.Text.Trim() + "%'";
                }
                else
                {
                    query += " WHERE IDCode LIKE '%" + txtTenSearch.Text.Trim() + "%'";
                    isAnd = true;
                }
            }
            if(dtNgayBatDau.Checked == true)
            {
                if(isAnd == true)
                {
                    query += " AND Createtime >= '" + dtNgayBatDau.Value.ToShortDateString() + "'";
                }
                else
                {
                    query += " WHERE Createtime >= '" + dtNgayBatDau.Value.ToShortDateString() + "'";
                    isAnd = true;
                }
            }
            if(dtNgayKetThuc.Checked == true)
            {
                if(isAnd == true)
                {
                    query += " AND Createtime <= '" + dtNgayKetThuc.Value.ToShortDateString() + "'";
                }
                else
                {
                    query += " WHERE Createtime <= '" + dtNgayKetThuc.Value.ToShortDateString() + "'";
                    isAnd = true;
                }
            }
            query += ";";
            dtaPatient = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaPatient);
            tbPatient = new DataTable();
            tbPatient.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaPatient.Fill(tbPatient);
            bsPatient.DataSource = tbPatient;

            // Resize the DataGridView columns to fit the newly loaded content.
            dgLogView.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dgLogView.DataSource = bsPatient;
            dgLogView.Columns[0].Visible = false;
            dgLogView.Columns[1].HeaderText = "Tên";
            dgLogView.Columns[2].HeaderText = "Tuổi";
            dgLogView.Columns[3].HeaderText = "Ngày sinh";
            dgLogView.Columns[4].HeaderText = "Ngày khám";
            setRowNumber(dgLogView);
            */
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
    }
}
