using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp4
{
    public partial class potom : Form
    {
        public potom()
        {
            InitializeComponent();
        }
        MySqlConnection conn;

        private void button1_Click(object sender, EventArgs e)
        {
            string file = "D:\\backup.sql";
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlBackup mb = new MySqlBackup(cmd))
                {
                    cmd.Connection = conn;
                    conn.Open();
                    mb.ExportToFile(file);
                    conn.Close();
                }
            }
        }

        private void potom_Load(object sender, EventArgs e)
        {
            string connStr = "server=caseum.ru;port=33333;user=st_2_8_19;database=st_2_8_19;password=46727777;";
            conn = new MySqlConnection(connStr);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string file = "D:\\backup.sql";
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlBackup mb = new MySqlBackup(cmd))
                {
                    cmd.Connection = conn;
                    conn.Open();
                    mb.ImportFromFile(file);
                    conn.Close();
                }
            }
        }
    }
}
