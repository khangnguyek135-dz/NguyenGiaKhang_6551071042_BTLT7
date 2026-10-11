namespace Câu_1

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
            txtTL = new TextBox();
            txtTenTheLoai = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtMota = new TextBox();
            label5 = new Label();
            lblNgayTao = new Label();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            dgvTheLoai = new DataGridView();
            txtTimKiem = new TextBox();
            btnTimKiem = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvTheLoai).BeginInit();
            SuspendLayout();
            // 
            // txtTL
            // 
            txtTL.Location = new Point(195, 33);
            txtTL.Name = "txtTL";
            txtTL.ReadOnly = true;
            txtTL.Size = new Size(331, 39);
            txtTL.TabIndex = 0;
            txtTL.TextChanged += txtTL_TextChanged;
            // 
            // txtTenTheLoai
            // 
            txtTenTheLoai.Location = new Point(195, 100);
            txtTenTheLoai.Name = "txtTenTheLoai";
            txtTenTheLoai.Size = new Size(331, 39);
            txtTenTheLoai.TabIndex = 1;
            txtTenTheLoai.TextChanged += txtTenTheLoai_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 33);
            label1.Name = "label1";
            label1.Size = new Size(140, 32);
            label1.TabIndex = 2;
            label1.Text = "Mã thể loại:";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(34, 103);
            label2.Name = "label2";
            label2.Size = new Size(144, 32);
            label2.TabIndex = 3;
            label2.Text = "Tên thể loại:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(34, 170);
            label3.Name = "label3";
            label3.Size = new Size(82, 32);
            label3.TabIndex = 5;
            label3.Text = "Mô tả:";
            //label3.Click += label3_Click;
            // 
            // txtMota
            // 
            txtMota.Location = new Point(195, 170);
            txtMota.Multiline = true;
            txtMota.Name = "txtMota";
            txtMota.Size = new Size(331, 142);
            txtMota.TabIndex = 4;
            txtMota.TextChanged += txtMota_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(861, 280);
            label5.Name = "label5";
            label5.Size = new Size(116, 32);
            label5.TabIndex = 7;
            label5.Text = "Ngày tạo:";
            // 
            // lblNgayTao
            // 
            lblNgayTao.AutoSize = true;
            lblNgayTao.Location = new Point(1022, 280);
            lblNgayTao.Name = "lblNgayTao";
            lblNgayTao.Size = new Size(224, 32);
            lblNgayTao.TabIndex = 8;
            lblNgayTao.Text = "Hiển thị ngày ở đây";
            lblNgayTao.Click += lblNgayTao_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(622, 33);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(144, 46);
            btnThem.TabIndex = 9;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(780, 33);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(144, 46);
            btnSua.TabIndex = 10;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(939, 33);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(144, 46);
            btnXoa.TabIndex = 11;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(1100, 33);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(144, 46);
            btnLamMoi.TabIndex = 12;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // dgvTheLoai
            // 
            dgvTheLoai.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTheLoai.Dock = DockStyle.Bottom;
            dgvTheLoai.Location = new Point(0, 433);
            dgvTheLoai.Name = "dgvTheLoai";
            dgvTheLoai.RowHeadersWidth = 82;
            dgvTheLoai.Size = new Size(1283, 468);
            dgvTheLoai.TabIndex = 13;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(34, 367);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(492, 39);
            txtTimKiem.TabIndex = 14;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            // 
            // btnTimKiem
            // 
            btnTimKiem.Location = new Point(556, 363);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(150, 46);
            btnTimKiem.TabIndex = 15;
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1283, 901);
            Controls.Add(btnTimKiem);
            Controls.Add(txtTimKiem);
            Controls.Add(dgvTheLoai);
            Controls.Add(btnLamMoi);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(lblNgayTao);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(txtMota);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtTenTheLoai);
            Controls.Add(txtTL);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvTheLoai).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTL;
        private TextBox txtTenTheLoai;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtMota;
        private Label label5;
        private Label lblNgayTao;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private DataGridView dgvTheLoai;
        private TextBox txtTimKiem;
        private Button btnTimKiem;
    }
}
