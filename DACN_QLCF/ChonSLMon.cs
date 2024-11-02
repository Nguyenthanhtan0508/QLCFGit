using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DACN_QLCF
{
    public partial class ChonSLMon : Form
    {
        public ChonSLMon()
        {
            InitializeComponent();
        }

        private void b1_Click(object sender, EventArgs e)
        {
            setNumber(1);
        }

        public void setNumber(int soluong)
        {
            if (textBox1.Text == "0")
                textBox1.Clear();
            textBox1.Text += soluong;
        }

        private void b2_Click(object sender, EventArgs e)
        {
            setNumber(2);
        }

        private void b3_Click(object sender, EventArgs e)
        {
            setNumber(3);
        }

        private void b4_Click(object sender, EventArgs e)
        {
            setNumber(4);
        }

        private void b5_Click(object sender, EventArgs e)
        {
            setNumber(5);
        }

        private void b6_Click(object sender, EventArgs e)
        {
            setNumber(6);
        }

        private void b7_Click(object sender, EventArgs e)
        {
            setNumber(7);
        }

        private void b8_Click(object sender, EventArgs e)
        {
            setNumber(8);
        }

        private void b9_Click(object sender, EventArgs e)
        {
            setNumber(9);
        }

        private void b0_Click(object sender, EventArgs e)
        {
            textBox1.Text += "0";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "-1";
            this.Close();
        }

        private void ChonSLMon_Load(object sender, EventArgs e)
        {
            textBox1.Text = "0";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text += "000";
        }
    }
}
