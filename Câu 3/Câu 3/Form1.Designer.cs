namespace Câu_3
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtSoPhong = new TextBox();
            numericUpDown = new NumericUpDown();
            cboxLoaiPhong = new ComboBox();
            cboxTinhTrang = new ComboBox();
            pictureBox = new PictureBox();
            btnChonAnh = new Button();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            cboxFilterTinhTrang = new ComboBox();
            cboxFilterLoaiPhong = new ComboBox();
            btnTimKiem = new Button();
            dataGridView = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // txtSoPhong
            // 
            txtSoPhong.Location = new Point(26, 73);
            txtSoPhong.Name = "txtSoPhong";
            txtSoPhong.Size = new Size(140, 39);
            txtSoPhong.TabIndex = 0;
            txtSoPhong.TextChanged += txtSoPhong_TextChanged;
            // 
            // numericUpDown
            // 
            numericUpDown.Location = new Point(218, 74);
            numericUpDown.Name = "numericUpDown";
            numericUpDown.Size = new Size(157, 39);
            numericUpDown.TabIndex = 1;
            numericUpDown.ValueChanged += numericUpDown_ValueChanged;
            // 
            // cboxLoaiPhong
            // 
            cboxLoaiPhong.FormattingEnabled = true;
            cboxLoaiPhong.Items.AddRange(new object[] { "Phòng Đơn", "Phòng Đôi", "Phòng VIP" });
            cboxLoaiPhong.Location = new Point(426, 74);
            cboxLoaiPhong.Name = "cboxLoaiPhong";
            cboxLoaiPhong.Size = new Size(169, 40);
            cboxLoaiPhong.TabIndex = 2;
            cboxLoaiPhong.SelectedIndexChanged += cboxLoaiPhong_SelectedIndexChanged;
            // 
            // cboxTinhTrang
            // 
            cboxTinhTrang.FormattingEnabled = true;
            cboxTinhTrang.Items.AddRange(new object[] { "Trống ", "Đang ở", "Đang dọn" });
            cboxTinhTrang.Location = new Point(638, 72);
            cboxTinhTrang.Name = "cboxTinhTrang";
            cboxTinhTrang.Size = new Size(169, 40);
            cboxTinhTrang.TabIndex = 3;
            cboxTinhTrang.SelectedIndexChanged += cboxTinhTrang_SelectedIndexChanged;
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(838, 23);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(205, 189);
            pictureBox.TabIndex = 4;
            pictureBox.TabStop = false;
            pictureBox.Click += pictureBox_Click;
            // 
            // btnChonAnh
            // 
            btnChonAnh.Location = new Point(850, 227);
            btnChonAnh.Name = "btnChonAnh";
            btnChonAnh.Size = new Size(177, 46);
            btnChonAnh.TabIndex = 5;
            btnChonAnh.Text = "Chọn ảnh từ thiết bị";
            btnChonAnh.UseVisualStyleBackColor = true;
            btnChonAnh.Click += btnChonAnh_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(1065, 23);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(152, 46);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(1263, 23);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(152, 46);
            btnSua.TabIndex = 7;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(1461, 23);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(152, 46);
            btnXoa.TabIndex = 8;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(1654, 23);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(152, 46);
            btnLamMoi.TabIndex = 9;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // cboxFilterTinhTrang
            // 
            cboxFilterTinhTrang.FormattingEnabled = true;
            cboxFilterTinhTrang.Items.AddRange(new object[] { "Trống", "Đang ở", "Đang dọn" });
            cboxFilterTinhTrang.Location = new Point(456, 323);
            cboxFilterTinhTrang.Name = "cboxFilterTinhTrang";
            cboxFilterTinhTrang.Size = new Size(270, 40);
            cboxFilterTinhTrang.TabIndex = 10;
            cboxFilterTinhTrang.Text = "Lọc theo tình trạng";
            cboxFilterTinhTrang.SelectedIndexChanged += cboxFilterTinhTrang_SelectedIndexChanged;
            // 
            // cboxFilterLoaiPhong
            // 
            cboxFilterLoaiPhong.FormattingEnabled = true;
            cboxFilterLoaiPhong.Items.AddRange(new object[] { "Phòng Đơn", "Phòng Đôi", "Phòng VIP" });
            cboxFilterLoaiPhong.Location = new Point(24, 323);
            cboxFilterLoaiPhong.Name = "cboxFilterLoaiPhong";
            cboxFilterLoaiPhong.Size = new Size(378, 40);
            cboxFilterLoaiPhong.TabIndex = 11;
            cboxFilterLoaiPhong.Text = "Lọc theo loại phòng";
            cboxFilterLoaiPhong.SelectedIndexChanged += cboxFilterLoaiPhong_SelectedIndexChanged;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(788, 319);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(185, 46);
            btnTimKiem.TabIndex = 12;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Dock = DockStyle.Bottom;
            dataGridView.Location = new Point(0, 386);
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersWidth = 82;
            dataGridView.Size = new Size(1831, 502);
            dataGridView.TabIndex = 13;
            dataGridView.CellContentClick += dataGridView_CellContentClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 23);
            label1.Name = "label1";
            label1.Size = new Size(118, 32);
            label1.TabIndex = 14;
            label1.Text = "Số phòng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(218, 23);
            label2.Name = "label2";
            label2.Size = new Size(98, 32);
            label2.TabIndex = 15;
            label2.Text = "Tầng số";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(638, 23);
            label3.Name = "label3";
            label3.Size = new Size(124, 32);
            label3.TabIndex = 16;
            label3.Text = "Tình trạng";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(426, 23);
            label4.Name = "label4";
            label4.Size = new Size(134, 32);
            label4.TabIndex = 17;
            label4.Text = "Loại phòng";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1831, 888);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridView);
            Controls.Add(btnTimKiem);
            Controls.Add(cboxFilterLoaiPhong);
            Controls.Add(cboxFilterTinhTrang);
            Controls.Add(btnLamMoi);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(btnChonAnh);
            Controls.Add(pictureBox);
            Controls.Add(cboxTinhTrang);
            Controls.Add(cboxLoaiPhong);
            Controls.Add(numericUpDown);
            Controls.Add(txtSoPhong);
            Name = "Form1";
            Text = "Quản Lý Phòng - Sunrise Homestay";
            ((System.ComponentModel.ISupportInitialize)numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtSoPhong;
        private NumericUpDown numericUpDown;
        private ComboBox cboxLoaiPhong;
        private ComboBox cboxTinhTrang;
        private PictureBox pictureBox;
        private Button btnChonAnh;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private ComboBox cboxFilterTinhTrang;
        private ComboBox cboxFilterLoaiPhong;
        private Button btnTimKiem;
        private DataGridView dataGridView;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
