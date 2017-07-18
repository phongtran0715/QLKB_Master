using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class InfoReportList : Form
    {
        private SqlDataAdapter dta = new SqlDataAdapter();
        private BindingSource bindSource = new BindingSource();
        DataTable table = new DataTable();

        public delegate void UpdateDataHandler(Dictionary<string, string> matrix);
        public UpdateDataHandler updateListInfo;

        public InfoReportList()
        {
            InitializeComponent();
        }

        private void InfoReportList_Load(object sender, System.EventArgs e)
        {
            // Load data to data grid view
            // reset data grid view
            dgvInfoList.DataSource = null;
            string query = "SELECT ItemCode,Content FROM CheckItem WHERE IsDisplay = '0';";

            //Load data from data base
            dta = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dta);

            table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dta.Fill(table);
            bindSource = new BindingSource();
            bindSource.DataSource = table;

            // Resize the DataGridView columns to fit the newly loaded content.
            dgvInfoList.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dgvInfoList.DataSource = bindSource;

            dgvInfoList.Columns["ItemCode"].Visible = false;
            dgvInfoList.Columns["Content"].HeaderText = "Nội Dung";

            //init check box on data grid view
            foreach (DataGridViewRow row in dgvInfoList.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["X"];
                chk.Value = chk.FalseValue;
            }
            //finish init check box
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Dictionary<string, string> matrix = new Dictionary<string, string>();
            for (int i = 0; i < dgvInfoList.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dgvInfoList.Rows[i].Cells["X"];
                if (chk.Value == chk.TrueValue)
                {
                    string itemCode = dgvInfoList.Rows[i].Cells["ItemCode"].Value.ToString().Trim();
                    string content = dgvInfoList.Rows[i].Cells["Content"].Value.ToString().Trim();
                    matrix.Add(itemCode, content);
                }
            }
            updateListInfo(matrix);
            this.Close();
        }

        private void btnThoat_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void dgvInfoList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvInfoList.Columns.Count; i++)
            {
                dgvInfoList.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (i == dgvInfoList.ColumnCount - 1)
                {
                    dgvInfoList.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
    }
}
