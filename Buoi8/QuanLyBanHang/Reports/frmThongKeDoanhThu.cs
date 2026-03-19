using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WinForms;
using QuanLyBanHang.Data;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static QuanLyBanHang.Reports.QLBHDataSet;

namespace QuanLyBanHang.Reports
{
    public partial class frmThongKeDoanhThu : Form
    {
        QLBHDbContext context = new QLBHDbContext();
        QLBHDataSet.DoanhThuSanPhamDataTable table = new QLBHDataSet.DoanhThuSanPhamDataTable();
        ReportViewer reportViewer;

        // Đường dẫn cố định của Lý
        private string reportsFolder = @"D:\AGU\Third_Year\LTQL\Thuchanh\QuanLyBanHang\QuanLyBanHang\Reports";

        public frmThongKeDoanhThu()
        {
            InitializeComponent();
        }

        private void frmThongKeDoanhThu_Load(object sender, EventArgs e)
        {
            // Khởi tạo ReportViewer
            reportViewer = new ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            this.Controls.Add(reportViewer);
            reportViewer.BringToFront();

            // Gọi hàm hiển thị ngay khi mở Form
            HienThiBaoCao();
        }

        private void btnLocKetQua_Click(object sender, EventArgs e)
        {
            // Gọi hàm hiển thị khi bấm nút Lọc
            HienThiBaoCao();
        }

        // --- HÀM XỬ LÝ CHUNG CHO CẢ LOAD VÀ CLICK ---
        private void HienThiBaoCao(bool isHienThiTatCa = false)
        {
            try
            {
                // Khởi tạo câu truy vấn ban đầu
                var query = context.HoaDon
                    .Include(h => h.HoaDon_ChiTiet)
                    .Include(h => h.NhanVien)
                    .Include(h => h.KhachHang)
                    .AsQueryable();

                string moTa = "";

                // Kiểm tra xem có phải đang bấm nút "Lọc kết quả" không
                if (isHienThiTatCa == false)
                {
                    DateTime tuNgay = dtpTuNgay.Value.Date;
                    DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);

                    // Ép điều kiện lọc theo ngày vào câu truy vấn
                    query = query.Where(h => h.NgayLap >= tuNgay && h.NgayLap <= denNgay);
                    moTa = $"Từ ngày {dtpTuNgay.Text} đến ngày {dtpDenNgay.Text}";
                }
                else
                {
                    moTa = "(Tất cả thời gian)";
                }

                // 1. Lấy dữ liệu chi tiết
                var data = query.AsEnumerable()
                    .Select(r => new
                    {
                        ID = r.ID,
                        NhanVienID = r.NhanVienID,
                        HoVaTenNhanVien = r.NhanVien?.HoVaTen ?? "Không có",
                        KhachHangID = r.KhachHangID,
                        HoVaTenKhachHang = r.KhachHang?.HoVaTen ?? "Khách lẻ",
                        NgayLap = r.NgayLap,
                        GhiChuHoaDon = r.GhiChuHoaDon ?? "",
                        TongTienHoaDon = r.HoaDon_ChiTiet.Sum(c => (double)c.SoLuongBan * (double)c.DonGiaBan)
                    })
                    .OrderBy(r => r.NgayLap)
                    .ToList();

                // 2. Đổ dữ liệu vào DataTable
                table.Clear();
                int stt = 1;
                foreach (var item in data)
                {
                    var newRow = table.NewDoanhThuSanPhamRow();
                    newRow.ID = stt++;
                    newRow.NhanVienID = item.NhanVienID;
                    newRow.HoVaTenNhanVien = item.HoVaTenNhanVien;
                    newRow.KhachHangID = item.KhachHangID;
                    newRow.HoVaTenKhachHang = item.HoVaTenKhachHang;
                    newRow.NgayLap = item.NgayLap;
                    newRow.GhiChuHoaDon = item.GhiChuHoaDon;
                    newRow.TongTienHoaDon = item.TongTienHoaDon;
                    table.AddDoanhThuSanPhamRow(newRow);
                }

                // 3. Hiển thị báo cáo
                string rdlcPath = Path.Combine(reportsFolder, "rptThongKeDoanhThu.rdlc");

                if (File.Exists(rdlcPath))
                {
                    reportViewer.LocalReport.ReportPath = rdlcPath;
                    reportViewer.LocalReport.DataSources.Clear();
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DoanhThuSanPham", (DataTable)table));

                    reportViewer.LocalReport.SetParameters(new ReportParameter("MoTaKetQuaHienThi", moTa));

                    reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                    reportViewer.ZoomMode = ZoomMode.Percent;
                    reportViewer.ZoomPercent = 100;
                    reportViewer.RefreshReport();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy file thiết kế báo cáo tại: " + rdlcPath);
                }
            }
            catch (Exception ex)
            {
                string loiChiTiet = ex.Message;
                if (ex.InnerException != null) loiChiTiet += "\nLỗi gốc: " + ex.InnerException.Message;
                MessageBox.Show("Lỗi thống kê: " + loiChiTiet);
            }
        }

        private void btnHienTatCa_Click(object sender, EventArgs e)
        {
            HienThiBaoCao(true);
        }
    }
}