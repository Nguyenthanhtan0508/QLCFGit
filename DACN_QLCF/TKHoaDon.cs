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
    public partial class TKHoaDon : Form
    {
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public TKHoaDon()
        {
            InitializeComponent();
        }

        private void TKHoaDon_Load(object sender, EventArgs e)
        {
            var listHD = data.HoaDons.ToList();
            BindGrid(listHD);
            this.reportViewer1.RefreshReport();
        }
        public void BindGrid(List<HoaDon> listHD)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listHD)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaHoaDon;
                dataGridView1.Rows[index].Cells[1].Value = item.NhanVien.TenNhanVien;
                dataGridView1.Rows[index].Cells[2].Value = item.NgayLap;
                if (item.MaCT != null)
                    dataGridView1.Rows[index].Cells[3].Value = item.ChuongTrinhKhuyenMai.TenCT;
                else
                    dataGridView1.Rows[index].Cells[3].Value = "Không áp dụng chương trình";
                if (item.ThanhTien != null)
                    dataGridView1.Rows[index].Cells[4].Value = item.ThanhTien;
                else
                    dataGridView1.Rows[index].Cells[4].Value = item.TongTien;
            }
        }

        private void btLoc_Click(object sender, EventArgs e)
        {
            var listHD = (from s in data.HoaDons.ToList()
                          where s.NgayLap >= dateTimePicker1.Value.AddDays(-1) 
                          && s.NgayLap <= dateTimePicker2.Value 
                          select s).ToList();
            BindGrid(listHD);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var hoaDon = data.HoaDons.FirstOrDefault(p => p.MaHoaDon == int.Parse(dataGridView1.Rows[e.RowIndex].Cells["MaHD"].Value.ToString()));
            int maKH;
            if (hoaDon.MaThe != null)
                maKH = (int)hoaDon.MaThe;
            else
                maKH = -1;
            reportCTHD(hoaDon.MaHoaDon, hoaDon.MaNV, maKH);
        }

        public void reportCTHD( int maHD, int maNV, int maKH)
        {
            List<CTHoaDon> CTHD = new List<CTHoaDon>();
            var listCTHD = (from s in data.CT_HDs
                            where s.MaHoaDon == maHD
                            select s).ToList();
            var nhanVien = data.NhanViens.FirstOrDefault(p => p.MaCV == maNV);
            ReportParameter[] param = new ReportParameter[7];
            param[0] = new ReportParameter("ngay", DateTime.Today.ToString());
            param[1] = new ReportParameter("nhanvien", nhanVien.TenNhanVien.ToString());
            if (maKH > 0)
            {
                var khachHang = data.KhachHangs.FirstOrDefault(p => p.MaKH == maKH);
                param[2] = new ReportParameter("khachhang", khachHang.TenKH.ToString());
            }
            else
                param[2] = new ReportParameter("khachhang", "Khách hàng mới");
            var hoaDon = data.HoaDons.FirstOrDefault(p => p.MaHoaDon == maHD);
            if (hoaDon.MaCT != null)
                param[3] = new ReportParameter("khuyenmai", hoaDon.ChuongTrinhKhuyenMai.TenCT.ToString() + hoaDon.ChuongTrinhKhuyenMai.KhuyenMai.PhanTramKhuyenMai.ToString());
            else
                param[3] = new ReportParameter("khuyenmai", "Không áp dụng khuyến mãi");
            param[4] = new ReportParameter("maHD", maHD.ToString());
            param[5] = new ReportParameter("tiendua", hoaDon.SoTienDua.ToString());
            param[6] = new ReportParameter("tienthoi", hoaDon.TienHoanTra.ToString());
            foreach (CT_HD item in listCTHD)
            {
                CTHoaDon unitCt = new CTHoaDon();
                unitCt.name = item.Mon.TenMon;
                unitCt.soluong = int.Parse(item.SoLuong.ToString());
                unitCt.donggia = decimal.Parse(item.Mon.DonGia.ToString());
                int maKM;
                if (item.HoaDon.MaCT == null)
                    maKM = 0;
                else
                    maKM = int.Parse(item.HoaDon.MaCT.ToString());
                var CTKM = (from s in data.CT_CTs
                            where s.MaCT == maKM
                            select s).ToList();

                foreach (var itemKM in CTKM)
                {
                    if (item.MaMon == itemKM.MaMon)
                        unitCt.khuyenmai = decimal.Parse(itemKM.ChuongTrinhKhuyenMai.KhuyenMai.PhanTramKhuyenMai.ToString());
                }
                if (unitCt.khuyenmai != 0)
                    unitCt.thanhtien = unitCt.soluong * unitCt.donggia * unitCt.khuyenmai;
                else
                    unitCt.thanhtien = unitCt.soluong * unitCt.donggia;
                CTHD.Add(unitCt);
            }
            this.reportViewer1.LocalReport.ReportPath = @"../../HoaDon.rdlc";
            this.reportViewer1.LocalReport.SetParameters(param);
            var reportDataSource = new ReportDataSource("CTHD", CTHD);
            this.reportViewer1.LocalReport.DataSources.Clear(); //clear
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Report inTKHD = new Report();
            inTKHD.since = dateTimePicker1.Value;
            inTKHD.to = dateTimePicker2.Value;
            inTKHD.Show();
        }
    }
}
