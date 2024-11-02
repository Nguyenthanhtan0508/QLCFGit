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
    public partial class HoaDonKH : Form
    {
        public List<CTHoaDon> CTHD = new List<CTHoaDon>();
        public int maHD;
        public int maKH;
        public int maNV;
        DataClasses2DataContext data = new DataClasses2DataContext();
        public List<CT_HD> listCTHD = new List<CT_HD>();
        public HoaDonKH()
        {
            InitializeComponent();
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            listCTHD = (from s in data.CT_HDs
                        where s.MaHoaDon == maHD
                        select s).ToList();
            var nhanVien = data.NhanViens.FirstOrDefault(p => p.MaCV == maNV);
            ReportParameter[] param = new ReportParameter[7];
            param[0] = new ReportParameter("ngay", DateTime.Today.ToString());
            param[1] = new ReportParameter("nhanvien", nhanVien.TenNhanVien.ToString());
            if(maKH > 0)
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
                CTHoaDon  unitCt =new CTHoaDon();
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

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
    
}
