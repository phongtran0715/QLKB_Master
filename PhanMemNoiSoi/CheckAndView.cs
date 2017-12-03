using DirectX.Capture;
using System;
using System.ComponentModel;
using System.Diagnostics;
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
        bool captureStatic = false;
        bool recordingVideo = false;
        string pId;
        string pName;
        string folderImgPath;
        string[] imagesPatient = { };
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

        public bool initCamera(PictureBox pbox)
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
                    int vCompressIndex = Properties.Settings.Default.vCompressIndex;
                    capture.VideoCompressor = (vCompressIndex > 0 ? filters.VideoCompressors[vCompressIndex - 1] : null);
                    capture.FrameRate = vFrameRate;
                    Size size = new Size(vFrameSizeX, vFrameSizeY);
                    capture.FrameSize = size;
                    capture.PreviewWindowFrame = pbox;
                   
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
            pbRecordIcon.Visible = false;
            lbRecord.Visible = false;
            try
            {
                filters = new Filters();
                initCamera(pbVideo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không kết nối được đến thiết bị thu video. Vui lòng kiểm tra lại camera.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.ToString());
                this.Close();
            }

            //check base folder is exist
            string imgFolder = Properties.Settings.Default.imageFolder;
            if (!Directory.Exists(imgFolder))
            {
                System.IO.Directory.CreateDirectory(DEFAULT_IMG_FOLDER);
                Properties.Settings.Default.imageFolder = DEFAULT_IMG_FOLDER;
                Properties.Settings.Default.Save();
            }

            this.KeyPreview = true;
            //Init Camera
            camStatus = true;

            // Set the ImageSize property to a larger size 
            listImage.View = View.LargeIcon;
            imageList1.ImageSize = new Size(200, 150);
            listImage.LargeImageList = imageList1;
            listImage.Sorting = SortOrder.Descending;

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
                    DialogResult dlgResult = MessageBox.Show("Bạn có muốn xóa hình ảnh đã chọn?", "Thông báo",
                                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dlgResult == DialogResult.OK)
                    {
                        foreach (ListViewItem eachItem in listImage.SelectedItems)
                        {
                            int index = eachItem.Index;
                            //delete image on disk
                            string filePath = folderImgPath + listImage.Items[index].Name;
                            Console.WriteLine(filePath);
                            if (File.Exists(filePath))
                            {
                                try
                                {
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                    File.Delete(filePath);
                                    //delete from list view
                                    listImage.Items.RemoveAt(index);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Không thể xóa ảnh. Vui lòng thử lại sau! \n" + ex.Message, "Thông báo",
                                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
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
                    btnDungHinh.Text = "Tiếp tục";
                    btnDungHinh.Image = Properties.Resources.continue_24;
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
                initCamera(pbVideo);
                camStatus = true;
                btnDungHinh.Text = "Dừng hình";
                btnDungHinh.Image = Properties.Resources.pause_24;
                btnDungHinh.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void btnChupHinh_Click(object sender, EventArgs e)
        {
            string ss = Properties.Settings.Default.captureType;
            if (string.Equals(Properties.Settings.Default.captureType, "capture_only"))
            {
                captureFrame();
            }else
            {
                if (captureStatic)
                {
                    //int camera again
                    initCamera(pbVideo);
                    captureStatic = false;
                }
                else
                {
                    // pause camera
                    heFrame = new Capture.HeFrame(CaptureComplete);
                    capture.FrameEvent2 += heFrame;
                    capture.GrapImg();
                    capture.PreviewWindowFrame = null;
                    pbVideo.Image = imgCapture;
                    captureFrame();
                    captureStatic = true;
                }
            }
        }

        private void captureFrame()
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
                    string imageName = pId + "_" + System.Guid.NewGuid() + ".jpg";
                    item.Name = imageName;
                    item.Text = DateTime.Now.ToLongTimeString();
                    listImage.Items.Add(item);

                    //save image to disk
                    myHelper.SaveImageCapture(imgCapture, folderImgPath + imageName);
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
                btnChupHinh_Click(null, null);
            }else if(e.KeyCode == Keys.F11)
            {
                btnFullScreen_Click_1(null, null);
            }
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            this.Hide();
            gbCheckRecord rpFrm = new gbCheckRecord(pId, folderImgPath, this.checkId);
            rpFrm.ShowDialog();
            this.Close();
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

        private void btnSaveVideo_Click(object sender, EventArgs e)
        {
            //TODO : set video compress
            if (recordingVideo)
            {
                //stop record video
                try
                {
                    if (capture == null)
                        throw new ApplicationException("Please select a video and/or audio device.");
                    capture.Stop();
                    //initCamera();
                    btnSaveVideo.Text = "  Quay Video";
                    btnSaveVideo.Image = Properties.Resources.camera_24;
                    btnSaveVideo.ImageAlign = ContentAlignment.MiddleLeft;
                    recordingVideo = false;
                    pbRecordIcon.Visible = false;
                    lbRecord.Visible = false;
                    btnDungHinh.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.ToString());
                }
            }else
            {
                try
                {
                    // record video
                    if (capture == null)
                        throw new ApplicationException("Please select a video and/or audio device.");
                    if (!capture.Cued)
                        capture.Filename = folderImgPath + pId + "_" + DateTime.Now.Millisecond + ".mp4";
                    btnSaveVideo.Text = "  Dừng Quay";
                    //btnSaveVideo.Image = Properties.Resources.pause_24;
                    btnSaveVideo.ImageAlign = ContentAlignment.MiddleLeft;
                    recordingVideo = true;
                    pbRecordIcon.Visible = true;
                    lbRecord.Visible = true;
                    btnDungHinh.Enabled = false;
                    capture.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.ToString());
                }
            }
        }

        private void itemOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(folderImgPath);
            }
            catch (Win32Exception win32Exception)
            {
                //The system cannot find the file specified...
                Console.WriteLine(win32Exception.Message);
            }
        }

        private void btnFullScreen_Click_1(object sender, EventArgs e)
        {
            FullScreen fsr = new FullScreen(this);
            fsr.captureImg += fullScreenCallBack;
            fsr.ShowDialog();
            initCamera(pbVideo);
        }

        private void fullScreenCallBack()
        {
            btnChupHinh_Click(null, null);
        }
    }
}
