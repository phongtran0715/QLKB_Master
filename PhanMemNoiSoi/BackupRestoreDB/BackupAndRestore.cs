using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using OD.Forms.Security;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class BackupAndRestore : SecureBaseForm
    {
        private DataTable table = new DataTable();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private BindingSource bindingSource = new BindingSource();
        MyBackupRestore mybk;
        Helper helper;
        string databaseName = Properties.Settings.Default.dbName;

        public BackupAndRestore(IPrincipal userPrincipal) :
            base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
            helper = new Helper();
            try
            {
                mybk = new MyBackupRestore();
                mybk.connect();
            }
            catch (Exception)
            {
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbThang_CheckedChanged(object sender, EventArgs e)
        {
            if (rbThang.Checked)
            {
                cbThang.Enabled = true;
                cbNamThang.Enabled = true;
            }
            else
            {
                cbThang.Enabled = false;
                cbNamThang.Enabled = false;
            }
        }

        private void rbNam_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNam.Checked) cbNam.Enabled = true;
            else cbNam.Enabled = false;
        }

        private void BackupAndRestore_Load(object sender, EventArgs e)
        {
            rbNam.Checked = true;
            cbThang.Enabled = false;
            cbNamThang.Enabled = false;
            int currYear = DateTime.Today.Year;
            for (int i = currYear - 10; i <= currYear; i++)
            {
                cbNam.Items.Add(i.ToString().Trim());
                cbNamThang.Items.Add(i.ToString().Trim());
            }
            for (int i = 1; i <= 12; i++)
            {
                cbThang.Items.Add(i.ToString().Trim());
            }

            cbNam.Text = DateTime.Today.Year.ToString();
            cbNamThang.Text = DateTime.Today.Year.ToString();
            cbThang.Text = DateTime.Today.Month.ToString();
            dgvBackup.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            loadDgvData();
        }

        private void loadDgvData()
        {
            try
            {
                table = new System.Data.DataTable();
                bindingSource = new BindingSource();
                string selectCommand = "SELECT * FROM BackupInfo;";
                dataAdapter = new SqlDataAdapter(selectCommand, DBConnection.Instance.sqlConn);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                table = new System.Data.DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource.DataSource = table;
                dgvBackup.DataSource = bindingSource;
                dgvBackup.Columns[0].Visible = false;
                dgvBackup.Columns[1].HeaderText = "Tên File";
                dgvBackup.Columns[2].HeaderText = "Thư mục";
                dgvBackup.Columns[3].HeaderText = "Thời gian sao lưu";
                dgvBackup.Columns[4].HeaderText = "Ngày bắt đầu";
                dgvBackup.Columns[5].HeaderText = "Ngày kết thúc";
                helper.setRowNumber(dgvBackup);
                foreach (DataGridViewColumn col in dgvBackup.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold, GraphicsUnit.Pixel);
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "bak files (*.bak)|*.bak|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            string fileFullName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileFullName = saveFileDialog1.FileName;
                string bakFileName = Path.GetFileName(fileFullName).Trim();
                string bakFilePath = System.IO.Path.GetDirectoryName(fileFullName).Trim() + "\\";
                bool isSuccess = false;
                try
                {
                    isSuccess = mybk.backup(bakFilePath + bakFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Khôi phục dữ liệu không thành công.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.ToString());
                }

                if (isSuccess)
                {
                    DateTime backupStart = new DateTime(DateTime.Now.Year, 1, 1);
                    DateTime backupEnd = new DateTime(DateTime.Now.Year, 12, 31);

                    if (rbNam.Checked)
                    {
                        int year = int.Parse(cbNam.Text.Trim());
                        backupStart = new DateTime(year, 1, 1);
                        backupEnd = new DateTime(year, 12, 31);
                    }
                    else if (rbThang.Checked)
                    {
                        int year = int.Parse(cbNamThang.Text.Trim());
                        int month = int.Parse(cbThang.Text.Trim());
                        backupStart = new DateTime(year, month, 1);
                        backupEnd = backupStart.AddMonths(1).AddDays(-1);
                    }
                    try
                    {
                        string sqlCommand = "INSERT INTO BackupInfo (BackupName, BackupPath, BackupTime, BackupStart, BackupEnd) VALUES "
                                                        + "(@bkName, @bkPath, @bkTime, @bkStart, @bkEnd)";
                        SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                        //TODO: get filename / backup start date / backup end date
                        mySQL.Parameters.Add("@bkName", SqlDbType.NChar).Value = bakFileName;
                        mySQL.Parameters.Add("@bkPath", SqlDbType.NChar).Value = bakFilePath;
                        mySQL.Parameters.Add("@bkTime", SqlDbType.DateTime).Value = DateTime.Now;
                        mySQL.Parameters.Add("@bkStart", SqlDbType.DateTime).Value = backupStart;
                        mySQL.Parameters.Add("@bkEnd", SqlDbType.DateTime).Value = backupEnd;
                        mySQL.ExecuteNonQuery();
                        MessageBox.Show("Sao lưu dữ liệu " + bakFileName + " thành công!", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể lưu thông tin backup . Lỗi kết nối đến cơ sở dữ liệu \n" + ex.ToString(),
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.ToString());
                    }
                    //Update data grid view
                    loadDgvData();
                }else
                {
                    MessageBox.Show(@"Sao lưu dữ liệu thất bại. Vui lòng thay đổi thư mục lưu dữ liệu ! (VD: D:\QLKB)", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                return;
            }
        }

        public void ProgressEventHandler(object sender, PercentCompleteEventArgs e)
        {
            this.progressBar1.Value = e.Percent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedRowCount =
                dgvBackup.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount <= 0)
                {
                    MessageBox.Show("Bạn chưa chọn file để tiến hành khôi phục dữ liệu.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int currRowIndex = dgvBackup.SelectedRows[selectedRowCount - 1].Index;
                string num = dgvBackup.Rows[currRowIndex].Cells[0].Value.ToString().Trim();
                string bakFileName = dgvBackup.Rows[currRowIndex].Cells["BackupPath"].Value.ToString().Trim() +
                                     dgvBackup.Rows[currRowIndex].Cells["BackupName"].Value.ToString().Trim();

                DBConnection.Instance.CloseConnection();

                string sqlSErverInstance = DBConnection.Instance.sqlInstanceName;
                ServerConnection conn = new ServerConnection();
                conn.ServerInstance = sqlSErverInstance;
                Server server = new Server(conn);
                server.KillAllProcesses(databaseName);
                Restore destination = new Restore();
                destination.Action = RestoreActionType.Database;
                destination.Database = databaseName;
                BackupDeviceItem source = new BackupDeviceItem(bakFileName, DeviceType.File);
                destination.Devices.Add(source);
                destination.ReplaceDatabase = true;
                destination.SqlRestore(server);
                //open connection again
                bool check = DBConnection.Instance.OpenConnection();
                MessageBox.Show("Khôi phục dữ liệu thành công. Vui lòng khởi động lại phần mềm", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
                Environment.Exit(0);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Khôi phục dữ liệu thất bại. \n " +  ex.Message, "Thông báo",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "bak files (*.bak)|*.bak|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string bakFileName = openFileDialog1.FileName;
                restoreDatabase(bakFileName);
            }
        }

        /// <summary>
        /// Backup database function
        /// </summary>
        /// <param name="bakFileName">backup file name </param>
        /// <param name="bakFilePath">back up folder path</param>
        /// <param name="databaseName">database name to backup</param>
        /// <returns></returns>
        private bool backupDatabase(string bakFileName, string bakFilePath, string databaseName)
        {
            bool isSuccess = false;
            Backup bkp = new Backup();
            try
            {
                //create db server 
                string sqlSErverInstance = DBConnection.Instance.sqlInstanceName;
                ServerConnection conn = new ServerConnection();
                conn.ServerInstance = sqlSErverInstance;
                Server srv = new Server(conn);

                bool test = helper.createFile(bakFilePath + bakFileName);

                bkp.Action = BackupActionType.Database;
                bkp.Database = databaseName;
                bkp.Devices.AddDevice(bakFilePath + bakFileName, DeviceType.File);
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = 100;
                this.progressBar1.Value = 10;

                bkp.PercentCompleteNotification = 10;
                bkp.PercentComplete += new PercentCompleteEventHandler(ProgressEventHandler);

                bkp.SqlBackup(srv);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                helper.deleteFile(bakFilePath + bakFileName);
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.progressBar1.Value = 0;
            }
            return isSuccess;
        }

        /// <summary>
        /// Restore database function
        /// </summary>
        /// <param name="bakFileName">full path backup file </param>
        /// <param name="databaseName">database name to restore</param>
        /// <returns></returns>
        public void restoreDatabase(string bakFileName)
        {
            try
            {
                DBConnection.Instance.CloseConnection();
                string sqlSErverInstance = DBConnection.Instance.sqlInstanceName;
                ServerConnection conn = new ServerConnection();
                conn.ServerInstance = sqlSErverInstance;
                Server server = new Server(conn);
                server.KillAllProcesses(databaseName);
                Restore destination = new Restore();
                destination.Action = RestoreActionType.Database;
                destination.Database = databaseName;
                BackupDeviceItem source = new BackupDeviceItem(bakFileName, DeviceType.File);
                destination.Devices.Add(source);
                destination.ReplaceDatabase = true;
                destination.SqlRestore(server);
                DBConnection.Instance.OpenConnection();
                MessageBox.Show("Khôi phục dữ liệu thành công. Phần mềm sẽ khởi động lại.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
                Environment.Exit(0);
            }
            catch (SmoException exSMO)
            {
                MessageBox.Show("Không thể khôi phục dữ liệu : \n" + exSMO.Message, "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(exSMO.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khôi phục dữ liệu : \n" + ex.Message, "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this.progressBar1.Value = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvBackup.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int currRowIndexCheck = dgvBackup.SelectedRows[selectedRowCount - 1].Index;
            DialogResult dlResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nội dung này?", "Thông báo",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dlResult == DialogResult.Cancel)
            {
                return;
            }

            string num = dgvBackup.Rows[currRowIndexCheck].Cells["Num"].Value.ToString().Trim();
            //delete from database
            try
            {
                string sqlCommand = "DELETE FROM BackupInfo WHERE Num = @num";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@num", SqlDbType.NChar).Value = num;
                mySQL.ExecuteReader();

                // update data grid view
                table.Rows.RemoveAt(currRowIndexCheck);
                bindingSource.DataSource = table;
                dgvBackup.DataSource = bindingSource;
                dgvBackup.Update();
                dgvBackup.Refresh();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Không thể xóa dữ liệu. \n Không kết nối được đến cơ sở dữ liệu. \n Vui lòng thử lại sau!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void BackupAndRestore_UserIsDenied(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn không có quyền truy cập vào danh mục này.\nVui lòng liên hệ với admin!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
