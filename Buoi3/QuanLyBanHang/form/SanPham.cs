using QuanLyBanHang.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuanLyBanHang.Data.SanPham;
using SlugGenerator;
namespace QuanLyBanHang.form
{
    public partial class SanPham : Form
    {
        public SanPham()
        {
            InitializeComponent();
        }
        QLBHContext context = new QLBHContext(); // Khởi tạo biến ngữ cảnh CSDL
        bool xuLyThem = false; // Kiểm tra có nhấn vào nút Thêm hay không?
        int id; // Lấy mã sản phẩm (dùng cho Sửa và Xóa)
        string imagesFolder = Application.StartupPath.Replace("bin\\Debug\\net8.0-windows", "Images");
        private void BatTatChucNang(bool giaTri)
        {
            btnLuu.Enabled = giaTri;
            btnHuyBo.Enabled = giaTri;
            cboHangSanXuat.Enabled = giaTri;
            cboLoaiSanPham.Enabled = giaTri;
            txtTenSanPham.Enabled = giaTri;
            numSoLuong.Enabled = giaTri;
            numDonGia.Enabled = giaTri;
            txtMoTa.Enabled = giaTri;
            picHinhAnh.Enabled = giaTri;
            btnThem.Enabled = !giaTri;
            btnDoiAnh.Enabled = giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
            btnTimKiem.Enabled = !giaTri;
            btnNhap.Enabled = !giaTri;
            btnXuat.Enabled = !giaTri;
        }
        public void LayLoaiSanPhamVaoComboBox()
        {
            cboLoaiSanPham.DataSource = context.LoaiSanPham.ToList();
            cboLoaiSanPham.ValueMember = "ID";
            cboLoaiSanPham.DisplayMember = "LoaiSanPham";
        }
        public void LayHangSanXuatVaoComboBox()
        {
            cboHangSanXuat.DataSource = context.LoaiSanPham.ToList();
            cboHangSanXuat.ValueMember = "ID";
            cboHangSanXuat.DisplayMember = "TenHangSanXuat";
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            BatTatChucNang(false);
            LayLoaiSanPhamVaoComboBox();
            LayHangSanXuatVaoComboBox();
            dataGridView.AutoGenerateColumns = false;
            List<DanhSachSanPham> sp = new List<DanhSachSanPham>();
            sp = context.SanPham.Select(r => new DanhSachSanPham
            {
                ID = r.ID,
                LoaiSanPhamID = r.LoaiSanPhamID,
                TenLoai = r.LoaiSanPham.TenLoai,
                HangSanXuatID = r.HangSanXuatID,
                TenHangSanXuat = r.HangSanXuat.TenHangSanXuat,
                TenSanPham = r.TenSanPham,
                SoLuong = r.SoLuong,
                DonGia = r.DonGia,
                HinhAnh = r.HinhAnh
            }).ToList();
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = sp;
            cboLoaiSanPham.DataBindings.Clear();
            cboLoaiSanPham.DataBindings.Add("SelectedValue", bindingSource, "LoaiSanPhamID", false, DataSourceUpdateMode.Never);
            cboHangSanXuat.DataBindings.Clear();
            cboHangSanXuat.DataBindings.Add("SelectedValue", bindingSource, "HangSanXuatID", false, DataSourceUpdateMode.Never);
            txtTenSanPham.DataBindings.Clear();
            txtTenSanPham.DataBindings.Add("Text", bindingSource, "TenSanPham", false, DataSourceUpdateMode.Never);
            txtMoTa.DataBindings.Clear();
            txtMoTa.DataBindings.Add("Text", bindingSource, "MoTa", false, DataSourceUpdateMode.Never);
            numSoLuong.DataBindings.Clear();
            numSoLuong.DataBindings.Add("Value", bindingSource, "SoLuong", false, DataSourceUpdateMode.Never);
            numDonGia.DataBindings.Clear();
            numDonGia.DataBindings.Add("Value", bindingSource, "DonGia", false, DataSourceUpdateMode.Never);
            picHinhAnh.DataBindings.Clear();
            Binding hinhAnh = new Binding("ImageLocation", bindingSource, "HinhAnh");
            hinhAnh.Format += (s, e) =>
            {
                e.Value = Path.Combine(imagesFolder, e.Value.ToString());
            };
            picHinhAnh.DataBindings.Add(hinhAnh);
            dataGridView.DataSource = bindingSource;
        }
        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*if (dataGridView.Columns[e.ColumnIndex].Name == "HinhAnh")
            {

                Image image = Image.FromFile(Path.Combine(imagesFolder, e.Value.ToString()));
                image = new Bitmap(image, 24, 24);
                e.Value = image;
            }*/
            if (dataGridView.Columns[e.ColumnIndex].Name == "HinhAnh" && e.Value != null)
            {
                try
                {
                    string fileName = e.Value.ToString();
                    if (string.IsNullOrWhiteSpace(fileName)) return;

                    string fullPath = Path.Combine(imagesFolder, fileName);
                    if (File.Exists(fullPath))
                    {
                        using (Image imgGoc = Image.FromFile(fullPath))
                        {
                            e.Value = new Bitmap(imgGoc, new Size(50, 50));
                        }
                    }
                    else
                    {
                        e.Value = null;
                    }
                }
                catch
                {
                    e.Value = null;
                }
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            xuLyThem = true;
            BatTatChucNang(true);
            cboLoaiSanPham.Text = "";
            cboHangSanXuat.Text = "";
            txtTenSanPham.Clear();
            txtMoTa.Clear();
            numSoLuong.Value = 0;
            numDonGia.Value = 0;
            picHinhAnh.Image = null;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            xuLyThem = false;
            BatTatChucNang(true);
            id = Convert.ToInt32(dataGridView.CurrentRow.Cells["ID"].Value.ToString());
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboLoaiSanPham.Text))
                MessageBox.Show("Vui lòng chọn loại sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrWhiteSpace(cboHangSanXuat.Text))
                MessageBox.Show("Vui lòng chọn hãng sản xuất.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrWhiteSpace(txtTenSanPham.Text))
                MessageBox.Show("Vui lòng nhập tên sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (numSoLuong.Value <= 0)
                MessageBox.Show("Số lượng phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (numDonGia.Value <= 0)
                MessageBox.Show("Đơn giá sản phẩm phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (xuLyThem)
                {
                    QuanLyBanHang.Data.SanPham sp = new QuanLyBanHang.Data.SanPham();
                    sp.MoTa = txtMoTa.Text;
                    sp.TenSanPham = txtTenSanPham.Text;
                    sp.HangSanXuatID = Convert.ToInt32(cboHangSanXuat.SelectedValue);
                    sp.LoaiSanPhamID = Convert.ToInt32(cboLoaiSanPham.SelectedValue);
                    sp.SoLuong = Convert.ToInt32(numSoLuong.Value);
                    sp.DonGia = Convert.ToInt32(numDonGia.Value);
                    sp.HinhAnh = Path.GetFileName(picHinhAnh.ImageLocation);
                    context.SanPham.Add(sp);
                    context.SaveChanges();
                }
                else
                {
                    QuanLyBanHang.Data.SanPham sp = context.SanPham.Find(id);
                    if (sp != null)
                    {
                        sp.MoTa = txtMoTa.Text;
                        sp.TenSanPham = txtTenSanPham.Text;
                        sp.HangSanXuatID = Convert.ToInt32(cboHangSanXuat.SelectedValue);
                        sp.LoaiSanPhamID = Convert.ToInt32(cboLoaiSanPham.SelectedValue);
                        sp.SoLuong = Convert.ToInt32(numSoLuong.Value);
                        sp.DonGia = Convert.ToInt32(numDonGia.Value);
                        sp.HinhAnh = Path.GetFileName(picHinhAnh.ImageLocation);
                        context.SanPham.Update(sp);
                        context.SaveChanges();
                    }
                }
                frmSanPham_Load(sender, e);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận xóa sản phẩm " + txtTenSanPham.Text + "?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
DialogResult.Yes)
            {
                id = Convert.ToInt32(dataGridView.CurrentRow.Cells["ID"].Value.ToString());
                QuanLyBanHang.Data.SanPham sp = context.SanPham.Find(id);
                if (sp != null)
                {
                    context.SanPham.Remove(sp);
                }
                context.SaveChanges();
                frmSanPham_Load(sender, e);
            }
        }
        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            frmSanPham_Load(sender, e);
        }
        private void btnDoiAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Cập nhật hình ảnh sản phẩm";
            openFileDialog.Filter = "Tập tin hình ảnh|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*jfif";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string sourceFilePath = openFileDialog.FileName;
                    string sourceDir = Path.GetDirectoryName(sourceFilePath);

                    string normSourceDir = Path.GetFullPath(sourceDir).TrimEnd('\\');
                    string normImagesDir = Path.GetFullPath(imagesFolder).TrimEnd('\\');

                    string fileNameDb = "";

                    if (string.Equals(normSourceDir, normImagesDir, StringComparison.OrdinalIgnoreCase))
                    {
                        fileNameDb = Path.GetFileName(sourceFilePath);
                    }
                    else
                    {
                        string fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
                        string ext = Path.GetExtension(sourceFilePath);

                        fileNameDb = fileName.GenerateSlug() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                        string destPath = Path.Combine(imagesFolder, fileNameDb);

                        if (picHinhAnh.Image != null)
                        {
                            picHinhAnh.Image.Dispose();
                            picHinhAnh.Image = null;
                        }

                        File.Copy(sourceFilePath, destPath, true);
                    }

                    id = Convert.ToInt32(dataGridView.CurrentRow.Cells["ID"].Value.ToString());
                    QuanLyBanHang.Data.SanPham sp = context.SanPham.Find(id);

                    if (sp != null)
                    {
                        sp.HinhAnh = fileNameDb;
                        context.SanPham.Update(sp);
                        context.SaveChanges();

                        MessageBox.Show("Đổi ảnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmSanPham_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đổi ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
