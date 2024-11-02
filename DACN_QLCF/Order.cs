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
    public partial class Order : Form
    {
        public List<Ban> listBan = new List<Ban>();
        public List<Mon> listMon = new List<Mon>();
        public List<String> listTang = new List<string>();
        public List<String> listloaiMon = new List<string>();
        public bool isTableSelect;
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public int maCTKM;
        public List<CT_CT> listMonKM = new List<CT_CT>();
        public List<Mon> listTempDonGia = new List<Mon>();
        public int STTBan;
        public int maNV;
        public Order()
        {
            InitializeComponent();
        }

        private void Order_Load(object sender, EventArgs e)
        {
            DataClasses2DataContext data = new DataClasses2DataContext();
            imlTable.Images.Add(Image.FromFile("../../Image/Table.png"));
            listBan = data.Bans.ToList();
            listMon = data.Mons.ToList();
            BindGridBan(listBan, listTang);
            isTableSelect = true;
            btHuy.Enabled = false;
            btThanhToan.Enabled = false;
            btCTKM.Enabled = false;
            pbLoGO.Image = Image.FromFile("../../Image/LoGo.jpg");
            tbNhan.Enabled = false;
            var nhanVien = data.NhanViens.FirstOrDefault(p => p.MaNV == maNV);
            if (nhanVien.ChucVu.MaCV == 4)
                thốngKêToolStripMenuItem.Enabled = true;
            else
                thốngKêToolStripMenuItem.Enabled = false;
            label5.Text = nhanVien.TenNhanVien.ToString();
            maCTKM = 0;
        }

        private void BindGridBan(List<Ban> listBan, List<String> listTang)
        {
            lvBan.Items.Clear();
            lvBan.Groups.Clear();

            lvBan.LargeImageList = imlTable;
            foreach (var ban in listBan)
            {
                if (listTang.FirstOrDefault(p => p == ban.ViTriCuThe.ToString()) == null)
                    listTang.Add(ban.ViTriCuThe.ToString());
            }
            foreach (var item in listTang)
            {
                ListViewGroup newGr = new ListViewGroup(item);
                lvBan.Groups.Add(newGr);
                foreach (var ban in listBan)
                {
                    if (ban.ViTriCuThe.ToString() == item)
                    {
                        int index = lvBan.Items.Count;
                        lvBan.Items.Add(ban.TenBan);
                        lvBan.Items[index].ImageIndex = 0;
                        lvBan.Items[index].SubItems.Add(ban.LoaiBan);
                        lvBan.Items[index].SubItems.Add(ban.ViTriCuThe);
                        lvBan.Items[index].SubItems.Add(ban.STT.ToString());
                        lvBan.Items[index].Group = newGr;
                    }

                }
            }

        }

        private void BindGridMon(List<Mon> listMon, List<String> listLoaimon)
        {
            lvBan.Items.Clear();
            lvBan.Groups.Clear();
            foreach (var mon in listMon)
            {
                imlMon.Images.Add(Image.FromFile("../../Image/" + mon.TenMon + ".jpg"));
            }
            lvBan.LargeImageList = imlMon;
            foreach (var mon in listMon)
            {
                if (listLoaimon.FirstOrDefault(p => p == mon.PhanLoai.TenLoai.ToString()) == null)
                    listLoaimon.Add(mon.PhanLoai.TenLoai.ToString());
            }
            foreach (var item in listLoaimon)
            {
                ListViewGroup newGr = new ListViewGroup(item);
                lvBan.Groups.Add(newGr);
                foreach (var mon in listMon)
                {
                    if (mon.PhanLoai.TenLoai.ToString() == item && mon.TrangThai == 1)
                    {
                        int index = lvBan.Items.Count;
                        lvBan.Items.Add(mon.TenMon);
                        lvBan.Items[index].ImageIndex = index;
                        lvBan.Items[index].SubItems.Add(mon.MaMon.ToString());
                        lvBan.Items[index].Group = newGr;
                    }

                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            BindGridBan(listBan, listTang);
            lbBan.Text = "Bàn: ";
            tbTongTien.Text = "0";
            lbKM.Text = "Không áp dụng CTKM!";
            dgvCTHD.Rows.Clear();
            isTableSelect = true;
            btHuy.Enabled = false;
            btThanhToan.Enabled = false;
            btCTKM.Enabled = false;
            tbNhan.Enabled = false;
            tbNhan.Text = "0";
            tbThua.Text = "0";
            listTempDonGia.Clear();
        }

        private void lvBan_MouseClick(object sender, MouseEventArgs e)
        {
            bool checkMon = false;
            if (isTableSelect)
            {
                STTBan = int.Parse(lvBan.SelectedItems[0].SubItems[3].Text);
                lbBan.Text = "Bàn: " + STTBan.ToString();
                BindGridMon(listMon, listloaiMon);
                isTableSelect = false; 
                btHuy.Enabled = true;
                btCTKM.Enabled = true;
                tbNhan.Enabled = true;

            }
            else
            {
                var mon = lvBan.SelectedItems[0].SubItems[1].Text;
                var find = data.Mons.FirstOrDefault(p => p.MaMon.ToString() == mon);
                if (dgvCTHD.Rows.Count == 0)
                {
                    int index = dgvCTHD.Rows.Add();
                    dgvCTHD.Rows[index].Cells["TenMon"].Value = find.TenMon.ToString();
                    dgvCTHD.Rows[index].Cells["SoLuong"].Value = 1;
                    dgvCTHD.Rows[index].Cells["DonGia"].Value = find.DonGia.ToString();
                    listTempDonGia.Add(find);
                }
                else
                {
                    for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                    {
                        if (dgvCTHD.Rows[i].Cells["TenMon"].Value.ToString() == find.TenMon)
                        {
                            dgvCTHD.Rows[i].Cells["SoLuong"].Value = int.Parse(dgvCTHD.Rows[i].Cells["SoLuong"].Value.ToString()) + 1;
                            checkMon = true;
                        }
                    }
                    if (checkMon == false)
                    {
                        int index = dgvCTHD.Rows.Add();
                        dgvCTHD.Rows[index].Cells["TenMon"].Value = find.TenMon.ToString();
                        dgvCTHD.Rows[index].Cells["SoLuong"].Value = 1;
                        dgvCTHD.Rows[index].Cells["DonGia"].Value = find.DonGia.ToString();
                        checkMon = true;
                        listTempDonGia.Add(find);
                    }
                }
            }
            tbTongTien.Text = TongTien(dgvCTHD).ToString();

        }

        private void dgvCTHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            ChonSLMon chonSLMon = new ChonSLMon();
            chonSLMon.Show();
            dgvCTHD.Enabled = false;
            lvBan.Enabled = false;
            chonSLMon.textBox1.Enabled = false;
            chonSLMon.FormClosing += (o, x) =>
            {
                if (int.Parse(chonSLMon.textBox1.Text.ToString()) > 0)
                    dgvCTHD.Rows[e.RowIndex].Cells["SoLuong"].Value = chonSLMon.textBox1.Text.ToString();
                else
                {
                    dgvCTHD.Rows.RemoveAt(e.RowIndex);
                    listTempDonGia.RemoveAt(e.RowIndex);
                }    
                    
                dgvCTHD.Enabled = true;
                lvBan.Enabled = true;
                tbTongTien.Text = TongTien(dgvCTHD).ToString();
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CTKhuyenMai cTKhuyenMai = new CTKhuyenMai();
            cTKhuyenMai.Show();
            cTKhuyenMai.FormClosing += (o, x) =>
            {
                maCTKM = int.Parse(cTKhuyenMai.maCT.ToString());
                if ( maCTKM > 0)
                {
                    var cTKM = data.ChuongTrinhKhuyenMais.FirstOrDefault(p => p.MaCT.ToString() == maCTKM.ToString());
                    if (cTKM.NgayBatDau > DateTime.Today || cTKM.NgayKetThuc < DateTime.Today)
                    {
                        lbKM.Text = "Chương Trình Khuyến Mãi đã kết thúc";
                        maCTKM = -1;
                    }    
                    else
                        lbKM.Text = cTKM.TenCT.ToString() + ": " + (cTKM.KhuyenMai.PhanTramKhuyenMai * 100).ToString() + " %";
                }    
                else
                {
                    maCTKM = -1;
                    lbKM.Text = "Không áp dụng CTKM!";
                } 
                if(maCTKM > 0)
                {
                    var listCTCT = data.CT_CTs.ToList();
                    foreach(CT_CT item in listCTCT)
                    {
                        if( item.MaCT == maCTKM)
                        {
                            listMonKM.Add(item);
                        }    
                    }
                    for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                    {
                        dgvCTHD.Rows[i].Cells["DonGia"].Value = listTempDonGia[i].DonGia;
                    }
                    addKhuyenMaiHD(listMonKM);
                }
                else
                {
                    for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                    {
                        dgvCTHD.Rows[i].Cells["DonGia"].Value = listTempDonGia[i].DonGia;
                    }
                }
                tbTongTien.Text = TongTien(dgvCTHD).ToString();
            };
        }

        public void addKhuyenMaiHD(List<CT_CT> listMonKm)
        {
            if(listMonKM.Count > 0)
            {
                foreach( CT_CT item in listMonKM)
                {
                    for (int i = 0; i < dgvCTHD.Rows.Count; i++)
                    {
                        if (dgvCTHD.Rows[i].Cells["TenMon"].Value.ToString() == item.Mon.TenMon)
                        {
                            dgvCTHD.Rows[i].Cells["DonGia"].Value = decimal.Parse(dgvCTHD.Rows[i].Cells["DonGia"].Value.ToString()) - decimal.Parse(dgvCTHD.Rows[i].Cells["DonGia"].Value.ToString()) * item.ChuongTrinhKhuyenMai.KhuyenMai.PhanTramKhuyenMai;
                        }
                    }
                }
                listMonKM.Clear();
            }    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HoaDon hd = new HoaDon();
            var soHD = data.HoaDons.Count();
            hd.MaHoaDon = soHD + 1;
            hd.STT = STTBan;
            if(maCTKM <= 0)
                hd.MaCT = null;
            else
                hd.MaCT = maCTKM;
            hd.TenHoaDon = "Hóa Đơn Số: " + (soHD + 1).ToString();
            hd.NgayLap = DateTime.Today;
            hd.SoTienDua = decimal.Parse(tbNhan.Text.ToString());
            hd.TienHoanTra = decimal.Parse(tbThua.Text.ToString());
            hd.MaNV = maNV;
            data.HoaDons.InsertOnSubmit(hd);
            data.SubmitChanges();
            for (int i = 0; i < dgvCTHD.Rows.Count; i++)
            {
                CT_HD CTHD = new CT_HD();
                CTHD.MaHoaDon = hd.MaHoaDon;
                CTHD.MaMon = listTempDonGia[i].MaMon;
                CTHD.SoLuong = int.Parse(dgvCTHD.Rows[i].Cells["SoLuong"].Value.ToString());
                data.CT_HDs.InsertOnSubmit(CTHD);
            }
            data.SubmitChanges();
            MessageBox.Show("Nhập Hóa Đơn Thành Công! ");
            BindGridBan(listBan, listTang);
            lbBan.Text = "Bàn: ";
            tbTongTien.Text = "0";
            lbKM.Text = "Không áp dụng CTKM!";
            dgvCTHD.Rows.Clear();
            isTableSelect = true;
            btHuy.Enabled = false;
            btThanhToan.Enabled = false;
            btCTKM.Enabled = false;
            tbNhan.Enabled = false;
            tbNhan.Text = "0";
            tbThua.Text = "0";
            listTempDonGia.Clear();
            HoaDonKH inHD = new HoaDonKH();
            inHD.maHD = hd.MaHoaDon;
            inHD.maNV = maNV;
            inHD.maKH = -1;
            inHD.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        public decimal TongTien(DataGridView CTHD)
        {
            decimal tong = 0;
            for (int i = 0; i < CTHD.Rows.Count; i++)
            {
                tong += decimal.Parse(CTHD.Rows[i].Cells["SoLuong"].Value.ToString()) * decimal.Parse(CTHD.Rows[i].Cells["DonGia"].Value.ToString());
            }
            return tong;
        }

        private void tbNhan_TextChanged(object sender, EventArgs e)
        {
            if (tbNhan.Text != "")
                tbThua.Text = (decimal.Parse(tbNhan.Text.ToString()) - decimal.Parse(tbTongTien.Text.ToString())).ToString();
        }


        private void tbNhan_Click(object sender, EventArgs e)
        {
            ChonSLMon chonSLMon = new ChonSLMon();
            chonSLMon.Show();
            chonSLMon.textBox1.Enabled = false;
            chonSLMon.FormClosing += (o, x) =>
            {
                tbNhan.Text = chonSLMon.textBox1.Text;
                if (decimal.Parse(tbNhan.Text.ToString()) < decimal.Parse(tbTongTien.Text.ToString()))
                {
                    MessageBox.Show("Tiền nhận phải lớn hơn hoặc bằng tổng tiên!");
                    tbNhan.Text = "";
                    btThanhToan.Enabled = false;
                }    
                else
                {
                    btThanhToan.Enabled = true;
                }    
                    
            };
        }

        private void thốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TKHoaDon tkHoaDon = new TKHoaDon();
            tkHoaDon.Show();
        }

        private void bảngLươngNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BangLuong bLuong = new BangLuong();
            bLuong.Show();
        }

        private void lậpPhiếuNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PhieuOrder phieuOrder = new PhieuOrder();
            phieuOrder.maNV = maNV;
            phieuOrder.Show();
        }

        private void lậpHóaĐơnNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaoHoaDonNhap taoHDN = new TaoHoaDonNhap();
            taoHDN.maNV = maNV;
            taoHDN.Show();
        }

        private void nhậpPhiếuKiếmKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PhieuKiemKho taoPKK = new PhieuKiemKho();
            taoPKK.maNV = maNV;
            taoPKK.Show();
        }

        private void nhậpPhiếuHủyHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PhieuHuyHang taoPH = new PhieuHuyHang();
            taoPH.maNV = maNV;
            taoPH.Show();
        }

        private void hóaĐơnNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TKHoaDonNhap tkHoaDon = new TKHoaDonNhap();
            tkHoaDon.Show();
        }

        private void phiếuKiểmKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TKPhieuKiemKho tkPhieuKK = new TKPhieuKiemKho();
            tkPhieuKK.Show();
        }

        private void phiếuHủyHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TKPhieuHuyHang tkPhieuHH = new TKPhieuHuyHang();
            tkPhieuHH.Show();
        }
    }
}
