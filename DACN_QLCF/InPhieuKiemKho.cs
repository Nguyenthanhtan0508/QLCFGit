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
    public partial class InPhieuKiemKho : Form
    {
        public int maPK;
        DataClasses2DataContext data = new DataClasses2DataContext();
        public List<CT_KT> listCTPK = new List<CT_KT>();
        public InPhieuKiemKho()
        {
            InitializeComponent();
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            listCTPK = (from s in data.CT_KTs
                        where s.MaPhieuKiem == maPK
                        select s).ToList();
            foreach(var item in listCTPK)
            {
                item.tenNL = item.NguyenLieu.TenNL;
                item.hSD = (item.NguyenLieu.HSD - item.NguyenLieu.NSX).Value.Days.ToString() + " ngày";
            }
            var phiekiem = data.PhieuKiemHangTons.FirstOrDefault(p => p.MaPhieuKiem == maPK);
            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("ngay", DateTime.Today.ToString());
            param[1] = new ReportParameter("nhanvien", phiekiem.NhanVien.TenNhanVien.ToString());
            param[2] = new ReportParameter("maHD", phiekiem.TenPhieu.ToString());
            this.reportViewer1.LocalReport.ReportPath = @"../../InPhieuKiemKho.rdlc";
            this.reportViewer1.LocalReport.SetParameters(param);
            var reportDataSource = new ReportDataSource("CTPK", listCTPK);
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
