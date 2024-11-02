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
    public partial class InPhieuNhap : Form
    {
        public int maHD;
        DataClasses2DataContext data = new DataClasses2DataContext();
        public List<CTNhapHang> listCTPH = new List<CTNhapHang>();
        public InPhieuNhap()
        {
            InitializeComponent();
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            listCTPH = (from s in data.CTNhapHangs
                        where s.MaPhieuNhap == maHD
                        select s).ToList();
            foreach(var item in listCTPH)
            {
                item.tenNguyenLieu = item.NguyenLieu.TenNL;
                item.hSD = (item.NguyenLieu.HSD - item.NguyenLieu.NSX).Value.Days.ToString() + " ngày";
                item.donGia = (decimal)item.NguyenLieu.GiaTien;
            }
            var phieuNhap = data.PhieuNhapHangs.FirstOrDefault(p => p.MaPhieuNhap == maHD);
            ReportParameter[] param = new ReportParameter[4];
            param[0] = new ReportParameter("ngay", DateTime.Today.ToString());
            param[1] = new ReportParameter("NhaCC", phieuNhap.NhaCC.TenNCC + " chi nhánh "+ phieuNhap.NhaCC.DiaChiCuThe);
            param[2] = new ReportParameter("nhanvien", phieuNhap.MaNV.ToString());
            param[3] = new ReportParameter("maHD", phieuNhap.MaPhieuNhap.ToString());
            this.reportViewer1.LocalReport.ReportPath = @"../../InPhieuNhap.rdlc";
            this.reportViewer1.LocalReport.SetParameters(param);
            var reportDataSource = new ReportDataSource("CTPN", listCTPH);
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
