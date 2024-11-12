using System;
using System.Windows.Forms;
namespace GDKlaba3WordSearcher
{
    public partial class Avtorizatsiya : Form
    {
        public Avtorizatsiya()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "1" && textBox2.Text == "1")
            {
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!!!");
            }
        }
    }
}
