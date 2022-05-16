using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
        public string SelectedTable;
        string commandStr;
        public string comm;
        delegate void LoadDel();
        MySqlConnection conn;

        public Sotrudniki()
        {
            InitializeComponent();
        }

        public void Loading()
        {
            LoadDel loadDel = SampleForDelegate;
            Invoke(loadDel);
        }

        private void Sotrudniki_Load(object sender, EventArgs e)
        {
            Program.Podkl connn = new Program.Podkl();
            conn = new MySqlConnection(connn.Connstring);
            ThreadStart threadStart = new ThreadStart(Loading);
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        public void SampleForDelegate()
        {
            items();
            ChangeColorDGV();
            dataGridView1.Columns[0].FillWeight = 5;
            dataGridView1.Columns[1].FillWeight = 34;
            dataGridView1.Columns[2].FillWeight = 10;
            dataGridView1.Columns[3].FillWeight = 13;
            dataGridView1.Columns[4].FillWeight = 13;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.RowHeadersVisible = false;
        }

        public void GetSelectedIDString()
        {
            string index_selected_rows;
            index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
            id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
        }
        
        public void GetCount()
        {
            for (int i = 1; i <= 3; i++)
            {
                conn.Open();
                string commandStr = $"SELECT * FROM Sotrudniki_{i}";
                MySqlCommand com = new MySqlCommand(commandStr, conn);
                MySqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    listBox1.Items.Add(reader[0].ToString());
                }
                reader.Close();
                conn.Close();
            }
            int count_rows = listBox1.Items.Count;
            toolStripLabel1.Text = (count_rows).ToString();
        }

        public void GetListUsers()
        {
            listBox1.Items.Clear();
            SelectedTable = Convert.ToString(toolStripComboBox1.Text);
            commandStr = $"SELECT id, FIO as ФИО, age as Возраст, dolg as Должность, phone_number as 'номер телефона', status as 'Статус' FROM {SelectedTable}";
            try
            {
                conn.Open();
                MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
                MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
                MyDA.Fill(table);
                bSource.DataSource = table;
                dataGridView1.DataSource = bSource;
            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        public void GetPekInfo()
        {
            listBox2.Items.Clear();
            switch (SelectedTable)
            {
                case "Sotrudniki_1":
                    comm = $"SELECT Pekarnya_number, Rayon, Street, Phone_number, Phone_number FROM adreses where Pekarnya_number = 1";
                    break;
                case "Sotrudniki_2":
                    comm = $"SELECT Pekarnya_number, Rayon, Street, Phone_number, Phone_number FROM adreses where Pekarnya_number = 2";
                    break;
                case "Sotrudniki_3":
                    comm = $"SELECT Pekarnya_number, Rayon, Street, Phone_number, Phone_number FROM adreses where Pekarnya_number = 3";
                    break;
                    default:
                    return;
            }
            conn.Open();
            MySqlCommand com1 = new MySqlCommand(comm, conn);
            MySqlDataReader reader1 = com1.ExecuteReader();
            while (reader1.Read())
            {
                listBox2.Items.Add("Номер пекарни - " + reader1[0].ToString());
                listBox2.Items.Add("Район - " + reader1[1].ToString());
                listBox2.Items.Add("Улица - " + reader1[2].ToString());
                listBox2.Items.Add("Номер телефон - " + reader1[3].ToString());
                listBox2.Items.Add("Часы работы - " + reader1[4].ToString());
            }
            reader1.Close();
            conn.Close();
        }

        public void reload_list()
        {
            table.Clear();
            GetListUsers();
            ChangeColorDGV();
            GetPekInfo();
            GetCount();
        }

        private void ChangeColorDGV()
        {
            int count_rows = dataGridView1.RowCount - 1;
            for (int i = 0; i < count_rows; i++)
            {
                string status = Convert.ToString(dataGridView1.Rows[i].Cells[5].Value);
                if (status == "стажёр")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (status == "в отпуске")
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
            toolStripComboBox1.Items.Add("Sotrudniki_1");
            toolStripComboBox1.Items.Add("Sotrudniki_2");
            toolStripComboBox1.Items.Add("Sotrudniki_3");
            toolStripComboBox1.Text = "Sotrudniki_1";
        }

        public bool InsertSotrudniki(string Ifio, string Iage, string Idolg, string Inumber, string Istatus)
        {
            int InsertCount = 0;
            bool result = false;
            conn.Open();
            string query = $"INSERT INTO {SelectedTable} (FIO, age, dolg, phone_number, status) VALUES ('{Ifio}', '{Iage}', '{Idolg}', '{Inumber}', '{Istatus}')";
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            GetSelectedIDString();
            string delete = ($"DELETE FROM {SelectedTable} WHERE id=" + id_selected_rows);
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
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            label5.Visible = false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string Ifio = textBox1.Text;
            string Iage = textBox2.Text;
            string Idolg = textBox3.Text;
            string Inumber = textBox4.Text;
            string Istatus = comboBox1.Text;
            if (InsertSotrudniki(Ifio, Iage, Idolg, Inumber, Istatus))
            {
                reload_list();
            }
            else
            {
                MessageBox.Show("Произошла ошибка.", "status: fail");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SotrudnikiRedact sotrudnikiRedact = new SotrudnikiRedact();
            sotrudnikiRedact.select = toolStripComboBox1.Text;
            sotrudnikiRedact.Show();
            reload_list();
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            reload_list();
        }
    }
}
