using PhanMemNoiSoi.Properties;
using System;
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
        GroupBox[] gbList = null;
        TextBox[] txtList = null;
        string[] contentShowRp = null;
        object missing = Missing.Value;
        string imagePath = null;
        string checkId = null;
        int numImgChecked = 0;
        string[] imagesPatient = { };

        const int TWO_IMAGE_REPORT = 2;
        const int FOUR_IMAGE_REPORT = 4;
        Size img2Size;
        Size img4Size;
        ReportWord rp;

        public gbCheckRecord(string patientId, string imagePath, string checkId)
        {
            InitializeComponent();
            this.patientId = patientId;
            this.imagePath = imagePath;
            this.checkId = checkId;
            // Load image from disk
            //imagesPatient = Directory.GetFiles(imagePath, "*.jpg", SearchOption.AllDirectories);
            var files = Directory.GetFiles(imagePath, "*.jpg", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).CreationTime);
            imagesPatient = files.ToArray();
            //add to imageList
            listImage.View = View.LargeIcon;
            imageList1.ImageSize = new Size(195, 150);
            listImage.Sorting = System.Windows.Forms.SortOrder.Descending;
            listImage.LargeImageList = imageList1;

            for (int i = 0; i < imagesPatient.Length; i++)
            {
                Image img = Image.FromFile(imagesPatient[i]);
                imageList1.Images.Add(img);
                ListViewItem item = new ListViewItem();
                item.ImageIndex = imageList1.Images.Count - 1;
                item.Name = Path.GetFileName(imagesPatient[i]);
                listImage.Items.Add(item);
            }
            lbNumImgChecked.Text = "0 ảnh đã chọn.";
            img2Size.Width = Settings.Default.img2Width;
            img2Size.Height = Settings.Default.img2Height;
            img4Size.Width = Settings.Default.img4Width;
            img4Size.Height = Settings.Default.img4Heigh;
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
            }catch(Exception ex)
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
                    gbList[index].Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    gbList[index].Location = new System.Drawing.Point(8, 23 + index * 80);
                    gbList[index].Name = rdr["ItemCode"].ToString();
                    gbList[index].Size = new System.Drawing.Size(groupBox1.Width * 98 / 100, 76);
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

                    contentShowRp[index] = rdr["Content"].ToString();
                    //increase index for next round
                    index++;
                }
            }
            catch(Exception ex)
            {
            }
            
            if(txtList != null)
            {
                for (int i = 0; i < txtList.Length; i++)
                {
                    txtList[i].Text = loadTxtBoxData(txtList[i].Name.Trim(), this.patientId);
                }
            }

            numImgChecked = 0;
            lbNumImgChecked.Text = "0 ảnh đã chọn.";
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
            catch(Exception ex)
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
            showCheckItemForm(txtbox.Name.Trim());
        }

        private void showCheckItemForm(string itemCode)
        {
            CheckItem checkItemFr = new CheckItem(itemCode, patientId);
            checkItemFr.updateData += updateTextbox;
            checkItemFr.ShowDialog();
        }

        private void updateTextbox(string content)
        {
            this.adapTxtBox.Text = content;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(Session.Instance.ActiveLicense == false)
            {
                using (CountDownForm formOptions = new CountDownForm())
                {
                    formOptions.ShowDialog();

                    bool result = formOptions.GetMyResult();
                    if(result == false)
                    {
                        return;
                    }
                }
            }
            rp = new ReportWord();
            rp.openFile();
            //check number image select
            int numImg = 0;
            for (int i = 0; i < listImage.Items.Count; i++)
            {
                if (listImage.Items[i].Checked)
                {
                    numImg++;
                }
            }
            if (numImg == 0)
            {
                DialogResult result = MessageBox.Show("Bạn chưa chọn ảnh. Bạn có muốn tiếp tục xem báo cáo?", "Thông báo",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            fillReport(rp, numImg);
            string tmp = Log.Instance.GetTempPath() + "tmp.docx";
            rp.saveFile(tmp, true);           
        }

        private void fillReport(ReportWord rp, int rpMode)
        {
            Patient patientInfo = new Patient().getPatientByNum(this.patientId);
            rp.insertText("NumId", this.checkId);
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

            string result = "";
            if(txtList != null)
            {
                for (int i = 0; i < txtList.Length; i++)
                {
                    result += contentShowRp[i].Trim() + " : ";
                    result += txtList[i].Text.Trim();
                    if (result.Trim().EndsWith(","))
                    {
                        result = result.Remove(result.Length - 1);
                    }
                    result += "\n";
                }
            }
            
            //insert image 
            int numImgChecked = 0;
            if(listImage != null)
            {
                int imgCount = listImage.Items.Count;
                for (int i = 0; i < imgCount; i++)
                {
                    if (listImage.Items[i].Checked)
                    {
                        numImgChecked++;
                        if (numImgChecked <= rpMode)
                        {
                            if(imgCount <= 2)
                            {
                                rp.insertImage(imagePath + listImage.Items[i].Name, numImgChecked, img2Size);
                            }
                            else
                            {
                                rp.insertImage(imagePath + listImage.Items[i].Name, numImgChecked, img4Size);
                            }
                        }
                    }
                }
            }
            rp.insertText("ThongTin", result);
            rp.insertText("Date", DateTime.Now.Day.ToString());
            rp.insertText("Month", DateTime.Now.Month.ToString());
            rp.insertText("Year", DateTime.Now.Year.ToString());
        }

        private void listImage_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (listImage.Items[e.Item.Index].Checked)
            {
                if (numImgChecked <= listImage.Items.Count - 1) numImgChecked++;
            }
            else
            {
                if (numImgChecked > 0) numImgChecked--;
            }
            lbNumImgChecked.Text = numImgChecked.ToString() + " ảnh đã chọn.";
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
    }
}
