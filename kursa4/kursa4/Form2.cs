using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace kursa4
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {
          
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        public void getValueRows(string row1, string row2)
        {
            textBox1.Text = row2;
            textBox2.Text = row1;
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            

        }
    }
}
