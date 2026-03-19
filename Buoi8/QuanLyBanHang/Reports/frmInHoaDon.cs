using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WinForms;
using QuanLyBanHang.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static QuanLyBanHang.Reports.QLBHDataSet;

namespace QuanLyBanHang.Reports
{
    public partial class frmInHoaDon : Form
    {
        QLBHDbContext context = new QLBHDbContext();
        QLBHDataSet.DoanhSachHoaDon_ChiTietDataTable table = new QLBHDataSet.DoanhSachHoaDon_ChiTietDataTable();

        // Khai báo biến ReportViewer
        ReportViewer reportViewer1;

        // Dùng đường dẫn cố định an toàn nhất (giống form trước của Lý)
        string reportsFolder = @"D:\AGU\Third_Year\LTQL\Thuchanh\QuanLyBanHang\QuanLyBanHang\Reports";

        int id; // Mã hóa đơn

        public frmInHoaDon(int maHoaDon = 0)
        {
            InitializeComponent();
            id = maHoaDon;
        }
        private void frmInHoaDon_Load_1(object sender, EventArgs e)
        {
            // 1. Khởi tạo ReportViewer tự động bằng code
            reportViewer1 = new ReportViewer();
            reportViewer1.Dock = DockStyle.Fill;
            this.Controls.Add(reportViewer1);
            reportViewer1.BringToFront();

            try
            {
                // 2. Lấy thông tin Hóa đơn
                var hoaDon = context.HoaDon
                    .Include(r => r.KhachHang)
                    .Include(r => r.HoaDon_ChiTiet)
                    .Where(r => r.ID == id)
                    .SingleOrDefault();

                if (hoaDon != null)
                {
                    // 3. Lấy chi tiết các sản phẩm trong hóa đơn đó
                    var hoaDonChiTiet = context.HoaDon_ChiTiet
                        .Include(r => r.SanPham)
                        .Where(r => r.HoaDonID == id)
                        .ToList();

                    // 4. Đổ dữ liệu vào DataTable (Cách an toàn)
                    table.Clear();
                    foreach (var row in hoaDonChiTiet)
                    {
                        var newRow = table.NewDoanhSachHoaDon_ChiTietRow();
                        newRow.ID = row.ID;
                        newRow.HoaDonID = row.HoaDonID;
                        newRow.SanPhamID = row.SanPhamID;
                        newRow.TenSanPham = row.SanPham?.TenSanPham ?? "Không rõ";
                        newRow.SoLuongBan = row.SoLuongBan;
                        newRow.DonGiaBan = (int)row.DonGiaBan;
                        newRow.ThanhTien = (int)(row.SoLuongBan * row.DonGiaBan);

                        table.AddDoanhSachHoaDon_ChiTietRow(newRow);
                    }

                    string rdlcPath = Path.Combine(reportsFolder, "rptInHoaDon.rdlc");

                    if (File.Exists(rdlcPath))
                    {
                        // 5. Gán đường dẫn trước
                        reportViewer1.LocalReport.ReportPath = rdlcPath;

                        // 6. Gán DataSource (Tên phải khớp 100% với file rdlc)
                        reportViewer1.LocalReport.DataSources.Clear();
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DanhSachHoaDon_ChiTiet", (DataTable)table));

                        // 7. Truyền 8 cái Parameters
                        IList<ReportParameter> param = new List<ReportParameter>
                        {
                            new ReportParameter("NgayLap", string.Format("Ngày {0} Tháng {1} Năm {2}", hoaDon.NgayLap.Day, hoaDon.NgayLap.Month, hoaDon.NgayLap.Year)),
                            new ReportParameter("NguoiBan_Ten", "CÔNG TY TNHH LAZY ANT"),
                            new ReportParameter("NguoiBan_DiaChi", "Mỹ Phước, TP. Long Xuyên, An Giang"),
                            new ReportParameter("NguoiBan_MaSoThue", "1602162070"),
                            new ReportParameter("NguoiMua_Ten", hoaDon.KhachHang?.HoVaTen ?? "Khách lẻ"),
                            new ReportParameter("NguoiMua_DiaChi", hoaDon.KhachHang?.DiaChi ?? "Không có"),
                            new ReportParameter("NguoiMua_MaSoThue", ""),
                            
                            // Tổng tiền
                            new ReportParameter("TongTien", hoaDon.HoaDon_ChiTiet.Sum(r => r.SoLuongBan * r.DonGiaBan).ToString("N0"))
                        };

                        reportViewer1.LocalReport.SetParameters(param);

                        // 8. Cấu hình và hiển thị
                        reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                        reportViewer1.ZoomMode = ZoomMode.Percent;
                        reportViewer1.ZoomPercent = 100;
                        reportViewer1.RefreshReport();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy file mẫu hóa đơn tại: " + rdlcPath);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dữ liệu cho Hóa đơn số: " + id);
                }
            }
            catch (Exception ex)
            {
                string loiChiTiet = ex.Message;
                if (ex.InnerException != null) loiChiTiet += "\nLỗi gốc: " + ex.InnerException.Message;
                MessageBox.Show("Lỗi in hóa đơn: " + loiChiTiet);
            }
        }
    
    }
}