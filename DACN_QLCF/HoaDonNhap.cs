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
    public partial class TaoHoaDonNhap : Form
    {
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public int maPN;
        public int maNV;
        public TaoHoaDonNhap()
        {
            InitializeComponent();
        }

        private void TaoHoaDonNhap_Load(object sender, EventArgs e)
        {
            BindGridPNH(data.PhieuNhapHangs.ToList());
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            dataGridView3.ClearSelection();
        }
        public void BindGridPNH(List<PhieuNhapHang> listPNH)
        {
            DataClasses2DataContext data2 = new DataClasses2DataContext();
            dataGridView1.Rows.Clear();
            foreach (var item in listPNH)
            {
                var CountListHN = (from s in data2.HangNhaps
                              where s.MaPhieuNhap == item.MaPhieuNhap
                              select s).ToList().Count;
                var CountListCTPN = (from s in data2.CTNhapHangs
                                   where s.MaPhieuNhap == item.MaPhieuNhap
                                   select s).ToList().Count;
                int index = dataGridView1.Rows.Add();
                if (CountListCTPN == CountListHN)
                {
                    dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Maroon;
                    dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.White;
                }
                dataGridView1.Rows[index].Cells[0].Value = item.MaPhieuNhap;
                dataGridView1.Rows[index].Cells[1].Value = item.NhaCC.TenNCC + " chi nhánh " + item.NhaCC.DiaChiCuThe;
                dataGridView1.Rows[index].Cells[2].Value = item.TenPhieuNhap;
                dataGridView1.Rows[index].Cells[3].Value = item.NgayNhap;
            }
            
        }
        public void BindGridCTPN(List<CTNhapHang> listCTPN, int maPN)
        {
            dataGridView2.Rows.Clear();
            var listHN = (from s in data.HangNhaps
                          where s.MaPhieuNhap == maPN
                          select s).ToList();
            foreach (var item in listCTPN)
            {
                int index = dataGridView2.Rows.Add();
                foreach (var hn in listHN)
                    if(hn.MaPhieuNhap == item.MaPhieuNhap && hn.MaNL == item.MaNL)
                    {
                        dataGridView2.Rows[index].DefaultCellStyle.BackColor = Color.Maroon;
                        dataGridView2.Rows[index].DefaultCellStyle.ForeColor = Color.White;
                    }  
                dataGridView2.Rows[index].Cells[0].Value = item.MaNL;
                dataGridView2.Rows[index].Cells[1].Value = item.NguyenLieu.TenNL;
                dataGridView2.Rows[index].Cells[2].Value = (item.NguyenLieu.HSD - item.NguyenLieu.NSX).Value.Days + " ngày";
                dataGridView2.Rows[index].Cells[3].Value = item.SoLuong;
                dataGridView2.Rows[index].Cells[4].Value = item.NguyenLieu.GiaTien;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var listCTPN = (from s in data.CTNhapHangs
                            where s.MaPhieuNhap == (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value
                            select s).ToList();
            if (dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor == Color.Maroon)
                dataGridView2.Enabled = false;
            else
                dataGridView2.Enabled = true;
            BindGridCTPN(listCTPN, (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            maPN = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            dataGridView2.CurrentCell = null;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.Maroon)
            {
                int index = dataGridView3.Rows.Add();
                dataGridView3.Rows[index].Cells[0].Value = maPN;
                dataGridView3.Rows[index].Cells[1].Value = dataGridView2.Rows[e.RowIndex].Cells[0].Value;
                dataGridView3.Rows[index].Cells[2].Value = dataGridView2.Rows[e.RowIndex].Cells[1].Value;
                dataGridView3.Rows[index].Cells[3].Value = dataGridView2.Rows[e.RowIndex].Cells[3].Value;
                dataGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Maroon;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            dataGridView2.Rows.Clear();
            BindGridPNH(data.PhieuNhapHangs.ToList());
            dataGridView1.CurrentCell = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HoaDonNhap newHD = new HoaDonNhap();
            newHD.MaHoaDon = data.HoaDonNhaps.Count() + 1;
            newHD.TenHoaDon = "Hóa đơn nhập ngày " + DateTime.Today.Day.ToString() +"/"+ DateTime.Today.Month.ToString() +"/"+ DateTime.Today.Year.ToString();
            newHD.NgayGiaoHang = DateTime.Today;
            newHD.MaNV = maNV;
            data.HoaDonNhaps.InsertOnSubmit(newHD);
            data.SubmitChanges();
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                HangNhap HN = new HangNhap();
                HN.MaHoaDon = newHD.MaHoaDon;
                HN.MaPhieuNhap = (int)dataGridView3.Rows[i].Cells["MaPhieuNhap"].Value;
                HN.MaNL = (int)dataGridView3.Rows[i].Cells["MaNguyenLieu"].Value;
                data.HangNhaps.InsertOnSubmit(HN);
            }
            data.SubmitChanges();
            dataGridView3.Rows.Clear();
            dataGridView2.Rows.Clear();
            BindGridPNH(data.PhieuNhapHangs.ToList());

        }
    }
}
