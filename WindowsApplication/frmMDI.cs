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
    public partial class frmMDI : Form
    {
        public frmMDI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Given:  Opens the client form in the MDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmClient client = new frmClient();
            client.MdiParent = this;
            client.Show();
        }

        private void batchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBatch batch = new frmBatch();
            batch.MdiParent = this;
            batch.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }
    }
}
