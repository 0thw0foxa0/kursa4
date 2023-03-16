using System.Windows.Forms;

namespace kursa4
{
    public partial class Form3 : Form
    {
        public int SelectedIndex =0;
        public Form3()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Form1 frm1 = new Form1();
            
            frm1.ChooseColor(SelectedIndex);
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            SelectedIndex = comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            
        }
    }
}
