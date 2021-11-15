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
    public partial class Form2 : Form
    {
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bSource = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        string id_selected_rows = "0";

        public Form2()
        {
            InitializeComponent();
        }

        public void RightMouseClick()
        {

        }

        public void GetSelectedIDString()
        {
            string SelectedRows;
            SelectedRows = dataGridView1.SelectedCells[0].RowIndex.ToString();
            id_selected_rows = dataGridView1.Rows[Convert.ToInt32(SelectedRows)].Cells[0].Value.ToString();
            toolStripLabel1.Text = id_selected_rows;
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!e.RowIndex.Equals(-1) && !e.ColumnIndex.Equals(-1) && e.Button.Equals(MouseButtons.Right))
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
                dataGridView1.CurrentCell.Selected = true;              
                GetSelectedIDString();
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
            dataGridView1.CurrentRow.Selected = true;
            GetSelectedIDString();
        }

        public void GetListUsers()
        {
            string commandStr = "SELECT Номер_Пекарни AS 'Номер', Район, Номер_телефона AS 'Статус' FROM адреса";
            conn.Open();
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            MyDA.Fill(table);
            bSource.DataSource = table;
            dataGridView1.DataSource = bSource;
            conn.Close();
            int count_rows = dataGridView1.RowCount - 1;
            toolStripLabel2.Text = (count_rows).ToString();
        }

        public void reload_list()
        {
            table.Clear();
            GetListUsers();
        }

        private void ChangeColorDGV()
        {
            int count_rows = dataGridView1.RowCount - 1;
            toolStripLabel2.Text = (count_rows).ToString();
            for (int i = 0; i < count_rows; i++)
            {
                int id_selected_status = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                if (id_selected_status == 1)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                if (id_selected_status == 2)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }

        public void ChangeStateStudent(string new_state)
        {
            string redact_id = id_selected_rows;
            conn.Open();
            string query2 = $"UPDATE адреса SET да='{new_state}' WHERE (Номер_Пекарни='{redact_id}')";
            MySqlCommand command = new MySqlCommand(query2, conn);
            command.ExecuteNonQuery();
            conn.Close();
            reload_list();
            ChangeColorDGV();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string connStr = "server=caseum.ru;port=33333;user=st_2_8_19;database=st_2_8_19;password=46727777;";
            conn = new MySqlConnection(connStr);
            GetListUsers();
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[0].FillWeight = 15;
            dataGridView1.Columns[1].FillWeight = 40;
            dataGridView1.Columns[2].FillWeight = 15;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = true;
            ChangeColorDGV();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("не важно");
        }
    }
}
