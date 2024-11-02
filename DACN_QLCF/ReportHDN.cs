﻿using Microsoft.Reporting.WinForms;
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
    public partial class ReportHDN : Form
    {
        public DateTime since;
        public DateTime to;
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public ReportHDN()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            var listHDN = (from s in data.HoaDonNhaps.ToList()
                          where s.NgayGiaoHang >= since.AddDays(-1)
                          && s.NgayGiaoHang <= to
                          select s).ToList();
            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("ngay", DateTime.Today.ToString());
            param[1] = new ReportParameter("since", since.ToString());
            param[2] = new ReportParameter("to", to.ToString());
            this.reportViewer1.LocalReport.ReportPath = @"../../TKHoaDonNhap.rdlc";
            this.reportViewer1.LocalReport.SetParameters(param);
            var reportDataSource = new ReportDataSource("HoaDonNhap", listHDN);
            this.reportViewer1.LocalReport.DataSources.Clear(); //clear
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}
