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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        MySqlConnection conn;

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            string createnewtable = "create table st_2_8_19.неважно select * from st_2_8_19.сотрудники1;";
            MySqlCommand cmd = new MySqlCommand(createnewtable, conn);
            cmd.BeginExecuteNonQuery();
            conn.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            string connstr = "server=caseum.ru;port=33333;user=st_2_8_19;database=st_2_8_19;password=46727777;";
            conn = new MySqlConnection(connstr);
        }
    }
}
