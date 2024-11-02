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
    public partial class DangNhap : Form
    {
        DataClasses2DataContext data = new DataClasses2DataContext();
        public DangNhap()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                var find = data.NhanViens.FirstOrDefault(p => p.MaNV == int.Parse(textBox1.Text.ToString()));
                if (find != null)
                {
                    Order order = new Order();
                    order.maNV = int.Parse(textBox1.Text.ToString());
                    order.Show();
                    this.Hide();
                    order.FormClosing += (o, x) =>
                    {
                        this.Show();
                    };
                }
                else
                {
                    MessageBox.Show("Bạn không phải nv quán?");
                }
            }  
            else
                MessageBox.Show("Bạn không phải nv quán?");
        }
    }
}
