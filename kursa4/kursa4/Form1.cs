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
        public bool newRowAdding = false;

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
                    sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM Items", sqlConnection);

                    sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                    sqlBuilder.GetInsertCommand();

                    sqlBuilder.GetUpdateCommand();

                    sqlBuilder.GetDeleteCommand();

                    dataSet = new DataSet();

                    sqlDataAdapter.Fill(dataSet, "Items");

                    dataGridView1.DataSource = dataSet.Tables["Items"];

                    for( int i=0; i<dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                        dataGridView1[3, i] = linkCell;
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
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[3, i] = linkCell;
                }
                //MessageBox.Show("Вроде сохранилось", "Сохранилось?", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
           
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int index = dataGridView1.SelectedRows[0].Index;
                if (index >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[index];
                    string row1 = row.Cells["Count"].Value.ToString();
                    string row2 = row.Cells["description"].Value.ToString();

                    Form2 frm2 = new Form2();
                    frm2.Show();
                    frm2.getValueRows(row1, row2, index);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
           
        }

        private void dataGridView1_UserAddedRow_1(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[3, lastRow] = linkCell;

                    row.Cells["Delete"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.ColumnIndex==3)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    if(task == "Delete")
                    {
                        DialogResult dr = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (dr == DialogResult.Cancel)
                        {
                            return;
                        }
                        else 
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["Items"].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, "Items");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Items"].NewRow();

                        row["Count"] = dataGridView1.Rows[rowIndex].Cells["Count"].Value;
                        row["description"] =  dataGridView1.Rows[rowIndex].Cells["description"].Value;

                        dataSet.Tables["Items"].Rows.Add(row);

                        dataSet.Tables["Items"].Rows.RemoveAt(dataSet.Tables["Items"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[3].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Items");
                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int rowIndex = e.RowIndex;
                        dataSet.Tables["Items"].Rows[rowIndex]["Count"] = dataGridView1.Rows[rowIndex].Cells["Count"].Value;
                        dataSet.Tables["Items"].Rows[rowIndex]["description"] = dataGridView1.Rows[rowIndex].Cells["description"].Value;

                        sqlDataAdapter.Update(dataSet, "Items");
                        dataGridView1.Rows[e.RowIndex].Cells[3].Value = "Delete";
                    }
                    saveData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
           
            
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);

            if (dataGridView1.CurrentCell.ColumnIndex == 1)
            {
                TextBox textBox = e.Control as TextBox;
                if(textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
             
                }
            }

        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editinRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[3, rowIndex] = linkCell;

                    editinRow.Cells["Delete"].Value = "Update";
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }







        public void setValueRow(string row1, string row2, int index)
        {
            try
            {
                DataGridViewRow row = dataGridView1.Rows[index];
                dataGridView1.Rows[index].SetValues(new object[] { 1, "2", 3.01D, DateTime.Now, null });
                row.Cells["Count"].Value = row1;

                row.Cells["description"].Value = row2;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
        }

        private void SaveBtn_Click_1(object sender, EventArgs e)
        {
            saveData();

        }

        private void button4_Click(object sender, EventArgs e)
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

       

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            //if (dr == DialogResult.Cancel)
            //{
            //    return;
            //}
        }


    }
    
}
