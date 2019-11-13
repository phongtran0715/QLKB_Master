using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class CheckItem : Form
    {
        private SqlDataAdapter dta = new SqlDataAdapter();
        private BindingSource bindSource = new BindingSource();
        DataTable table = new DataTable();
        Helper helper = new Helper();
        string itemCode;
        string patientId;

        public delegate void UpdateDataHandler(string content);
        public UpdateDataHandler updateData;

        public CheckItem()
        {
            InitializeComponent();
        }

        public CheckItem(string itemCode, string patientId, string currentText)
        {
            InitializeComponent();
            this.patientId = patientId;
            this.itemCode = itemCode;
            //txtSelected.Text = currentText;
        }

        private void CheckItem_Load(object sender, EventArgs e)
        {
            // Load data to data grid view
            //reset data grid view
            dgvContentDetail.DataSource = null;
            string query = "SELECT ContentCode, Content FROM CheckItemContent WHERE ItemCode = '" + this.itemCode + "';";

            //Load data from data base
            try
            {
                dta = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dta);
                table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dta.Fill(table);
                bindSource = new BindingSource();
                bindSource.DataSource = table;
                dgvContentDetail.DataSource = bindSource;
                dgvContentDetail.Columns["ContentCode"].Visible = false;
                dgvContentDetail.Columns["Content"].HeaderText = "Nội Dung";
                helper.setRowNumber(dgvContentDetail);
                //init checkbox on datagridview
                foreach (DataGridViewRow row in dgvContentDetail.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["X"];
                    chk.Value = chk.FalseValue;
                }
                //finish init check box
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // check item selected
            try
            {
                query = "SELECT ItemContentCode FROM CheckRecord WHERE CheckNum = @patientId AND ItemCode = @itemCode";
                SqlCommand sqlItemContent = new SqlCommand(query, DBConnection.Instance.sqlConn);
                sqlItemContent.Parameters.Add("@patientId", SqlDbType.NChar).Value = patientId;
                sqlItemContent.Parameters.Add("@itemCode", SqlDbType.NChar).Value = itemCode;
                SqlDataReader rdrItemContent = sqlItemContent.ExecuteReader();

                while (rdrItemContent.Read())
                {
                    string itemContentCode = rdrItemContent["ItemContentCode"].ToString().Trim();
                    // update selected value on data grid
                    foreach (DataGridViewRow row in dgvContentDetail.Rows)
                    {
                        if (row.Cells["ContentCode"].Value.ToString().Trim() == itemContentCode)
                        {
                            DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["X"];
                            chk.Value = chk.TrueValue;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvContentDetail.Rows.Count; i++)
            {
                string itemContentCode = dgvContentDetail.Rows[i].Cells["ContentCode"].Value.ToString().Trim();
                string itemContent = dgvContentDetail.Rows[i].Cells["Content"].Value.ToString().Trim();

                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dgvContentDetail.Rows[i].Cells["X"];
                if (chk.Value == chk.TrueValue)
                {
                    try
                    {
                        //check content exist in DB
                        string query = "SELECT COUNT(*) FROM CheckRecord WHERE CheckNum = @patientId "
                                        + " AND ItemCode = @itemCode AND ItemContentCode = @contentCode;";
                        SqlCommand mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                        mySQL.Parameters.Add("@patientId", SqlDbType.NChar).Value = patientId;
                        mySQL.Parameters.Add("@itemCode", SqlDbType.NChar).Value = itemCode;
                        mySQL.Parameters.Add("@contentCode", SqlDbType.NChar).Value = itemContentCode;
                        int numRow = int.Parse(mySQL.ExecuteScalar().ToString());
                        if (numRow == 0)
                        {
                            //Insert record to DB
                            query = "INSERT INTO CheckRecord (CheckNum, ItemCode, ItemContentCode) " +
                                            " VALUES ( @patientId, @itemCode ,@itemContentCode); ";
                            mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                            mySQL.Parameters.Add("@patientId", SqlDbType.NChar).Value = patientId;
                            mySQL.Parameters.Add("@itemCode", SqlDbType.NChar).Value = itemCode;
                            mySQL.Parameters.Add("@itemContentCode", SqlDbType.NChar).Value = itemContentCode;
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
                        string query = "SELECT COUNT(*) FROM CheckRecord WHERE CheckNum = @patientId "
                                            + " AND ItemCode = @itemCode AND ItemContentCode = @contentCode;";
                        SqlCommand mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                        mySQL.Parameters.Add("@patientId", SqlDbType.NChar).Value = patientId;
                        mySQL.Parameters.Add("@itemCode", SqlDbType.NChar).Value = itemCode;
                        mySQL.Parameters.Add("@contentCode", SqlDbType.NChar).Value = itemContentCode;
                        int numRow = int.Parse(mySQL.ExecuteScalar().ToString());
                        if (numRow != 0)
                        {
                            //delete record from DB
                            query = "DELETE FROM CheckRecord WHERE CheckNum = @patientId  "
                                    + "AND ItemCode = @itemCode AND ItemContentCode = @contentCode ;";
                            mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                            mySQL.Parameters.Add("@patientId", SqlDbType.NChar).Value = patientId;
                            mySQL.Parameters.Add("@itemCode", SqlDbType.NChar).Value = itemCode;
                            mySQL.Parameters.Add("@contentCode", SqlDbType.NChar).Value = itemContentCode;
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

            if (updateData != null)
            {
                string data = txtSelected.Text.Trim();
                if (data.EndsWith(","))
                {
                    data = data.Remove(data.Length - 1);
                }
                updateData(data);
            }
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.Hide();
            IPrincipal userPrincipal = new GenericPrincipal(WindowsIdentity.GetCurrent(),
                                           new string[] { RolesList.CHANGE_GLOSSARY });
            GlossaryMainteance glmFr = new GlossaryMainteance(userPrincipal);
            glmFr.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvContentDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvContentDetail.Columns.Count; i++)
            {
                dgvContentDetail.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (i == dgvContentDetail.ColumnCount - 1)
                {
                    dgvContentDetail.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void dgvContentDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvContentDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvContentDetail_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dgvContentDetail.Rows[e.RowIndex].Cells["X"];
                string content = dgvContentDetail.Rows[e.RowIndex].Cells["Content"].Value.ToString().Trim();
                if (chk.Value == chk.TrueValue)
                {
                    txtSelected.Text += content + ", ";
                }
                else
                {
                    int index = txtSelected.Text.IndexOf(content);
                    if (index != -1)
                    {
                        txtSelected.Text = txtSelected.Text.Remove(index, (content + ", ").Length);
                    }
                }
            }
        }

        private void dgvContentDetail_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dgvContentDetail.Rows[e.RowIndex];
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["X"];
                row.Selected = true;
                if (chk.Value == null || (bool)chk.Value == false)
                    chk.Value = true;
                else
                    chk.Value = false;
                dgvContentDetail.EndEdit();
                dgvContentDetail.RefreshEdit();

                string content = dgvContentDetail.Rows[e.RowIndex].Cells["Content"].Value.ToString().Trim();
                if (chk.Value!= null && (bool)chk.Value == true)
                {
                    txtSelected.Text += content + ", ";
                }
                else
                {
                    int index = txtSelected.Text.IndexOf(content);
                    if (index != -1)
                    {
                        txtSelected.Text = txtSelected.Text.Remove(index, (content + ", ").Length);
                    }
                }
            }
        }
    }
}
