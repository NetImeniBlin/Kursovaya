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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        MySqlConnection conn;

        public void GetListPrepods(ListBox lb)
        {
            lb.Items.Clear();
            conn.Open();
            string sql = $"SELECT * FROM сотрудники1";
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lb.Items.Add($"FIO: {reader[1].ToString()} age: {reader[2].ToString()} dolg: {reader[3].ToString()}");

            }
            reader.Close();
            conn.Close();
        }

        public bool InsertPrepods(string Ifio, string Iage, string Idolg)
        {
            int InsertCount = 0;
            bool result = false;
            conn.Open();
            string query = $"INSERT INTO сотрудники1 (FIO, age, dolg) VALUES ('{Ifio}', '{Iage}', '{Idolg}')";
            try
            {
                MySqlCommand command = new MySqlCommand(query, conn);
                InsertCount = command.ExecuteNonQuery();
            }
            catch
            {
                InsertCount = 0;
            }
            finally
            {
                conn.Close();
                if (InsertCount != 0)
                {
                    result = true;
                }
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Ifio = textBox1.Text;
            string Iage = textBox2.Text;
            string Idolg = textBox3.Text;
            if (InsertPrepods(Ifio, Iage, Idolg))
            {
                GetListPrepods(listBox1);
            }
            else
            {
                MessageBox.Show("Произошла ошибка.", "Ошибка");
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string connStr = "server=caseum.ru;port=33333;user=st_2_8_19;database=st_2_8_19;password=46727777;";
            conn = new MySqlConnection(connStr);
            GetListPrepods(listBox1);
        }
    }
}
