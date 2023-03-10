using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace kursa4
{
    public partial class Form1 : Form
    {
         Form2 frm2 = new Form2();

        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder  = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;

        public Form1()
        {
            InitializeComponent(); 
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vladb\source\repos\kursa4\kursa4\Database1.mdf;Integrated Security=True");

            sqlConnection.Open();

            LoadData();
            radioButton1.Checked = true;
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////
        /// </summary>

        private void LoadData()
        {
            try
            {
               

                if (sqlDataAdapter != null)
                {
                    dataSet.Tables["Items"].Clear();
                }

                if (sqlDataAdapter == null)
                {
                    sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Items", sqlConnection);

                    sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                    sqlBuilder.GetInsertCommand();

                    sqlBuilder.GetUpdateCommand();

                    sqlBuilder.GetDeleteCommand();

                    dataSet = new DataSet();

                    sqlDataAdapter.Fill(dataSet, "Items");

                    dataGridView1.DataSource = dataSet.Tables["Items"];
                }

               

              



                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Что-то пошло не так", MessageBoxButtons.OK);
            }
        }


        public void saveData()
        {
            try
            {
                dataSet.Tables["Items"].Clear();
                sqlDataAdapter.Fill(dataSet, "Items");
                dataGridView1.DataSource = dataSet.Tables["Items"];

                MessageBox.Show("Вроде сохранилось", "Сохранилось?", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
           
        }

       
        public void addData(bool save, string name, int value)
        {
 
        }


        private void button4_Click(object sender, EventArgs e)
        {
            
        }


        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            //if (dr == DialogResult.Cancel)
            //{
            //    return;
            //}
        }






        private void SaveBtn_Click_1(object sender, EventArgs e)
        {
            saveData();

        }



        private void button2_Click(object sender, EventArgs e) // del
        {
            DialogResult dr = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            else {
                
                int index = dataGridView1.SelectedRows[0].Index;
                dataGridView1.Rows.RemoveAt(index);
                dataSet.Tables["Items"].Rows[index].Delete();
                sqlDataAdapter.Update(dataSet, "Items");
            }
            //saveData();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            

            Form2 frm2 = new Form2();
            frm2.Show();
            //DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
            //frm2.Text = row.Cells["Количество на складе"].Value.ToString();
            //.Text = row.Cells["Наименование"].Value.ToString();
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string row1 = row.Cells["Count"].Value.ToString();
                string row2 = row.Cells["description"].Value.ToString();

                Form2 frm2 = new Form2();
                frm2.Show();
                frm2.getValueRows(row1, row2);
               
            }
            
        }
                

          public void setValueRow(string row1, string row2)
        {

        }
        

        private void DataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

           
        }








        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
    
}
