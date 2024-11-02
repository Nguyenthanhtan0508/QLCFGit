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
    public partial class Report : Form
    {
        public DateTime since;
        public DateTime to;
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            var listHD = (from s in data.HoaDons.ToList()
                          where s.NgayLap >= since.AddDays(-1)
                          && s.NgayLap <= to
                          select s).ToList();
            foreach (var item in listHD)
            {
                if (item.ThanhTien != null)
                    item.tongSoTien = (decimal)item.ThanhTien;
                else
                    item.tongSoTien = (decimal)item.TongTien;
                if (item.MaCT != null)
                    item.tenKhuyenMai = item.ChuongTrinhKhuyenMai.TenCT;
                else
                    item.tenKhuyenMai = "Không áp dụng khuyến mãi";
            }
            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("ngay", DateTime.Today.ToString());
            param[1] = new ReportParameter("since", since.ToString());
            param[2] = new ReportParameter("to", to.ToString());
            this.reportViewer1.LocalReport.ReportPath = @"../../TKHoaDon.rdlc";
            this.reportViewer1.LocalReport.SetParameters(param);
            var reportDataSource = new ReportDataSource("DataSet1", listHD);
            this.reportViewer1.LocalReport.DataSources.Clear(); //clear
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}
