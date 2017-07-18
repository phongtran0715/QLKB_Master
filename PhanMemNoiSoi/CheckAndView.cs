using DirectX.Capture;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class CheckAndView : Form
    {
        #region Capture camera variable
        int vDeviceIndex;
        int vCompressIndex;
        int vSourceIndex;
        int vFrameSizeX;
        int vFrameSizeY;
        double vFrameRate;
        private Capture capture = null;
        private Filters filters;
        Capture.HeFrame heFrame = null;
        Image imgCapture = null;
        #endregion

        Helper myHelper;
        bool camStatus;
        string pId;
        string pName;
        string folderImgPath;
        string[] imagesPatient = { };
        string BASE_FOLDER;
        string DEFAULT_IMG_FOLDER = @"D:\QLKB";
        string checkId;

        public CheckAndView()
        {
            InitializeComponent();
        }

        public CheckAndView(string checkId, string patientId, string pName, string pAge,
                             string cause, string doctor, string folderImgPath)
        {
            InitializeComponent();

            #region reset field on GUI
            lbPhieu.Text = "";
            lbName.Text = "";
            lbAge.Text = "";
            lbCause.Text = "";
            lbdoc.Text = "";
            #endregion
            myHelper = new Helper();
            this.pId = patientId;
            this.pName = pName;
            this.checkId = checkId;
            this.folderImgPath = folderImgPath;
            #region set value for field on GUI
            lbPhieu.Text = checkId;
            lbName.Text = pName;
            lbAge.Text = pAge;
            lbCause.Text = cause;
            lbdoc.Text = Session.Instance.UserName;
            lbDate.Text = DateTime.Today.ToShortDateString();
            #endregion
        }

        private bool initCamera()
        {
            bool exitCode = false;
            #region get value from configuration file 
            vDeviceIndex = Properties.Settings.Default.vDeviceIndex;
            vFrameSizeX = Properties.Settings.Default.vFrameSizeX;
            vFrameSizeY = Properties.Settings.Default.vFrameSizeY;
            vFrameRate = Properties.Settings.Default.vFrameRate;
            vCompressIndex = Properties.Settings.Default.vCompressIndex;
            vSourceIndex = Properties.Settings.Default.vSourceIndex;
            #endregion
            try
            {
                // Get current devices and dispose of capture object
                // because the video and audio device can only be changed
                // by creating a new Capture object.
                Filter videoDevice = null;
                if (capture != null)
                {
                    videoDevice = capture.VideoDevice;
                    capture.Dispose();
                    capture = null;
                }

                // Get new video device
                if (vDeviceIndex < filters.VideoInputDevices.Count && vDeviceIndex >= 0)
                {
                    videoDevice = filters.VideoInputDevices[vDeviceIndex];
                }
                else
                {
                    videoDevice = filters.VideoInputDevices[0];
                }

                // Create capture object
                if ((videoDevice != null))
                {
                    capture = new Capture(videoDevice);
                    capture.FrameRate = vFrameRate;
                    Size size = new Size(vFrameSizeX, vFrameSizeY);
                    capture.FrameSize = size;
                    capture.PreviewWindowFrame = pbVideo;
                    exitCode = true;
                }
                heFrame = new Capture.HeFrame(CaptureComplete);
                capture.FrameEvent2 += heFrame;
                capture.GrapImg();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thiết bị thu video không được hỗ trợ. Vui lòng kiểm tra lại camera!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
            }
            return exitCode;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (capture != null)
                capture.Dispose();
            Close();
        }

        private void CheckAndView_Load(object sender, EventArgs e)
        {
            try
            {
                filters = new Filters();
                initCamera();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được đến thiết bị thu video. Vui lòng kiểm tra lại camera.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                this.Close();
            }

            //check base folder is exist
            if (!Directory.Exists(BASE_FOLDER))
            {
                BASE_FOLDER = DEFAULT_IMG_FOLDER;
                System.IO.Directory.CreateDirectory(BASE_FOLDER);
                Properties.Settings.Default.imageFolder = BASE_FOLDER;
            }
            else
            {
                BASE_FOLDER = Properties.Settings.Default.imageFolder;
            }

            this.KeyPreview = true;
            //Init Camera
            camStatus = true;

            // Set the ImageSize property to a larger size 
            listImage.View = View.LargeIcon;
            imageList1.ImageSize = new Size(200, 150);
            listImage.LargeImageList = imageList1;

            //load exist image from disk
            if (Directory.Exists(folderImgPath))
            {
                imagesPatient = Directory.GetFiles(folderImgPath, "*.jpg", SearchOption.AllDirectories);
                for (int i = 0; i < imagesPatient.Length; i++)
                {
                    Image img = Image.FromFile(imagesPatient[i]);
                    imageList1.Images.Add(img);
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = imageList1.Images.Count - 1;
                    item.Name = Path.GetFileName(imagesPatient[i]);
                    listImage.Items.Add(item);
                }
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listImage.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    DialogResult dlgResult = MessageBox.Show("Bạn có muốn xóa hình ảnh này?", "Thông báo",
                                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dlgResult == DialogResult.OK)
                    {
                        int index = listImage.FocusedItem.Index;

                        //delete image on disk
                        string filePath = folderImgPath + listImage.Items[index].Name;
                        Console.WriteLine(filePath);
                        if (File.Exists(filePath))
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            File.Delete(filePath);
                        }

                        //delete from list view
                        listImage.Items.RemoveAt(index);
                    }
                }
            }
        }

        private void btnDungHinh_Click(object sender, EventArgs e)
        {
            if (camStatus)
            {
                try
                {
                    if (capture == null)
                    {
                        MessageBox.Show("Chưa kết nối thiết bị thu video. Vui lòng kiểm tra lại camera.", "Thông báo",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    heFrame = new Capture.HeFrame(CaptureComplete);
                    capture.FrameEvent2 += heFrame;
                    capture.GrapImg();

                    capture.PreviewWindowFrame = null;
                    pbVideo.Image = imgCapture;
                    string path = Environment.CurrentDirectory + @"\continue_24.png";
                    Image image = Image.FromFile(path);
                    btnDungHinh.Text = "Tiếp tục";
                    btnDungHinh.Image = Image.FromFile(path);
                    btnDungHinh.ImageAlign = ContentAlignment.MiddleLeft;
                    camStatus = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.ToString());
                }
            }
            else
            {
                if (capture == null)
                {
                    MessageBox.Show("Chưa kết nối thiết bị thu video. Vui lòng kiểm tra lại camera.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                initCamera();
                camStatus = true;
                string path = Environment.CurrentDirectory + @"\pause_24.png";
                Image image = Image.FromFile(path);
                btnDungHinh.Text = "Dừng hình";
                btnDungHinh.Image = Image.FromFile(path);
                btnDungHinh.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void btnChupHinh_Click(object sender, EventArgs e)
        {
            try
            {
                heFrame = new Capture.HeFrame(CaptureComplete);
                capture.FrameEvent2 += heFrame;
                capture.GrapImg();
                if (imgCapture != null)
                {
                    //add to imageList
                    imageList1.Images.Add(imgCapture);
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = imageList1.Images.Count - 1;

                    //save image to disk
                    int fCount = Directory.GetFiles(folderImgPath, "*", SearchOption.TopDirectoryOnly).Length;
                    string imageName = pId + "_" + System.Guid.NewGuid() + ".jpg";
                    myHelper.SaveImageCapture(imgCapture, folderImgPath + imageName);

                    item.Name = imageName;
                    listImage.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.ToString());
            }
        }

        private void CaptureComplete(System.Drawing.Bitmap e)
        {
            imgCapture = (Image)e;
            capture.FrameEvent2 -= heFrame;
        }

        private void CheckAndView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (capture != null)
                capture.Dispose();
        }

        private void CheckAndView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)       // F1 press
            {
                e.SuppressKeyPress = true;  // Stops other controls on the form receiving event.
                heFrame = new Capture.HeFrame(CaptureComplete);
                capture.FrameEvent2 += heFrame;
                capture.GrapImg();

                if (imgCapture != null)
                {
                    //add to imageList
                    imageList1.Images.Add(imgCapture);
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = imageList1.Images.Count - 1;

                    //save image to disk
                    int fCount = Directory.GetFiles(folderImgPath, "*", SearchOption.TopDirectoryOnly).Length;
                    string imageName = pId + "_" + System.Guid.NewGuid() + ".jpg";
                    myHelper.SaveImageCapture(imgCapture, folderImgPath + imageName);

                    item.Name = imageName;
                    listImage.Items.Add(item);
                }
            }
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            this.Hide();
            gbCheckRecord rpFrm = new gbCheckRecord(pId, folderImgPath, this.checkId);
            rpFrm.ShowDialog();
            this.Close();
        }

        private void listImage_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //if (e.Item.Selected == Convert.ToBoolean(CheckState.Unchecked))
            //{
            //    MessageBox.Show("checked on " + e.Item.Name);
            //}
            //else if (e.Item.Selected == Convert.ToBoolean(CheckState.Checked))
            //{
            //    MessageBox.Show("unchecked on " + e.Item.Name);
            //}
        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            //int  maxx, maxy;
            //maxx = maxy = int.MinValue;

            //foreach (Screen screen in Screen.AllScreens)
            //{
            //    var bounds = screen.Bounds;
            //    maxx = Math.Max(maxx, bounds.Right);
            //    maxy = Math.Max(maxy, bounds.Bottom);
            //}

            //Rectangle tempRect = new Rectangle(1, 0, maxx, maxy);
            //this.DesktopBounds = tempRect;
            MessageBox.Show("Chức năng đang được hoàn thiện. Vui lòng quay lại sau!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void caifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (capture != null)
                capture.Dispose();
            CaptureTest capTestFr = new CaptureTest();
            capTestFr.ShowDialog();
            this.Close();
        }
    }
}
