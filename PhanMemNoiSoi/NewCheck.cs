﻿using OD.Forms.Security;
using System.Security.Principal;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class NewCheck : SecureBaseForm
    {
        private SqlDataAdapter dtaSick = new SqlDataAdapter();
        private BindingSource bsSick = new BindingSource();
        DataTable tbSick;
        Helper helper = new Helper();
        int currRowIndex;
        string BASE_IMG_FOLDER;
        bool ruleEnableDelete = false;
        public NewCheck(IPrincipal userPrincipal) : base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
            BASE_IMG_FOLDER = Properties.Settings.Default.imageFolder;
            // Set KeyPreview object to true to allow the form to process 
            // the key before the control with focus processes it.
            this.KeyPreview = true;
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

                    helper.setRowNumber(dgvBenhNhan, 20);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Thêm dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. \n Vui lòng thử lại sau",
                               "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    Console.WriteLine(ex.ToString());
                    return;
                }
            }

            // Add new sick data
            try
            {
                sickNum = DateTime.Now.ToString("yyyyMMddhhmmss");
                //create folder to store patient data
                string dataPath = helper.creatPatientFolder(sickNum);
                string query = "INSERT INTO dbo.SickData (SickNum, SickName, Age, Address, Sex, InsuranceId, Telephone, Occupation, CauseCheck, Createtime, DataPath)"
                    + " VALUES(@SickNum,@SickName,@Age, @Address, @Sex, @InsuranceId, @Telephone,@Occupation, @CauseCheck, @Createtime, @DataPath)";
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
                if(dataPath != null)
                {
                    command.Parameters.Add("@DataPath", SqlDbType.NChar).Value = dataPath;
                }
                command.ExecuteNonQuery();

                //update sick data grid view
                DataRow newRow = tbSick.NewRow();
                newRow["SickNum"] = sickNum;
                newRow["SickName"] = txtName.Text.Trim();
                newRow["Age"] = txtAge.Text.Trim();
                newRow["Address"] = txtAddress.Text.Trim();
                newRow["Occupation"] = txtJob.Text.Trim();
                newRow["Telephone"] = txtTelephone.Text.Trim();
                newRow["InsuranceId"] = txtInsureId.Text.Trim();
                newRow["Sex"] = sex;
                newRow["CauseCheck"] = txtCauseCheck.Text.Trim();
                tbSick.Rows.InsertAt(newRow, 0);
                dgvBenhNhan.DataSource = tbSick;
                dgvBenhNhan.Refresh();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Thêm dữ liệu không thành công. \n Không thể kết nối đến cơ sở dữ liệu. Vui lòng thử lại sau." + ex.ToString(),
                                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                return;
            }
            //enable button
            btnChupHinh.Enabled = true;
            btnDeleteSick.Enabled = true & ruleEnableDelete;
            //update number sick to label
            updateNumSick(dgvBenhNhan.Rows.Count - 1);
            
            helper.setRowNumber(dgvBenhNhan, 20);
            //set focus to new row

            for (int i = 0; i < dgvBenhNhan.RowCount; i++)
            {
                if (string.Equals(dgvBenhNhan.Rows[i].Cells["SickNum"].Value, sickNum))
                {
                    dgvBenhNhan.Rows[i].Selected = true;
                    break;
                }
            }
            //update checkId
            string outputValue = String.Format("{0:D6}", helper.getIdCheck());
            txtId.Text = outputValue;
        }


        private void txtTuoiBn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void NewCheck_Load(object sender, EventArgs e)
        {
            loadDgvData();

            string outputValue = String.Format("{0:D6}", helper.getIdCheck());
            txtId.Text = outputValue;

            DateTime dt = DateTime.Today;
            btnThemMoi.Enabled = false;
            btnDeleteSick.Enabled = false;
            btnChupHinh.Enabled = false;
            btnChiTiet.Enabled = false;
            //select last row in dgv
            int lastRowIndex = dgvBenhNhan.Rows.Count - 1;
            currRowIndex = lastRowIndex;
            selectRowInDgv(dgvBenhNhan, lastRowIndex);
        }

        private void loadDgvData()
        {
            string createTime = DateTime.Today.ToString("yyyy-MM-dd");
            string query = string.Format("SELECT SickNum,SickName, Age, Address, Occupation, Telephone, "
                                            + " InsuranceId, Sex, CauseCheck, DataPath FROM SickData WHERE Createtime = '"
                                            + createTime + "' ORDER BY SickNum DESC");
            dtaSick = new SqlDataAdapter(query, DBConnection.Instance.sqlConn);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dtaSick);
            tbSick = new DataTable();
            tbSick.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtaSick.Fill(tbSick);
            bsSick.DataSource = tbSick;
            dgvBenhNhan.DataSource = bsSick;

            dgvBenhNhan.Columns["SickNum"].Visible = false;
            dgvBenhNhan.Columns["DataPath"].Visible = false;
            dgvBenhNhan.Columns["SickName"].HeaderText = "Tên bệnh nhân";
            dgvBenhNhan.Columns["SickName"].Width = dgvBenhNhan.Width / 8;
            dgvBenhNhan.Columns["Age"].HeaderText = "Tuổi";
            dgvBenhNhan.Columns["Age"].Width = dgvBenhNhan.Width / 18;
            dgvBenhNhan.Columns["Age"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBenhNhan.Columns["Address"].HeaderText = "Địa chỉ";
            dgvBenhNhan.Columns["Address"].Width = dgvBenhNhan.Width / 9;
            dgvBenhNhan.Columns["Occupation"].HeaderText = "Nghề nghiệp";
            dgvBenhNhan.Columns["Occupation"].Width = dgvBenhNhan.Width / 9;
            dgvBenhNhan.Columns["Telephone"].HeaderText = "Điện thoại";
            dgvBenhNhan.Columns["Telephone"].Width = dgvBenhNhan.Width / 9;
            //dgvBenhNhan.Columns["Telephone"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBenhNhan.Columns["InsuranceId"].HeaderText = "Số BHYT";
            dgvBenhNhan.Columns["InsuranceId"].Width = dgvBenhNhan.Width / 9;
            dgvBenhNhan.Columns["Sex"].HeaderText = "Giới tính";
            dgvBenhNhan.Columns["Sex"].Width = dgvBenhNhan.Width / 15;
            //dgvBenhNhan.Columns["Sex"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBenhNhan.Columns["CauseCheck"].HeaderText = "Lý do khám";
            dgvBenhNhan.Columns["CauseCheck"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            helper.setRowNumber(dgvBenhNhan, 20);
            foreach (DataGridViewColumn col in dgvBenhNhan.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 13F, FontStyle.Bold, GraphicsUnit.Pixel);
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
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
            string dataPath = dgvBenhNhan.Rows[currRowIndex].Cells["DataPath"].Value.ToString().Trim();

            //delete data in disk
            if (Directory.Exists(dataPath))
            {
                try
                {
                    Directory.Delete(dataPath, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Không thể xóa thư mục chứa thông tin bệnh nhân. Vui lòng thử lại sau!", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //delete from database
            try
            {
                string sqlCommand = "DELETE FROM SickData WHERE SickNum = @sickNum";
                SqlCommand mySQL = new SqlCommand(sqlCommand, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@sickNum", SqlDbType.NChar).Value = sickNum;
                mySQL.ExecuteReader();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Không thể xóa dữ liệu. \n Không kết nối được đến cơ sở dữ liệu. \n Vui lòng thử lại sau!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // delete from data grid view
            dgvBenhNhan.Rows.RemoveAt(currRowIndex);
            updateNumSick(dgvBenhNhan.Rows.Count - 1);

            //select first row in dgv
            int lastRowIndex = dgvBenhNhan.Rows.Count - 1;
            if(lastRowIndex == 0)
            {
                btnChiTiet.Enabled = false;
                btnChupHinh.Enabled = false;
                btnDeleteSick.Enabled = false;
            }else
            {
                selectRowInDgv(dgvBenhNhan, 0);
                btnChiTiet.Enabled = true;
                btnChupHinh.Enabled = true;
                btnDeleteSick.Enabled = true & ruleEnableDelete;
            }
            currRowIndex = 0;
            helper.setRowNumber(dgvBenhNhan, 20);
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
                btnDeleteSick.Enabled = false;
                btnThemMoi.Enabled = false;
                btnChupHinh.Enabled = false;
                btnChiTiet.Enabled = false;
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

            btnDeleteSick.Enabled = true & ruleEnableDelete;
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
                //dgvBenhNhan.FirstDisplayedScrollingRowIndex = nRowIndex;
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
            if(dgvBenhNhan.SelectedRows.Count <= 0)
            {
                MessageBox.Show("Chưa có bệnh nhân nào được chọn. Vui lòng chọn bệnh nhân trước khi chụp ảnh!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Hide();
            currRowIndex = dgvBenhNhan.SelectedRows[0].Index;
            string patientId = dgvBenhNhan.Rows[currRowIndex].Cells["SickNum"].Value.ToString().Trim();
            string dataPath = dgvBenhNhan.Rows[currRowIndex].Cells["DataPath"].Value.ToString().Trim();
            string pName = txtName.Text.Trim();
            string pAge = txtAge.Text.Trim();
            string pInsurance = txtInsureId.Text.Trim();
            string doctor = "";
            string checkId = txtId.Text.Trim();
            //check dataPat is exist
            if(Directory.Exists(dataPath) == false)
            {
                //create new data path
                dataPath = helper.creatPatientFolder(patientId);
                //save new datapath to database
                if(dataPath != null)
                {
                    updateDataPath(patientId, dataPath);
                }
            }
            CheckAndView cavfr = new CheckAndView(checkId, patientId, pName, pAge, txtCauseCheck.Text.Trim(),
                                                    doctor, dataPath);
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
            try
            {
                currRowIndex = dgvBenhNhan.SelectedRows[selectedRowCount - 1].Index;
                string num = dgvBenhNhan.Rows[currRowIndex].Cells["SickNum"].Value.ToString().Trim();
                PatientDetail patienDetail = new PatientDetail(num);
                patienDetail.RefreshDgv += updateDgv;
                patienDetail.ShowDialog();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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

        private void NewCheck_SizeChanged(object sender, EventArgs e)
        {
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
            dgvBenhNhan.Columns["CauseCheck"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void updateDataPath(string sickNum, string dataPath)
        {
            try
            {
                string query = "UPDATE dbo.SickData SET DataPath = @DataPath WHERE SickNum = @SickNum";
                SqlCommand command = new SqlCommand(query, DBConnection.Instance.sqlConn);
                command.Parameters.Add("@DataPath", SqlDbType.NChar).Value = dataPath;
                command.Parameters.Add("@SickNum", SqlDbType.NChar).Value = sickNum;
                command.ExecuteNonQuery();

                //update data grid view
                dgvBenhNhan.Rows[currRowIndex].Cells["DataPath"].Value = dataPath;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void NewCheck_UserIsDenied(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn không có quyền truy cập vào danh mục này.\nVui lòng liên hệ với admin!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void NewCheck_UserIsAllowed(object sender, EventArgs e)
        {
            ruleEnableDelete = helper.myValidateRoles(RolesList.DELETE_PATIENT);
            btnDeleteSick.Enabled = ruleEnableDelete;
        }
    }
}
