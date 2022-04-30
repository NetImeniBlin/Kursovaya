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
        string copyName;
        string path = $"{Application.StartupPath}\\path.txt";
        string selectedFile;
        FolderLog folderLog = new FolderLog();

        public void LogRefresh()
        {
            folderLog.listBox1.Items.Clear();
            string[] log = File.ReadAllLines(Path.Combine(path), Encoding.Default);
            foreach (string line in log)
            {
                folderLog.listBox1.Items.Add(line);
            }
        }

        private void Backup_Load(object sender, EventArgs e)
        {
            Program.Podkl connn = new Program.Podkl();
            conn = new MySqlConnection(connn.Connstring);
            if (File.Exists(path) == false)
            {
                File.Create(path);
            }
            LogRefresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            selectedFolder = folderBrowserDialog1.SelectedPath;
            textBox1.Text = selectedFolder;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime time = DateTime.Now;
                string backupdate = (time.ToString("MM.dd.yyyy.HH.mm.ss"));
                copyName = $"{selectedFolder}\\{backupdate}.sql";
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(copyName);
                        conn.Close();
                    }
                }
                File.AppendAllText(path, (selectedFolder + Environment.NewLine), Encoding.Default);
                LogRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ошибка, не выбрана папка или она не допустима для записи", "status: fail");
                return;
            }
            finally
            {
                MessageBox.Show("копия создана");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            selectedFile = openFileDialog1.FileName;
            textBox2.Text = selectedFile;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            folderLog.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                MySqlBackup ex = new MySqlBackup(cmd);
                cmd.Connection = conn;
                conn.Open();
                ex.ImportFromFile(selectedFile);
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "status: fail");
                return;
            }
            finally
            {
                MessageBox.Show("копия успешно восстановлена", "status: succes");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            File.Delete(selectedFile);
            MessageBox.Show("копия была удалена");
        }
    }
}
