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
    public partial class Sotrudniki : Form
    {
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bSource = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        string id_selected_rows = "0";
        string SelectedTable;
        string commandStr;

        public Sotrudniki()
        {
            InitializeComponent();
        }

        MySqlConnection conn;

        public void GetSelectedIDString()
        {
            string index_selected_rows;
            index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
            id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
        }

        public void GetListUsers()
        {
            SelectedTable = Convert.ToString(toolStripComboBox1.Text);
            commandStr = $"SELECT id, FIO, age, dolg FROM {SelectedTable}";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bSource.DataSource = table;
            dataGridView1.DataSource = bSource;
            conn.Close();
            int count_rows = dataGridView1.RowCount - 1;
        }

        public void reload_list()
        {
            table.Clear();
            GetListUsers();
            ChangeColorDGV();
        }

        private void ChangeColorDGV()
        {
            int count_rows = dataGridView1.RowCount - 1;
            for (int i = 0; i < count_rows; i++)
            {
                string dolgstatus = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value);
                if (dolgstatus == "стажер")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (dolgstatus == "уволен")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                else
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }

        public void items()
        {
            toolStripComboBox1.Items.Add("сотрудники1");
            toolStripComboBox1.Items.Add("сотрудники2");
            toolStripComboBox1.Items.Add("сотрудники3");
            toolStripComboBox1.Text = "сотрудники1";
        }

        public bool InsertSotrudniki(string Ifio, string Iage, string Idolg)
        {
            int InsertCount = 0;
            bool result = false;
            conn.Open();
            string query = $"INSERT INTO {SelectedTable} (FIO, age, dolg) VALUES ('{Ifio}', '{Iage}', '{Idolg}')";
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

        private void Form3_Load(object sender, EventArgs e)
        {
            string connStr = "server=caseum.ru;port=33333;user=st_2_8_19;database=st_2_8_19;password=46727777;";
            conn = new MySqlConnection(connStr);
            ToolTip tip = new ToolTip();
            items();
            GetListUsers();
            ChangeColorDGV();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            GetSelectedIDString();
            MessageBox.Show("Содержимое поля Код, в выбранной строке" + id_selected_rows);
            string delete = ("DELETE FROM сотрудники1 WHERE id=" + id_selected_rows);
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
                Application.Exit();
            }
            finally
            {
                conn.Close();
                reload_list();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            reload_list();
            MessageBox.Show("обновлено","status");
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            SelectedTable = Convert.ToString(toolStripComboBox1.Text);
            reload_list();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            label1.Visible = false;
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            label2.Visible = false;
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            label3.Visible = false;
        }

        private void textBox4_MouseClick(object sender, MouseEventArgs e)
        {
            label4.Visible = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string Ifio = textBox1.Text;
            string Iage = textBox2.Text;
            string Idolg = textBox3.Text;
            if (InsertSotrudniki(Ifio, Iage, Idolg))
            {
                reload_list();
            }
            else
            {
                MessageBox.Show("Произошла ошибка.", "Ошибка");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            GetSelectedIDString();
            string newdolg = Convert.ToString(textBox4.Text);  
            string cm = ($"UPDATE {SelectedTable} SET dolg='{newdolg}' WHERE id='{id_selected_rows}'");
            MySqlCommand redact = new MySqlCommand(cm, conn);
            conn.Open();
            redact.ExecuteNonQuery();
            conn.Close();
            reload_list();
        }
    }
}
