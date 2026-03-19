using Microsoft.Reporting.WinForms;
using QuanLyBanHang.Data;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanHang.Reports
{
    public partial class frmThongKeSanPham : Form
    {
        QLBHDbContext context = new QLBHDbContext();
        QLBHDataSet.DanhSachSanPhamDataTable danhSachSanPhamDataTable = new QLBHDataSet.DanhSachSanPhamDataTable();
        ReportViewer reportViewer = new ReportViewer();
        private string reportsFolder = @"D:\AGU\Third_Year\LTQL\Thuchanh\QuanLyBanHang\QuanLyBanHang\Reports";


        // Sửa lại cách lấy đường dẫn gốc của Project
        string projectRoot = Directory.GetParent(Application.StartupPath).Parent.Parent.Parent.FullName;

        public frmThongKeSanPham()
        {
            InitializeComponent();
        }
        public void LayLoaiSanPhamVaoComboBox()
        {
            var dsLoai = context.LoaiSanPham.ToList();
            cboLoaiSanPham.DataSource = dsLoai;
            cboLoaiSanPham.DisplayMember = "TenLoai";
            cboLoaiSanPham.ValueMember = "ID";
            cboLoaiSanPham.SelectedIndex = -1;
        }

        public void LayHangSanXuatVaoComboBox()
        {
            var dsHang = context.HangSanXuat.ToList();
            cboHangSanXuat.DataSource = dsHang;
            cboHangSanXuat.DisplayMember = "TenHangSanXuat";
            cboHangSanXuat.ValueMember = "ID";
            cboHangSanXuat.SelectedIndex = -1;
        }
        private void frmThongKeSanPham_Load(object sender, EventArgs e)
        {

            LayLoaiSanPhamVaoComboBox();
            LayHangSanXuatVaoComboBox();


            // 1. Xóa nó khỏi các container cũ (nếu có) để tránh xung đột
            panel2.Controls.Clear();

            // 2. Thêm reportViewer vào trong panel2
            panel2.Controls.Add(reportViewer);

            // 3. Thiết lập Dock để nó tự lấp đầy diện tích của panel2
            reportViewer.Dock = DockStyle.Fill;

            // 4. Cập nhật lại báo cáo
            reportViewer.RefreshReport();


            // 2. Lấy dữ liệu (Dùng "new" ẩn danh để tránh lỗi class DanhSachSanPham)
            var danhSachSanPham = context.SanPham.Select(r => new DanhSachSanPham
            {
                ID = r.ID,
                HangSanXuatID = r.HangSanXuatID,
                TenHangSanXuat = r.HangSanXuat.TenHangSanXuat,
                LoaiSanPhamID = r.LoaiSanPhamID,
                TenLoai = r.LoaiSanPham.TenLoai,
                TenSanPham = r.TenSanPham,
                DonGia = r.DonGia,
                SoLuong = r.SoLuong,
                HinhAnh = r.HinhAnh,
                MoTa = r.MoTa
            }).ToList();

            // 3. Đổ dữ liệu vào DataTable
            danhSachSanPhamDataTable.Clear();
            foreach (var row in danhSachSanPham)
            {
                danhSachSanPhamDataTable.AddDanhSachSanPhamRow(
                    row.ID,
                    row.HangSanXuatID,
                    row.TenHangSanXuat,
                    row.LoaiSanPhamID,
                    row.TenLoai,
                    row.TenSanPham,
                    row.DonGia,
                    row.SoLuong,
                    row.HinhAnh,
                    row.MoTa
                );
            }

            // 4. Cấu hình DataSource
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DanhSachSanPham"; // Phải khớp với tên Dataset trong file .rdlc
            reportDataSource.Value = danhSachSanPhamDataTable;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);

            // 5. Thiết lập đường dẫn file thiết kế (.rdlc)
            // Lưu ý: "Reports" là tên thư mục chứa file .rdlc của bạn
            string rdlcPath = Path.Combine(projectRoot, "Reports", "rptThongKeSanPham.rdlc");

            reportViewer.LocalReport.ReportPath = rdlcPath;

            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);

            ReportParameter reportParameter = new ReportParameter("MoTaKetQuaHienThi", "(Tất cả sản phẩm)");
            reportViewer.LocalReport.SetParameters(reportParameter);

            if (File.Exists(rdlcPath))
            {
                reportViewer.LocalReport.ReportPath = rdlcPath;

                // 6. Cấu hình hiển thị
                reportViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer.ZoomMode = ZoomMode.Percent;
                reportViewer.ZoomPercent = 100;

                reportViewer.RefreshReport();
            }
            else
            {
                MessageBox.Show("Không tìm thấy file thiết kế báo cáo tại: " + rdlcPath);
            }
        }

        private void btnLocKetQua_Click(object sender, EventArgs e)
        {
            if (cboHangSanXuat.Text == "" && cboLoaiSanPham.Text == "")
            {
                // Nếu cả 2 ComboBox đều bỏ trống thì hiển thị tất cả 
                frmThongKeSanPham_Load(sender, e);
            }
            else
            {
                var danhSachSanPham = context.SanPham.Select(r => new DanhSachSanPham
                {
                    ID = r.ID,
                    HangSanXuatID = r.HangSanXuatID,
                    TenHangSanXuat = r.HangSanXuat.TenHangSanXuat,
                    LoaiSanPhamID = r.LoaiSanPhamID,
                    TenLoai = r.LoaiSanPham.TenLoai,
                    TenSanPham = r.TenSanPham,
                    DonGia = r.DonGia,
                    SoLuong = r.SoLuong,
                    HinhAnh = r.HinhAnh,
                    MoTa = r.MoTa
                });

                string hangSanXuat = null;
                string loaiSanPham = null;

                if (cboHangSanXuat.Text != "")
                {
                    int hangSanXuatID = Convert.ToInt32(cboHangSanXuat.SelectedValue.ToString());
                    hangSanXuat = "Hãng sản xuất: " + cboHangSanXuat.Text;
                    danhSachSanPham = danhSachSanPham.Where(r => r.HangSanXuatID == hangSanXuatID);
                }
                if (cboLoaiSanPham.Text != "")
                {
                    int loaiSanPhamID = Convert.ToInt32(cboLoaiSanPham.SelectedValue.ToString());
                    loaiSanPham += "Phân loại: " + cboLoaiSanPham.Text;
                    danhSachSanPham = danhSachSanPham.Where(r => r.LoaiSanPhamID == loaiSanPhamID);
                }

                danhSachSanPhamDataTable.Clear();
                foreach (var row in danhSachSanPham)
                {
                    danhSachSanPhamDataTable.AddDanhSachSanPhamRow(row.ID,
                        row.HangSanXuatID,
                        row.TenHangSanXuat,
                        row.LoaiSanPhamID,
                        row.TenLoai,
                        row.TenSanPham,
                        row.DonGia,
                        row.SoLuong,
                        row.HinhAnh,
                        row.MoTa);
                }

                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DanhSachSanPham";
                reportDataSource.Value = danhSachSanPhamDataTable;

                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(reportDataSource);
                reportViewer.LocalReport.ReportPath = Path.Combine(reportsFolder, "rptThongKeSanPham.rdlc");

                // 1. Đảm bảo chuỗi không bị null để tránh lỗi cộng chuỗi
                string hSX = string.IsNullOrEmpty(hangSanXuat) ? "Tất cả" : hangSanXuat;
                string lSP = string.IsNullOrEmpty(loaiSanPham) ? "Tất cả" : loaiSanPham;
                string moTaHienThi = $"({hSX} - {lSP})";

                try
                {
                    // 2. Gán Path trước khi gán Parameter (Bắt buộc)
                    string rdlcPath = Path.Combine(reportsFolder, "rptThongKeSanPham.rdlc");
                    if (File.Exists(rdlcPath))
                    {
                        reportViewer.LocalReport.ReportPath = rdlcPath;

                        // 3. Khởi tạo Parameter an toàn
                        ReportParameter p = new ReportParameter("MoTaKetQuaHienThi", moTaHienThi);

                        // 4. SetParameters phải nằm TRƯỚC RefreshReport
                        reportViewer.LocalReport.SetParameters(new ReportParameter[] { p });

                        reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                        reportViewer.RefreshReport();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy file RDLC tại: " + rdlcPath);
                    }
                }
                catch (Exception ex)
                {
                    // Xem lỗi chi tiết nhất ở InnerException
                    MessageBox.Show("Lỗi chi tiết: " + ex.InnerException?.Message ?? ex.Message);
                }
                
                reportViewer.ZoomMode = ZoomMode.Percent;
                reportViewer.ZoomPercent = 100;
                reportViewer.RefreshReport();
            }
        }
    }
}