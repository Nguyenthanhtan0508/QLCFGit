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
    public partial class PhieuOrder : Form
    {
        DataClasses2DataContext data = new DataClasses2DataContext();
        public int maNV;
        public PhieuOrder()
        {
            InitializeComponent();
        }

        private void PhieuNhapHang_Load(object sender, EventArgs e)
        {
            var listNCC = data.NhaCCs.ToList();
            foreach(var item in listNCC)
            {
                item.tenNhaCC = item.TenNCC + " chi nhánh " + item.DiaChiCuThe;
            }
            FillBLCombobox(listNCC);
            BindGridNL(data.NguyenLieus.ToList());
            label1.Text = DateTime.Today.ToString();
            btHuy.Enabled = false;
            btTao.Enabled = false;
        }
        private void FillBLCombobox(List<NhaCC> listNCC)
        {
            this.comboBox1.DataSource = listNCC;
            this.comboBox1.DisplayMember = "TenNhacc";
            this.comboBox1.ValueMember = "MaNCC";
        }
        public void BindGridNL(List<NguyenLieu> listNL)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listNL)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaNL;
                dataGridView1.Rows[index].Cells[1].Value = item.TenNL;
                dataGridView1.Rows[index].Cells[2].Value = (item.HSD - item.NSX).Value.Days + " ngày";
                dataGridView1.Rows[index].Cells[3].Value = item.GiaTien;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool checkNL = false;
            if (dataGridView2.Rows.Count == 0)
            {
                int index = dataGridView2.Rows.Add();
                dataGridView2.Rows[index].Cells[0].Value = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                dataGridView2.Rows[index].Cells[1].Value = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                dataGridView2.Rows[index].Cells[2].Value = 1;
            }
            else
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[0].Value == dataGridView1.Rows[e.RowIndex].Cells[0].Value)
                    {
                        dataGridView2.Rows[i].Cells[2].Value = int.Parse(dataGridView2.Rows[i].Cells[2].Value.ToString()) + 1;
                        checkNL = true;
                    }
                }
                if (checkNL == false)
                {
                    int index = dataGridView2.Rows.Add();
                    dataGridView2.Rows[index].Cells[0].Value = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                    dataGridView2.Rows[index].Cells[1].Value = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                    dataGridView2.Rows[index].Cells[2].Value = 1;
                }
            }
            btHuy.Enabled = true;
            btTao.Enabled = true;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ChonSLMon chonSLMon = new ChonSLMon();
            chonSLMon.Show();
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;
            chonSLMon.textBox1.Enabled = false;
            chonSLMon.FormClosing += (o, x) =>
            {
                if (int.Parse(chonSLMon.textBox1.Text.ToString()) > 0)
                    dataGridView2.Rows[e.RowIndex].Cells["SoLuong"].Value = chonSLMon.textBox1.Text.ToString();
                else
                {
                    dataGridView2.Rows.RemoveAt(e.RowIndex);
                }

                dataGridView2.Enabled = true;
                dataGridView1.Enabled = true;
            };
        }

        private void btHuy_Click(object sender, EventArgs e)
        {
            btHuy.Enabled = false;
            btTao.Enabled = false;
            dataGridView2.Rows.Clear();
        }

        private void btTao_Click(object sender, EventArgs e)
        {
            PhieuNhapHang newPN = new PhieuNhapHang();
            newPN.MaPhieuNhap = data.PhieuNhapHangs.Count() + 1;
            newPN.MaNCC = (int)comboBox1.SelectedValue;
            newPN.TenPhieuNhap = textBox1.Text;
            newPN.NgayNhap = DateTime.Today;
            newPN.MaNV = maNV;
            data.PhieuNhapHangs.InsertOnSubmit(newPN);
            data.SubmitChanges();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                CTNhapHang CTNH = new CTNhapHang();
                CTNH.MaPhieuNhap = newPN.MaPhieuNhap;
                CTNH.MaNL = (int)dataGridView2.Rows[i].Cells[0].Value;
                CTNH.SoLuong = int.Parse(dataGridView2.Rows[i].Cells["SoLuong"].Value.ToString());
                data.CTNhapHangs.InsertOnSubmit(CTNH);
            }
            data.SubmitChanges();
            //MessageBox.Show("Tạo Phiếu Nhập Thành Công! ");
            dataGridView2.Rows.Clear();
            btHuy.Enabled = false;
            btTao.Enabled = false;
            textBox1.Text = "";
            InPhieuNhap inPN = new InPhieuNhap();
            inPN.maHD = newPN.MaPhieuNhap;
            inPN.Show();
        }
    }
}
