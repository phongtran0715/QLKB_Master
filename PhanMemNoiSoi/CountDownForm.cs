using System;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    public partial class CountDownForm : Form
    {
        private int counter = 15;
        bool exitStatus = false;

        public CountDownForm()
        {
            InitializeComponent();
        }

        private void btnSoftwareActive_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.timer1.Stop();
            new ActiveKeyForm().ShowDialog();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            if (counter == 0)
            {
                timer1.Stop();
                exitStatus = true;
                this.Close();
            }
            lblCountDown.Text = counter.ToString();
        }

        private void CountDownForm_Load(object sender, EventArgs e)
        {
            this.timer1.Start();
            lblCountDown.Text = counter.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            exitStatus = false;
            this.Close();
        }

        public bool GetMyResult()
        {
            return exitStatus;
        }
    }
}
