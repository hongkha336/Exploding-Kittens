using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exploding_Kittens
{
    public partial class frmStart : Form
    {
       
        
        public frmStart()
        {
            InitializeComponent();
           
        }

  
        private void btnTaoPhong_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Hide();
            
                frmMain f = new frmMain(isStartSever);
                f.ShowDialog();
            this.Show();
        }

        private void frmStart_Load(object sender, EventArgs e)
        {
          
        }


        bool isStartSever = false;

        private void chkStart_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStart.Checked)
                isStartSever = true;
            else
                isStartSever = false;
        }
    }
}
