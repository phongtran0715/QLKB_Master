using DirectX.Capture;
using PhanMemNoiSoi.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class CheckAndView : Form
    {
        #region Capture camera variable
        private Capture capture = null;
        private Filters filters = null;
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

        public CheckAndView(string checkId, string patientId, string pName, string pAge,
                             string cause, string doctor, string folderImgPath)
        {
            InitializeComponent();
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
            this.Size = new Size(Settings.Default.CheckViewSizeW, Settings.Default.CheckViewSizeH);
            this.KeyPreview = true;
        }

        public bool initCamera(PictureBox pBox)
        {
            bool isSucess = false;
            if (capture != null)
            {
                capture.Dispose();
            }
            //Init with config param loaded from DB
            try
            {
                //Load config from DB
                string videoDevice = Settings.Default.vDevice;
                string videoCompressor = Settings.Default.vCompress;
                filters = new Filters();
                if ((videoDevice != null) && (videoDevice != ""))
                {
                    capture = new Capture(new Filter(videoDevice));
                    //set video compressor
                    if ((videoCompressor != null) && (videoCompressor != ""))
                    {
                        try
                        {
                            capture.VideoCompressor = new Filter(videoCompressor);
                        }catch(Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            capture.VideoCompressor = null;
                        }
                    }
                    capture.PreviewWindowFrame = pBox;
                    isSucess = true;
                }else
                {
                    isSucess = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                isSucess = false;
            }

            if (isSucess == false)
            {
                //Auto reinit camera with default param
                try
                {
                    if(filters != null)
                    {
                        capture = new Capture(filters.VideoInputDevices[0]);
                        capture.PreviewWindow = pBox;
                        //Save new config param 
                        Settings.Default.vDevice = capture.VideoDevice.MonikerString;
                        if (capture.VideoCompressor != null)
                        {
                            Settings.Default.vCompress = capture.VideoCompressor.MonikerString;
                        }
                        Settings.Default.Save();
                        isSucess = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    isSucess = false;
                }
            }

            //init for capture frame
            if(isSucess)
            {
                try
                {
                    heFrame = new Capture.HeFrame(CaptureComplete);
                    capture.FrameEvent2 += heFrame;
                    capture.GrapImg();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return isSucess;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            try
            {
                if (capture != null)
                    capture.Dispose();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            Close();
        }

        private void CheckAndView_Load(object sender, EventArgs e)
        {
            pbRecordIcon.Visible = false;
            lbRecord.Visible = false;
            camStatus = initCamera(pbVideo);
            if(camStatus == false)
            {
                MessageBox.Show("Không kết nối được đến thiết bị thu video. Vui lòng kiểm tra lại kết nối camera và thông số cài đặt.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            //check base folder is exist
            string imgFolder = Properties.Settings.Default.imageFolder;
            if (!Directory.Exists(imgFolder))
            {
                try {
                    Directory.CreateDirectory(DEFAULT_IMG_FOLDER);
                    Properties.Settings.Default.imageFolder = DEFAULT_IMG_FOLDER;
                    Properties.Settings.Default.Save();
                }
                catch (Exception)
                {
                    MessageBox.Show("Thư mục lưu ảnh không tồn tại : " + DEFAULT_IMG_FOLDER + "\n Vui lòng cài đặt lại thư mục lưu ảnh mặc định!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }

            // Set the ImageSize property to a larger size 
            listImage.View = View.LargeIcon;
            imageList.ImageSize = new Size(200, 150);
            listImage.LargeImageList = imageList;
            listImage.Sorting = SortOrder.Descending;

            //load exist image from disk
            if (Directory.Exists(folderImgPath))
            {
                imagesPatient = Directory.GetFiles(folderImgPath, "*.jpg", SearchOption.AllDirectories);
                var files = Directory.GetFiles(folderImgPath, "*.jpg", SearchOption.AllDirectories).OrderByDescending(d => new FileInfo(d).CreationTime);
                imagesPatient = files.ToArray();
                for (int i = 0; i < imagesPatient.Length; i++)
                {
                    Image img = Image.FromFile(imagesPatient[i]);
                    imageList.Images.Add(img);
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = i;
                    item.Name = Path.GetFileName(imagesPatient[i]);
                    listImage.Items.Add(item);
                }
            }
            this.ActiveControl = btnChupHinh;
        }

        private void listImage_Click(object sender, MouseEventArgs e)
        {
            if (sender == null || e == null)
            {
                return;
            }
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
                    MessageBox.Show("Dừng hình thất bại. Vui lòng kiểm tra lại thông số cài đặt video!", "Thông báo", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    initCamera(pbVideo);
                    Console.WriteLine(ex.Message + "\n\n" + ex.ToString());
                }
            }
            else
            {
                if (capture == null)
                {
                    MessageBox.Show("Chưa kết nối thiết bị thu video. Vui lòng kiểm tra lại kết nối camera!", "Thông báo",
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
            try
            {
                if (string.Equals(Properties.Settings.Default.captureType, "capture_only"))
                {
                    captureFrame();
                }
                else
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
                        capture.GrapImg();
                        capture.PreviewWindowFrame = null;
                        pbVideo.Image = imgCapture;
                        captureFrame();
                        captureStatic = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void captureFrame()
        {
            try
            {
                capture.GrapImg();
                if (imgCapture != null)
                {
                    //add to imageList
                    imageList.Images.Add(imgCapture);
                    ListViewItem item = new ListViewItem();
                    item.ImageIndex = imageList.Images.Count - 1;

                    //save image to disk
                    if(Directory.Exists(folderImgPath))
                    {
                        int fCount = Directory.GetFiles(folderImgPath, "*", SearchOption.TopDirectoryOnly).Length;
                        string imageName = pId + "_" + System.Guid.NewGuid() + ".jpg";
                        item.Name = imageName;
                        myHelper.SaveImageCapture(imgCapture, folderImgPath + imageName);
                        listImage.Items.Insert(0, item);
                    }
                    else
                    {
                        MessageBox.Show("Thư mục lưu trữ ảnh cho bệnh nhân này không tồn tại.", 
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
            }
            else if (e.KeyCode == Keys.F11)
            {
                btnFullScreen_Click_1(null, null);
            }
            else
            {
                Console.WriteLine("Unknow key");
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
            if (recordingVideo)
            {
                //stop record video
                try
                {
                    capture.Stop();
                    btnSaveVideo.Text = "  Quay Video";
                    btnSaveVideo.Image = Properties.Resources.camera_24;
                    btnSaveVideo.ImageAlign = ContentAlignment.MiddleLeft;
                    recordingVideo = false;
                    pbRecordIcon.Visible = false;
                    lbRecord.Visible = false;
                    btnDungHinh.Enabled = true;
                    //reinit video
                    initCamera(pbVideo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n\n" + ex.ToString());
                    MessageBox.Show("Dừng quay video thất bại. Vui lòng kiểm tra lại thông số cài đặt video!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    initCamera(pbVideo);
                }
            }
            else
            {
                try
                {
                    if (!capture.Cued)
                        capture.Filename = folderImgPath + pId + "_" + DateTime.Now.Millisecond + ".mp4";
                    capture.Start();
                    btnSaveVideo.Text = "  Dừng Quay";
                    btnSaveVideo.Image = Properties.Resources.pause_24;
                    btnSaveVideo.ImageAlign = ContentAlignment.MiddleLeft;
                    recordingVideo = true;
                    pbRecordIcon.Visible = true;
                    lbRecord.Visible = true;
                    btnDungHinh.Enabled = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n\n" + ex.ToString());
                    MessageBox.Show("Quay video thất bại.Vui lòng kiểm tra lại thông số cài đặt video!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    initCamera(pbVideo);
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

        private void btnSetFormSize_Click(object sender, EventArgs e)
        {
            Settings.Default.CheckViewSizeH = this.Height;
            Settings.Default.CheckViewSizeW = this.Width;
            Settings.Default.Save();
            MessageBox.Show("Lưu thông tin kích thước khung hình thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.listImage_Click(null, null);
        }

        private void listImage_MouseClick(object sender, MouseEventArgs e)
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
                            //delete image on disk
                            string filePath = folderImgPath + eachItem.Name;
                            Console.WriteLine(filePath);
                            if (File.Exists(filePath))
                            {
                                try
                                {
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                    File.Delete(filePath);
                                    //delete from list view
                                    listImage.Items.Remove(eachItem);
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
    }
}