using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FBApplication
{
    public partial class MDIParent1 : Form
    {
        public MDIParent1()
        {
            InitializeComponent();
        }
        private void MDIParent1_Load(object sender, EventArgs e)
        {
            label2.Text = "Connecting to facebook.....";
            Form1 frmLogin = new Form1();
            frmLogin.MdiParent = this;
            frmLogin.Show();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
