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
    public partial class Autenth : Form
    {
        public Autenth()
        {
            InitializeComponent();
        }

        MySqlConnection conn;

        private void Autenth_Load(object sender, EventArgs e)
        {
            Program.Podkl connn = new Program.Podkl();
            conn = new MySqlConnection(connn.Connstring);
        }

        public void Autenthi()
        {
            string com = $"SELECT * FROM users WHERE log ='{textBox1.Text}' and pass ='{textBox2.Text}'";
            string com1 = $"SELECT fio FROM users WHERE log ='{textBox1.Text}' and pass ='{textBox2.Text}'";
            conn.Open();
            MySqlCommand command = new MySqlCommand(com, conn);
            MySqlCommand command1 = new MySqlCommand(com1, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader[0].ToString());
            }
            conn.Close();
            conn.Open();
            if (listBox1.Items.Count > 0)
            {
                this.Hide();
                string fio = command1.ExecuteScalar().ToString();
                MessageBox.Show($"здраствуйте {fio}", "status: succes");
                Menu menu = new Menu();
                menu.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверные данные авторизации!", "status: fail");
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Autenthi();
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Autenthi();
            }
        }
    }
}
