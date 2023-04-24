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
        private DateTimePicker date;
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        public DataSet dataSet = null;
        private DataSet dataSettmp = null;
        public bool isAdmin = false;
        public bool newRowAdding = false;
        public string TableName = "Items";
        int TableIndex = 9;
        Color color1 = Properties.Settings.Default.color1;
        Color color2 = Properties.Settings.Default.color2;
        Color textColor = Properties.Settings.Default.textcolor;
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
                    dataGridView1.Columns["Count"].ReadOnly = true;
                }
                else if (TableName == "Operation")
                {
                    Operation();
                    dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet, TableName);
                    dataGridView1.DataSource = dataSet.Tables[TableName];
                    dataGridView1.Columns["Command"].DisplayIndex = TableIndex;
                    dataGridView1.Columns["Id type"].Visible = false;
                    dataGridView1.Columns["Count"].ReadOnly = false;
                    dataGridView1.Columns["type"].ReadOnly = true;

                }
                
                else
                {
                    
                    dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet, TableName);
                    dataGridView1.DataSource = dataSet.Tables[TableName];
                    dataGridView1.Columns["Command"].DisplayIndex = TableIndex;
                    if (TableName == "[Types Operation]")
                    {
                        dataGridView1.Columns["type"].ReadOnly = false;
                    }
                }               
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[TableIndex, i] = linkCell;
                    dataGridView1.Rows[i].Cells[TableIndex].Style.ForeColor = Color.White;
                }
                PaintElementsDark(color1, color2, textColor);           
            }     
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Что-то пошло не так", MessageBoxButtons.OK);
            }
}
        public string[] convert (string myString)
        {
            return new[] { myString };
        }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "Items";
            TableIndex = 9;
            LoadData();
            this.Text = "Таблица Товары";
            if (newRowAdding == true)
            {
                newRowAdding = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "Operation";
            TableIndex = 7;
            LoadData();
            this.Text = "Таблица Операции";
            if (newRowAdding == true)
            {
                newRowAdding = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            TableName = "[Types Operation]";
            TableIndex = 2;
            LoadData();
            this.Text = "Таблица типы операций";
            if (newRowAdding == true)
            {
                newRowAdding = false;
            }
        }

        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            TableName = "Category";
            TableIndex = 2;
            LoadData();
            this.Text = "Таблица Категории";
            if(newRowAdding == true)
            {
                newRowAdding = false;
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
            if (newRowAdding == true)
            {
                newRowAdding = false;
            }
        }

        public void addData( int count, int id)
        {
            sqlDataAdapter = new SqlDataAdapter("UPDATE Items SET Count = Count +" +count+ " WHERE Id = "+id, sqlConnection);
            sqlDataAdapter.Fill(dataSet, TableName);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4 && TableName == "Operation")
                {
                    date = new DateTimePicker();
                    dataGridView1.Controls.Add(date);
                    date.Format = DateTimePickerFormat.Short;
                    Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex,e.RowIndex, true);
                    date.Size = new Size(rectangle.Width, rectangle.Height);
                    date.Location = new Point(rectangle.X, rectangle.Y);
                    date.CloseUp += new EventHandler(datetimepicker_closeup);
                    date.TextChanged += new EventHandler(datetimepicker_textchanged);
                    date.Visible = true;

                }  

                else if (e.ColumnIndex == TableIndex)
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
                        dataSet = new DataSet();
                        sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM " + TableName, sqlConnection);
                        sqlDataAdapter.Fill(dataSet, TableName);
                        sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                        sqlBuilder.GetInsertCommand();
                        sqlBuilder.GetUpdateCommand();
                        sqlBuilder.GetDeleteCommand();
                        dataSet.Tables[TableName].Rows.Add();
                        if (TableName == "Items")
                        {
                            dataGridView1.Rows[e.RowIndex].Cells["Count"].Value = 0;
                            dataSet.Tables[TableName].Rows[rowIndex]["Count"] = dataGridView1.Rows[rowIndex].Cells["Count"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["description"] = dataGridView1.Rows[rowIndex].Cells["description"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Cost"] = dataGridView1.Rows[rowIndex].Cells["Cost"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Size"] = dataGridView1.Rows[rowIndex].Cells["Size"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Weight"] = dataGridView1.Rows[rowIndex].Cells["Weight"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Category Id"] = dataGridView1.Rows[rowIndex].Cells["Category Id"].Value;
                        }
                        else if (TableName == "Operation")
                        {
                            string cellValue = dataGridView1.Rows[rowIndex].Cells["Item Id"].Value.ToString();
                            string count1 = dataGridView1.Rows[rowIndex].Cells["count"].Value.ToString();
                            int b = System.Convert.ToInt32(cellValue);
                            int count = System.Convert.ToInt32(count1);
                            string idoper = dataGridView1.Rows[rowIndex].Cells["type Operation"].Value.ToString();
                            int idoper1 = System.Convert.ToInt32(idoper);
                            if (idoper1 == 1)
                            {
                                addData(count, b);
                            }
                            else if (idoper1 == 2)
                            {
                                addData(0 - count, b);
                            }
                            sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM " + TableName, sqlConnection);
                            dataSet = new DataSet();
                            sqlDataAdapter.Fill(dataSet, TableName);
                            dataSet.Tables[TableName].Rows.Add();
                            dataSet.Tables[TableName].Rows[rowIndex]["Item Id"] = dataGridView1.Rows[rowIndex].Cells["Item Id"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["type Operation"] = dataGridView1.Rows[rowIndex].Cells["type Operation"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["date"] = dataGridView1.Rows[rowIndex].Cells["date"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["count"] = dataGridView1.Rows[rowIndex].Cells["count"].Value;
                        }
                        else if (TableName == "Category")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Id category"] = dataGridView1.Rows[rowIndex].Cells["Id category"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["category"] = dataGridView1.Rows[rowIndex].Cells["category"].Value;
                        }
                        else if (TableName == "[Types Operation]")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["type"] = dataGridView1.Rows[rowIndex].Cells["type"].Value;
                        }
                        else if (TableName == "Users")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Login"] = dataGridView1.Rows[rowIndex].Cells["Login"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Password"] = dataGridView1.Rows[rowIndex].Cells["Password"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Admin"] = dataGridView1.Rows[rowIndex].Cells["Admin"].Value;
                        }
                        dataGridView1.Rows[e.RowIndex].Cells[TableIndex].Value = "Delete";
                        sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                        sqlBuilder.GetInsertCommand();
                        sqlBuilder.GetUpdateCommand();
                        sqlBuilder.GetDeleteCommand();
                        sqlDataAdapter.Update(dataSet, TableName);
                        sqlDataAdapter.Fill(dataSet, TableName);
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
                            dataSet.Tables[TableName].Rows[rowIndex]["date"] = dataGridView1.Rows[rowIndex].Cells["date"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["count"] = dataGridView1.Rows[rowIndex].Cells["count"].Value;  
                        }
                        else if (TableName == "Users")
                        {
                            dataSet.Tables[TableName].Rows[rowIndex]["Login"] = dataGridView1.Rows[rowIndex].Cells["Login"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Password"] = dataGridView1.Rows[rowIndex].Cells["Password"].Value;
                            dataSet.Tables[TableName].Rows[rowIndex]["Admin"] = dataGridView1.Rows[rowIndex].Cells["Admin"].Value;
                        }
                        sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                        sqlBuilder.GetInsertCommand();

                        sqlBuilder.GetUpdateCommand();

                        sqlBuilder.GetDeleteCommand();
                        sqlDataAdapter.Update(dataSet, TableName);
                        dataGridView1.Rows[e.RowIndex].Cells[TableIndex].Value = "Delete";

                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Все сломалось", "Что-то пошло не так", MessageBoxButtons.YesNo);
                    }

                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }  
        }

        private void datetimepicker_textchanged(object sender, EventArgs e)
        {
            try
            {
             dataGridView1.CurrentCell.Value = date.Text.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK);
            }
        }

        private void datetimepicker_closeup(object sender, EventArgs e)
        {
            date.Visible = false;
            date = null;
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            if (TableName=="Items")
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 6 || dataGridView1.CurrentCell.ColumnIndex == 3)
                {
                    TextBox textBox = e.Control as TextBox;
                    if (textBox != null)
                    {
                        textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                    }
                }
            }  
            else if(TableName== "Operation")
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 0 || dataGridView1.CurrentCell.ColumnIndex == 1 || dataGridView1.CurrentCell.ColumnIndex == 2 || dataGridView1.CurrentCell.ColumnIndex == 3)
                {
                    TextBox textBox = e.Control as TextBox;
                    if (textBox != null)
                    {
                        textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                    }
                }
            }
            else if (TableName == "[Types Operation]")
            {
                if (dataGridView1.CurrentCell.ColumnIndex == 0)
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

       public void PaintElementsDark(Color paint1, Color paint2,Color color )
        { 
            for (int row = 0; row < dataGridView1.Rows.Count; row++)
            {       
                for (int col = 0; col <dataGridView1.ColumnCount; col++)
                {          
                    dataGridView1[col, row].Style.BackColor = paint1;
                    dataGridView1[col, row].Style.ForeColor = color;
                }
            }
            panelLogo.BackColor = paint1;
            tableLayoutPanel2.BackColor = paint2;
            tableLayoutPanel2.ForeColor = color;
            btnMainPage.ForeColor = color;
            btnLogOut.ForeColor = color;
            btnLogTable.ForeColor = color;
            btnQuit.ForeColor = color;
            radioButton1.BackColor= paint2;
            btnSettingsPage.ForeColor = color;
            dataGridView1.BackgroundColor = paint2;
            panelMenu.BackColor = paint2;
            label1.ForeColor = color;
            label2.ForeColor = color;
            frm3.panel1.BackColor = paint2;
            frm3.comboBox1.BackColor = paint1;
            frm3.comboBox2.BackColor = paint1;
            frm3.comboBox1.ForeColor = color;
            frm3.comboBox2.ForeColor = color;
            frm3.label1.ForeColor = color;
            frm3.label2.ForeColor = color;
            frm3.label3.ForeColor = color;
            color1 = Properties.Settings.Default.color1 =paint1;
            Properties.Settings.Default.color1 = paint1;
            color2 = Properties.Settings.Default.color2=paint2;
            Properties.Settings.Default.color2 = paint2;
            textColor = Properties.Settings.Default.textcolor=color;
            Properties.Settings.Default.textcolor = color;
            Properties.Settings.Default.Save();
        }

        public void ChooseColor(int CurrentIndex)
        {
            if (CurrentIndex == 0)
            {
                Color color = Color.FromName("White");
                Color painting1 = Color.FromArgb( 39, 39, 58 );
                Color painting2 = Color.FromArgb(51, 51, 72);
                PaintElementsDark(painting1, painting2, color);
            }
            else if (CurrentIndex == 1)
            {
                Color color = Color.FromName("Black");
                Color painting1 = Color.FromArgb(230, 255, 255 );
                Color painting2 = Color.FromArgb(210, 245, 245 );
                PaintElementsDark(painting1, painting2, color);
            }
            
            
            else if (CurrentIndex == 2)
            {
                Random rnd = new Random();

                int tmp1 = rnd.Next(0, 255);
                int tmp2 = rnd.Next(0, 255);
                int tmp3 = rnd.Next(0, 255);
                Color painting1 = Color.FromArgb( tmp1, tmp2, tmp3 );
                 tmp1 = rnd.Next(0, 255);
                 tmp2 = rnd.Next(0, 255);
                 tmp3 = rnd.Next(0, 255);
                Color painting2 = Color.FromArgb( tmp1, tmp2, tmp3 );
                Color color = Color.FromName("Grey");
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
        { }

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
        { }

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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.color1 = color1;
            Properties.Settings.Default.color2 = color1;
            Properties.Settings.Default.textcolor = textColor;
        }
    }
}