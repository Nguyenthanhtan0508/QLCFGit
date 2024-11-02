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
    public partial class PhieuHuyHang : Form
    {
        DataClasses2DataContext data = new DataClasses2DataContext();
        public int maNV;
        public string[] listLyDo = { "Hết HSD", "Hư hỏng", "Không đạt chất lượng" };
        DataGridViewComboBoxCell cellLyDo = new DataGridViewComboBoxCell();
        DataGridViewComboBoxColumn columeLyDo = new DataGridViewComboBoxColumn();

        public PhieuHuyHang()
        {
            InitializeComponent();
        }

        private void PhieuNhapHang_Load(object sender, EventArgs e)
        {
            var listNCC = data.NhaCCs.ToList();
            foreach(var item in listNCC)
            {
                item.tenNhaCC = item.TenNCC + " chi nhánh " + item.DiaChiCuThe;
            }
            BindGridNL(data.NguyenLieus.ToList());
            label1.Text = DateTime.Today.ToString();
            btHuy.Enabled = false;
            btTao.Enabled = false;
            cellLyDo.DataSource = listLyDo;
            columeLyDo.HeaderText = "Lý do hủy";
            columeLyDo.Name = "LyDo";
            columeLyDo.MaxDropDownItems = 4;
            columeLyDo.DataSource = listLyDo;
            columeLyDo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            columeLyDo.FillWeight = 100;
            dataGridView2.Columns.Add(columeLyDo);
        }
        public void BindGridNL(List<NguyenLieu> listNL)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listNL)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaNL;
                dataGridView1.Rows[index].Cells[1].Value = item.TenNL;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool checkNL = false;
            if (dataGridView2.Rows.Count == 0)
            {
                int index = dataGridView2.Rows.Add();
                dataGridView2.Rows[index].Cells[0].Value = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                dataGridView2.Rows[index].Cells[1].Value = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                dataGridView2.Rows[index].Cells[2].Value = 1;
            }
            else
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[i].Cells[0].Value == dataGridView1.Rows[e.RowIndex].Cells[0].Value)
                    {
                        dataGridView2.Rows[i].Cells[2].Value = int.Parse(dataGridView2.Rows[i].Cells[2].Value.ToString()) + 1;
                        checkNL = true;
                    }
                }
                if (checkNL == false)
                {
                    int index = dataGridView2.Rows.Add();
                    dataGridView2.Rows[index].Cells[0].Value = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                    dataGridView2.Rows[index].Cells[1].Value = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
                    dataGridView2.Rows[index].Cells[2].Value = 1;
                }
            }
            btHuy.Enabled = true;
            btTao.Enabled = true;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2 || e.ColumnIndex == 0 || e.ColumnIndex == 1)
            {
                ChonSLMon chonSLMon = new ChonSLMon();
                chonSLMon.Show();
                dataGridView1.Enabled = false;
                dataGridView2.Enabled = false;
                chonSLMon.textBox1.Enabled = false;
                chonSLMon.FormClosing += (o, x) =>
                {
                    if (int.Parse(chonSLMon.textBox1.Text.ToString()) > 0)
                        dataGridView2.Rows[e.RowIndex].Cells["SoLuong"].Value = chonSLMon.textBox1.Text.ToString();
                    else if (int.Parse(chonSLMon.textBox1.Text.ToString()) == 0)
                    {
                        dataGridView2.Rows[e.RowIndex].Cells["SoLuong"].Value = "0";
                    }
                    else
                        dataGridView2.Rows.RemoveAt(e.RowIndex);
                    dataGridView2.Enabled = true;
                    dataGridView1.Enabled = true;
                };
            }    
        }

        private void btHuy_Click(object sender, EventArgs e)
        {
            btHuy.Enabled = false;
            btTao.Enabled = false;
            dataGridView2.Rows.Clear();
        }

        private void btTao_Click(object sender, EventArgs e)
        {
            PhieuHuy newPH = new PhieuHuy();
            newPH.MaPhieu = data.PhieuHuys.Count() + 1;
            newPH.TenPhieu = "Phiếu hủy ngày " + DateTime.Today.Date + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
            newPH.NgayHuy = DateTime.Today;
            newPH.NgayLap = DateTime.Today;
            newPH.MaNV = maNV;
            data.PhieuHuys.InsertOnSubmit(newPH);
            data.SubmitChanges();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                CT_Huy CTH = new CT_Huy();
                CTH.MaPhieu = newPH.MaPhieu;
                CTH.MaNL = (int)dataGridView2.Rows[i].Cells[0].Value;
                CTH.SoLuongHuy = int.Parse(dataGridView2.Rows[i].Cells["SoLuong"].Value.ToString());
                CTH.LyDo = (string)dataGridView2.Rows[i].Cells[3].Value;
                data.CT_Huys.InsertOnSubmit(CTH);
            }
            data.SubmitChanges();
            MessageBox.Show("Tạo Phiếu Hủy Thành Công! ");
            dataGridView2.Rows.Clear();
            btHuy.Enabled = false;
            btTao.Enabled = false;
            InPhieuHuy inPH = new InPhieuHuy();
            inPH.maPH = newPH.MaPhieu;
            inPH.Show();
        }
    }
}
