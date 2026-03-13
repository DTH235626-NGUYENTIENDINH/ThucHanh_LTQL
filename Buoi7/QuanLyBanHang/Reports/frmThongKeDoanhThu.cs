using Microsoft.Reporting.WinForms;
using QuanLyBanHang.Data;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
namespace QuanLyBanHang.Reports
{
    public partial class frmThongKeDoanhThu : Form
    {
        QLBHDbContext context = new QLBHDbContext();
        QLBHDataSet.DoanhThuSanPhamDataTable table = new QLBHDataSet.DoanhThuSanPhamDataTable();
        string reportsFolder = Application.StartupPath.Replace("bin\\Debug\\net5.0-windows", "Reports");
        ReportViewer reportViewer;

        public frmThongKeDoanhThu()
        {
            InitializeComponent();
        }

        private void frmThongKeDoanhThu_Load(object sender, EventArgs e)
        {
            // A. Khởi tạo ReportViewer (Nếu bạn không kéo thả vào Form)
            reportViewer = new ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            this.Controls.Add(reportViewer);
            reportViewer.BringToFront();

            try
            {
                // B. Bước 1: Lấy dữ liệu thô từ SQL và tính toán trên RAM (AsEnumerable)
                // Điều này giúp tránh lỗi "Translation" của Entity Framework
                var data = context.HoaDon
                    .Include(h => h.HoaDon_ChiTiet)
                    .AsEnumerable()
                    .GroupBy(h => h.NgayLap.Date)
                    .Select(g => new
                    {
                        Ngay = g.Key,
                        SoHoaDon = g.Count(),
                        // Tính tổng doanh thu (ép kiểu double để khớp với DataSet)
                        TongDoanhThu = g.Sum(h => h.HoaDon_ChiTiet.Sum(c => (double)c.SoLuongBan * (double)c.DonGiaBan))
                    })
                    .OrderBy(r => r.Ngay)
                    .ToList();

                // C. Bước 2: Đổ dữ liệu vào DataTable (Trung gian để truyền vào Report)
                table.Clear();
                foreach (var item in data)
                {
                    table.AddDoanhThuSanPhamRow(item.Ngay, item.SoHoaDon, (double)item.TongDoanhThu);
                }

                // D. Bước 3: Tìm file thiết kế và truyền dữ liệu vào ReportViewer
                string projectRoot = Directory.GetParent(Application.StartupPath).Parent.Parent.Parent.FullName;
                string path = Path.Combine(projectRoot, "Reports", "rptThongKeDoanhThu.rdlc");

                if (File.Exists(path))
                {
                    reportViewer.LocalReport.ReportPath = path;
                    reportViewer.LocalReport.DataSources.Clear();

                    // QUAN TRỌNG: Chữ "DoanhThuSanPham" PHẢI KHỚP 100% với tên Dataset trong file .rdlc
                    ReportDataSource rds = new ReportDataSource("DoanhThuSanPham", (DataTable)table);
                    reportViewer.LocalReport.DataSources.Add(rds);

                    // Cấu hình hiển thị và làm mới báo cáo
                    reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                    reportViewer.ZoomMode = ZoomMode.Percent;
                    reportViewer.ZoomPercent = 100;
                    reportViewer.RefreshReport();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy file thiết kế báo cáo!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truyền dữ liệu: " + ex.Message);
            }
        }
    }
}