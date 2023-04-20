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
    public partial class Form2 : Form
    {
        private SqlConnection sqlCon = null;
        private SqlDataAdapter sqlDataAdap = null;
        DataSet users = new DataSet();
        public string gpassword;
        public string glogin;
        Form1 frm1 = new Form1();
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vladb\source\repos\kursa4\kursa4\Database1.mdf;Integrated Security=True");
            sqlCon.Open();
            sqlDataAdap = new SqlDataAdapter("SELECT * FROM Users" , sqlCon);
            sqlDataAdap.Fill(users, "Users");
            
            string log = textBox1.Text;
            string pass = textBox2.Text;
            for(int i=0; i<users.Tables["Users"].Columns.Count-1; i++)
            {
                glogin = users.Tables["Users"].Rows[i][1].ToString();
                gpassword = users.Tables["Users"].Rows[i][2].ToString();
                string acces = users.Tables["Users"].Rows[i][3].ToString();
                if (log == glogin && pass == gpassword)
                {
                    
                    if (acces == "True")
                    {
                        frm1.Show();
                        frm1.label1.Text = glogin;
                        frm1.label2.Text = "Admin";
                        this.Hide();
                    }
                    else if(acces== "False")
                    {
                        frm1.Show();
                        frm1.label1.Text = glogin;
                        frm1.label2.Text = "User";
                    }
                    else
                    {
                        MessageBox.Show("Вообще не пон", "Что-то пошло не так", MessageBoxButtons.OK);
                    }

                    frm1.checkAdmin();


                    return;
                }
                
            }
            label3.Visible = true;
            label3.ForeColor = Color.FromName("Red");
            label3.Text = "Вы неверно ввели логин или пароль";
            
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button2.FlatStyle = FlatStyle.Popup;
            button1.Enabled = true;
            button1.FlatStyle = FlatStyle.Flat;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            button1.Enabled = false;
            button1.FlatStyle = FlatStyle.Popup;
            button2.Enabled = true;
            button2.FlatStyle = FlatStyle.Flat ;
        }
    }
}
