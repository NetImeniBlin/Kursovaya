using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class auth : Form
    {
        public auth()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Backup backup = new Backup();   
            backup.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sotrudniki sotrudniki = new Sotrudniki();
            sotrudniki.ShowDialog();
        }
    }
}
