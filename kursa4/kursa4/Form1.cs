﻿using System;
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

        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        public bool newRowAdding = false;
        public string TableName = "Items";
        int[] IndexCommand = new int[3] {3,4,6};
        public int TableIndex = 0;

            
        

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
                if (dataGridView1 != null)
                {
                  
                    dataGridView1.Refresh();
                }
                
                    sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM " + TableName, sqlConnection);

                    sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                    sqlBuilder.GetInsertCommand();

                    sqlBuilder.GetUpdateCommand();

                    sqlBuilder.GetDeleteCommand();

                    dataSet = new DataSet();

                    sqlDataAdapter.Fill(dataSet, TableName);

                dataGridView1.DataSource = dataSet.Tables[TableName];

                    for( int i=0; i<IndexCommand[TableIndex]; i++)
                    {
                        DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                        dataGridView1[IndexCommand[TableIndex], i] = linkCell;
                    }
                label1.Text = Convert.ToString(IndexCommand[TableIndex]);


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
                dataSet.Tables[TableName].Clear();
                sqlDataAdapter.Fill(dataSet, TableName);
                dataGridView1.DataSource = dataSet.Tables[TableName];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[IndexCommand[TableIndex], i] = linkCell;
                }
                //

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

                    dataGridView1[IndexCommand[TableIndex], lastRow] = linkCell;

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
                if(e.ColumnIndex== IndexCommand[TableIndex])
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[IndexCommand[TableIndex]].Value.ToString();
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

                            dataSet.Tables[TableName].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, TableName);
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables[TableName].NewRow();
                        if (TableIndex == 0)
                        {
                            row["Count"] = dataGridView1.Rows[rowIndex].Cells["Count"].Value;
                            row["description"] = dataGridView1.Rows[rowIndex].Cells["description"].Value;

                        }

                        dataSet.Tables[TableName].Rows.Add(row);

                        dataSet.Tables[TableName].Rows.RemoveAt(dataSet.Tables[TableName].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[IndexCommand[TableIndex]].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, TableName);
                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int rowIndex = e.RowIndex;
                        if (TableIndex == 0)
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Count"] = dataGridView1.Rows[rowIndex].Cells["Count"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["description"] = dataGridView1.Rows[rowIndex].Cells["description"].Value;

                        }

                        sqlDataAdapter.Update(dataSet, TableName);
                        dataGridView1.Rows[e.RowIndex].Cells[IndexCommand[TableIndex]].Value = "Delete";
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
            TableName = "Items";
            TableIndex = 0;
            LoadData();
            saveData();
        }

       

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSettingsPage_Click(object sender, EventArgs e)
        {
            // panel3 // Основная панель
            Form3 frm3 = new Form3() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            panel3.Controls.Clear();
            panel3.Controls.Add(frm3);
            frm3.Show();

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "Requsts";
            TableIndex = 1;
            LoadData();
            
    }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "Test";
            TableIndex = 2;
            LoadData();
        }
    }
    
}
//public void changeRow(string row1, string row2, int index)
//{
//    try
//    {
//        //dataSet.Tables[TableName].Rows[index]["Count"] = row1;
//        //dataSet.Tables[TableName].Rows[index]["description"] = row2;

//        int RowIndex = index;
//        //dataGridView1.Rows[RowIndex].Cells["description"].Value = row2;
//        //dataGridView1.Rows[RowIndex].Cells["Count"].Value = row1;


//        dataSet.Tables[TableName].Rows[RowIndex]["Count"] = dataGridView1.Rows[RowIndex].Cells["Count"].Value;
//        dataSet.Tables[TableName].Rows[RowIndex]["description"] = dataGridView1.Rows[RowIndex].Cells["description"].Value;

//        sqlDataAdapter.Update(dataSet, TableName);
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);

//    }

//}
//private void button2_Click(object sender, EventArgs e) // del
//{
//    DialogResult dr = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
//    if (dr == DialogResult.Cancel)
//    {
//        return;
//    }
//    else
//    {

//        int index = dataGridView1.SelectedRows[0].Index;
//        dataGridView1.Rows.RemoveAt(index);
//        dataSet.Tables[TableName].Rows[index].Delete();
//        sqlDataAdapter.Update(dataSet, TableName);
//    }
//}


//private void button3_Click(object sender, EventArgs e)
//{
//    try
//    {
//        int index = dataGridView1.SelectedRows[0].Index;
//        if (index >= 0)
//        {
//            DataGridViewRow row = this.dataGridView1.Rows[index];
//            string row1 = row.Cells["Count"].Value.ToString();
//            string row2 = row.Cells["description"].Value.ToString();

//            Form2 frm2 = new Form2();
//            frm2.Show();
//            frm2.getValueRows(row1, row2, index);

//        }
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
//    }

//}

//private void button4_Click(object sender, EventArgs e)
//{

//}



//private void SaveBtn_Click_1(object sender, EventArgs e)
//{
//    try
//    {
//        saveData();
//        MessageBox.Show("Вроде сохранилось", "Сохранилось?", MessageBoxButtons.OK, MessageBoxIcon.Warning);

//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
//    }


//}