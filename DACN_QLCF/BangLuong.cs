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
    
    public partial class BangLuong : Form
    {
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public int thang;
        public int nam;
        public BangLuong()
        {
            InitializeComponent();
        }

        private void BangLuong_Load(object sender, EventArgs e)
        {
            thang = DateTime.Today.AddMonths(-1).Month;
            nam = DateTime.Today.AddMonths(-1).Year;
            var listBL = data.BangLuong_NNVs.ToList();
            foreach (var item in listBL)
                item.tenBangLuong = "Bảng lương tháng " + item.Thang + " năm " + item.Nam;
            FillBLCombobox(listBL,data);
            label1.Text = "Tạo bảng lương tháng: " + thang + " năm " + nam;
            var lastBL = data.BangLuong_NNVs.ToList();
            reportBangLuong(lastBL.Last().MaBL, data);
        }
        private void FillBLCombobox(List<BangLuong_NNV> listBangLuong, DataClasses2DataContext data)
        {
            this.comboBox1.DataSource = listBangLuong;
            this.comboBox1.DisplayMember = "TenBangLuong";
            this.comboBox1.ValueMember = "MaBL";
        }
        public void reportBangLuong(int maBL,DataClasses2DataContext data)
        {
            var listCTBL = (from s in data.CTBLs
                            where s.MaBL == maBL
                            select s).ToList();
            var bangLuong = data.BangLuong_NNVs.FirstOrDefault(p => p.MaBL == maBL);
            ReportParameter[] param = new ReportParameter[4];
            param[0] = new ReportParameter("ngay", DateTime.Today.ToString());
            param[1] = new ReportParameter("thangnam", "THÁNG " + bangLuong.Thang + " NĂM " + bangLuong.Nam);
            param[2] = new ReportParameter("since", "1/" + bangLuong.Thang + "/" + bangLuong.Nam);
            param[3] = new ReportParameter("to", "30/" + bangLuong.Thang + "/" + bangLuong.Nam);
            this.reportViewer1.LocalReport.ReportPath = @"../../BangLuong.rdlc";
            this.reportViewer1.LocalReport.SetParameters(param);
            var reportDataSource = new ReportDataSource("BangLuong", listCTBL);
            this.reportViewer1.LocalReport.DataSources.Clear(); //clear
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //reportBangLuong((int)comboBox1.SelectedValue);
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
           // reportBangLuong((int)comboBox1.SelectedValue);
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataClasses2DataContext data2 = new DataClasses2DataContext();
            reportBangLuong((int)comboBox1.SelectedValue,data2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var listBL = data.BangLuong_NNVs.ToList();
            foreach(var item in listBL)
            {
                if (item.Thang == thang.ToString() && item.Nam == nam.ToString())
                {
                    MessageBox.Show("Bảng lương tháng này đã được tạo!");
                    return;
                }
            }
            BangLuong_NNV newBL = new BangLuong_NNV();
            newBL.MaBL = data.BangLuong_NNVs.Count() + 1;
            newBL.Thang = thang.ToString();
            newBL.Nam = nam.ToString();
            data.BangLuong_NNVs.InsertOnSubmit(newBL);
            data.SubmitChanges();
            foreach(var nv in data.NhanViens.ToList())
            {
                var newCTBL = new CTBL();
                newCTBL.MaBL = newBL.MaBL;
                newCTBL.MaNV = nv.MaNV;
                data.CTBLs.InsertOnSubmit(newCTBL);
            }
            data.SubmitChanges();
            DataClasses2DataContext data2 = new DataClasses2DataContext();
            var listBL2 = data2.BangLuong_NNVs.ToList();
            foreach (var item in listBL2)
                item.tenBangLuong = "Bảng lương tháng " + item.Thang + " năm " + item.Nam;
            FillBLCombobox(listBL2,data2);
            reportBangLuong(newBL.MaBL,data2);
        }
    }
}
