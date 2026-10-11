namespace Câu_2
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
            txtHoTen = new TextBox();
            txtSDT = new TextBox();
            txtEmail = new TextBox();
            cboxHangThanhVien = new ComboBox();
            label5 = new Label();
            radioBtnNam = new RadioButton();
            radioBtnNu = new RadioButton();
            checkBox = new CheckBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            txtTimKiem = new TextBox();
            cboxTimKiem = new ComboBox();
            btnTimKiem = new Button();
            dateTimePicker = new DateTimePicker();
            label7 = new Label();
            dataGridView = new DataGridView();
            groupBox1 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 30);
            label1.Name = "label1";
            label1.Size = new Size(87, 32);
            label1.TabIndex = 0;
            label1.Text = "Họ tên";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(33, 93);
            label2.Name = "label2";
            label2.Size = new Size(156, 32);
            label2.TabIndex = 1;
            label2.Text = "Số điện thoại";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(33, 159);
            label3.Name = "label3";
            label3.Size = new Size(71, 32);
            label3.TabIndex = 2;
            label3.Text = "Email";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(33, 223);
            label4.Name = "label4";
            label4.Size = new Size(121, 32);
            label4.TabIndex = 3;
            label4.Text = "Ngày sinh";
            // 
            // txtHoTen
            // 
            txtHoTen.Location = new Point(266, 27);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(399, 39);
            txtHoTen.TabIndex = 4;
            txtHoTen.TextChanged += txtHoTen_TextChanged;
            // 
            // txtSDT
            // 
            txtSDT.Location = new Point(266, 90);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(399, 39);
            txtSDT.TabIndex = 5;
            txtSDT.TextChanged += txtSDT_TextChanged;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(266, 156);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(399, 39);
            txtEmail.TabIndex = 6;
            txtEmail.TextChanged += txtEmail_TextChanged;
            // 
            // cboxHangThanhVien
            // 
            cboxHangThanhVien.FormattingEnabled = true;
            cboxHangThanhVien.Items.AddRange(new object[] { "Basic", "VIP", "Premieum" });
            cboxHangThanhVien.Location = new Point(266, 292);
            cboxHangThanhVien.Name = "cboxHangThanhVien";
            cboxHangThanhVien.Size = new Size(399, 40);
            cboxHangThanhVien.TabIndex = 8;
            cboxHangThanhVien.SelectedIndexChanged += cboxHangThanhVien_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(33, 292);
            label5.Name = "label5";
            label5.Size = new Size(192, 32);
            label5.TabIndex = 9;
            label5.Text = "Hạng thành viên";
            // 
            // radioBtnNam
            // 
            radioBtnNam.AutoSize = true;
            radioBtnNam.Location = new Point(30, 63);
            radioBtnNam.Name = "radioBtnNam";
            radioBtnNam.Size = new Size(96, 36);
            radioBtnNam.TabIndex = 11;
            radioBtnNam.TabStop = true;
            radioBtnNam.Text = "Nam";
            radioBtnNam.UseVisualStyleBackColor = true;
            radioBtnNam.CheckedChanged += radioBtnNam_CheckedChanged;
            // 
            // radioBtnNu
            // 
            radioBtnNu.AutoSize = true;
            radioBtnNu.Location = new Point(170, 65);
            radioBtnNu.Name = "radioBtnNu";
            radioBtnNu.Size = new Size(77, 36);
            radioBtnNu.TabIndex = 12;
            radioBtnNu.TabStop = true;
            radioBtnNu.Text = "Nữ";
            radioBtnNu.UseVisualStyleBackColor = true;
            radioBtnNu.CheckedChanged += radioBtnNu_CheckedChanged;
            // 
            // checkBox
            // 
            checkBox.AutoSize = true;
            checkBox.Location = new Point(33, 358);
            checkBox.Name = "checkBox";
            checkBox.Size = new Size(221, 36);
            checkBox.TabIndex = 13;
            checkBox.Text = "Đang hoạt động";
            checkBox.UseVisualStyleBackColor = true;
            checkBox.CheckedChanged += checkBox_CheckedChanged;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(1097, 27);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(150, 46);
            btnThem.TabIndex = 14;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(1097, 104);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(150, 46);
            btnSua.TabIndex = 15;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(1097, 184);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(150, 46);
            btnXoa.TabIndex = 16;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(1097, 262);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(150, 46);
            btnLamMoi.TabIndex = 17;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(33, 437);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(551, 39);
            txtTimKiem.TabIndex = 18;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // cboxTimKiem
            // 
            cboxTimKiem.FormattingEnabled = true;
            cboxTimKiem.Items.AddRange(new object[] { "Basic", "VIP", "Premieum" });
            cboxTimKiem.Location = new Point(841, 437);
            cboxTimKiem.Name = "cboxTimKiem";
            cboxTimKiem.Size = new Size(187, 40);
            cboxTimKiem.TabIndex = 19;
            cboxTimKiem.SelectedIndexChanged += cboxTimKiem_SelectedIndexChanged;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(1097, 433);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(150, 46);
            btnTimKiem.TabIndex = 20;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // dateTimePicker
            // 
            dateTimePicker.Format = DateTimePickerFormat.Short;
            dateTimePicker.Location = new Point(266, 225);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(400, 39);
            dateTimePicker.TabIndex = 21;
            dateTimePicker.ValueChanged += dateTimePicker_ValueChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(643, 440);
            label7.Name = "label7";
            label7.Size = new Size(192, 32);
            label7.TabIndex = 22;
            label7.Text = "Hạng thành viên";
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Dock = DockStyle.Bottom;
            dataGridView.Location = new Point(0, 500);
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersWidth = 82;
            dataGridView.Size = new Size(1284, 347);
            dataGridView.TabIndex = 23;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioBtnNam);
            groupBox1.Controls.Add(radioBtnNu);
            groupBox1.Location = new Point(731, 30);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(297, 152);
            groupBox1.TabIndex = 24;
            groupBox1.TabStop = false;
            groupBox1.Text = "Giới tính";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1284, 847);
            Controls.Add(groupBox1);
            Controls.Add(dataGridView);
            Controls.Add(label7);
            Controls.Add(dateTimePicker);
            Controls.Add(btnTimKiem);
            Controls.Add(cboxTimKiem);
            Controls.Add(txtTimKiem);
            Controls.Add(btnLamMoi);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(checkBox);
            Controls.Add(label5);
            Controls.Add(cboxHangThanhVien);
            Controls.Add(txtEmail);
            Controls.Add(txtSDT);
            Controls.Add(txtHoTen);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Quản Lí Hội Viên Phòng Gym FitZone";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtHoTen;
        private TextBox txtSDT;
        private TextBox txtEmail;
        private ComboBox cboxHangThanhVien;
        private Label label5;
        private RadioButton radioBtnNam;
        private RadioButton radioBtnNu;
        private CheckBox checkBox;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private TextBox txtTimKiem;
        private ComboBox cboxTimKiem;
        private Button btnTimKiem;
        private DateTimePicker dateTimePicker;
        private Label label7;
        private DataGridView dataGridView;
        private GroupBox groupBox1;
    }
}
