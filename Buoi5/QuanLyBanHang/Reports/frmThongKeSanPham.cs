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
        ReportViewer reportViewer;

        // Sửa lại cách lấy đường dẫn gốc của Project
        string projectRoot = Directory.GetParent(Application.StartupPath).Parent.Parent.Parent.FullName;

        public frmThongKeSanPham()
        {
            InitializeComponent();
        }

        private void frmThongKeSanPham_Load(object sender, EventArgs e)
        {
            // 1. Khởi tạo ReportViewer
            reportViewer = new ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            this.Controls.Add(reportViewer);

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
    }
}