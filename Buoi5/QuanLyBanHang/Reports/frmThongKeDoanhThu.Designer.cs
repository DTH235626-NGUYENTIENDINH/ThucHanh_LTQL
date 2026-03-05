namespace QuanLyBanHang.Reports
{
    partial class frmThongKeDoanhThu
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
            ((System.ComponentModel.ISupportInitialize)qlbhDataSet1).BeginInit();
            SuspendLayout();
            // 
            // qlbhDataSet1
            // 
            qlbhDataSet1.DataSetName = "QLBHDataSet";
            qlbhDataSet1.Namespace = "http://tempuri.org/QLBHDataSet.xsd";
            qlbhDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // frmThongKeDoanhThu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "frmThongKeDoanhThu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thống kê doanh thu";
            Load += frmThongKeDoanhThu_Load;
            ((System.ComponentModel.ISupportInitialize)qlbhDataSet1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private QLBHDataSet qlbhDataSet1;
    }
}