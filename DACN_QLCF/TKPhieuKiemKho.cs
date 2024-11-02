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
    public partial class TKPhieuKiemKho : Form
    {
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public TKPhieuKiemKho()
        {
            InitializeComponent();
        }

        private void TKHoaDon_Load(object sender, EventArgs e)
        {
            var listPKK = data.PhieuKiemHangTons.ToList();
            BindGrid(listPKK);
            this.reportViewer1.RefreshReport();
        }
        public void BindGrid(List<PhieuKiemHangTon> listPKK)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listPKK)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaPhieuKiem;
                dataGridView1.Rows[index].Cells[1].Value = item.NhanVien.TenNhanVien;
                dataGridView1.Rows[index].Cells[2].Value = item.TenPhieu;
                dataGridView1.Rows[index].Cells[3].Value = item.NgayKiemTra;
            }
        }

        private void btLoc_Click(object sender, EventArgs e)
        {
            var listPKK = (from s in data.PhieuKiemHangTons.ToList()
                          where s.NgayLapPhieu >= dateTimePicker1.Value.AddDays(-1) 
                          && s.NgayLapPhieu <= dateTimePicker2.Value 
                          select s).ToList();
            BindGrid(listPKK);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var PhieuKK = data.PhieuKiemHangTons.FirstOrDefault(p => p.MaPhieuKiem == int.Parse(dataGridView1.Rows[e.RowIndex].Cells["MaPK"].Value.ToString()));
            reportCTHD(PhieuKK.MaPhieuKiem);
        }

        public void reportCTHD( int maPK)
        {
            var listCTPK = (from s in data.CT_KTs
                        where s.MaPhieuKiem == maPK
                        select s).ToList();
            foreach (var item in listCTPK)
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
            ReportPKK inTKPK = new ReportPKK();
            inTKPK.since = dateTimePicker1.Value;
            inTKPK.to = dateTimePicker2.Value;
            inTKPK.Show();
        }
    }
}
