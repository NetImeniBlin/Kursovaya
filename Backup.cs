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
using System.IO;

namespace WindowsFormsApp4
{
    public partial class Backup : Form
    {
        public Backup()
        {
            InitializeComponent();
        }

        MySqlConnection conn;
        string selectedFolder;
        string file;

        private void Backup_Load(object sender, EventArgs e)
        {
            Program.Podkl connn = new Program.Podkl();
            conn = new MySqlConnection(connn.Connstring);
            string path =  Application.StartupPath;
            if (File.Exists($"{path}\\path.txt") == false)
            {
                File.Create($"{path}\\path.txt");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            selectedFolder = folderBrowserDialog1.SelectedPath;
            textBox1.Text = selectedFolder;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(selectedFolder == null == false)
            {
                DateTime time = DateTime.Now;
                string backupdate = (time.ToString("MM.dd.yyyy.HH.mm.ss"));
                file = $"{selectedFolder}\\{backupdate}.sql";
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
                MessageBox.Show("копия создана");
            }
            else
            {
                MessageBox.Show("не выбрана папка для копии");
            }
        }
    }
}
