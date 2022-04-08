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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        MySqlConnection conn;

        private void Menu_Load(object sender, EventArgs e)
        {
            Program.Podkl connn = new Program.Podkl();
            conn = new MySqlConnection(connn.Connstring);
        }

        private void button1_Click(object sender, EventArgs e)
        {
                string com = $"SELECT * FROM polz WHERE bimbimbambom ='{textBox1.Text}' and pass ='{textBox2.Text}'";
                conn.Open();
                MySqlCommand command = new MySqlCommand(com, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listBox1.Items.Add(reader[0].ToString());
                }
                if (listBox1.Items.Count > 0)
                {
                    auth papa = new auth();
                    papa.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверные данные авторизации!");
                }
                conn.Close();
        }
    }
}
