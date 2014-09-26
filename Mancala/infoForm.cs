using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mancala
{
    public partial class infoForm : Form
    {
        public infoForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void infoForm_Load(object sender, EventArgs e)
        {

        }

        private void infoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.isShown = false;
        }
    }
}
