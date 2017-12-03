using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhanMemNoiSoi.SplashScreen
{
    public partial class SplashScreenFrm : Form, ISplashForm
    {
        public SplashScreenFrm()
        {
            InitializeComponent();
        }

        public void SetStatusInfo(string NewStatusInfo)
        {
            lbStatusInfo.Text = NewStatusInfo;
        }
    }
}
