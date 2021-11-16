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
            string NameNewTable = Convert.ToString(textBox1.Text);
            string createnewtable = $"create table st_2_8_19.{NameNewTable} select * from st_2_8_19.сотрудники1;";
            MySqlCommand cmd = new MySqlCommand(createnewtable, conn);
            cmd.BeginExecuteNonQuery();
            conn.Close();
        }

        public void GetComboBoxList()
        {
            conn.Open();
            DataTable List_p = new DataTable();
            string tables = "SELECT TABLE_NAME FROM st_2_8_19.INFORMATION_SCHEMA";
            MySqlCommand ST = new MySqlCommand(tables, conn);
            MySqlDataReader list_stud_reader;
            List_p.Columns.Add(new DataColumn("Table", System.Type.GetType("System.String")));
            try
            {
                list_stud_reader = ST.ExecuteReader();
                while (list_stud_reader.Read())
                {
                    DataRow rowToAdd = List_p.NewRow();
                    rowToAdd["fio"] = list_stud_reader[0].ToString();
                    List_p.Rows.Add(rowToAdd);
                }
                list_stud_reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения списка ЦП \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
            }
            conn.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            string connstr = "server=caseum.ru;port=33333;user=st_2_8_19;database=st_2_8_19;password=46727777;";
            conn = new MySqlConnection(connstr);
           // GetComboBoxList();
           // GetTables();
        }
    }
}
