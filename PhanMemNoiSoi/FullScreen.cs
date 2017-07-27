using DirectX.Capture;
using System;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class FullScreen : Form
    {
        public delegate void DoEvent();
        public event DoEvent captureImg;
        public FullScreen(CheckAndView checkView)
        {
            InitializeComponent();
            checkView.initCamera(pictureBox1);
        }

        private void FullScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.F1)       
            {
                e.SuppressKeyPress = true;
                captureImg();
            }
        }

        private void FullScreen_Load(object sender, EventArgs e)
        {
            //this.capture.Stop();
            //this.capture.Cue();
            //this.capture.PreviewWindowFrame = pictureBox1;
            //this.capture.Start();
        }
    }
}
