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
    public partial class Search : SecureBaseForm
    {
        SqlDataAdapter dtaPatient = new SqlDataAdapter();
        DataTable tbPatient = new DataTable();
        BindingSource bsPatient = new BindingSource();
        string[] imagesPatient = { };
        int imgIndex = 0;
        string BASE_IMG_FOLDER;
        Helper helper = new Helper();

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
            //TODO: check base folder is exist
            BASE_IMG_FOLDER = Properties.Settings.Default.imageFolder;
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
            dgvPatient.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dgvPatient.DataSource = bsPatient;
            dgvPatient.Columns[0].Visible = false;
            dgvPatient.Columns[1].HeaderText = "Tên";
            dgvPatient.Columns[2].HeaderText = "Tuổi";
            dgvPatient.Columns[3].HeaderText = "Ngày sinh";
            dgvPatient.Columns[4].HeaderText = "Ngày khám";
            //setRowNumber(dgvPatient);

            //update number record to label
            gbResult.Text = "Danh sách bệnh nhân (" + tbPatient.Rows.Count + " kết quả)";
            setRowNumber(dgvPatient);
        }

        /// <summary>
        /// auto set row index for data grid view
        /// </summary>
        /// <param name="dgv"></param>
        private void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);

            }
            dgv.RowHeadersWidth = dgv.Width / 12;
            dgv.RowHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11f);
        }

        private void dgPatient_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn column in dgvPatient.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 0; i < dgvPatient.Columns.Count; i++)
            {
                dgvPatient.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (i == dgvPatient.ColumnCount - 1)
                {
                    dgvPatient.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
            // create sql command
            bool isAnd = false;
            string query = "SELECT SickNum,SickName, Age, Birthday, Createtime FROM SickData ";
            if (!string.IsNullOrEmpty(txtTenSearch.Text.Trim()))
            {
                query += " WHERE SickName LIKE N'%" + txtTenSearch.Text.Trim() + "%'";
                isAnd = true;
            }
            if (!string.IsNullOrEmpty(txtCmndSearch.Text.Trim()))
            {
                if (isAnd == true)
                {
                    query += " AND IDCode LIKE N'%" + txtTenSearch.Text.Trim() + "%'";
                }
                else
                {
                    query += " WHERE IDCode LIKE 'N%" + txtTenSearch.Text.Trim() + "%'";
                    isAnd = true;
                }
            }
            if (dtNgayBatDau.Checked == true)
            {
                if (isAnd == true)
                {
                    query += " AND Createtime >= '" + dtNgayBatDau.Value.ToShortDateString() + "'";
                }
                else
                {
                    query += " WHERE Createtime >= '" + dtNgayBatDau.Value.ToShortDateString() + "'";
                    isAnd = true;
                }
            }
            if (dtNgayKetThuc.Checked == true)
            {
                if (isAnd == true)
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
            Console.WriteLine(query);
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
            dgvPatient.Columns[0].Visible = false;
            dgvPatient.Columns[1].HeaderText = "Tên";
            dgvPatient.Columns[2].HeaderText = "Tuổi";
            dgvPatient.Columns[3].HeaderText = "Ngày sinh";
            dgvPatient.Columns[4].HeaderText = "Ngày khám";
            setRowNumber(dgvPatient);
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
                string imageFolder = helper.getImgFolderById(sickNum);
                if (Directory.Exists(imageFolder))
                {
                    imagesPatient = Directory.GetFiles(imageFolder, "*.jpg", SearchOption.AllDirectories);
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
                //delete from database
                try
                {
                    string sqlCommand = "DELETE FROM SickData WHERE SickNum = '" + sickNum + "'";
                    SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                    mySQL.ExecuteReader();

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Không kết nối được đến cơ sở dữ liệu. Vui lòng thử lại sau! \n " + ex.ToString(),
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.ToString());
                    return;
                }
                //TODO: delete data in disk
                // delete data grid view
                tbPatient.Rows.RemoveAt(currRowIndexCheck);
                bsPatient.DataSource = tbPatient;
                dgvPatient.DataSource = bsPatient;
                dgvPatient.Update();
                dgvPatient.Refresh();
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
            btnDeleteSick.Enabled = ValidatedUserRoles.Contains(RolesList.DELETE_PATIENT);
        }
    }
}
