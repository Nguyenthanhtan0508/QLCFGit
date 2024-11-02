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
    public partial class PhieuKiemKho : Form
    {
        DataClasses2DataContext data = new DataClasses2DataContext();
        public int maNV;
        public PhieuKiemKho()
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
            BindGridNL(data.NguyenLieus.ToList());
            label1.Text = DateTime.Today.ToString();
            btHuy.Enabled = false;
            btTao.Enabled = false;
        }
        public void BindGridNL(List<NguyenLieu> listNL)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listNL)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaNL;
                dataGridView1.Rows[index].Cells[1].Value = item.TenNL;
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
                else if(int.Parse(chonSLMon.textBox1.Text.ToString()) == 0)
                {
                    dataGridView2.Rows[e.RowIndex].Cells["SoLuong"].Value = "0";
                }
                else
                    dataGridView2.Rows.RemoveAt(e.RowIndex);
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
            PhieuKiemHangTon newPKK = new PhieuKiemHangTon();
            newPKK.MaPhieuKiem = data.PhieuKiemHangTons.Count() + 1;
            newPKK.TenPhieu = "Phiếu kiểm kho ngày " + DateTime.Today.Date + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
            newPKK.NgayKiemTra = DateTime.Today;
            newPKK.NgayLapPhieu = DateTime.Today;
            newPKK.MaNV = maNV;
            data.PhieuKiemHangTons.InsertOnSubmit(newPKK);
            data.SubmitChanges();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                CT_KT CTKT = new CT_KT();
                CTKT.MaPhieuKiem = newPKK.MaPhieuKiem;
                CTKT.MaNL = (int)dataGridView2.Rows[i].Cells[0].Value;
                CTKT.SoLuongCon = int.Parse(dataGridView2.Rows[i].Cells["SoLuong"].Value.ToString());
                data.CT_KTs.InsertOnSubmit(CTKT);
            }
            data.SubmitChanges();
            MessageBox.Show("Tạo Phiếu Nhập Thành Công! ");
            dataGridView2.Rows.Clear();
            btHuy.Enabled = false;
            btTao.Enabled = false;
            InPhieuKiemKho inPK = new InPhieuKiemKho();
            inPK.maPK = newPKK.MaPhieuKiem;
            inPK.Show();
        }
    }
}
