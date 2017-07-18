using OD.Forms.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class GlossaryMainteance : SecureBaseForm
    {
        IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                          Session.Instance.UserRole);

        Helper helper = new Helper();
        enum dataTable { DTA_CONTENT = 0, DTA_CONTENT_DETAIL, DTA_INFO_REPORT, DTA_TABLE_END }
        enum bindSource { DTA_CONTENT = 0, DTA_CONTENT_DETAIL, DTA_INFO_REPORT, DTA_BIND_SOURCE_END }
        enum dataAdapter { DTA_CONTENT = 0, DTA_CONTENT_DETAIL, DTA_INFO_REPORT, DTA_ADAPTER_END }

        DataTable[] tbAdap = new DataTable[Enum.GetNames(typeof(dataTable)).Length - 1];
        SqlDataAdapter[] dtaAdap = new SqlDataAdapter[Enum.GetNames(typeof(dataAdapter)).Length - 1];
        BindingSource[] bsAdap = new BindingSource[Enum.GetNames(typeof(bindSource)).Length - 1];

        const int TAB_CHECK_RECORD = 0;
        const int TAB_INFO_IN_REPORT = 1;

        public GlossaryMainteance(IPrincipal userPrincipal) :
            base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
            Init();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GlossaryMainteance_Load(object sender, EventArgs e)
        {
            // load data for data grid view
            string selectCommand = "SELECT ItemCode,Content FROM CheckItem;";
            dtaAdap[(int)dataAdapter.DTA_CONTENT] = new SqlDataAdapter(selectCommand, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaAdap[(int)dataAdapter.DTA_CONTENT]);

            tbAdap[(int)dataTable.DTA_CONTENT].Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaAdap[(int)dataAdapter.DTA_CONTENT].Fill(tbAdap[(int)dataTable.DTA_CONTENT]);
            bsAdap[(int)bindSource.DTA_CONTENT].DataSource = tbAdap[(int)dataTable.DTA_CONTENT];

            // Resize the DataGridView columns to fit the newly loaded content.
            dgvCheck.DataSource = bsAdap[(int)bindSource.DTA_CONTENT];
            dgvCheck.Columns[0].Visible = false;
            dgvCheck.Columns[1].HeaderText = "Danh Mục";
            dgvCheck.Columns[1].Width = this.dgvCheck.Size.Width;

            helper.setRowNumber(dgvCheck);
        }

        private void Init()
        {
            //Init variable
            for (int i = (int)dataAdapter.DTA_CONTENT; i < (int)dataAdapter.DTA_ADAPTER_END; i++)
            {
                dtaAdap[i] = new SqlDataAdapter();

            }
            for (int i = (int)bindSource.DTA_CONTENT; i < (int)bindSource.DTA_BIND_SOURCE_END; i++)
            {
                bsAdap[i] = new BindingSource();
            }
            for (int i = (int)dataTable.DTA_CONTENT; i < (int)dataTable.DTA_TABLE_END; i++)
            {
                tbAdap[i] = new DataTable();
            }

            //Init data grid view
            this.dgvCheck.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12f);
            this.dgvCheck.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12f);
            this.dgCheckContentDetail.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12f);
            this.dgCheckContentDetail.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12f);
        }

        private void dgDanhMuc_SelectionChanged(object sender, EventArgs e)
        {
            //clear noi dung dgv
            int countRow = dgCheckContentDetail.Rows.Count;
            if (countRow > 0)
            {
                dgCheckContentDetail.DataSource = null;
                dgCheckContentDetail.Rows.Clear();
                dgCheckContentDetail.Refresh();
            }

            if (dgvCheck.CurrentRow == null)
            {
                return;
            }

            int selectedRowCount =
            dgvCheck.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }

            int currRowIndexCheck = dgvCheck.SelectedRows[selectedRowCount - 1].Index;

            string itemCode = dgvCheck.Rows[currRowIndexCheck].Cells[0].Value.ToString().Trim();

            string selectCommand = "SELECT ContentCode,Content FROM CheckItemContent WHERE ItemCode = '" + itemCode + "'";

            //Load data from data base
            dtaAdap[(int)dataAdapter.DTA_CONTENT_DETAIL] = new SqlDataAdapter(selectCommand, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaAdap[(int)dataAdapter.DTA_CONTENT_DETAIL]);

            tbAdap[(int)dataTable.DTA_CONTENT_DETAIL] = new DataTable();
            tbAdap[(int)dataTable.DTA_CONTENT_DETAIL].Locale = System.Globalization.CultureInfo.InvariantCulture;

            dtaAdap[(int)dataAdapter.DTA_CONTENT_DETAIL].Fill(tbAdap[(int)dataTable.DTA_CONTENT_DETAIL]);
            bsAdap[(int)bindSource.DTA_CONTENT_DETAIL].DataSource = tbAdap[(int)dataTable.DTA_CONTENT_DETAIL];

            // Resize the DataGridView columns to fit the newly loaded content.
            dgCheckContentDetail.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            dgCheckContentDetail.DataSource = bsAdap[(int)bindSource.DTA_CONTENT_DETAIL];
            dgCheckContentDetail.Columns[0].Visible = false;
            dgCheckContentDetail.Columns[1].HeaderText = "Nội Dung";
            dgCheckContentDetail.Columns[1].Width = this.dgCheckContentDetail.Size.Width;
            helper.setRowNumber(dgvCheck);
        }

        private void btnSua_DM_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvCheck.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }

            int currRowIndexCheck = dgvCheck.SelectedRows[selectedRowCount - 1].Index;
            string code = dgvCheck.Rows[currRowIndexCheck].Cells[0].Value.ToString().Trim();
            string content = dgvCheck.Rows[currRowIndexCheck].Cells[1].Value.ToString().Trim();

            EditGlossaryItem glossItemFr = new EditGlossaryItem(userPrincipal, content, code);
            //call back function update data grid
            glossItemFr.updateData += updateDgvDanhMuc;
            glossItemFr.ShowDialog();
        }

        private void updateDgvDanhMuc(string msg, string code)
        {
            int selectedRowCount =
            dgvCheck.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }

            int currRowIndexCheck = dgvCheck.SelectedRows[selectedRowCount - 1].Index;
            //update database
            try
            {
                string sqlCommand = "UPDATE CheckItem SET Content = N'" + msg + "' WHERE ItemCode = N'" + code + "'";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Sửa dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau",
                                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
                return;
            }
            //update dgv
            DataGridViewRow newDataRow = dgvCheck.Rows[currRowIndexCheck];
            newDataRow.Cells[0].Value = code;
            newDataRow.Cells[1].Value = msg;
        }

        private void addNewDgvDanhMuc(string msg)
        {
            //add new record to database
            //create new item code
            string sqlCommand = "SELECT NUM FROM CheckItem WHERE NUM = (SELECT MAX(NUM) FROM CheckItem);";
            SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
            SqlDataReader rdrNum = mySQL.ExecuteReader();
            string newItemCode = "C";
            if (rdrNum.Read())
            {
                newItemCode += rdrNum["NUM"].ToString();
            }

            try
            {
                sqlCommand = "INSERT INTO CheckItem (ItemCode,Content, ShowNum, IsDisplay) VALUES (@itemCode, @content, @showNum, @isDisplay)";
                mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@itemCode", SqlDbType.NChar).Value = newItemCode;
                mySQL.Parameters.Add("@content", SqlDbType.NChar).Value = msg;
                mySQL.Parameters.Add("@showNum", SqlDbType.Int).Value = 0;
                mySQL.Parameters.Add("@isDisplay", SqlDbType.Int).Value = 0;
                mySQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau",
                                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
                return;
            }
            //update dgv
            tbAdap[(int)dataTable.DTA_CONTENT].Rows.Add(newItemCode, msg);
            dgvCheck.DataSource = tbAdap[(int)dataTable.DTA_CONTENT];
            helper.setRowNumber(dgvCheck);
        }

        private void updateDgvNoiDung(string msg, string code)
        {
            int selectedRowCount =
            dgCheckContentDetail.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }

            int currRowIndexCheck = dgCheckContentDetail.SelectedRows[selectedRowCount - 1].Index;
            //update database
            try
            {
                string sqlCommand = "UPDATE CheckItemContent SET Content = N'" + msg + "' WHERE ContentCode = N'" + code + "'";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Sửa dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau",
                                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
                return;
            }
            //update dgv
            DataGridViewRow newDataRow = dgCheckContentDetail.Rows[currRowIndexCheck];
            newDataRow.Cells[0].Value = code;
            newDataRow.Cells[1].Value = msg;
        }

        private void addNewDgvNoiDung(string msg)
        {
            //add new record to database
            int selectedRowCount =
            dgvCheck.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }

            int currRowIndexCheck = dgvCheck.SelectedRows[selectedRowCount - 1].Index;

            string itemCode = dgvCheck.Rows[currRowIndexCheck].Cells[0].Value.ToString().Trim();
            string contentCode = itemCode;
            try
            {
                string sqlCommand = "SELECT NUM FROM CheckItemContent WHERE NUM = (SELECT MAX(NUM) FROM CheckItemContent)";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                SqlDataReader rdrContenCode = mySQL.ExecuteReader();
                if (rdrContenCode.Read())
                {
                    contentCode += rdrContenCode["NUM"].ToString();
                }
                sqlCommand = "INSERT INTO CheckItemContent(ItemCode,ContentCode,Content) VALUES (N'"
                                    + itemCode + "', N'" + contentCode + "',N'" + msg + "')";
                mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Thêm dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau",
                                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Log.Instance.LogMessageToFile(ex.ToString());
                return;
            }
            //update dgv
            tbAdap[(int)dataTable.DTA_CONTENT_DETAIL].Rows.Add(contentCode, msg);
            dgCheckContentDetail.DataSource = tbAdap[(int)dataTable.DTA_CONTENT_DETAIL];
            helper.setRowNumber(dgvCheck);
        }

        private void btnThem_DM_Click(object sender, EventArgs e)
        {
            NewGlossaryItem glossItemFr = new NewGlossaryItem(userPrincipal);
            glossItemFr.updateData += addNewDgvDanhMuc;
            glossItemFr.ShowDialog();
        }

        private void btnXoa_DM_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvCheck.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int currRowIndexCheck = dgvCheck.SelectedRows[selectedRowCount - 1].Index;
            //check delete is valid
            int countRowND = dgCheckContentDetail.Rows.Count;
            if (countRowND > 0)
            {
                MessageBox.Show("Không thể xóa danh mục này! \n Danh mục này có chứa nội dung con bên trong!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            DialogResult dlResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nội dung này?", "Thông báo",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dlResult == DialogResult.Cancel)
            {
                return;
            }

            string itemCode = dgvCheck.Rows[currRowIndexCheck].Cells[0].Value.ToString().Trim();

            //delete from database
            try
            {
                string sqlCommand = "DELETE FROM CheckItem WHERE ItemCode = '" + itemCode + "'";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.ExecuteReader();

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Không thể xóa dữ liệu. \n Không kết nối được đến cơ sở dữ liệu. \n Vui lòng thử lại sau!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Instance.LogMessageToFile(ex.ToString());
                return;
            }

            // update data grid view
            tbAdap[(int)dataTable.DTA_CONTENT].Rows.RemoveAt(currRowIndexCheck);
            bsAdap[(int)bindSource.DTA_CONTENT].DataSource = tbAdap[(int)dataTable.DTA_CONTENT];
            dgvCheck.DataSource = bsAdap[(int)bindSource.DTA_CONTENT];
            dgvCheck.Update();
            dgvCheck.Refresh();
            helper.setRowNumber(dgvCheck);
        }

        private void btnXoa_ND_Click(object sender, EventArgs e)
        {

            int selectedRowCount =
            dgCheckContentDetail.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }

            DialogResult dlResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nội dung này?", "Thông báo",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dlResult == DialogResult.Cancel)
            {
                return;
            }

            int currRowIndexCheck = dgvCheck.SelectedRows[selectedRowCount - 1].Index;
            string contentCode = dgCheckContentDetail.Rows[currRowIndexCheck].Cells[0].Value.ToString().Trim();

            //delete from database
            try
            {
                string sqlCommand = "DELETE FROM CheckItemContent WHERE ContentCode = '" + contentCode + "'";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.ExecuteReader();

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Không thể xóa dữ liệu. \n Không kết nối được đến cơ sở dữ liệu. \n Vui lòng thử lại sau!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Instance.LogMessageToFile(ex.ToString());
                return;
            }

            // delete from data grid view
            tbAdap[(int)dataTable.DTA_CONTENT_DETAIL].Rows.RemoveAt(currRowIndexCheck);
            bsAdap[(int)bindSource.DTA_CONTENT_DETAIL].DataSource = tbAdap[(int)dataTable.DTA_CONTENT_DETAIL];
            dgCheckContentDetail.DataSource = bsAdap[(int)bindSource.DTA_CONTENT_DETAIL];
            dgCheckContentDetail.Update();
            dgCheckContentDetail.Refresh();
            helper.setRowNumber(dgvCheck);
        }

        private void btnThem_ND_Click(object sender, EventArgs e)
        {
            NewGlossaryItemContent newGlossItemContent = new NewGlossaryItemContent();
            newGlossItemContent.updateData += addNewDgvNoiDung;
            newGlossItemContent.ShowDialog();
        }

        private void btnSua_ND_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
       dgCheckContentDetail.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                MessageBox.Show("now row select");
                return;
            }

            int currRowIndexCheckContent = dgCheckContentDetail.SelectedRows[selectedRowCount - 1].Index;
            string code = dgCheckContentDetail.Rows[currRowIndexCheckContent].Cells[0].Value.ToString().Trim();
            string content = dgCheckContentDetail.Rows[currRowIndexCheckContent].Cells[1].Value.ToString().Trim();

            EditGlossaryItemContent glossItemContentFr = new EditGlossaryItemContent(content, code);
            //call back function update data grid
            glossItemContentFr.updateData += updateDgvNoiDung;
            glossItemContentFr.ShowDialog();
        }

        private void dgDanhMuc_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgDanhMuc.Rows[0].Selected = true;
            //currRowIndexDM = 0;
            foreach (DataGridViewColumn column in dgvCheck.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < dgvCheck.Columns.Count; i++)
            {
                dgvCheck.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (i == dgvCheck.ColumnCount - 1)
                {
                    dgvCheck.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void dgNoiDung_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dgNoiDung.Rows[0].Selected = true;
            //currRowIndexND = 0;
            foreach (DataGridViewColumn column in dgCheckContentDetail.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < dgCheckContentDetail.Columns.Count; i++)
            {
                dgCheckContentDetail.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (i == dgCheckContentDetail.ColumnCount - 1)
                {
                    dgCheckContentDetail.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            int tabIndex = e.TabPageIndex;
            switch (tabIndex)
            {
                case TAB_CHECK_RECORD:
                    {
                        loadCheckRecordData();
                        break;
                    }
                case TAB_INFO_IN_REPORT:
                    {
                        loadInfoReport();
                        if (dgvInfo.Rows.Count > 0)
                        {
                            setSelectedRow(dgvInfo, 0);
                            btnMoveUp.Enabled = false;
                            btnMoveDown.Enabled = true;
                        }
                        else
                        {
                            btnMoveUp.Enabled = false;
                            btnMoveDown.Enabled = false;
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private void loadCheckRecordData()
        {
            // load data for data grid view
            string query = "SELECT ItemCode,Content FROM CheckItem;";

            dtaAdap[(int)dataAdapter.DTA_CONTENT] = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaAdap[(int)dataAdapter.DTA_CONTENT]);
            tbAdap[(int)dataTable.DTA_CONTENT] = new DataTable();

            tbAdap[(int)dataTable.DTA_CONTENT].Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaAdap[(int)dataAdapter.DTA_CONTENT].Fill(tbAdap[(int)dataTable.DTA_CONTENT]);
            bsAdap[(int)bindSource.DTA_CONTENT].DataSource = tbAdap[(int)dataTable.DTA_CONTENT];

            // Resize the DataGridView columns to fit the newly loaded content.
            dgvCheck.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dgvCheck.DataSource = bsAdap[(int)bindSource.DTA_CONTENT];
            dgvCheck.Columns["ItemCode"].Visible = false;
            dgvCheck.Columns["Content"].HeaderText = "Danh Mục";
            helper.setRowNumber(dgvCheck);
        }

        private void loadInfoReport()
        {
            // load data for data grid view
            string query = "SELECT ShowNum,ItemCode,Content FROM CheckItem WHERE IsDisplay = '1' ORDER BY ShowNum ASC;";

            dtaAdap[(int)dataAdapter.DTA_INFO_REPORT] = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaAdap[(int)dataAdapter.DTA_INFO_REPORT]);
            tbAdap[(int)dataTable.DTA_INFO_REPORT] = new DataTable();

            tbAdap[(int)dataTable.DTA_INFO_REPORT].Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaAdap[(int)dataAdapter.DTA_INFO_REPORT].Fill(tbAdap[(int)dataTable.DTA_INFO_REPORT]);
            bsAdap[(int)bindSource.DTA_INFO_REPORT].DataSource = tbAdap[(int)dataTable.DTA_INFO_REPORT];

            // Resize the DataGridView columns to fit the newly loaded content.
            dgvInfo.DataSource = bsAdap[(int)bindSource.DTA_INFO_REPORT];
            dgvInfo.Columns["ItemCode"].Visible = false;
            dgvInfo.Columns["Content"].HeaderText = "Danh Mục";
            dgvInfo.Columns["ShowNum"].HeaderText = "Thứ Tự";
            dgvInfo.Columns["ShowNum"].DisplayIndex = 0;
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var bb = sender as TabControl;
                this.contextMenuStrip1.Show(this.tabControl1, e.Location);
            }
        }

        private void dgvInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rIndex = e.RowIndex;
            if (rIndex < 0)
            {
                return;
            }
            if (dgvInfo.Rows.Count <= 1)
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                return;
            }

            if (rIndex == 0)
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = true;
            }
            else if (rIndex == dgvInfo.Rows.Count - 1)
            {
                btnMoveUp.Enabled = true;
                btnMoveDown.Enabled = false;
            }
            else
            {
                btnMoveUp.Enabled = true;
                btnMoveDown.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvInfo.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int currRowIndexCheck = dgvInfo.SelectedRows[selectedRowCount - 1].Index;

            DialogResult dlResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nội dung này?", "Thông báo",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dlResult == DialogResult.Cancel)
            {
                return;
            }

            string itemCode = dgvInfo.Rows[currRowIndexCheck].Cells["ItemCode"].Value.ToString().Trim();
            int showNum = int.Parse(dgvInfo.Rows[currRowIndexCheck].Cells["ShowNum"].Value.ToString().Trim());

            //delete from database
            try
            {
                string sqlCommand = "UPDATE CheckItem SET IsDisplay = @isDisplay WHERE ItemCode = @itemCode ;";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@isDisplay", SqlDbType.Int).Value = 0;
                mySQL.Parameters.Add("@itemCode", SqlDbType.NChar).Value = itemCode;
                mySQL.ExecuteReader();
                //reorder number in database and data grid view 
                updateShowNum(showNum);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể cập nhập cơ sở dữ liệu. \n Không kết nối được đến cơ sở dữ liệu. \n Vui lòng thử lại sau!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
                return;
            }

            // delete from data grid view
            tbAdap[(int)dataTable.DTA_INFO_REPORT].Rows.RemoveAt(currRowIndexCheck);
            bsAdap[(int)bindSource.DTA_CONTENT_DETAIL].DataSource = tbAdap[(int)dataTable.DTA_INFO_REPORT];
            dgvInfo.DataSource = bsAdap[(int)bindSource.DTA_INFO_REPORT];
            dgvInfo.Update();
            dgvInfo.Refresh();
        }

        private void updateShowNum(int numFrom)
        {
            try
            {
                //update database
                string sqlCommand = "SELECT ItemCode,ShowNum FROM CheckItem WHERE ShowNum > '"
                                    + numFrom + "' AND IsDisplay = '1' ORDER BY ShowNum ASC;";
                SqlCommand cmd = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string itemCode = dr["ItemCode"].ToString().Trim();
                    int snum = Convert.ToInt16(dr["ShowNum"]);
                    snum -= 1;
                    //Update number oder
                    string uQuery = "UPDATE CheckItem SET ShowNum = '"
                                               + snum + "' WHERE ItemCode = '" + itemCode + "';";
                    SqlCommand uSQL = new SqlCommand(uQuery, DBConnection.Instance.sqlConn);
                    uSQL.ExecuteReader();

                    //update data grid view
                    for (int i = 0; i < dgvInfo.Rows.Count; i++)
                    {
                        if (string.Equals(dgvInfo.Rows[i].Cells["ItemCode"].Value.ToString().Trim(), itemCode))
                        {
                            dgvInfo.Rows[i].Cells["ShowNum"].Value = snum.ToString();
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InfoReportList infoList = new InfoReportList();
            infoList.updateListInfo += updateInfoReport;
            infoList.ShowDialog();
        }

        private void updateInfoReport(Dictionary<string, string> matrix)
        {
            foreach (KeyValuePair<string, string> pair in matrix)
            {
                //update in data base
                try
                {
                    string query = "UPDATE CheckItem SET ShowNum = @showNum, IsDisplay = @isDisplay WHERE ItemCode = @itemCode ;";
                    SqlCommand mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                    mySQL.Parameters.Add("@showNum", SqlDbType.Int).Value = dgvInfo.Rows.Count + 1;
                    mySQL.Parameters.Add("@isDisplay", SqlDbType.Int).Value = 1;
                    mySQL.Parameters.Add("@itemCode", SqlDbType.NChar).Value = pair.Key.Trim();
                    mySQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }

                // update data grid view
                tbAdap[(int)dataTable.DTA_INFO_REPORT].Rows.Add(dgvInfo.Rows.Count + 1, pair.Key.Trim(), pair.Value.Trim());
                bsAdap[(int)bindSource.DTA_INFO_REPORT].DataSource = tbAdap[(int)dataTable.DTA_INFO_REPORT];
                dgvInfo.DataSource = bsAdap[(int)bindSource.DTA_INFO_REPORT];
                dgvInfo.Update();
                dgvInfo.Refresh();
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvInfo.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int currRowIndexCheck = dgvInfo.SelectedRows[selectedRowCount - 1].Index;
            int index = int.Parse(dgvInfo.Rows[currRowIndexCheck].Cells["ShowNum"].Value.ToString().Trim());
            if (index > 0)
            {
                string itemCode = dgvInfo.Rows[currRowIndexCheck].Cells["ItemCode"].Value.ToString().Trim();

                //update to increase previous item position in database
                string uQuery = "UPDATE CheckItem SET ShowNum = '" + index +
                                "' WHERE ItemCode = (SELECT ItemCode FROM CheckItem WHERE ShowNum = '" + (index - 1) + "')";
                SqlCommand uSQL = new SqlCommand(uQuery, DBConnection.Instance.sqlConn);
                uSQL.ExecuteReader();

                //update to decrease current item position in database
                uQuery = "UPDATE CheckItem SET ShowNum = '" + (index - 1).ToString() +
                                "' WHERE ItemCode = '" + itemCode + "';";
                uSQL = new SqlCommand(uQuery, DBConnection.Instance.sqlConn);
                uSQL.ExecuteReader();

                loadInfoReport();
                setSelectedRow(dgvInfo, currRowIndexCheck - 1);
                setStateOrderButon(currRowIndexCheck - 1);
            }
            ArrangeDgvCol();
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvInfo.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int currRowIndexCheck = dgvInfo.SelectedRows[selectedRowCount - 1].Index;
            int index = int.Parse(dgvInfo.Rows[currRowIndexCheck].Cells["ShowNum"].Value.ToString().Trim());
            if (index > 0)
            {
                string itemCode = dgvInfo.Rows[currRowIndexCheck].Cells["ItemCode"].Value.ToString().Trim();

                //update to decrease after item position in database
                string uQuery = "UPDATE CheckItem SET ShowNum = '" + index +
                                "' WHERE ItemCode = (SELECT ItemCode FROM CheckItem WHERE ShowNum = '" + (index + 1) + "')";
                SqlCommand uSQL = new SqlCommand(uQuery, DBConnection.Instance.sqlConn);
                uSQL.ExecuteReader();

                //update to decrease current item position in database
                uQuery = "UPDATE CheckItem SET ShowNum = '" + (index + 1).ToString() +
                                "' WHERE ItemCode = '" + itemCode + "';";
                uSQL = new SqlCommand(uQuery, DBConnection.Instance.sqlConn);
                uSQL.ExecuteReader();

                loadInfoReport();
                setSelectedRow(dgvInfo, currRowIndexCheck + 1);
                setStateOrderButon(currRowIndexCheck + 1);
            }
            ArrangeDgvCol();
        }

        /// <summary>
        /// Set selected row in data grid view
        /// </summary>
        /// <param name="dgv">Data grid view parent</param>
        /// <param name="rIndex">Position of row </param>
        private void setSelectedRow(DataGridView dgv, int rIndex)
        {
            if (dgv.Rows.Count > 0)
            {
                if ((rIndex >= 0) && (rIndex < dgv.Rows.Count))
                {
                    dgv.Rows[rIndex].Selected = true;
                }
                else
                {
                    dgv.Rows[0].Selected = true;
                }
            }
        }

        /// <summary>
        /// Set state for move up and move down button 
        /// following position of current item 
        /// </summary>
        /// <param name="index">position number of selected item in data grid view</param>
        private void setStateOrderButon(int index)
        {
            int numRow = dgvInfo.Rows.Count;
            if (numRow == 0)
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                return;
            }
            else
            {
                if (index == 0)
                {
                    btnMoveUp.Enabled = false;
                    btnMoveDown.Enabled = true;
                }
                else if (index == numRow - 1)
                {
                    btnMoveUp.Enabled = true;
                    btnMoveDown.Enabled = false;
                }
                else
                {
                    btnMoveUp.Enabled = true;
                    btnMoveDown.Enabled = true;
                }
            }
        }

        private void dgvInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ArrangeDgvCol();
        }

        private void ArrangeDgvCol()
        {
            for (int i = 0; i < dgvInfo.Columns.Count; i++)
            {
                dgvInfo.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (i == dgvInfo.ColumnCount - 1)
                {
                    dgvInfo.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void GlossaryMainteance_UserIsDenied(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn không có quyền truy cập vào danh mục này.\nVui lòng liên hệ với admin!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
