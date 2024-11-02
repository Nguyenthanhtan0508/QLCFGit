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
    public partial class CTKhuyenMai : Form
    {
        public List<ChuongTrinhKhuyenMai> listCTKM = new List<ChuongTrinhKhuyenMai>();
        public DataClasses2DataContext data = new DataClasses2DataContext();
        public String maCT;
        public CTKhuyenMai()
        {
            InitializeComponent();
        }

        private void CTKhuyenMai_Load(object sender, EventArgs e)
        {
            listCTKM = data.ChuongTrinhKhuyenMais.ToList();
            imageList1.Images.Add(Image.FromFile("../../Image/Table.png"));

            BindGridCTKM(listCTKM);
        }

        private void BindGridCTKM(List<ChuongTrinhKhuyenMai> listCTKM)
        {
            listView1.Items.Clear();
            listView1.LargeImageList = imageList1;
            foreach (var item in listCTKM)
                {
                        int index = listView1.Items.Count;
                        listView1.Items.Add(item.TenCT);
                        listView1.Items[index].ImageIndex = 0;
                        listView1.Items[index].SubItems.Add(item.MaCT.ToString());
                        listView1.Items[index].SubItems.Add(item.MaKhuyenMai.ToString());
                        if(item.NgayBatDau > DateTime.Today || item.NgayKetThuc < DateTime.Today)
                            listView1.Items[index].BackColor = Color.Red;
                        else
                            listView1.Items[index].BackColor = Color.Green;
            }
            }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            maCT = listView1.SelectedItems[0].SubItems[1].Text.ToString();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            maCT = "-1";
            this.Close();

        }
    }
    }
