using Microsoft.Reporting.WinForms;
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
    public partial class TKHoaDonNhap : Form
    {
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public TKHoaDonNhap()
        {
            InitializeComponent();
        }

        private void TKHoaDon_Load(object sender, EventArgs e)
        {
            var listHDN = data.HoaDonNhaps.ToList();
            BindGrid(listHDN);
            this.reportViewer1.RefreshReport();
        }
        public void BindGrid(List<HoaDonNhap> listHDN)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listHDN)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaHoaDon;
                dataGridView1.Rows[index].Cells[1].Value = item.NhanVien.TenNhanVien;
                dataGridView1.Rows[index].Cells[2].Value = item.TenHoaDon;
                dataGridView1.Rows[index].Cells[3].Value = item.NgayGiaoHang;
                dataGridView1.Rows[index].Cells[4].Value = item.TongTien;
            }
        }

        private void btLoc_Click(object sender, EventArgs e)
        {
            var listHDN = (from s in data.HoaDonNhaps.ToList()
                          where s.NgayGiaoHang >= dateTimePicker1.Value.AddDays(-1) 
                          && s.NgayGiaoHang <= dateTimePicker2.Value 
                          select s).ToList();
            BindGrid(listHDN);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var hoaDonNhap = data.HoaDonNhaps.FirstOrDefault(p => p.MaHoaDon == int.Parse(dataGridView1.Rows[e.RowIndex].Cells["MaHD"].Value.ToString()));
            reportCTHD(hoaDonNhap.MaHoaDon);
        }

        public void reportCTHD( int maHD)
        {
            var listCTNH = (from s in data.HangNhaps
                        where s.MaHoaDon == maHD
                        select s).ToList();
            foreach (var item in listCTNH)
            {
                var nl = data.NguyenLieus.FirstOrDefault(p => p.MaNL == item.MaNL);
                item.tenNguyenLieu = nl.TenNL;
                item.hSD = (nl.HSD - nl.NSX).Value.Days.ToString() + " ngày";
                item.soLuong = (int)data.CTNhapHangs.FirstOrDefault(p => p.MaPhieuNhap == item.MaPhieuNhap && p.MaNL == item.MaNL).SoLuong;
                item.ThanhTien = item.ThanhTien;
            }
            var hoaDonNhap = data.HoaDonNhaps.FirstOrDefault(p => p.MaHoaDon == maHD);
            ReportParameter[] param = new ReportParameter[4];
            param[0] = new ReportParameter("ngay", DateTime.Today.ToString());
            param[1] = new ReportParameter("NhaCC", hoaDonNhap.NgayGiaoHang.ToString());
            param[2] = new ReportParameter("nhanvien", hoaDonNhap.NhanVien.TenNhanVien.ToString());
            param[3] = new ReportParameter("maHD", hoaDonNhap.TenHoaDon.ToString());
            this.reportViewer1.LocalReport.ReportPath = @"../../InHDN.rdlc";
            this.reportViewer1.LocalReport.SetParameters(param);
            var reportDataSource = new ReportDataSource("hangnhap", listCTNH);
            this.reportViewer1.LocalReport.DataSources.Clear(); //clear
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReportHDN inTKHD = new ReportHDN();
            inTKHD.since = dateTimePicker1.Value;
            inTKHD.to = dateTimePicker2.Value;
            inTKHD.Show();
        }
    }
}
