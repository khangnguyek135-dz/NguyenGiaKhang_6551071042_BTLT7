namespace Câu_4
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            txtTenBN = new TextBox();
            txtSDT = new TextBox();
            dtpNgayKham = new DateTimePicker();
            dtpGioKham = new DateTimePicker();
            cboxBacSi = new ComboBox();
            cboxTrangThai = new ComboBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            dtpTuNgay = new DateTimePicker();
            dtpDenNgay = new DateTimePicker();
            label7 = new Label();
            label8 = new Label();
            cboxTimBacSi = new ComboBox();
            btnTimKiem = new Button();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 41);
            label1.Name = "label1";
            label1.Size = new Size(175, 32);
            label1.TabIndex = 0;
            label1.Text = "Tên bệnh nhân";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(21, 118);
            label2.Name = "label2";
            label2.Size = new Size(156, 32);
            label2.TabIndex = 1;
            label2.Text = "Số điện thoại";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 182);
            label3.Name = "label3";
            label3.Size = new Size(136, 32);
            label3.TabIndex = 2;
            label3.Text = "Ngày khám";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(595, 133);
            label4.Name = "label4";
            label4.Size = new Size(116, 32);
            label4.TabIndex = 3;
            label4.Text = "Giờ khám";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(821, 133);
            label5.Name = "label5";
            label5.Size = new Size(74, 32);
            label5.TabIndex = 4;
            label5.Text = "Bác sĩ";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(1374, 133);
            label6.Name = "label6";
            label6.Size = new Size(120, 32);
            label6.TabIndex = 5;
            label6.Text = "Trạng thái";
            // 
            // txtTenBN
            // 
            txtTenBN.Location = new Point(220, 38);
            txtTenBN.Name = "txtTenBN";
            txtTenBN.Size = new Size(334, 39);
            txtTenBN.TabIndex = 6;
            txtTenBN.TextChanged += txtTenBN_TextChanged;
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(220, 118);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(334, 39);
            txtSDT.TabIndex = 7;
            txtSDT.TextChanged += txtSDT_TextChanged;
            // 
            // dtpNgayKham
            // 
            dtpNgayKham.Format = DateTimePickerFormat.Short;
            dtpNgayKham.Location = new Point(220, 178);
            dtpNgayKham.Name = "dtpNgayKham";
            dtpNgayKham.Size = new Size(334, 39);
            dtpNgayKham.TabIndex = 8;
            dtpNgayKham.ValueChanged += dtpNgayKham_ValueChanged;
            // 
            // dtpGioKham
            // 
            dtpGioKham.Format = DateTimePickerFormat.Time;
            dtpGioKham.Location = new Point(595, 177);
            dtpGioKham.Name = "dtpGioKham";
            dtpGioKham.Size = new Size(176, 39);
            dtpGioKham.TabIndex = 9;
            dtpGioKham.ValueChanged += dtpGioKham_ValueChanged;
            // 
            // cboxBacSi
            // 
            cboxBacSi.FormattingEnabled = true;
            cboxBacSi.Location = new Point(821, 179);
            cboxBacSi.Name = "cboxBacSi";
            cboxBacSi.Size = new Size(519, 40);
            cboxBacSi.TabIndex = 10;
            cboxBacSi.SelectedIndexChanged += cboxBacSi_SelectedIndexChanged;
            // 
            // cboxTrangThai
            // 
            cboxTrangThai.FormattingEnabled = true;
            cboxTrangThai.Items.AddRange(new object[] { "Chờ khám", "Đã khám", "Đã hủy" });
            cboxTrangThai.Location = new Point(1374, 179);
            cboxTrangThai.Name = "cboxTrangThai";
            cboxTrangThai.Size = new Size(242, 40);
            cboxTrangThai.TabIndex = 11;
            cboxTrangThai.SelectedIndexChanged += cboxTrangThai_SelectedIndexChanged;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(953, 27);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(135, 46);
            btnThem.TabIndex = 12;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(1132, 27);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(135, 46);
            btnSua.TabIndex = 13;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(1307, 27);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(135, 46);
            btnXoa.TabIndex = 14;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(1481, 27);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(135, 46);
            btnLamMoi.TabIndex = 15;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpTuNgay.Location = new Point(127, 327);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(204, 39);
            dtpTuNgay.TabIndex = 16;
            dtpTuNgay.ValueChanged += dtpTuNgay_ValueChanged;
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Location = new Point(511, 327);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(200, 39);
            dtpDenNgay.TabIndex = 17;
            dtpDenNgay.ValueChanged += dtpDenNgay_ValueChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(21, 334);
            label7.Name = "label7";
            label7.Size = new Size(100, 32);
            label7.TabIndex = 18;
            label7.Text = "Từ ngày";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(367, 330);
            label8.Name = "label8";
            label8.Size = new Size(117, 32);
            label8.TabIndex = 19;
            label8.Text = "Đến ngày";
            // 
            // cboxTimBacSi
            // 
            cboxTimBacSi.FormattingEnabled = true;
            cboxTimBacSi.Location = new Point(748, 326);
            cboxTimBacSi.Name = "cboxTimBacSi";
            cboxTimBacSi.Size = new Size(519, 40);
            cboxTimBacSi.TabIndex = 20;
            cboxTimBacSi.SelectedIndexChanged += cboxTimBacSi_SelectedIndexChanged;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(1307, 322);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(135, 46);
            btnTimKiem.TabIndex = 21;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Bottom;
            dataGridView1.Location = new Point(0, 387);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 82;
            dataGridView1.Size = new Size(1644, 480);
            dataGridView1.TabIndex = 22;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1644, 867);
            Controls.Add(dataGridView1);
            Controls.Add(btnTimKiem);
            Controls.Add(cboxTimBacSi);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(dtpDenNgay);
            Controls.Add(dtpTuNgay);
            Controls.Add(btnLamMoi);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(cboxTrangThai);
            Controls.Add(cboxBacSi);
            Controls.Add(dtpGioKham);
            Controls.Add(dtpNgayKham);
            Controls.Add(txtSDT);
            Controls.Add(txtTenBN);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox txtTenBN;
        private TextBox txtSDT;
        private DateTimePicker dtpNgayKham;
        private DateTimePicker dtpGioKham;
        private ComboBox cboxBacSi;
        private ComboBox cboxTrangThai;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private DateTimePicker dtpTuNgay;
        private DateTimePicker dtpDenNgay;
        private Label label7;
        private Label label8;
        private ComboBox cboxTimBacSi;
        private Button btnTimKiem;
        private DataGridView dataGridView1;
    }
}
