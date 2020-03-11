using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsApplication
{
    public partial class frmBatch : Form
    {
        /// <summary>
        /// given:  This constructor will be used when called from 
        /// frmClient.  This constructor will receive 
        /// specific information about the client and bank account
        /// further code required:  
        /// </summary>
        public frmBatch()
        {
            InitializeComponent();

        }

        /// <summary>
        /// given:  open in top left of frame
        /// further code required:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBatch_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
        }

        /// <summary>
        /// Given:  Further code required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkProcess_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtKey.Text.Length != 8)
            {
                MessageBox.Show("A 64 bit key is required");
            }
        }


    }
}
