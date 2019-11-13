using PhanMemNoiSoi.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class gbCheckRecord : Form
    {
        Helper helper = new Helper();
        private SqlDataAdapter dta = new SqlDataAdapter();
        private BindingSource bindSource = new BindingSource();
        DataTable table = new DataTable();
        string patientId;

        TextBox adapTxtBox = new TextBox();
        GroupBox[] gbList = new GroupBox[] { };
        TextBox[] txtList = new TextBox[] { };
        string[] contentShowRp = null;
        object missing = Missing.Value;
        string imagePath = null;
        string checkId = null;
        string[] imagesPatient = { };
        List<string> listNoteImg = new List<string>();
        List<string> listNameImg = new List<string>();

        const int TWO_IMAGE_REPORT = 2;
        const int FOUR_IMAGE_REPORT = 4;
        Size img2Size;
        Size img3Size;
        Size img4Size;
        ReportWord rp;
        
        public gbCheckRecord(string patientId, string imagePath, string checkId)
        {
            InitializeComponent();
            //Init column for dgv
            DataGridViewColumn colSelect = new DataGridViewColumn();
            DataGridViewColumn colImage = new DataGridViewColumn();
            colSelect.HeaderText = "X";
            colSelect.Width = 30;
            colImage.HeaderText = "Ảnh";
            colImage.Width = 190;
            dgvMain.Columns.Add(colSelect);
            dgvMain.Columns.Add(colImage);

            loadNoteImg();
            this.patientId = patientId;
            this.imagePath = imagePath;
            this.checkId = checkId;
            // Load image from disk
            var files = Directory.GetFiles(imagePath, "*.jpg", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).CreationTime);
            imagesPatient = files.ToArray();
            //add data to datagidview
            int imgIndex = 0;
            for (int i = 0; i < imagesPatient.Length *2; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                if (i % 2 == 0)
                {
                    //insert combobox 
                    DataGridViewComboBoxCell cbCell = new DataGridViewComboBoxCell();
                    DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                    foreach (string it in listNoteImg)
                    {
                        cbCell.Items.Add(it);
                    }
                    row.Cells.Add(txtCell);
                    row.Cells.Add(cbCell);
                    int row_index = dgvMain.Rows.Add(row);
                    dgvMain.Rows[row_index].Cells[0].ReadOnly = true;
                }
                else
                {
                    //insert image
                    Image img = Image.FromFile(imagesPatient[imgIndex]);
                    listNameImg.Add(Path.GetFileName(imagesPatient[imgIndex]));
                    DataGridViewCheckBoxCell ckCell = new DataGridViewCheckBoxCell();
                    DataGridViewImageCell imgCell = new DataGridViewImageCell();
                    imgCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    row.Cells.Add(ckCell);
                    row.Cells.Add(imgCell);
                    imgCell.Value = img;
                    row.Height = 180;
                    int row_index = dgvMain.Rows.Add(row);
                    imgIndex++;
                }
            }
            lbNumImgChecked.Text = "0 ảnh đã chọn.";
            img2Size.Width = Settings.Default.img2Width;
            img2Size.Height = Settings.Default.img2Height;
            img3Size.Width = Settings.Default.img3Width;
            img3Size.Height = Settings.Default.img3Height;
            img4Size.Width = Settings.Default.img4Width;
            img4Size.Height = Settings.Default.img4Heigh;
            // Set KeyPreview object to true to allow the form to process 
            // the key before the control with focus processes it.
            this.KeyPreview = true;
        }

        private void Report_Load(object sender, EventArgs e)
        {
            //get number check record
            string query = "SELECT COUNT(*) FROM CheckItem WHERE IsDisplay = '1';";
            int numRecord = 0;
            SqlCommand mySQL = null;
            try
            {
                mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                numRecord = (int)mySQL.ExecuteScalar();
            }
            catch (Exception)
            {
            }

            if (numRecord <= 0)
            {
                return;
            }
            gbList = new GroupBox[numRecord];
            txtList = new TextBox[numRecord];
            contentShowRp = new string[numRecord];
            int index = 0;
            query = "SELECT ItemCode,Content FROM CheckItem WHERE IsDisplay = '1' ORDER BY ShowNum ASC;";
            try
            {
                mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                SqlDataReader rdr = mySQL.ExecuteReader();
                while (rdr.Read())
                {
                    gbList[index] = new System.Windows.Forms.GroupBox();
                    gbList[index].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F,
                        System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    if (index == 0)
                    {
                        gbList[index].Location = new System.Drawing.Point(8, 5 + index * (550 / numRecord));
                    }
                    else
                    {
                        gbList[index].Location = new System.Drawing.Point(8, 20 + index * (550 / numRecord));
                    }

                    gbList[index].Name = rdr["ItemCode"].ToString();
                    gbList[index].Size = new System.Drawing.Size(groupBox1.Width * 98 / 100, 550 / numRecord);
                    gbList[index].TabIndex = 1;
                    gbList[index].TabStop = false;
                    gbList[index].Text = rdr["Content"].ToString();
                    pnCheckRecord.Controls.Add(gbList[index]);

                    // 
                    // text box
                    // 
                    txtList[index] = new System.Windows.Forms.TextBox();
                    txtList[index].Dock = System.Windows.Forms.DockStyle.Fill;
                    txtList[index].Location = new System.Drawing.Point(3, 20);
                    txtList[index].Multiline = true;
                    txtList[index].Name = rdr["ItemCode"].ToString();
                    txtList[index].Size = new System.Drawing.Size(877, 40);
                    txtList[index].TabIndex = 0;
                    txtList[index].ReadOnly = true;
                    txtList[index].BackColor = System.Drawing.SystemColors.Window;
                    gbList[index].Controls.Add(txtList[index]);
                    txtList[index].ScrollBars = System.Windows.Forms.ScrollBars.Both;
                    txtList[index].MouseClick += new System.Windows.Forms.MouseEventHandler(textbox_MouseClick);
                    txtList[index].ForeColor = Color.Blue;

                    contentShowRp[index] = rdr["Content"].ToString();
                    //increase index for next round
                    index++;
                }
            }
            catch (Exception)
            {
            }

            if (txtList != null)
            {
                for (int i = 0; i < txtList.Length; i++)
                {
                    txtList[i].Text = loadTxtBoxData(txtList[i].Name.Trim(), this.patientId);
                }
            }
            lbNumImgChecked.Text = "0 ảnh đã chọn.";
        }

        private void loadNoteImg()
        {
            string query = "SELECT Content FROM NoteInfo ORDER BY ShowNum ASC;";
            try
            {
                SqlCommand mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                SqlDataReader rdr = mySQL.ExecuteReader();
                while (rdr.Read())
                {
                    listNoteImg.Add(rdr["Content"].ToString());
                }
            }
            catch (Exception)
            {
            }
        }

        private string loadTxtBoxData(string itemCode, string patientId)
        {
            string data = "";
            //Load data for precaution box
            string query = "SELECT ItemContentCode FROM CheckRecord WHERE ItemCode = "
                            + "@itemCode AND CheckNum = @patientId";
            try
            {
                SqlCommand mySQL = new SqlCommand(query, DBConnection.Instance.sqlConn);
                mySQL.Parameters.Add("@patientId", SqlDbType.NChar).Value = this.patientId;
                mySQL.Parameters.Add("@itemCode", SqlDbType.NChar).Value = itemCode;
                SqlDataReader rdr = mySQL.ExecuteReader();
                while (rdr.Read())
                {
                    string itemContentCode = rdr["ItemContentCode"].ToString().Trim();
                    query = "SELECT Content FROM CheckItemContent WHERE ContentCode = @itemContentCode;";
                    SqlCommand mySQLICC = new SqlCommand(query, DBConnection.Instance.sqlConn);
                    mySQLICC.Parameters.Add("@itemContentCode", SqlDbType.NChar).Value = itemContentCode;
                    SqlDataReader rdrICC = mySQLICC.ExecuteReader();
                    while (rdrICC.Read())
                    {
                        data += rdrICC["Content"].ToString().Trim() + " ,";
                    }
                }

            }
            catch (Exception)
            {
            }
            //remove end character
            if (data.EndsWith(","))
            {
                data = data.Remove(data.Length - 1);
            }
            return data;
        }

        private void textbox_MouseClick(object sender, MouseEventArgs e)
        {
            var txtbox = sender as TextBox;
            this.adapTxtBox = txtbox;
            showCheckItemForm(txtbox.Name.Trim(), txtbox.Text.Trim());
        }

        private void showCheckItemForm(string itemCode, string currentText)
        {
            CheckItem checkItemFr = new CheckItem(itemCode, patientId, currentText);
            checkItemFr.updateData += updateTextbox;
            checkItemFr.ShowDialog();
        }

        private void updateTextbox(string content)
        {
            this.adapTxtBox.Text = content;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            rp = new ReportWord();
            //check number image select
            List<MyImage> iList = new List<MyImage>();
            int index = 0;
            foreach(DataGridViewRow row in dgvMain.Rows)
            {
                DataGridViewCell cellChecked = (DataGridViewCell)row.Cells[0];
                if(row.Index %2 != 0)
                {
                    if (cellChecked.Value != null && (bool)cellChecked.Value == true)
                    {
                        MyImage myImg = new MyImage();
                        myImg.id = iList.Count;
                        myImg.imagePath = imagePath + listNameImg[index];
                        object noteInfo = dgvMain.Rows[row.Index - 1].Cells[1].Value;
                        if (noteInfo != null)
                            myImg.imageNote = noteInfo.ToString();
                        else
                            myImg.imageNote = "";
                        iList.Add(myImg);
                    }
                    index++;
                }
            }

            if (iList.Count == 0)
            {
                DialogResult result = MessageBox.Show("Bạn chưa chọn ảnh. Bạn có muốn tiếp tục xem báo cáo?", "Thông báo",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            rp.openTemplateFile();
            fillReport(rp, iList);
            string tmp = Log.Instance.GetTempPath() + "tmp.docx";
            rp.saveFile(tmp, true);
        }

        private void fillReport(ReportWord rp, List<MyImage> iList)
        {
            Patient patientInfo = new Patient().getPatientByNum(this.patientId);
            rp.insertText("NumId", this.checkId);
            rp.insertText("SickId", this.patientId);
            rp.insertText("Name", patientInfo.NameProperty);
            rp.insertText("Age", patientInfo.AgeProperty.ToString());
            rp.insertText("Address", patientInfo.AddrProperty);
            rp.insertText("Sex", patientInfo.SexProperty);
            rp.insertText("Telephone", patientInfo.TelephoneProperty);
            rp.insertText("Job", patientInfo.JobProperty);
            rp.insertText("Insurance", patientInfo.InsuranceIdProperty);
            rp.insertText("Cause", patientInfo.CauseCheckProperty);
            rp.insertText("Doctor", Session.Instance.UserName);
            rp.insertText("CreateTime", patientInfo.CreateTimeProperty.ToString(helper.getDateFormat(Settings.Default.datetimeFormat)));
            //validate text data
            if (txtList.Length > 0)
            {
                string[,] data = new string[txtList.Length, 2];
                for (int i = 0; i < txtList.Length; i++)
                {
                    string header = contentShowRp[i].Trim() + " : ";
                    data[i, 0] = header;
                    string content = txtList[i].Text.Trim();
                    if (content.EndsWith(","))
                    {
                        content = content.Remove(content.Length - 1);
                    }
                    data[i, 1] = content;
                }
                rp.createTable("table", data);
            }else
            {
                rp.searchReplace("<table>", "");
            }
            //validate image data
            if(iList.Count > 0)
            {
                rp.createImageTable("images", iList);
            }else
            {
                rp.searchReplace("<images>", "");
            }

            rp.insertText("Date", DateTime.Now.Day.ToString());
            rp.insertText("Month", DateTime.Now.Month.ToString());
            rp.insertText("Year", DateTime.Now.Year.ToString());
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gbCheckRecord_FormClosed(object sender, FormClosedEventArgs e)
        {
            //rp.release();
        }

        private int getIdCheck()
        {
            string sql = "SELECT  COUNT(*) FROM SickData";
            SqlCommand comd = new SqlCommand(sql, DBConnection.Instance.sqlConn);
            int count = Convert.ToInt32(comd.ExecuteScalar());
            return count;
        }
        private Image GetImage(string actfile)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] data = File.ReadAllBytes(actfile); // read the file, and release it
                ms.Write(data, 0, data.Length);
                return Image.FromStream(ms);
            }
        }

        private void dgvMain_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //no thing to do
        }

        private void dgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int count = 0;
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                if(e.RowIndex %2 != 0)
                {
                    DataGridViewRow row = dgvMain.Rows[e.RowIndex];
                    row.Cells[0].Value = !Convert.ToBoolean(row.Cells[0].EditedFormattedValue);

                    foreach (DataGridViewRow it in dgvMain.Rows)
                    {
                        object ckValue = dgvMain.Rows[it.Index].Cells[0].Value;
                        if (ckValue != null)
                        {
                            bool value = (bool)ckValue;
                            if (value == true)
                            {
                                count++;
                            }
                        }
                    }
                    lbNumImgChecked.Text = count + " ảnh đã chọn";
                }
            }
        }

        private void dgvMain_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int count = 0;
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex % 2 != 0)
                {
                    DataGridViewRow row = dgvMain.Rows[e.RowIndex];
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                    row.Selected = true;
                    if (chk.Value == null || (bool)chk.Value == false)
                        chk.Value = true;
                    else
                        chk.Value = false;
                    dgvMain.EndEdit();
                    dgvMain.RefreshEdit();

                    foreach (DataGridViewRow it in dgvMain.Rows)
                    {
                        object ckValue = dgvMain.Rows[it.Index].Cells[0].Value;
                        if (ckValue != null)
                        {
                            bool value = (bool)ckValue;
                            if (value == true)
                            {
                                count++;
                            }
                        }
                    }
                    lbNumImgChecked.Text = count + " ảnh đã chọn";
                }
            }
        }
    }
}
