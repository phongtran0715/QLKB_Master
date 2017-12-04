using OD.Forms.Security;
using PhanMemNoiSoi.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class Search : SecureBaseForm
    {
        SqlDataAdapter dtaPatient = new SqlDataAdapter();
        DataTable tbPatient = new DataTable();
        BindingSource bsPatient = new BindingSource();
        string[] imagesPatient = { };
        int imgIndex = 0;
        string BASE_IMG_FOLDER;
        Helper helper = new Helper();
        string curImgFolder;

        public Search(IPrincipal userPrincipal) :
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
            dtNgayBatDau.CustomFormat = helper.getDateFormat(Settings.Default.datetimeFormat);
            dtNgayKetThuc.CustomFormat = helper.getDateFormat(Settings.Default.datetimeFormat);
            loadDgvPatient();
            //TODO: check base folder is exist
            BASE_IMG_FOLDER = Properties.Settings.Default.imageFolder;
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            loadDgvPatient();
            //update number record to label
            gbResult.Text = "Danh sách bệnh nhân (" + tbPatient.Rows.Count + " kết quả)";
        }

        private void loadDgvPatient()
        {
            // load data for data grid view
            string str = Settings.Default.maxRowDisplay.ToString();
            string query = "SELECT TOP " + str + " SickNum,SickName, Age, Birthday, Createtime FROM SickData ORDER BY Createtime DESC;";
            dtaPatient = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaPatient);

            tbPatient = new DataTable();
            tbPatient.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaPatient.Fill(tbPatient);
            bsPatient.DataSource = tbPatient;


            // Resize the DataGridView columns to fit the newly loaded content.
            dgvPatient.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dgvPatient.DataSource = bsPatient;
            dgvPatient.Columns["SickNum"].Visible = false;
            dgvPatient.Columns["SickName"].HeaderText = "Tên";
            dgvPatient.Columns["Age"].HeaderText = "Tuổi";
            dgvPatient.Columns["Age"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPatient.Columns["Birthday"].HeaderText = "Ngày sinh";
            dgvPatient.Columns["Birthday"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPatient.Columns["Createtime"].HeaderText = "Ngày khám";
            dgvPatient.Columns["Createtime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPatient.Columns["Createtime"].DefaultCellStyle.Format = helper.getDateFormat(Settings.Default.datetimeFormat);

            dgvPatient.Columns["SickName"].Width = dgvPatient.Width * 4 / 10;
            dgvPatient.Columns["Age"].Width = dgvPatient.Width / 10;
            dgvPatient.Columns["Birthday"].Width = dgvPatient.Width * 2 / 10;
            dgvPatient.Columns["Createtime"].Width = dgvPatient.Width * 2 / 9;
            foreach (DataGridViewColumn col in dgvPatient.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            helper.setRowNumber(dgvPatient);
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

        private void rbNam_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNam.Checked == true)
            {
                dtNgayBatDau.Value = new DateTime(DateTime.Today.Year, 1, 1);
                dtNgayKetThuc.Value = new DateTime(DateTime.Today.Year, 12,
                                                    DateTime.DaysInMonth(DateTime.Today.Year, 12));
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
            txtTenSearch.Text = string.Empty;
            txtAge.Text = string.Empty;
            dtNgayBatDau.Checked = false;
            dtNgayKetThuc.Checked = false;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            //check start date & end date 
            int checkTime = DateTime.Compare(dtNgayBatDau.Value, dtNgayKetThuc.Value);
            if (checkTime > 0)
            {
                MessageBox.Show("Điều kiện tìm kiếm không hợp lệ. \n Ngày bắt đầu phải nhỏ hơn ngày kết thúc",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // create sql command
            bool isAnd = false;
            string query = "SELECT SickNum,SickName, Age, Birthday, Createtime FROM SickData ";
            if (!string.IsNullOrEmpty(txtTenSearch.Text.Trim()))
            {
                query += " WHERE SickName LIKE N'%" + txtTenSearch.Text.Trim() + "%'";
                isAnd = true;
            }
            if (!string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                if (isAnd == true)
                {
                    query += " AND Telephone LIKE N'%" + txtPhone.Text.Trim() + "%'";
                }
                else
                {
                    query += " WHERE Telephone LIKE N'%" + txtPhone.Text.Trim() + "%'";
                    isAnd = true;
                }
            }
            if (dtNgayBatDau.Checked == true)
            {
                if (isAnd == true)
                {
                    query += " AND Createtime >= '" + dtNgayBatDau.Value.ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    query += " WHERE Createtime >= '" + dtNgayBatDau.Value.ToString("yyyy-MM-dd") + "'";
                    isAnd = true;
                }
            }
            if (dtNgayKetThuc.Checked == true)
            {
                if (isAnd == true)
                {
                    query += " AND Createtime <= '" + dtNgayKetThuc.Value.ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    query += " WHERE Createtime <= '" + dtNgayKetThuc.Value.ToString("yyyy-MM-dd") + "'";
                    isAnd = true;
                }
            }
            query += " ORDER BY Createtime DESC;";
            Console.WriteLine(query);
            dtaPatient = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaPatient);
            tbPatient = new DataTable();
            tbPatient.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaPatient.Fill(tbPatient);
            bsPatient.DataSource = tbPatient;

            // Resize the DataGridView columns to fit the newly loaded content
            dgvPatient.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dgvPatient.DataSource = bsPatient;
            dgvPatient.Columns["SickNum"].Visible = false;
            dgvPatient.Columns["SickName"].HeaderText = "Tên";
            dgvPatient.Columns["Age"].HeaderText = "Tuổi";
            dgvPatient.Columns["Birthday"].HeaderText = "Ngày sinh";
            dgvPatient.Columns["Createtime"].HeaderText = "Ngày khám";

            dgvPatient.Columns["SickName"].Width = dgvPatient.Width * 4 / 10;
            dgvPatient.Columns["Age"].Width = dgvPatient.Width / 10;
            dgvPatient.Columns["Birthday"].Width = dgvPatient.Width * 2 / 10;
            dgvPatient.Columns["Createtime"].Width = dgvPatient.Width * 2 / 9;
            foreach (DataGridViewColumn col in dgvPatient.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            helper.setRowNumber(dgvPatient);
            //update number record to label
            gbResult.Text = "Danh sách bệnh nhân (" + tbPatient.Rows.Count + " kết quả)";
        }

        private void dgPatient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //reset data
            pbPatient.Image = null;
            lbimgCount.Text = "0 / 0";
            if (e.RowIndex != -1)
            {
                resetPatientDetail();
                string sickNum = dgvPatient.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                Patient patient = new Patient().getPatientByNum(sickNum);
                curImgFolder = helper.getImgFolderById(sickNum);
                if (Directory.Exists(curImgFolder))
                {
                    imagesPatient = Directory.GetFiles(curImgFolder, "*.jpg", SearchOption.AllDirectories);
                    if (imagesPatient.Length > 0)
                    {
                        Image img = Image.FromFile(imagesPatient[0]);
                        pbPatient.Image = img;
                        lbimgCount.Text = "1 / " + imagesPatient.Length;
                    }
                }

                updatePatientDetail(patient);
            }
        }

        private void updatePatientDetail(Patient patient)
        {
            txtJob.Text = patient.JobProperty;
            txtNote.Text = patient.NoteProperty;
            txtName.Text = patient.NameProperty;
            txtMarred.Text = patient.CauseCheckProperty;
            txtIdCode.Text = patient.IdCodeProperty;
            txtAge.Text = patient.AgeProperty.ToString();
            txtTelephone.Text = patient.TelephoneProperty;
            txtCreatetime.Text = patient.CreateTimeProperty.ToShortDateString();
            if (patient.BirthdayProperty != DateTime.MinValue)
            {
                txtBirthday.Text = patient.BirthdayProperty.ToShortDateString();
            }
        }

        private void resetPatientDetail()
        {
            txtIdCode.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            txtTelephone.Text = "";
            txtMarred.Text = "";
            txtCreatetime.Text = "";
            txtBirthday.Text = "";
            txtJob.Text = "";
            txtNote.Text = "";
        }

        private void btnNextImg_Click(object sender, EventArgs e)
        {
            if (imagesPatient.Length > 0)
            {
                if (imgIndex <= imagesPatient.Length - 1)
                {
                    if (imgIndex < imagesPatient.Length - 1)
                    {
                        imgIndex += 1;
                    }
                    pbPatient.Image = Image.FromFile(imagesPatient[imgIndex]);
                    lbimgCount.Text = (imgIndex + 1).ToString() + " / " + imagesPatient.Length;
                }
            }
        }

        private void btnPrevImg_Click(object sender, EventArgs e)
        {
            if (imagesPatient.Length > 0)
            {
                if (imgIndex >= 0)
                {
                    if (imgIndex > 0)
                    {
                        imgIndex -= 1;
                    }
                    pbPatient.Image = Image.FromFile(imagesPatient[imgIndex]);
                    lbimgCount.Text = (imgIndex + 1).ToString() + " / " + imagesPatient.Length;
                }
            }

        }

        private void btnDeleteSick_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvPatient.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            DialogResult result = MessageBox.Show("Bạn có muốn cóa thông tin bệnh nhân này?", "Thông báo",
                                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                int currRowIndexCheck = dgvPatient.SelectedRows[selectedRowCount - 1].Index;
                string sickNum = dgvPatient.Rows[currRowIndexCheck].Cells["SickNum"].Value.ToString().Trim();
                string sickName = dgvPatient.Rows[currRowIndexCheck].Cells["SickName"].Value.ToString().Trim();
                //delete from database
                try
                {
                    string sqlCommand = "DELETE FROM SickData WHERE SickNum = @sickNum";
                    SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                    mySQL.Parameters.Add("@sickNum", SqlDbType.NChar).Value = sickNum;
                    mySQL.ExecuteReader();
                    string msgLog = "Xóa thông tin bệnh nhân '" + sickName + "'";
                    Log.Instance.LogMessageToDB(DateTime.Now, Session.Instance.UserId, Session.Instance.UserName, msgLog);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Không kết nối được đến cơ sở dữ liệu. Vui lòng thử lại sau! \n " + ex.ToString(),
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.ToString());
                    return;
                }
                //TODO: delete data in disk
                //string tmp = curImgFolder;
                //helper.deleteFolder(curImgFolder);
                // delete data grid view
                tbPatient.Rows.RemoveAt(currRowIndexCheck);
                bsPatient.DataSource = tbPatient;
                dgvPatient.DataSource = bsPatient;
                dgvPatient.Update();
                dgvPatient.Refresh();
                if (dgvPatient.RowCount == 0)
                {
                    btnOpenFolder.Enabled = false;
                }
                //update number patient in label
                gbResult.Text = "Danh sách bệnh nhân ( " + tbPatient.Rows.Count + " kết quả)";
            }

        }

        private void btnReCheck_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvPatient.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            int currRowIndexCheck = dgvPatient.SelectedRows[selectedRowCount - 1].Index;
            string pId = dgvPatient.Rows[currRowIndexCheck].Cells["SickNum"].Value.ToString().Trim();
            Patient patient = new Patient().getPatientByNum(pId);
            string numId = String.Format("{0:D6}", helper.getIdCheck());
            string pName = patient.NameProperty;
            string pAge = patient.AgeProperty.ToString();
            string pCause = patient.CauseCheckProperty;
            string doctor = Session.Instance.UserName;
            string folderImgPath = new Patient().getFolderPathByPatient(patient); 
            this.Hide();
            CheckAndView checkFr = new CheckAndView(numId, pId, pName, pAge, pCause, doctor, folderImgPath);
            checkFr.ShowDialog();
            this.Close();
        }

        private void Search_UserIsDenied(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn không có quyền truy cập vào danh mục này.\nVui lòng liên hệ với admin!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Search_UserIsAllowed(object sender, EventArgs e)
        {
            btnDeleteSick.Enabled = helper.myValidateRoles(RolesList.DELETE_PATIENT);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dgvPatient.RowCount == 0)
            {
                MessageBox.Show("Danh sách bệnh nhân trống.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int selectedRowCount =
            dgvPatient.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                MessageBox.Show("Bạn chưa chọn bệnh nhân. Chọn bệnh nhân trên danh sách để xem chi tiết", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //check folder is existed
            if (Directory.Exists(curImgFolder))
            {
                try
                {
                    Process.Start(curImgFolder);
                }
                catch (Win32Exception win32Exception)
                {
                    //The system cannot find the file specified...
                    Console.WriteLine(win32Exception.Message);
                }
            }else
            {
                MessageBox.Show("Thư mục không tồn tại.", "Thông báo", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
    }
}
