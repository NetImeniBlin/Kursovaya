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
    public partial class SotrudnikiRedact : Form
    {
        MySqlConnection conn;
        public string select;
        public SotrudnikiRedact()
        {
            InitializeComponent();
        }

        private void SotrudnikiRedact_Load(object sender, EventArgs e)
        {
            Program.Podkl connn = new Program.Podkl();
            conn = new MySqlConnection(connn.Connstring);
            GetListId();
        }

        public void GetListId()
        {
            MessageBox.Show($"{select}");
            string que = $"select id from {select}";
            conn.Open();
            MySqlCommand com = new MySqlCommand(que, conn);
            MySqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            conn.Close();
        }
    }
}
