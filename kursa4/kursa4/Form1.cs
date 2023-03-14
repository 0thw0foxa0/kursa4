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

        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        public bool newRowAdding = false;
        public string TableName = "Items";
        //public static int[] IndexCommand = { 3, 4, 6 };
        List<int> IndexCommand = new List<int>() { 3,4,6 };
        public int TableIndex = 0;
        private Form currentChildForm;
        public int IndexPaint = 0;
        public bool IsMain = true;
        Form3 frm3 = new Form3();
        public Form1()
        {
            InitializeComponent();
             
       
    }
        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vladb\source\repos\kursa4\kursa4\Database1.mdf;Integrated Security=True");

            sqlConnection.Open();

            
            radioButton1.Checked = true;
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////
        /// </summary>

        private void LoadData()
        {
            
            try
            {
                
                
                    sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM " + TableName, sqlConnection);

                    sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                    sqlBuilder.GetInsertCommand();

                    sqlBuilder.GetUpdateCommand();

                    sqlBuilder.GetDeleteCommand();

                    dataSet = new DataSet();

                    sqlDataAdapter.Fill(dataSet, TableName);

                dataGridView1.DataSource = dataSet.Tables[TableName];

                    for( int i=0; i<dataGridView1.Rows.Count; i++)
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


       public void PaintElementsDark(int[] paint1, int[]paint2 )
        {

            panelLogo.BackColor = Color.FromArgb(paint1[0],paint1[1],paint1[2]);
            tableLayoutPanel2.BackColor = Color.FromArgb(paint1[0], paint1[1], paint1[2]);
            dataGridView1.BackgroundColor = Color.FromArgb(paint2[0], paint2[1], paint2[2]);
            panelMenu.BackColor = Color.FromArgb(paint2[0], paint2[1], paint2[2]);

        }




        public void ChooseColor(int CurrentIndex)
        {
            if (CurrentIndex == IndexPaint)
            {
                return;
            }
            if (CurrentIndex == 0)
            {
                int[] painting1 = { 39, 39, 58 };
                int[] painting2 = { 51, 51, 72 };
                PaintElementsDark(painting1,painting2);
                foreach (Label l in Controls.OfType<Label>())
                {
                    l.ForeColor = Color.White;
                }
                
            }
            else if (CurrentIndex == 1)
            {
                int[] painting1 = { 230, 255, 255 };
                int[] painting2 = { 210, 245, 245 };
                PaintElementsDark(painting1, painting2);
            }
                                          

            if (CurrentIndex == 2)
            {

            }
            IndexPaint = CurrentIndex;
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
            //Form3 frm3 = new Form3() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            //panel3.Controls.Clear();
            //panel3.Controls.Add(frm3);
            //frm3.Show();
            
            /*frm3.Controls.SetChildIndex();*//*Panel1.Controls.SetChildIndex(c, zIndex - 1);*/

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "Requsts";
            TableIndex = 1;
            //int tmp = IndexCommand[0];
            //IndexCommand[0]= IndexCommand[1];
            //IndexCommand[1] = tmp;
            LoadData();
            
    }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "Test";
            TableIndex = 2;
            LoadData();
        }

        private void btnSettingsPage_Click_1(object sender, EventArgs e)
        {
          
            //tableLayoutPanel2.Visible = false;
            frm3.TopLevel = false;
            frm3.FormBorderStyle = FormBorderStyle.None;
            frm3.Dock = DockStyle.Fill;
            panel3.Dock = DockStyle.None;
            panel2.Controls.Add(frm3);
            //panel2.Tag = frm3;

            frm3.BringToFront();
            frm3.Show();
            IsMain = false;
        }

        private void btnMainPage_Click(object sender, EventArgs e)
        {
            panel3.Dock = DockStyle.Top;
            //Dock = DockStyle.Fill;
            //tableLayoutPanel2.Visible = true ;
            panel3.Controls.Add(tableLayoutPanel2);
            panel2.Controls.Remove(frm3);
            

            IsMain = true;
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