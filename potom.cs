﻿using System;
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
    public partial class potom : Form
    {
        public potom()
        {
            InitializeComponent();
        }
        MySqlConnection conn;
        public void comboUpdate()
        {
            comboBox1.Items.Clear();
            string path = "D:\\backup";
            string[] files = Directory.GetFiles(path);
            foreach (string kolvo in files)
            {
                comboBox1.Items.Add(kolvo);
            }
        }
        private void potom_Load(object sender, EventArgs e)
        {
            string connStr = "server=caseum.ru;port=33333;user=st_2_8_19;database=st_2_8_19;password=46727777;";
            conn = new MySqlConnection(connStr);
            string path = "D:\\backup";
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string kolvo in files)
                {
                    comboBox1.Items.Add(kolvo);
                }
            }
            else
            {
                Directory.CreateDirectory("D:\\backup");
                string[] files = Directory.GetFiles(path);
                foreach (string kolvo in files)
                {
                    comboBox1.Items.Add(kolvo);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime time = DateTime.Today;
            string backupdate = (time.ToString("d"));
            string file = $"D:\\backup\\{backupdate}.sql";
            if (Directory.Exists(file))
            {
                MessageBox.Show("копия с таким названием уже есть, пожалуйста введите другое");
                button3.Enabled = true;
                button3.Visible = true;
                textBox1.Enabled = true;
                textBox1.Visible = true;
            }
            else
            {
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
                comboUpdate();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string file = Convert.ToString(comboBox1.Text);
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlBackup ex = new MySqlBackup(cmd))
                {
                    cmd.Connection = conn;
                    conn.Open();
                    ex.ImportFromFile(file);
                    conn.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
