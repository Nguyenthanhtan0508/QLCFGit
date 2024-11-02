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
    public partial class InPhieuHuy : Form
    {
        public int maPH;
        DataClasses2DataContext data = new DataClasses2DataContext();
        public List<CT_Huy> listCTPH = new List<CT_Huy>();
        public InPhieuHuy()
        {
            InitializeComponent();
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            listCTPH = (from s in data.CT_Huys
                        where s.MaPhieu == maPH
                        select s).ToList();
            foreach(var item in listCTPH)
            {
                item.tenNL = item.NguyenLieu.TenNL;
            }
            var phieuhuy = data.PhieuHuys.FirstOrDefault(p => p.MaPhieu == maPH);
            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("ngay", DateTime.Today.ToString());
            param[1] = new ReportParameter("nhanvien", phieuhuy.NhanVien.TenNhanVien.ToString());
            param[2] = new ReportParameter("maHD", phieuhuy.TenPhieu.ToString());
            this.reportViewer1.LocalReport.ReportPath = @"../../InPhieuHuy.rdlc";
            this.reportViewer1.LocalReport.SetParameters(param);
            var reportDataSource = new ReportDataSource("CTHuy", listCTPH);
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
