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
    public partial class Acc_add : Form
    {
        public Acc_add()
        {
            InitializeComponent();
        }

        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bSource = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        string commandStr;

        private void Acc_add_Load(object sender, EventArgs e)
        {
            Program.Podkl connn = new Program.Podkl();
            conn = new MySqlConnection(connn.Connstring);
            GetListUsers();
            GetListId();

            dataGridView1.Columns[0].FillWeight = 34;
            dataGridView1.Columns[1].FillWeight = 34;
            dataGridView1.Columns[2].FillWeight = 30;
            dataGridView1.Columns[3].FillWeight = 30;

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.RowHeadersVisible = false;
        }

        public void GetListUsers()
        {
            commandStr = $"SELECT id, log as Логин, pass as Пароль, fio as ФИО FROM users";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bSource.DataSource = table;
            dataGridView1.DataSource = bSource;
            conn.Close();
        }

        public void GetListId()
        {
            comboBox1.Items.Clear();
            toolStripComboBox1.Items.Clear();
            string que = "select id from users";
            MySqlCommand com = new MySqlCommand(que, conn);
            conn.Open();
            MySqlDataReader reader = com.ExecuteReader();
            while(reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
                toolStripComboBox1.Items.Add(reader.GetString(0));
            }
            conn.Close();
        }

        public void reload_list()
        {
            table.Clear();
            GetListUsers();
            GetListId();
        }

        public bool InsertSotrudniki(string Ilog, string Ipass, string Ifio)
        {
            int InsertCount = 0;
            bool result = false;
            conn.Open();
            string query = $"INSERT INTO users (log, pass, fio) VALUES ('{Ilog}', '{Ipass}', '{Ifio}')";
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

        public void UpData(TextBox txt, string column)
        {
            int selected_id = Convert.ToInt32(comboBox1.SelectedItem);
            string newdata = Convert.ToString(txt.Text);
            string cm = $"UPDATE users SET {column}='{newdata}' WHERE id='{selected_id}'";
            MySqlCommand redact = new MySqlCommand(cm, conn);
            conn.Open();
            redact.ExecuteNonQuery();
            conn.Close();
        }

        public void TextboxFill(TextBox txt, string column)
        {
            string que1 = $"select {column} from users where id={Convert.ToInt32(comboBox1.SelectedItem)}";
            MySqlCommand com1 = new MySqlCommand(que1, conn);
            conn.Open();
            string result = com1.ExecuteScalar().ToString();
            conn.Close();
            txt.Text = result;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            reload_list();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == ""| textBox2.Text == "" | textBox3.Text == "")
            {
                MessageBox.Show("пожалуйста, укажите необходимые данные","status: error");
            }
            else
            {
                UpData(textBox1, "log");
                UpData(textBox2, "pass");
                UpData(textBox3, "fio");
                MessageBox.Show("Данные рользователя были успешно изменены","status: succes");
            }
            reload_list();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" | textBox2.Text == "" | textBox3.Text == "")
            {
                MessageBox.Show("пожалуйста, укажите необходимые данные","status: error");
            }
            else
            {
                InsertSotrudniki(textBox1.Text, textBox2.Text, textBox3.Text);
            }
            reload_list();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            TextboxFill(textBox1, "log");
            TextboxFill(textBox2, "pass");
            TextboxFill(textBox3, "fio");
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            string delete = ($"DELETE FROM users WHERE id=" + toolStripComboBox1.SelectedText);
            MySqlCommand cm = new MySqlCommand(delete, conn);
            try
            {
                conn.Open();
                cm.ExecuteNonQuery();
                MessageBox.Show("Удаление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления строки \n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
                reload_list();
            }
        }
    }
}
