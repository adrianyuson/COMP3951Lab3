using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP3951Lab3
{
    public partial class Dialog : Form
    {
        public Dialog()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.DialogResult = DialogResult.No;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.DialogResult = DialogResult.Yes;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.DialogResult = DialogResult.Cancel;
        }
    }
}
