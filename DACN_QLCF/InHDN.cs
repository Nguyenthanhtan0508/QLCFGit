using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace DACN_QLCF
{
    public partial class InHDN : Form
    {
        public int maHD;
        DataClasses2DataContext data = new DataClasses2DataContext();
        public List<HangNhap> listCTNH = new List<HangNhap>();
        public InHDN()
        {
            InitializeComponent();
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            listCTNH = (from s in data.HangNhaps
                        where s.MaHoaDon == maHD
                        select s).ToList();
            foreach(var item in listCTNH)
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

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
