namespace QuanLyBanHang.Reports
{
    partial class frmThongKeSanPham
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            qlbhDataSet1 = new QLBHDataSet();
            cboHangSanXuat = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            cboLoaiSanPham = new ComboBox();
            btnLocKetQua = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)qlbhDataSet1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // qlbhDataSet1
            // 
            qlbhDataSet1.DataSetName = "QLBHDataSet";
            qlbhDataSet1.Namespace = "http://tempuri.org/QLBHDataSet.xsd";
            qlbhDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cboHangSanXuat
            // 
            cboHangSanXuat.FormattingEnabled = true;
            cboHangSanXuat.Location = new Point(142, 30);
            cboHangSanXuat.Name = "cboHangSanXuat";
            cboHangSanXuat.Size = new Size(160, 28);
            cboHangSanXuat.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 33);
            label1.Name = "label1";
            label1.Size = new Size(106, 20);
            label1.TabIndex = 1;
            label1.Text = "Hãng sản xuất:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(339, 33);
            label2.Name = "label2";
            label2.Size = new Size(108, 20);
            label2.TabIndex = 2;
            label2.Text = "Loại sản phẩm:";
            // 
            // cboLoaiSanPham
            // 
            cboLoaiSanPham.FormattingEnabled = true;
            cboLoaiSanPham.Location = new Point(453, 33);
            cboLoaiSanPham.Name = "cboLoaiSanPham";
            cboLoaiSanPham.Size = new Size(159, 28);
            cboLoaiSanPham.TabIndex = 3;
            // 
            // btnLocKetQua
            // 
            btnLocKetQua.Location = new Point(636, 33);
            btnLocKetQua.Name = "btnLocKetQua";
            btnLocKetQua.Size = new Size(118, 29);
            btnLocKetQua.TabIndex = 4;
            btnLocKetQua.Text = "Lọc kết quả";
            btnLocKetQua.UseVisualStyleBackColor = true;
            btnLocKetQua.Click += btnLocKetQua_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(cboLoaiSanPham);
            panel1.Controls.Add(btnLocKetQua);
            panel1.Controls.Add(cboHangSanXuat);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 78);
            panel1.TabIndex = 5;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 78);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 372);
            panel2.TabIndex = 6;
            // 
            // frmThongKeSanPham
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "frmThongKeSanPham";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thống kê sản phẩm";
            Load += frmThongKeSanPham_Load;
            ((System.ComponentModel.ISupportInitialize)qlbhDataSet1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private QLBHDataSet qlbhDataSet1;
        private ComboBox cboHangSanXuat;
        private Label label1;
        private Label label2;
        private ComboBox cboLoaiSanPham;
        private Button btnLocKetQua;
        private Panel panel1;
        private Panel panel2;
    }
}