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
    public partial class TKPhieuHuyHang : Form
    {
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public TKPhieuHuyHang()
        {
            InitializeComponent();
        }

        private void TKHoaDon_Load(object sender, EventArgs e)
        {
            var listPKH = data.PhieuHuys.ToList();
            BindGrid(listPKH);
            this.reportViewer1.RefreshReport();
        }
        public void BindGrid(List<PhieuHuy> listPKH)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listPKH)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaPhieu;
                dataGridView1.Rows[index].Cells[1].Value = item.NhanVien.TenNhanVien;
                dataGridView1.Rows[index].Cells[2].Value = item.TenPhieu;
                dataGridView1.Rows[index].Cells[3].Value = item.NgayHuy;
            }
        }

        private void btLoc_Click(object sender, EventArgs e)
        {
            var listPKH = (from s in data.PhieuHuys.ToList()
                          where s.NgayLap >= dateTimePicker1.Value.AddDays(-1) 
                          && s.NgayLap <= dateTimePicker2.Value 
                          select s).ToList();
            BindGrid(listPKH);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var PhieuHH = data.PhieuHuys.FirstOrDefault(p => p.MaPhieu == int.Parse(dataGridView1.Rows[e.RowIndex].Cells["MaPH"].Value.ToString()));
            reportCTHD(PhieuHH.MaPhieu);
        }

        public void reportCTHD( int maPH)
        {
            var listCTPH = (from s in data.CT_Huys
                        where s.MaPhieu == maPH
                        select s).ToList();
            foreach (var item in listCTPH)
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
            ReportPHH inTKPK = new ReportPHH();
            inTKPK.since = dateTimePicker1.Value;
            inTKPK.to = dateTimePicker2.Value;
            inTKPK.Show();
        }
    }
}
