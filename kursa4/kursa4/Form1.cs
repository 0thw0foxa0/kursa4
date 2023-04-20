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
        public DataSet dataSet = null;
        private DataSet dataSettmp = null;
        public bool isAdmin = false;
        public bool newRowAdding = false;
        public string TableName = "Items";
        int TableIndex = 9;
        int[] colornow1= {39,39,58 };
        int[] colornow2= {51,51,72 };
        string colortextnow="White";
        public int IndexPaint = 0;
        public bool IsMain = true;
        Form3 frm3 = new Form3();
        public Form1()
        {
            InitializeComponent();
            frm3.button1.Click += (sender1, el) =>
              {
                  ChooseColor(frm3.comboBox1.SelectedIndex);
              };
            
            
    }
        private void Form1_Load(object sender, EventArgs e)
        {
           
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vladb\source\repos\kursa4\kursa4\Database1.mdf;Integrated Security=True");
            sqlConnection.Open();    
            radioButton1.Checked = true;
        }
        
        public void items()
        {
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT *, 'Delete' AS[Command] FROM Items LEFT JOIN Category ON [Id category]=[Category Id]";
            sqlDataAdapter = new SqlDataAdapter(cmd);
        }
        public void Operation()
        {
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "SELECT *, 'Delete' AS[Command] FROM Operation LEFT JOIN [Types Operation] ON [type Operation]=[Id type]";
            sqlDataAdapter = new SqlDataAdapter(cmd);
        }

        private void LoadData()
        {
            
            try
            {
                if (sqlBuilder != null){
                    sqlBuilder = null;
                    dataSet = null;
                }
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM " + TableName, sqlConnection);
                dataSettmp = new DataSet();
                sqlDataAdapter.Fill(dataSettmp, TableName);

                if (TableName=="Items")
                {
                    items();
                    dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet, TableName);
                    dataGridView1.DataSource = dataSet.Tables[TableName];
                    dataGridView1.Columns["Command"].DisplayIndex = TableIndex;
                    dataGridView1.Columns["Id category"].Visible = false;
                    dataGridView1.Columns["Id"].ReadOnly = true;
                    dataGridView1.Columns["category"].ReadOnly = true;
                }
                else if (TableName == "Operation")
                {
                    Operation();
                    dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet, TableName);
                    dataGridView1.DataSource = dataSet.Tables[TableName];
                    dataGridView1.Columns["Command"].DisplayIndex = TableIndex;
                    dataGridView1.Columns["Id type"].Visible = false;

                }
                else
                {
                    dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet, TableName);
                    dataGridView1.DataSource = dataSet.Tables[TableName];
                    dataGridView1.Columns["Command"].DisplayIndex = TableIndex;
                }
                

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[TableIndex, i] = linkCell;
                    dataGridView1.Rows[i].Cells[TableIndex].Style.ForeColor = Color.White;
                }
               

                PaintElementsDark(colornow1, colornow2, colortextnow);

             
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Что-то пошло не так", MessageBoxButtons.OK);
            }
}


        //public void saveData()
        //{
        //    try
        //    {
        //        dataSet.Tables[TableName].Clear();
        //        dataSettmp.Tables[TableName].Clear();
        //        sqlDataAdapter.Fill(dataSettmp, TableName);
        //        dataGridView1.DataSource = dataSettmp.Tables[TableName];
        //        for (int i = 0; i < dataGridView1.Rows.Count; i++)
        //        {
        //            DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
        //            dataGridView1[TableIndex, i] = linkCell;
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
        //    }
           
        //}

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "Items";
            TableIndex = 9;
            LoadData();
            this.Text = "Таблица Товары";

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "Operation";
            TableIndex = 5;
            LoadData();
            this.Text = "Таблица Операции";

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "[Types Operation]";
            TableIndex = 2;
            LoadData();
            this.Text = "Таблица типы операций";
        }

        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            TableName = "Category";
            TableIndex = 2;
            LoadData();
            this.Text = "Таблица Категории";
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

                    dataGridView1[TableIndex, lastRow] = linkCell;

                    row.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
        }

        private void btnLogTable_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            if (IsMain)
            {
                TableName = "Users";
                TableIndex = 4;
                LoadData();
                this.Text = "Таблица Пользователи";
            }
            else
            {
                panel3.Dock = DockStyle.Top;
                panel3.Controls.Add(tableLayoutPanel2);
                panel2.Controls.Remove(frm3);
                IsMain = true;
                TableName = "Users";
                TableIndex = 4;
                LoadData();
                this.Text = "Таблица Пользователи";
                
            }
            

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex== TableIndex)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[TableIndex].Value.ToString();
                    if (task == "Delete")
                    {
                        DialogResult dr = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (dr == DialogResult.Cancel)
                        {
                            return;
                        }
                        else
                        {
                            
                       
                            int rowIndex = e.RowIndex;
                            dataGridView1.DataSource = dataSet.Tables[TableName];
                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM " + TableName, sqlConnection);
                            sqlDataAdapter.Fill(dataSet, TableName);
                            sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                            sqlBuilder.GetInsertCommand();

                            sqlBuilder.GetUpdateCommand();

                            sqlBuilder.GetDeleteCommand();

                            dataSet.Tables[TableName].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, TableName);

                            LoadData();

                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        //DataRow row = dataSettmp.Tables[TableName].NewRow();
                        dataSet = new DataSet();
                        sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM " + TableName, sqlConnection);
                        sqlDataAdapter.Fill(dataSet, TableName);
                        sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                        sqlBuilder.GetInsertCommand();

                        sqlBuilder.GetUpdateCommand();

                        sqlBuilder.GetDeleteCommand();

                        dataSet.Tables[TableName].Rows.Add();

                        if (TableName=="Items")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Count"] = dataGridView1.Rows[rowIndex].Cells["Count"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["description"] = dataGridView1.Rows[rowIndex].Cells["description"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Cost"] = dataGridView1.Rows[rowIndex].Cells["Cost"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Size"] = dataGridView1.Rows[rowIndex].Cells["Size"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Weight"] = dataGridView1.Rows[rowIndex].Cells["Weight"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Category Id"] = dataGridView1.Rows[rowIndex].Cells["Category Id"].Value;
                        }
                        else if (TableName=="Operation")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Item Id"] = dataGridView1.Rows[rowIndex].Cells["Item Id"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["type Operation"] = dataGridView1.Rows[rowIndex].Cells["type Operation"].Value;

                            //DateTime dt = DateTime.Parse(row["Date"].ToString());
                            //row["Date"] = dt.ToShortDateString();
                        }
                        else if (TableName == "Category")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Id category"] = dataGridView1.Rows[rowIndex].Cells["Id category"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["category"] = dataGridView1.Rows[rowIndex].Cells["category"].Value;
                        }
                        else if(TableName=="[Types Operation]")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["type"] = dataGridView1.Rows[rowIndex].Cells["type"].Value;
                        }
                        dataGridView1.Rows[e.RowIndex].Cells[TableIndex].Value = "Delete";
                        sqlDataAdapter.Update(dataSet, TableName);
                        sqlDataAdapter.Fill(dataSet, TableName);
                        //dataSet.Tables[TableName].Rows.Add(row);

                        //dataSet.Tables[TableName].Rows.RemoveAt(dataSet.Tables[TableName].Rows.Count - 1);

                        //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        newRowAdding = false;
                        LoadData();
                    }
                    else if (task == "Update")
                    {
                        dataSet = new DataSet();
                        sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM " + TableName, sqlConnection);
                        sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);


                        sqlDataAdapter.Fill(dataSet, TableName);


                        sqlBuilder.GetInsertCommand();

                        sqlBuilder.GetUpdateCommand();

                        sqlBuilder.GetDeleteCommand();
                        int rowIndex = e.RowIndex;
                        if (TableName == "Category")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Id category"] = dataGridView1.Rows[rowIndex].Cells["Id category"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["category"] = dataGridView1.Rows[rowIndex].Cells["category"].Value;
                        }
                        else if (TableName == "Types Operation")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["type"] = dataGridView1.Rows[rowIndex].Cells["type"].Value;

                        }
                        else if (TableName == "Items")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Count"] = dataGridView1.Rows[rowIndex].Cells["Count"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["description"] = dataGridView1.Rows[rowIndex].Cells["description"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Cost"] = dataGridView1.Rows[rowIndex].Cells["Cost"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Size"] = dataGridView1.Rows[rowIndex].Cells["Size"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Weight"] = dataGridView1.Rows[rowIndex].Cells["Weight"].Value;

                            DataSet dtsettmp = new DataSet();

                            dataSet.Tables[TableName].Rows[rowIndex]["Category Id"] = dataGridView1.Rows[rowIndex].Cells["Category Id"].Value;
                        }
                        else if (TableName == "Operation")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Item Id"] = dataGridView1.Rows[rowIndex].Cells["Item Id"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Type operation"] = dataGridView1.Rows[rowIndex].Cells["Type operation"].Value;
                        }
                        else if (TableName == "Users")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Login"] = dataGridView1.Rows[rowIndex].Cells["Login"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Password"] = dataGridView1.Rows[rowIndex].Cells["Password"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Admin"] = dataGridView1.Rows[rowIndex].Cells["Admin"].Value;
                        }
                            //dataSet = new DataSet();
                            //sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM " + TableName, sqlConnection);
                            //sqlDataAdapter.Fill(dataSet, TableName);
                            //sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                            //sqlBuilder.GetInsertCommand();

                            //sqlBuilder.GetUpdateCommand();

                            //sqlBuilder.GetDeleteCommand();

                            sqlDataAdapter.Update(dataSet, TableName);
                            dataGridView1.Rows[e.RowIndex].Cells[TableIndex].Value = "Delete";

                        LoadData();

                    }
                    
                    else
                    {
                        MessageBox.Show("дадааа", "Что-то пошло не так", MessageBoxButtons.YesNo);
                    }
                    
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
            if (TableName=="Items")
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 6)
                {
                    TextBox textBox = e.Control as TextBox;
                    if (textBox != null)
                    {
                        textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);

                    }
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

                    dataGridView1[TableIndex, rowIndex] = linkCell;

                    editinRow.Cells["Command"].Value = "Update";
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) )
            {
                e.Handled = true;
            }
        }


       public void PaintElementsDark(int[] paint1, int[]paint2,string color )
        {
            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {
                // Iterate over the total number of Columns
                for (int col = 0; col <dataGridView1.ColumnCount; col++)
                {
                    // Paint cell location (column, row)
                    dataGridView1[col, row].Style.BackColor = Color.FromArgb(paint1[0],paint1[1],paint1[2]);
                    dataGridView1[col, row].Style.ForeColor = Color.FromName(color);
                }
            }
       
        
            panelLogo.BackColor = Color.FromArgb(paint1[0], paint1[1], paint1[2]);
            tableLayoutPanel2.BackColor = Color.FromArgb(paint2[0], paint2[1], paint2[2]);
            tableLayoutPanel2.ForeColor = Color.FromName(color);
            btnMainPage.ForeColor = Color.FromName(color);
            btnLogOut.ForeColor = Color.FromName(color);
            btnQuit.ForeColor = Color.FromName(color);
            radioButton1.BackColor= Color.FromArgb(paint2[0], paint2[1], paint2[2]);
            btnSettingsPage.ForeColor = Color.FromName(color);
            dataGridView1.BackgroundColor = Color.FromArgb(paint2[0], paint2[1], paint2[2]);
            panelMenu.BackColor = Color.FromArgb(paint2[0], paint2[1], paint2[2]);
            label1.ForeColor = Color.FromName(color);


            frm3.panel1.BackColor= Color.FromArgb(paint2[0], paint2[1], paint2[2]);
            frm3.comboBox1.BackColor= Color.FromArgb(paint1[0], paint1[1], paint1[2]);
            frm3.comboBox2.BackColor= Color.FromArgb(paint1[0], paint1[1], paint1[2]);
            frm3.comboBox1.ForeColor = Color.FromName(color);
            frm3.comboBox2.ForeColor = Color.FromName(color);
            frm3.label1.ForeColor = Color.FromName(color);
            frm3.label2.ForeColor = Color.FromName(color);
            frm3.label3.ForeColor = Color.FromName(color);

            colornow1 = paint1;
            colornow2 = paint2;
            colortextnow = color;
        }




        public void ChooseColor(int CurrentIndex)

        {
            if (CurrentIndex == IndexPaint)
            {
                return;
            }
            if (CurrentIndex == 0)
            {
                string color = "White";
                int[] painting1 = { 39, 39, 58 };
                int[] painting2 = { 51, 51, 72 };
                PaintElementsDark(painting1, painting2, color);


            }
            else if (CurrentIndex == 1)
            {
                string color = "Black";
                int[] painting1 = { 230, 255, 255 };
                int[] painting2 = { 210, 245, 245 };
                PaintElementsDark(painting1, painting2, color);

            }
            
            
            else if (CurrentIndex == 2)
            {
                Random rnd = new Random();

                int tmp1 = rnd.Next(0, 255);
                int tmp2 = rnd.Next(0, 255);
                int tmp3 = rnd.Next(0, 255);
                int[] painting1 = { tmp1, tmp2, tmp3 };
                 tmp1 = rnd.Next(0, 255);
                 tmp2 = rnd.Next(0, 255);
                 tmp3 = rnd.Next(0, 255);
                int[] painting2 = { tmp1, tmp2, tmp3 };
                string color = "Grey";
                PaintElementsDark(painting1, painting2,color);
            }
            IndexPaint = CurrentIndex;
        }





        

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

       

       

        private void btnSettingsPage_Click_1(object sender, EventArgs e)
        {
            frm3.TopLevel = false;
            frm3.FormBorderStyle = FormBorderStyle.None;
            frm3.Dock = DockStyle.Fill;
            panel3.Dock = DockStyle.None;
            panel2.Controls.Add(frm3);
            frm3.BringToFront();
            frm3.Show();
            IsMain = false;
            this.Text = "Настройки";
        }

        private void btnMainPage_Click(object sender, EventArgs e)
        {
            panel3.Dock = DockStyle.Top;
            panel3.Controls.Add(tableLayoutPanel2);
            panel2.Controls.Remove(frm3);
            IsMain = true;
            switch (TableName)
            {
                case "Users":
                    this.Text = "Таблица Пользователи";
                break;
                case "Operation":
                    this.Text = "Таблица Операции";
                break;
                case "[Type Operation]":
                    this.Text = "Таблица типы операций";
                break;
                case "Category":
                    this.Text = "Таблица категории";
                break;
                case "Items":
                    this.Text = "Таблица товары";
                break;
            }
            
        }
        public void checkAdmin()
        {
            if (label2.Text.ToString() == "Admin")
            {
                btnLogTable.Visible = true;
            }
            else if (label2.Text.ToString() == "User")
            {
                btnLogTable.Visible = false;
            }
        }

        public void panelLogo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        { }

        private void panel2_Paint(object sender, PaintEventArgs e)
        { }
        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        { }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        { }

        private void btnSettingsPage_Click(object sender, EventArgs e)
        { }

        public void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            this.Hide();
            frm2.Show();
        }
    }
}