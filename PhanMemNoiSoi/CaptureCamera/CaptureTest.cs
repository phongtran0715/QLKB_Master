// ------------------------------------------------------------------
// CaptureTest.cs
// Sample application to show the DirectX.Capture class library.
//
// History:
//	2003-Jan-25		BL		- created
//
// Copyright (c) 2003 Brian Low
// ------------------------------------------------------------------

using DirectX.Capture;
using PhanMemNoiSoi.CaptureCamera;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public class CaptureTest : Form
    {
        CaptureWrapper capWrapper;
        private Capture capture = null;
        private Filters filters = new Filters();

        #region variable auto generate
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem mnuDevices;
        private System.Windows.Forms.MenuItem mnuVideoDevices;
        private System.Windows.Forms.MenuItem mnuVideoCompressors;
        private System.Windows.Forms.MenuItem mnuVideoSources;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem mnuFrameSizes;
        private System.Windows.Forms.MenuItem mnuFrameRates;
        private System.Windows.Forms.MenuItem mnuPropertyPages;
        private System.Windows.Forms.MenuItem mnuVideoCaps;
        private MenuItem menuItem4;
        private PictureBox panelVideo;
        private IContainer components;
        #endregion

        public CaptureTest()
        {
            InitializeComponent();
            capWrapper = new CaptureWrapper();
            initCamera();
            // Update the main menu
            // Much of the interesting work of this sample occurs here
            try { updateMenu(); } catch { }
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptureTest));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuDevices = new System.Windows.Forms.MenuItem();
            this.mnuVideoDevices = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuVideoCompressors = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mnuVideoSources = new System.Windows.Forms.MenuItem();
            this.mnuFrameSizes = new System.Windows.Forms.MenuItem();
            this.mnuFrameRates = new System.Windows.Forms.MenuItem();
            this.mnuVideoCaps = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mnuPropertyPages = new System.Windows.Forms.MenuItem();
            this.panelVideo = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.panelVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.mnuDevices,
            this.menuItem7});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuExit});
            this.menuItem1.Text = "File";
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 0;
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuDevices
            // 
            this.mnuDevices.Index = 1;
            this.mnuDevices.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuVideoDevices,
            this.menuItem4,
            this.mnuVideoCompressors});
            this.mnuDevices.Text = "Thiết bị";
            // 
            // mnuVideoDevices
            // 
            this.mnuVideoDevices.Index = 0;
            this.mnuVideoDevices.Text = "Danh sách video";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "-";
            // 
            // mnuVideoCompressors
            // 
            this.mnuVideoCompressors.Index = 2;
            this.mnuVideoCompressors.Text = "Định dạng nén video";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 2;
            this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuVideoSources,
            this.mnuFrameSizes,
            this.mnuFrameRates,
            this.mnuVideoCaps,
            this.menuItem5,
            this.mnuPropertyPages});
            this.menuItem7.Text = "Tùy chọn";
            // 
            // mnuVideoSources
            // 
            this.mnuVideoSources.Index = 0;
            this.mnuVideoSources.Text = "Chọn video đầu vào";
            // 
            // mnuFrameSizes
            // 
            this.mnuFrameSizes.Index = 1;
            this.mnuFrameSizes.Text = "Kích thước khung hình";
            // 
            // mnuFrameRates
            // 
            this.mnuFrameRates.Index = 2;
            this.mnuFrameRates.Text = "Tốc độ khung hình";
            this.mnuFrameRates.Click += new System.EventHandler(this.mnuFrameRates_Click);
            // 
            // mnuVideoCaps
            // 
            this.mnuVideoCaps.Index = 3;
            this.mnuVideoCaps.Text = "Thông tin video camera";
            this.mnuVideoCaps.Click += new System.EventHandler(this.mnuVideoCaps_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 4;
            this.menuItem5.Text = "-";
            // 
            // mnuPropertyPages
            // 
            this.mnuPropertyPages.Index = 5;
            this.mnuPropertyPages.Text = "Cài đặt chi tiết";
            // 
            // panelVideo
            // 
            this.panelVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelVideo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelVideo.Location = new System.Drawing.Point(12, 12);
            this.panelVideo.Name = "panelVideo";
            this.panelVideo.Size = new System.Drawing.Size(678, 484);
            this.panelVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panelVideo.TabIndex = 6;
            this.panelVideo.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Image = global::PhanMemNoiSoi.Properties.Resources.delete_24x24;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(592, 505);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(98, 35);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "      Thoát";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::PhanMemNoiSoi.Properties.Resources.save_24;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(456, 505);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 35);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "     Lưu và thoát";
            this.btnSave.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // CaptureTest
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.BackColor = System.Drawing.Color.SeaShell;
            this.ClientSize = new System.Drawing.Size(698, 550);
            this.Controls.Add(this.panelVideo);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "CaptureTest";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cấu hình camera";
            ((System.ComponentModel.ISupportInitialize)(this.panelVideo)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            if (capture != null)
            {
                try
                {
                    capture.Dispose();
                }
                catch (Exception)
                {

                }
            }

            this.Close();
        }

        private void btnCue_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (capture == null)
                    throw new ApplicationException("Please select a video and/or audio device.");
                capture.Cue();
                //MessageBox.Show("Ready to capture.\n\nUse Cue() before Start() to " +
                //    "do all the preparation work that needs to be done to start a " +
                //    "capture. Now, when you click Start the capture will begin faster " +
                //    "than if you had just clicked Start. Using Cue() is completely " +
                //    "optional. The downside to using Cue() is the preview is disabled until " +
                //    "the capture begins.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.ToString());
            }
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (capture == null)
                    throw new ApplicationException("Please select a video and/or audio device.");
                capture.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.ToString());
            }
        }

        private void btnStop_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (capture != null)
                {
                    //Save information
                    Properties.Settings.Default.vDeviceIndex = capWrapper.vDeviceIndex;
                    Properties.Settings.Default.vCompressIndex = capWrapper.vCompressIndex;
                    Properties.Settings.Default.vSourceIndex = capWrapper.vSourceIndex;
                    Properties.Settings.Default.vFrameSizeX = capWrapper.vFrameSizeX;
                    Properties.Settings.Default.vFrameSizeY = capWrapper.vFrameSizeY;
                    Properties.Settings.Default.vFrameRate = capWrapper.vFrameRate;
                    Properties.Settings.Default.Save();
                    capture.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.ToString());
            }
            this.Close();
        }

        private void updateMenu()
        {
            MenuItem m;
            Filter f;
            Source s;
            Source current;
            PropertyPage p;
            Control oldPreviewWindow = null;

            // Disable preview to avoid additional flashes (optional)
            if (capture != null)
            {
                oldPreviewWindow = capture.PreviewWindow;
                capture.PreviewWindow = null;
            }

            // Load video devices
            Filter videoDevice = null;
            if (capture != null)
                videoDevice = capture.VideoDevice;
            mnuVideoDevices.MenuItems.Clear();
            for (int c = 0; c < filters.VideoInputDevices.Count; c++)
            {
                f = filters.VideoInputDevices[c];
                m = new MenuItem(f.Name, new EventHandler(mnuVideoDevices_Click));
                m.Checked = (videoDevice == f);
                mnuVideoDevices.MenuItems.Add(m);
            }
            mnuVideoDevices.Enabled = (filters.VideoInputDevices.Count > 0);

            // Load video compressors
            try
            {
                mnuVideoCompressors.MenuItems.Clear();
                m = new MenuItem("(None)", new EventHandler(mnuVideoCompressors_Click));
                m.Checked = (capture.VideoCompressor == null);
                mnuVideoCompressors.MenuItems.Add(m);
                for (int c = 0; c < filters.VideoCompressors.Count; c++)
                {
                    f = filters.VideoCompressors[c];
                    m = new MenuItem(f.Name, new EventHandler(mnuVideoCompressors_Click));
                    m.Checked = (capture.VideoCompressor == f);
                    mnuVideoCompressors.MenuItems.Add(m);
                }
                mnuVideoCompressors.Enabled = ((capture.VideoDevice != null) && (filters.VideoCompressors.Count > 0));
            }
            catch { mnuVideoCompressors.Enabled = false; }

            // Load video sources
            try
            {
                mnuVideoSources.MenuItems.Clear();
                current = capture.VideoSource;
                for (int c = 0; c < capture.VideoSources.Count; c++)
                {
                    s = capture.VideoSources[c];
                    m = new MenuItem(s.Name, new EventHandler(mnuVideoSources_Click));
                    m.Checked = (current == s);
                    mnuVideoSources.MenuItems.Add(m);
                }
                mnuVideoSources.Enabled = (capture.VideoSources.Count > 0);
            }
            catch { mnuVideoSources.Enabled = false; }

            // Load frame rates
            try
            {
                mnuFrameRates.MenuItems.Clear();
                int frameRate = (int)(capture.FrameRate * 1000);
                m = new MenuItem("15 fps", new EventHandler(mnuFrameRates_Click));
                m.Checked = (frameRate == 15000);
                mnuFrameRates.MenuItems.Add(m);
                m = new MenuItem("24 fps (Film)", new EventHandler(mnuFrameRates_Click));
                m.Checked = (frameRate == 24000);
                mnuFrameRates.MenuItems.Add(m);
                m = new MenuItem("25 fps (PAL)", new EventHandler(mnuFrameRates_Click));
                m.Checked = (frameRate == 25000);
                mnuFrameRates.MenuItems.Add(m);
                m = new MenuItem("29.997 fps (NTSC)", new EventHandler(mnuFrameRates_Click));
                m.Checked = (frameRate == 29997);
                mnuFrameRates.MenuItems.Add(m);
                m = new MenuItem("30 fps (~NTSC)", new EventHandler(mnuFrameRates_Click));
                m.Checked = (frameRate == 30000);
                mnuFrameRates.MenuItems.Add(m);
                m = new MenuItem("59.994 fps (2xNTSC)", new EventHandler(mnuFrameRates_Click));
                m.Checked = (frameRate == 59994);
                mnuFrameRates.MenuItems.Add(m);
                mnuFrameRates.Enabled = true;
            }
            catch { mnuFrameRates.Enabled = false; }

            // Load frame sizes
            try
            {
                mnuFrameSizes.MenuItems.Clear();
                Size frameSize = capture.FrameSize;
                m = new MenuItem("160 x 120", new EventHandler(mnuFrameSizes_Click));
                m.Checked = (frameSize == new Size(160, 120));
                mnuFrameSizes.MenuItems.Add(m);
                m = new MenuItem("320 x 240", new EventHandler(mnuFrameSizes_Click));
                m.Checked = (frameSize == new Size(320, 240));
                mnuFrameSizes.MenuItems.Add(m);
                m = new MenuItem("640 x 480", new EventHandler(mnuFrameSizes_Click));
                m.Checked = (frameSize == new Size(640, 480));
                mnuFrameSizes.MenuItems.Add(m);
                m = new MenuItem("1024 x 768", new EventHandler(mnuFrameSizes_Click));
                m.Checked = (frameSize == new Size(1024, 768));
                mnuFrameSizes.MenuItems.Add(m);
                mnuFrameSizes.Enabled = true;
            }
            catch { mnuFrameSizes.Enabled = false; }

            // Load property pages
            try
            {
                mnuPropertyPages.MenuItems.Clear();
                for (int c = 0; c < capture.PropertyPages.Count; c++)
                {
                    p = capture.PropertyPages[c];
                    m = new MenuItem(p.Name + "...", new EventHandler(mnuPropertyPages_Click));
                    mnuPropertyPages.MenuItems.Add(m);
                }
                mnuPropertyPages.Enabled = (capture.PropertyPages.Count > 0);
            }
            catch { mnuPropertyPages.Enabled = false; }

            // Enable/disable caps
            mnuVideoCaps.Enabled = ((capture != null) && (capture.VideoCaps != null));

            // Reenable preview if it was enabled before
            if (capture != null)
                capture.PreviewWindow = oldPreviewWindow;
        }

        private void mnuVideoDevices_Click(object sender, System.EventArgs e)
        {
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
                MenuItem m = sender as MenuItem;
                videoDevice = (m.Index >= 0 ? filters.VideoInputDevices[m.Index] : null);

                // Create capture object
                if ((videoDevice != null))
                {
                    capture = new Capture(videoDevice);
                    if (capture.PreviewWindow == null)
                    {
                        capture.PreviewWindow = panelVideo;
                    }
                    capWrapper.vDeviceIndex = m.Index;
                }

                // Update the menu
                updateMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Video device not supported.\n\n" + ex.Message);
            }
        }

        private void mnuVideoCompressors_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Change the video compressor
                // We subtract 1 from m.Index beacuse the first item is (None)
                MenuItem m = sender as MenuItem;
                capture.VideoCompressor = (m.Index > 0 ? filters.VideoCompressors[m.Index - 1] : null);
                capWrapper.vCompressIndex = m.Index;
                updateMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Video compressor not supported.\n\n" + ex.Message + "\n\n" + ex.ToString());
            }

        }

        private void mnuVideoSources_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Choose the video source
                // If the device only has one source, this menu item will be disabled
                MenuItem m = sender as MenuItem;
                capture.VideoSource = capture.VideoSources[m.Index];
                updateMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to set video source. Please submit bug report.\n\n" + ex.Message + "\n\n" + ex.ToString());
            }
        }

        private void mnuExit_Click(object sender, System.EventArgs e)
        {
            if (capture != null)
            {
                try
                {
                    capture.Stop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            this.Close();
        }

        private void mnuFrameSizes_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Disable preview to avoid additional flashes (optional)
                bool preview = (capture.PreviewWindow != null);
                capture.PreviewWindow = null;

                // Update the frame size
                MenuItem m = sender as MenuItem;
                string[] s = m.Text.Split('x');
                Size size = new Size(int.Parse(s[0]), int.Parse(s[1]));
                capture.FrameSize = size;

                // Update the menu
                updateMenu();

                capWrapper.vFrameSizeX = int.Parse(s[0]);
                capWrapper.vFrameSizeY = int.Parse(s[1]);

                // Restore previous preview setting
                capture.PreviewWindow = (preview ? panelVideo : null);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Frame size not supported.\n\n" + ex.Message + "\n\n" + ex.ToString());
                MessageBox.Show("Kích thước khung hình không được hỗ trợ.\n\n" + ex.Message);
            }
        }

        private void mnuFrameRates_Click(object sender, System.EventArgs e)
        {
            try
            {
                MenuItem m = sender as MenuItem;
                string[] s = m.Text.Split(' ');
                capture.FrameRate = double.Parse(s[0]);
                capWrapper.vFrameRate = double.Parse(s[0]);
                updateMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tốc độ khung hình không được hỗ trợ.\n\n" + ex.Message);
            }
        }

        private void mnuPreview_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (capture.PreviewWindow == null)
                {
                    capture.PreviewWindow = panelVideo;
                }
                else
                {
                    capture.PreviewWindow = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to enable/disable preview. Please submit a bug report.\n\n" + ex.Message + "\n\n" + ex.ToString());
            }
        }

        private void mnuPropertyPages_Click(object sender, System.EventArgs e)
        {
            try
            {
                MenuItem m = sender as MenuItem;
                capture.PropertyPages[m.Index].Show(this);
                updateMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể hiển thị cấu hình chi tiết.\n\n" + ex.Message);
            }
        }

        private void mnuVideoCaps_Click(object sender, System.EventArgs e)
        {
            try
            {
                string s;
                s = String.Format(
                    "Video Device Capabilities\n" +
                    "--------------------------------\n\n" +
                    "Input Size:\t\t{0} x {1}\n" +
                    "\n" +
                    "Min Frame Size:\t\t{2} x {3}\n" +
                    "Max Frame Size:\t\t{4} x {5}\n" +
                    "Frame Size Granularity X:\t{6}\n" +
                    "Frame Size Granularity Y:\t{7}\n" +
                    "\n" +
                    "Min Frame Rate:\t\t{8:0.000} fps\n" +
                    "Max Frame Rate:\t\t{9:0.000} fps\n",
                    capture.VideoCaps.InputSize.Width, capture.VideoCaps.InputSize.Height,
                    capture.VideoCaps.MinFrameSize.Width, capture.VideoCaps.MinFrameSize.Height,
                    capture.VideoCaps.MaxFrameSize.Width, capture.VideoCaps.MaxFrameSize.Height,
                    capture.VideoCaps.FrameSizeGranularityX,
                    capture.VideoCaps.FrameSizeGranularityY,
                    capture.VideoCaps.MinFrameRate,
                    capture.VideoCaps.MaxFrameRate);
                MessageBox.Show(s);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable display video capabilities. Please submit a bug report.\n\n" + ex.Message + "\n\n" + ex.ToString());
            }
        }

        private void OnCaptureComplete(object sender, EventArgs e)
        {
            // Demonstrate the Capture.CaptureComplete event.
            Debug.WriteLine("Capture complete.");
        }

        private bool initCamera()
        {
            bool exitCode = false;
            try
            {
                if (capWrapper.vDeviceIndex >= filters.VideoInputDevices.Count || capWrapper.vDeviceIndex < 0)
                {
                    capWrapper.vDeviceIndex = 0;
                }

                capture = new Capture(filters.VideoInputDevices[capWrapper.vDeviceIndex]);
                capture.CaptureComplete += new EventHandler(OnCaptureComplete);
                capture.PreviewWindow = panelVideo;
                exitCode = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Video device not supported.\n\n" + ex.Message + "\n\n" + ex.ToString());
                exitCode = false;
                //MessageBox.Show("Thiết bị thu video không được hỗ trợ. Vui lòng kiểm tra camera \n\n" + ex.Message);
            }
            return exitCode;
        }
    }
}
