using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class NewCheck : Form
    {
        private SqlDataAdapter dtaSick = new SqlDataAdapter();
        private BindingSource bsSick = new BindingSource();
        DataTable tbSick;
        Helper helper = new Helper();
        int currRowIndex;
        string BASE_IMG_FOLDER;
        string folderImgPath = null;
        public NewCheck()
        {
            InitializeComponent();
            BASE_IMG_FOLDER = Properties.Settings.Default.imageFolder;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnLuuBn_Click(object sender, EventArgs e)
        {
            string sex;
            if (rdMen.Checked)
            {
                sex = "Nam";
            }
            else
            {
                sex = "Nữ";
            }
            //check name and age
            if (string.IsNullOrWhiteSpace(txtName.Text.ToString()))
            {
                MessageBox.Show("Bạn chưa khai báo tên bệnh nhân!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtAge.Text.ToString()))
            {
                MessageBox.Show("Bạn chưa khai báo tuổi bệnh nhân!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string sickNum;
            //check exist sick
            if (currRowIndex != dgvBenhNhan.Rows.Count - 1)
            {
                try
                {
                    //update case
                    sickNum = dgvBenhNhan.Rows[currRowIndex].Cells["SickNum"].Value.ToString().Trim();
                    string query = "UPDATE dbo.SickData SET SickName = @SickName, Age = @Age, " +
                                    " Sex = @Sex ,Address = @Address , Telephone = @Telephone, CauseCheck = @CauseCheck, " +
                                    " Occupation = @Occupation, InsuranceId = @InsuranceId WHERE SickNum = @SickNum";
                    SqlCommand command = new SqlCommand(query, DBConnection.Instance.sqlConn);
                    command.Parameters.Add("@SickName", SqlDbType.NChar).Value = txtName.Text.Trim();
                    command.Parameters.Add("@Age", SqlDbType.Int).Value = int.Parse(txtAge.Text.Trim());
                    command.Parameters.Add("@Sex", SqlDbType.NChar).Value = sex;
                    command.Parameters.Add("@Address", SqlDbType.NChar).Value = txtAddress.Text.Trim();
                    command.Parameters.Add("@Telephone", SqlDbType.NChar).Value = txtTelephone.Text.Trim();
                    command.Parameters.Add("@CauseCheck", SqlDbType.NChar).Value = txtCauseCheck.Text.Trim();
                    command.Parameters.Add("@Occupation", SqlDbType.NChar).Value = txtJob.Text.Trim();
                    command.Parameters.Add("@InsuranceId", SqlDbType.NChar).Value = txtInsureId.Text.Trim();
                    command.Parameters.Add("@SickNum", SqlDbType.NChar).Value = sickNum;
                    command.ExecuteNonQuery();

                    //update data grid view
                    dgvBenhNhan.Rows[currRowIndex].Cells["SickName"].Value = txtName.Text.Trim();
                    dgvBenhNhan.Rows[currRowIndex].Cells["Age"].Value = txtAge.Text.Trim();
                    dgvBenhNhan.Rows[currRowIndex].Cells["Address"].Value = txtAddress.Text.Trim();
                    dgvBenhNhan.Rows[currRowIndex].Cells["Occupation"].Value = txtJob.Text.Trim();
                    dgvBenhNhan.Rows[currRowIndex].Cells["Telephone"].Value = txtTelephone.Text.Trim();
                    dgvBenhNhan.Rows[currRowIndex].Cells["InsuranceId"].Value = txtInsureId.Text.Trim();
                    dgvBenhNhan.Rows[currRowIndex].Cells["Sex"].Value = sex;
                    dgvBenhNhan.Rows[currRowIndex].Cells["CauseCheck"].Value = txtCauseCheck.Text.Trim();

                    helper.setRowNumberDgvSick(dgvBenhNhan);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Thêm dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau",
                               "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    Console.WriteLine(ex.ToString());
                    Log.Instance.LogMessageToFile(ex.ToString());
                    return;
                }
            }

            // Add new sick data
            try
            {
                sickNum = DateTime.Now.ToString("yyyyMMddhhmmss");
                string query = "INSERT INTO dbo.SickData (SickNum, SickName, Age, Address, Sex, InsuranceId, Telephone, Occupation, CauseCheck, Createtime)"
                    + " VALUES(@SickNum,@SickName,@Age, @Address, @Sex, @InsuranceId, @Telephone,@Occupation, @CauseCheck, @Createtime)";
                SqlCommand command = new SqlCommand(query, DBConnection.Instance.sqlConn);
                command.Parameters.Add("@SickNum", SqlDbType.NChar).Value = sickNum;
                command.Parameters.Add("@SickName", SqlDbType.NChar).Value = txtName.Text.ToString().Trim();
                command.Parameters.Add("@Age", SqlDbType.Int).Value = int.Parse(txtAge.Text.ToString().Trim());
                command.Parameters.Add("@Address", SqlDbType.NChar).Value = txtAddress.Text.ToString().Trim();
                command.Parameters.Add("@Sex", SqlDbType.NChar).Value = sex;
                command.Parameters.Add("@InsuranceId", SqlDbType.NChar).Value = txtInsureId.Text.ToString().Trim();
                command.Parameters.Add("@Telephone", SqlDbType.NChar).Value = txtTelephone.Text.ToString().Trim();
                command.Parameters.Add("@Occupation", SqlDbType.NChar).Value = txtJob.Text.Trim();
                command.Parameters.Add("@CauseCheck", SqlDbType.NChar).Value = txtCauseCheck.Text.Trim();
                command.Parameters.Add("@Createtime", SqlDbType.DateTime).Value = DateTime.Today.ToShortDateString();
                command.ExecuteNonQuery();

                //update sick data grid view
                tbSick.Rows.Add(sickNum,
                                txtName.Text.Trim(),
                                txtAge.Text.Trim(),
                                txtAddress.Text.Trim(),
                                txtJob.Text.Trim(),
                                txtTelephone.Text.Trim(),
                                txtInsureId.Text.Trim(),
                                sex,
                                txtCauseCheck.Text.Trim());
                dgvBenhNhan.DataSource = tbSick;
                dgvBenhNhan.Refresh();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Thêm dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. Vui lòng thử lại sau." + ex.ToString(),
                                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                Log.Instance.LogMessageToFile(ex.ToString());
                return;
            }
            //enable button
            btnChupHinh.Enabled = true;
            btnXoaBN.Enabled = true;
            //update number sick to label
            updateNumSick(dgvBenhNhan.Rows.Count - 1);
            //create folder to store patient data
            creatPatientFolder(sickNum);
            helper.setRowNumberDgvSick(dgvBenhNhan);
            //set focus to new row
            //dgvBenhNhan.Rows[dgvBenhNhan.RowCount - 1].Selected = true;
            for (int i = 0; i < dgvBenhNhan.RowCount; i++)
            {
                if (string.Equals(dgvBenhNhan.Rows[i].Cells["SickNum"].Value, sickNum))
                {
                    dgvBenhNhan.Rows[i].Selected = true;
                    break;
                }
            }
            //update checkId
            string outputValue = String.Format("{0:D6}", getIdCheck());
            txtId.Text = outputValue;
        }

        private void creatPatientFolder(string sickNum)
        {
            //check base folder is exist
            if (!Directory.Exists(BASE_IMG_FOLDER))
            {
                BASE_IMG_FOLDER = Properties.Settings.Default.defaultImgFolder;
                System.IO.Directory.CreateDirectory(BASE_IMG_FOLDER);
                Properties.Settings.Default.imageFolder = BASE_IMG_FOLDER;
            }
            //check month folder
            folderImgPath = BASE_IMG_FOLDER + "\\"
                                  + DateTime.Today.Year.ToString()
                                  + DateTime.Today.Month.ToString();
            bool exists = System.IO.Directory.Exists(folderImgPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(folderImgPath);
            }
            //check day folder
            folderImgPath += "\\" + DateTime.Today.Day.ToString();
            exists = System.IO.Directory.Exists(folderImgPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(folderImgPath);
            }
            //check patient folder
            //folderImgPath += "\\" + name + "_" +  sickNum + "\\";
            folderImgPath += "\\" + sickNum + "\\";
            exists = System.IO.Directory.Exists(folderImgPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(folderImgPath);
            }
        }
        private void txtTuoiBn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void NewCheck_Load(object sender, EventArgs e)
        {
            loadDgvData();

            string outputValue = String.Format("{0:D6}", getIdCheck());
            txtId.Text = outputValue;

            DateTime dt = DateTime.Today;
            btnThemMoi.Enabled = false;
            btnXoaBN.Enabled = false;
            btnChupHinh.Enabled = false;
            btnChiTiet.Enabled = false;
            //select last row in dgv
            int lastRowIndex = dgvBenhNhan.Rows.Count - 1;
            currRowIndex = lastRowIndex;
            selectRowInDgv(dgvBenhNhan, lastRowIndex);
        }

        private void loadDgvData()
        {
            string query = string.Format("SELECT SickNum,SickName, Age, Address, Occupation, Telephone, InsuranceId, Sex, CauseCheck "
                                            + " FROM SickData WHERE Createtime = '"
                                            + DateTime.Today + "'");
            dtaSick = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaSick);
            tbSick = new DataTable();
            tbSick.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaSick.Fill(tbSick);
            bsSick.DataSource = tbSick;

            dgvBenhNhan.DataSource = bsSick;
            dgvBenhNhan.Columns["SickNum"].Visible = false;
            dgvBenhNhan.Columns["STT"].Width = dgvBenhNhan.Width / 18 ;
            dgvBenhNhan.Columns["SickName"].HeaderText = "Tên bệnh nhân";
            dgvBenhNhan.Columns["SickName"].Width = dgvBenhNhan.Width / 9;
            dgvBenhNhan.Columns["Age"].HeaderText = "Tuổi";
            dgvBenhNhan.Columns["Age"].Width = dgvBenhNhan.Width / 18;
            dgvBenhNhan.Columns["Address"].HeaderText = "Địa chỉ";
            dgvBenhNhan.Columns["Address"].Width = dgvBenhNhan.Width / 9;
            dgvBenhNhan.Columns["Occupation"].HeaderText = "Nghề nghiệp";
            dgvBenhNhan.Columns["Occupation"].Width = dgvBenhNhan.Width / 9;
            dgvBenhNhan.Columns["Telephone"].HeaderText = "Điện thoại";
            dgvBenhNhan.Columns["Telephone"].Width = dgvBenhNhan.Width / 9;
            dgvBenhNhan.Columns["InsuranceId"].HeaderText = "Số BHYT";
            dgvBenhNhan.Columns["InsuranceId"].Width = dgvBenhNhan.Width / 9;
            dgvBenhNhan.Columns["Sex"].HeaderText = "Giới tính";
            dgvBenhNhan.Columns["Sex"].Width = dgvBenhNhan.Width / 15;
            dgvBenhNhan.Columns["CauseCheck"].HeaderText = "Lý do khám";
            dgvBenhNhan.Columns["CauseCheck"].Width = (dgvBenhNhan.Width * 3) / 9;
            helper.setRowNumberDgvSick(dgvBenhNhan);
            foreach (DataGridViewColumn col in dgvBenhNhan.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold, GraphicsUnit.Pixel);
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private string creatSickNum()
        {
            string sickNum = "SN";
            sickNum += DateTime.Today.Year.ToString();
            sickNum += DateTime.Today.Month.ToString();
            string dt = DateTime.Now.ToString("MMMM dd, yyyy");
            MessageBox.Show(dt);
            return null;
        }

        private void btnXoaBN_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvBenhNhan.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            currRowIndex = dgvBenhNhan.SelectedRows[selectedRowCount - 1].Index;

            DialogResult dlResult = MessageBox.Show("Bạn có chắc chắn muốn xóa bệnh nhân này?", "Thông báo",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dlResult == DialogResult.Cancel)
            {
                return;
            }

            string sickNum = dgvBenhNhan.Rows[currRowIndex].Cells["SickNum"].Value.ToString().Trim();

            //delete from database
            try
            {
                string sqlCommand = "DELETE FROM SickData WHERE SickNum = @sickNum";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@sickNum", SqlDbType.NChar).Value = sickNum;
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
            dgvBenhNhan.Rows.RemoveAt(currRowIndex);
            updateNumSick(dgvBenhNhan.Rows.Count - 1);
            //delete image in disk
            string imgPath = helper.getImgFolderById(sickNum);
            if (Directory.Exists(imgPath))
            {
                try
                {
                    Directory.Delete(imgPath, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            //select last row in dgv
            int lastRowIndex = dgvBenhNhan.Rows.Count - 1;
            currRowIndex = lastRowIndex;
            selectRowInDgv(dgvBenhNhan, lastRowIndex);
            helper.setRowNumberDgvSick(dgvBenhNhan);
        }

        private void dgvBenhNhan_SelectionChanged(object sender, EventArgs e)
        {
            initSickData();
            int selectedRowCount =
            dgvBenhNhan.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }
            currRowIndex = dgvBenhNhan.SelectedRows[selectedRowCount - 1].Index;
            if (currRowIndex == dgvBenhNhan.Rows.Count - 1)
            {
                btnXoaBN.Enabled = false;
                btnThemMoi.Enabled = false;
                btnChupHinh.Enabled = false;
                return;
            }

            txtName.Text = dgvBenhNhan.Rows[currRowIndex].Cells["SickName"].Value.ToString().Trim();
            txtAge.Text = dgvBenhNhan.Rows[currRowIndex].Cells["Age"].Value.ToString().Trim();
            txtAddress.Text = dgvBenhNhan.Rows[currRowIndex].Cells["Address"].Value.ToString().Trim();
            txtJob.Text = dgvBenhNhan.Rows[currRowIndex].Cells["Occupation"].Value.ToString().Trim();
            txtTelephone.Text = dgvBenhNhan.Rows[currRowIndex].Cells["Telephone"].Value.ToString().Trim();
            txtInsureId.Text = dgvBenhNhan.Rows[currRowIndex].Cells["InsuranceId"].Value.ToString().Trim();
            if (string.Equals(dgvBenhNhan.Rows[currRowIndex].Cells["Sex"].Value.ToString().Trim(), "Nam"))
                rdMen.Checked = true;
            else
                rdWomen.Checked = true;
            txtCauseCheck.Text = dgvBenhNhan.Rows[currRowIndex].Cells["CauseCheck"].Value.ToString().Trim();

            btnXoaBN.Enabled = true;
            btnChupHinh.Enabled = true;
            btnThemMoi.Enabled = true;
            btnChiTiet.Enabled = true;
        }

        private void initSickData()
        {
            txtName.Text = string.Empty;
            txtAge.Text = string.Empty;
            rdWomen.Checked = true;
            txtAddress.Text = string.Empty;
            txtTelephone.Text = string.Empty;
            txtCauseCheck.Text = string.Empty;
            txtInsureId.Text = string.Empty;
            txtJob.Text = string.Empty;
        }

        private void btnThemMoi_Click(object sender, EventArgs e)
        {
            initSickData();
            //select last row in dgv
            int nRowIndex = dgvBenhNhan.Rows.Count - 1;
            if (nRowIndex < 0)
            {
                return;
            }
            else
            {
                currRowIndex = nRowIndex;
                dgvBenhNhan.Rows[nRowIndex].Selected = true;
                //In case if you want to scroll down as well.
                dgvBenhNhan.FirstDisplayedScrollingRowIndex = nRowIndex;
            }
        }

        private void selectRowInDgv(DataGridView dgv, int index)
        {
            //select last row in dgv
            if (index < 0)
            {
                return;
            }
            else
            {
                dgv.Rows[index].Selected = true;
                //In case if you want to scroll down as well.
                dgv.FirstDisplayedScrollingRowIndex = index;
            }
        }

        private void updateNumSick(int numSick)
        {
            gbBenhNhan.Text = "Thông tin bệnh nhân (số lượng : " + numSick + ")";
        }

        private void btnChupHinh_Click(object sender, EventArgs e)
        {
            this.Hide();
            currRowIndex = dgvBenhNhan.SelectedRows[0].Index;
            string patientId = dgvBenhNhan.Rows[currRowIndex].Cells["SickNum"].Value.ToString().Trim();
            string pName = txtName.Text.Trim();
            string pAge = txtAge.Text.Trim();
            string pInsurance = txtInsureId.Text.Trim();
            string doctor = "";
            string checkId = txtId.Text.Trim();
            //TODO : get patient image 
            folderImgPath = Properties.Settings.Default.imageFolder + "\\" + DateTime.Today.Year.ToString()
                        + DateTime.Today.Month + "\\"
                        + DateTime.Today.Day + "\\"
                        + patientId + "\\";
            if (!Directory.Exists(folderImgPath))
            {
                folderImgPath = "D:\\QLKB" + "\\" + DateTime.Today.Year.ToString()
                                        + DateTime.Today.Month + "\\"
                                        + DateTime.Today.Day + "\\"
                                        + patientId + "\\";
            }
            CheckAndView cavfr = new CheckAndView(checkId, patientId, pName, pAge, txtCauseCheck.Text.Trim(),
                                                    doctor, folderImgPath);
            cavfr.ShowDialog();
            this.Show();
            //set focus to new row
            dgvBenhNhan.Rows[tbSick.Rows.Count].Selected = true;
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvBenhNhan.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0)
            {
                return;
            }

            currRowIndex = dgvBenhNhan.SelectedRows[selectedRowCount - 1].Index;
            string num = dgvBenhNhan.Rows[currRowIndex].Cells["SickNum"].Value.ToString().Trim();
            PatientDetail patienDetail = new PatientDetail(num);
            patienDetail.RefreshDgv += updateDgv;
            patienDetail.ShowDialog();
        }

        private void updateDgv(string name, string age, string address,
                                     string job, string telephone, string insuranceId,
                                     string sex, string causeCheck)
        {
            //update info component
            txtName.Text = name;
            txtAge.Text = age;
            txtAddress.Text = address;
            txtJob.Text = job;
            txtTelephone.Text = telephone;
            txtInsureId.Text = insuranceId;
            if (string.Equals(sex, "Nam"))
                rdMen.Checked = true;
            else
                rdWomen.Checked = true;
            txtCauseCheck.Text = causeCheck;

            //update dgv
            dgvBenhNhan.Rows[currRowIndex].Cells["SickName"].Value = name;
            dgvBenhNhan.Rows[currRowIndex].Cells["Age"].Value = age;
            dgvBenhNhan.Rows[currRowIndex].Cells["Address"].Value = address;
            dgvBenhNhan.Rows[currRowIndex].Cells["Occupation"].Value = job;
            dgvBenhNhan.Rows[currRowIndex].Cells["Telephone"].Value = telephone;
            dgvBenhNhan.Rows[currRowIndex].Cells["InsuranceId"].Value = insuranceId;
            dgvBenhNhan.Rows[currRowIndex].Cells["Sex"].Value = sex;
            dgvBenhNhan.Rows[currRowIndex].Cells["CauseCheck"].Value = causeCheck;
        }
        private void dgvBenhNhan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                string num = dgvBenhNhan.Rows[e.RowIndex].Cells["SickNum"].Value.ToString().Trim();
                PatientDetail patienDetail = new PatientDetail(num);
                patienDetail.RefreshDgv += updateDgv;
                patienDetail.ShowDialog();
            }
        }

        private void txtTuoiBn_TextChanged(object sender, EventArgs e)
        {
            if (txtAge.Text != null && txtAge.Text != "")
            {
                if (1 > int.Parse(txtAge.Text.Trim()) ||
               120 < int.Parse(txtAge.Text.Trim()))
                {
                    MessageBox.Show("Tuổi bệnh nhân phải nằm trong khoảng 1 đến 120", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtAge.Text = "";
                }
            }
        }

        private int getIdCheck()
        {
            string sql = "SELECT  COUNT(*) FROM SickData";
            SqlCommand comd = new SqlCommand(sql, DBConnection.Instance.sqlConn);
            int count = Convert.ToInt32(comd.ExecuteScalar());
            return count;
        }
    }
}
