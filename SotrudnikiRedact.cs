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

        public void UPDate(TextBox txt, string column)
        {
            int selected_id = Convert.ToInt32(comboBox1.SelectedItem);
            string newdata = Convert.ToString(txt.Text);
            string cm = $"UPDATE {select} SET {column}='{newdata}' WHERE id='{selected_id}'";
            MySqlCommand redact = new MySqlCommand(cm, conn);
            conn.Open();
            redact.ExecuteNonQuery();
            conn.Close();
        }

        public void TextboxFill(TextBox txt, string column)
        {
            string que1 = $"select {column} from {select} where id={Convert.ToInt32(comboBox1.SelectedItem)}";
            MySqlCommand com1 = new MySqlCommand(que1, conn);
            conn.Open();
            string result = com1.ExecuteScalar().ToString();
            conn.Close();
            txt.Text = result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" | textBox2.Text == "" | textBox3.Text == "" | textBox4.Text == "" | comboBox1.Text == "")
            {
                MessageBox.Show("Введите необходимые данные","status: failed");
                return;
            }
            UPDate(textBox1, "FIO");
            UPDate(textBox2, "age");
            UPDate(textBox3, "dolg");
            UPDate(textBox4, "Phone_number");
            MessageBox.Show("Редактирование прошло успешно","status: succes");
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            TextboxFill(textBox1, "FIO");
            TextboxFill(textBox2, "age");
            TextboxFill(textBox3, "dolg");
            TextboxFill(textBox4, "Phone_number");
        }
    }
}
