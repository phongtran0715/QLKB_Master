﻿using OD.Forms.Security;
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
        SqlDataAdapter sqlDta = new SqlDataAdapter();
        string[] imagesPatient = { };
        int imgIndex = 0;
        string BASE_IMG_FOLDER;
        Helper helper = new Helper();
        DataSet ds;
        private int PageSize = 15;
        private int CurrentPageIndex = 1;
        private int TotalPage = 0;
        private SqlCommand sqlCmd;
        bool ruleEnableDelete = false;

        public Search(IPrincipal userPrincipal) 
            :base(Session.Instance.UserRole, userPrincipal)
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Search_Load(object sender, EventArgs e)
        {
            PageSize = Properties.Settings.Default.maxRowDisplay;
            Console.WriteLine("page size is " + PageSize);
            dtNgayBatDau.CustomFormat = helper.getDateFormat(Settings.Default.datetimeFormat);
            dtNgayKetThuc.CustomFormat = helper.getDateFormat(Settings.Default.datetimeFormat);
            loadFilterData();
            //TODO: check base folder is exist
            BASE_IMG_FOLDER = Properties.Settings.Default.imageFolder;
        }

        private void formatDgv()
        {
            try
            {
                // Resize the DataGridView columns to fit the newly loaded content.
                dgvPatient.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                dgvPatient.Columns["DataPath"].Visible = false;
                dgvPatient.Columns["SickNum"].HeaderText = "Mã BN";
                dgvPatient.Columns["SickNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPatient.Columns["SickName"].HeaderText = "Tên";
                dgvPatient.Columns["Age"].HeaderText = "Tuổi";
                dgvPatient.Columns["Age"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPatient.Columns["Telephone"].HeaderText = "SĐT";
                dgvPatient.Columns["Telephone"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPatient.Columns["Createtime"].HeaderText = "Ngày khám";
                dgvPatient.Columns["Createtime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPatient.Columns["Createtime"].DefaultCellStyle.Format = helper.getDateFormat(Settings.Default.datetimeFormat);

                dgvPatient.Columns["SickNum"].Width = dgvPatient.Width * 2 / 10;
                dgvPatient.Columns["SickName"].Width = dgvPatient.Width * 3 / 10;
                dgvPatient.Columns["Age"].Width = dgvPatient.Width / 10;
                dgvPatient.Columns["Telephone"].Width = dgvPatient.Width * 2 / 10;
                dgvPatient.Columns["Createtime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                foreach (DataGridViewColumn col in dgvPatient.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                helper.setRowNumber(dgvPatient, this.CurrentPageIndex, this.CurrentPageIndex - 1, this.PageSize);
                if (dgvPatient.Rows.Count > 0)
                    setFieldEnable(true);
                else
                    setFieldEnable(false);
            }
            catch (Exception)
            {
                //nothing todo
            }
        }

        private DataTable GetFilterRecord(int page)
        {
            DataTable dt = new DataTable();
            dt.TableName = "SickData";
            // create sql command
            bool isAnd = false;
            string baseQuery;
            string extQuery = "";
            if (page == 0)
            {
                baseQuery = "SELECT SickNum,SickName, Age, Telephone, Createtime, DataPath FROM SickData ";
            }else
            {
                baseQuery = "SELECT TOP " + PageSize + " SickNum,SickName, Age, Telephone, Createtime, DataPath FROM SickData ";
            }
            if (!string.IsNullOrEmpty(txtSicknum.Text.Trim()))
            {
                extQuery += " WHERE SickNum LIKE N'%" + txtSicknum.Text.Trim() + "%'";
                isAnd = true;
            }
            if (!string.IsNullOrEmpty(txtTenSearch.Text.Trim()))
            {
                if (isAnd == true)
                {
                    extQuery += " AND SickName LIKE N'%" + txtTenSearch.Text.Trim() + "%'";
                }
                else
                {
                    extQuery += " WHERE SickName LIKE N'%" + txtTenSearch.Text.Trim() + "%'";
                    isAnd = true;
                }   
            }
            if (!string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                if (isAnd == true)
                {
                    extQuery += " AND Telephone LIKE N'%" + txtPhone.Text.Trim() + "%'";
                }
                else
                {
                    extQuery += " WHERE Telephone LIKE N'%" + txtPhone.Text.Trim() + "%'";
                    isAnd = true;
                }
            }
            if (dtNgayBatDau.Checked == true)
            {
                if (isAnd == true)
                {
                    extQuery += " AND Createtime >= '" + dtNgayBatDau.Value.ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    extQuery += " WHERE Createtime >= '" + dtNgayBatDau.Value.ToString("yyyy-MM-dd") + "'";
                    isAnd = true;
                }
            }
            if (dtNgayKetThuc.Checked == true)
            {
                if (isAnd == true)
                    extQuery += " AND Createtime <= '" + dtNgayKetThuc.Value.ToString("yyyy-MM-dd") + "'";
                else
                {
                    extQuery += " WHERE Createtime <= '" + dtNgayKetThuc.Value.ToString("yyyy-MM-dd") + "'";
                    isAnd = true;
                }
            }
            if(page > 1)
            {
                int PreviouspageLimit = (page - 1) * PageSize;
                if (isAnd == true)
                    extQuery += " AND SickNum NOT IN " + "(SELECT TOP " + PreviouspageLimit + " SickNum FROM SickData " + extQuery + " ORDER BY SickNum) ";
                else
                {
                    extQuery += " WHERE SickNum NOT IN " + "(SELECT TOP " + PreviouspageLimit + " SickNum FROM SickData " + extQuery  + " ORDER BY SickNum) ";
                    isAnd = true;
                }
                    
            }
            baseQuery += extQuery;
            baseQuery += " ORDER BY SickNum DESC;";
            Console.WriteLine(baseQuery);
            try
            {
                sqlCmd = new SqlCommand(baseQuery, DBConnection.Instance.sqlConn);
                this.sqlDta.SelectCommand = sqlCmd;
                this.sqlDta.Fill(dt);
                if(page == 0)
                {
                    this.ds.Tables.Add(dt);
                }
            }
            catch (Exception)
            {
                //nothing todo
            }
            return dt;
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
            txtPhone.Text = string.Empty;
            dtNgayBatDau.Checked = false;
            dtNgayKetThuc.Checked = false;
        }

        private void loadFilterData()
        {
            ds = new DataSet();
            this.CurrentPageIndex = 1;
            this.CalculateTotalPages();
            // Load the first page of data;
            this.dgvPatient.DataSource = GetFilterRecord(1);
            formatDgv();
            int rowCount = ds.Tables["SickData"].Rows.Count;
            if (rowCount > 0)
                setFieldEnable(true);
            else
                setFieldEnable(false);
            //update number record to label
            gbResult.Text = "Danh sách bệnh nhân (" + rowCount + " kết quả)";
            this.lbCurrentPage.Text = this.CurrentPageIndex.ToString();
            this.lbTotalPage.Text = this.TotalPage.ToString();
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            //check start date & end date 
            if(dtNgayBatDau.Value.Date.CompareTo(dtNgayKetThuc.Value.Date) > 0)
            {
                MessageBox.Show("Điều kiện tìm kiếm không hợp lệ. \nNgày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                loadFilterData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Câu truy vấn không thành công. Vui lòng xem lại giá trị tìm kiếm đầu vào!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgPatient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //reset data
            pbPatient.Image = null;
            lbimgCount.Text = "0 / 0";
            if (e.RowIndex != -1)
            {
                string sickNum = dgvPatient.Rows[e.RowIndex].Cells["SickNum"].Value.ToString().Trim();
                string dataPath = dgvPatient.Rows[e.RowIndex].Cells["DataPath"].Value.ToString().Trim();
                Patient patient = new Patient().getPatientByNum(sickNum);
                if (Directory.Exists(dataPath))
                {
                    imagesPatient = Directory.GetFiles(dataPath, "*.jpg", SearchOption.AllDirectories);
                    if (imagesPatient.Length > 0)
                    {
                        using (FileStream stream = new FileStream(imagesPatient[0], FileMode.Open, FileAccess.Read))
                        {
                            pbPatient.Image = Image.FromStream(stream);
                            lbimgCount.Text = "1 / " + imagesPatient.Length;
                        }
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
            txtAge.Text = patient.AgeProperty.ToString();
            txtTelephone.Text = patient.TelephoneProperty;
            txtAddr.Text = patient.AddrProperty;
            txtCreatetime.Text = patient.CreateTimeProperty.ToShortDateString();
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
                    try
                    {
                        pbPatient.Image = Image.FromFile(imagesPatient[imgIndex]);
                        lbimgCount.Text = (imgIndex + 1).ToString() + " / " + imagesPatient.Length;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("File ảnh không còn tồn tại! Vui lòng kiểm tra lại thư mục lưu ảnh hoặc khởi động lại phần mềm để cập nhập.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.ToString());
                    }
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
                    try
                    {
                        pbPatient.Image = Image.FromFile(imagesPatient[imgIndex]);
                        lbimgCount.Text = (imgIndex + 1).ToString() + " / " + imagesPatient.Length;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("File ảnh không còn tồn tại! Vui lòng kiểm tra lại thư mục lưu ảnh hoặc khởi động lại phần mềm để cập nhập.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.ToString());
                    }
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
                string dataPath = dgvPatient.Rows[currRowIndexCheck].Cells["DataPath"].Value.ToString().Trim();
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
                // delete data grid view
                dgvPatient.Rows.RemoveAt(currRowIndexCheck);
                ds.Tables["SickData"].Rows.RemoveAt(currRowIndexCheck);
                ds.AcceptChanges();
                dgvPatient.Update();
                dgvPatient.Refresh();
                if (dgvPatient.RowCount == 0)
                {
                    btnOpenFolder.Enabled = false;
                }
                resetPatientField();
                //delete data in disk
                if (pbPatient.Image != null)
                    pbPatient.Image = null;
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                if (Directory.Exists(dataPath))
                {
                    try
                    {
                        Directory.Delete(dataPath, true);
                    }
                    catch (Exception)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
                //update number patient in label
                int rowCount = ds.Tables["SickData"].Rows.Count;
                gbResult.Text = "Danh sách bệnh nhân ( " + rowCount + " kết quả)";
                helper.setRowNumber(dgvPatient);
                if (dgvPatient.Rows.Count > 0)
                {
                    setFieldEnable(true);
                }
                else
                {
                    setFieldEnable(false);
                }
            }
        }

        private void resetPatientField()
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtCreatetime.Text = "";
            txtTelephone.Text = "";
            txtJob.Text = "";
            txtAddr.Text = "";
            txtNote.Text = "";
            pbPatient.Image = null;
        }

        private void btnReCheck_Click(object sender, EventArgs e)
        {
            int selectedRowCount =
            dgvPatient.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount <= 0) return;
            int rowIndex = dgvPatient.SelectedRows[selectedRowCount - 1].Index;
            string pId = dgvPatient.Rows[rowIndex].Cells["SickNum"].Value.ToString().Trim();
            Patient patient = new Patient().getPatientByNum(pId);
            string numId = String.Format("{0:D6}", helper.getIdCheck());
            string pName = patient.NameProperty;
            string pAge = patient.AgeProperty.ToString();
            string pCause = patient.CauseCheckProperty;
            string doctor = Session.Instance.UserName;
            string folderImgPath = dgvPatient.Rows[rowIndex].Cells["DataPath"].Value.ToString().Trim(); 
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
            ruleEnableDelete = helper.myValidateRoles(RolesList.DELETE_PATIENT);
            btnDeleteSick.Enabled = ruleEnableDelete;
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
            int currRowIndexCheck = dgvPatient.SelectedRows[selectedRowCount - 1].Index;
            string dataPath = dgvPatient.Rows[currRowIndexCheck].Cells["DataPath"].Value.ToString().Trim();
            //check folder is existed
            if (Directory.Exists(dataPath))
            {
                try
                {
                    Process.Start(dataPath);
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

        private void setFieldEnable(bool enableState)
        {
            btnReCheck.Enabled = enableState;
            btnDeleteSick.Enabled = enableState & ruleEnableDelete;
            btnOpenFolder.Enabled = enableState;
            btnNextImg.Enabled = enableState;
            btnPrevImg.Enabled = enableState;
        }

        private void CalculateTotalPages()
        {
            GetFilterRecord(0);
            int rowCount = ds.Tables["SickData"].Rows.Count;
            TotalPage = rowCount / PageSize;
            // if any row left after calculated pages, add one more page 
            if (rowCount % PageSize > 0)
                TotalPage += 1;
            Console.WriteLine("Total page is " + TotalPage);
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            this.CurrentPageIndex = 1;
            this.dgvPatient.DataSource = GetFilterRecord(this.CurrentPageIndex);
            formatDgv();
            this.lbCurrentPage.Text = this.CurrentPageIndex.ToString();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (this.CurrentPageIndex < this.TotalPage)
            {
                this.CurrentPageIndex++;
                this.dgvPatient.DataSource = GetFilterRecord(this.CurrentPageIndex);
                formatDgv();
                this.lbCurrentPage.Text = this.CurrentPageIndex.ToString();
            }
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (this.CurrentPageIndex > 1)
            {
                this.CurrentPageIndex--;
                this.dgvPatient.DataSource = GetFilterRecord(this.CurrentPageIndex);
                formatDgv();
                this.lbCurrentPage.Text = this.CurrentPageIndex.ToString();
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            this.CurrentPageIndex = TotalPage;
            this.dgvPatient.DataSource = GetFilterRecord(this.CurrentPageIndex);
            formatDgv();
            this.lbCurrentPage.Text = this.CurrentPageIndex.ToString();
        }
    }
}
