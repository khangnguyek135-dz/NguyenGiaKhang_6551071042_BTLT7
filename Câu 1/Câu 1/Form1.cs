using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Câu_1.Models;

namespace Câu_1
{
    public partial class Form1 : Form
    {
        private readonly QuanLyNhaSachContext context;

        public Form1()
        {
            InitializeComponent();

            dgvTheLoai.SelectionChanged += dataGridView1_SelectionChanged;

            var options = new DbContextOptionsBuilder<QuanLyNhaSachContext>()
                .UseSqlServer("Server=N-Khang\\SQLEXPRESS;Database=QuanLyNhaSach;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            context = new QuanLyNhaSachContext(options);

            this.Load += Form1_Load;
            this.FormClosed += Form1_FormClosed;
        }

        // ==================== HIỂN THỊ DỮ LIỆU ====================
        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync(string keyword = "")
        {
            try
            {
                var query = context.TheLoaiSaches
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = keyword.Trim();
                    query = query.Where(x => x.TenTheLoai.Contains(keyword));
                }

                var danhSach = await query
                    .OrderBy(x => x.MaTl)
                    .ToListAsync();

                dgvTheLoai.DataSource = null;
                dgvTheLoai.DataSource = danhSach;

                if (dgvTheLoai.Columns.Contains("MaTl"))
                    dgvTheLoai.Columns["MaTl"].HeaderText = "Mã thể loại";
                if (dgvTheLoai.Columns.Contains("TenTheLoai"))
                    dgvTheLoai.Columns["TenTheLoai"].HeaderText = "Tên thể loại";
                if (dgvTheLoai.Columns.Contains("MoTa"))
                    dgvTheLoai.Columns["MoTa"].HeaderText = "Mô tả";
                if (dgvTheLoai.Columns.Contains("SoLuongSach"))
                    dgvTheLoai.Columns["SoLuongSach"].HeaderText = "Số lượng sách";
                if (dgvTheLoai.Columns.Contains("NgayTao"))
                    dgvTheLoai.Columns["NgayTao"].HeaderText = "Ngày tạo";

                dgvTheLoai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTheLoai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvTheLoai.MultiSelect = false;
                dgvTheLoai.ReadOnly = true;
                dgvTheLoai.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==================== CHỌN DÒNG ====================
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTheLoai.CurrentRow?.DataBoundItem is not TheLoaiSach item)
                return;

            txtTL.Text = item.MaTl.ToString();
            txtTenTheLoai.Text = item.TenTheLoai;
            txtMota.Text = item.MoTa ?? "";
            lblNgayTao.Text = $"{item.NgayTao:dd/MM/yyyy HH:mm:ss}";
        }

        // Các sự kiện trống (giữ lại để không lỗi với Designer)
        private void label1_Click(object sender, EventArgs e) { }
        private void txtTimKiem_TextChanged(object sender, EventArgs e) { }
        private void txtTL_TextChanged(object sender, EventArgs e) { }
        private void txtTenTheLoai_TextChanged(object sender, EventArgs e) { }
        private void txtMota_TextChanged(object sender, EventArgs e) { }
        private void lblNgayTao_Click(object sender, EventArgs e) { }

        // ==================== THÊM ====================
        private async void btnThem_Click(object sender, EventArgs e)
        {
            string ten = txtTenTheLoai.Text.Trim();
            string moTa = txtMota.Text.Trim();

            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Vui lòng nhập tên thể loại!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTheLoai.Focus();
                return;
            }

            try
            {
                bool daTonTai = await context.TheLoaiSaches
                    .AnyAsync(x => x.TenTheLoai == ten);

                if (daTonTai)
                {
                    MessageBox.Show("Tên thể loại đã tồn tại!", "Trùng tên",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var item = new TheLoaiSach
                {
                    TenTheLoai = ten,
                    MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa,
                    SoLuongSach = 0,
                    NgayTao = DateTime.Now
                };

                context.TheLoaiSaches.Add(item);
                await context.SaveChangesAsync();

                MessageBox.Show("Thêm thể loại thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync(txtTimKiem.Text);
                ClearInputs();
            }
            catch (DbUpdateException)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể thêm dữ liệu. Có thể tên thể loại đã tồn tại.",
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== SỬA ====================
        private async void btnSua_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtTL.Text, out int maTL))
            {
                MessageBox.Show("Vui lòng chọn thể loại cần sửa!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ten = txtTenTheLoai.Text.Trim();
            string moTa = txtMota.Text.Trim();

            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Tên thể loại không được để trống!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTheLoai.Focus();
                return;
            }

            try
            {
                bool trungTen = await context.TheLoaiSaches
                    .AnyAsync(x => x.TenTheLoai == ten && x.MaTl != maTL);

                if (trungTen)
                {
                    MessageBox.Show("Tên thể loại này đã được sử dụng!", "Trùng tên",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var item = await context.TheLoaiSaches.FindAsync(maTL);

                if (item == null)
                {
                    MessageBox.Show("Không tìm thấy thể loại cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                item.TenTheLoai = ten;
                item.MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa;

                await context.SaveChangesAsync();

                MessageBox.Show("Cập nhật thể loại thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync(txtTimKiem.Text);
                ClearInputs();
            }
            catch (DbUpdateException)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể cập nhật. Hãy kiểm tra dữ liệu và ràng buộc.",
                    "Lỗi dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== XÓA ====================
        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtTL.Text, out int maTL))
            {
                MessageBox.Show("Vui lòng chọn thể loại cần xóa!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa thể loại này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                var item = await context.TheLoaiSaches.FindAsync(maTL);

                if (item == null)
                {
                    MessageBox.Show("Thể loại không còn tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await LoadDataAsync(txtTimKiem.Text);
                    ClearInputs();
                    return;
                }

                context.TheLoaiSaches.Remove(item);
                await context.SaveChangesAsync();

                MessageBox.Show("Xóa thể loại thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync(txtTimKiem.Text);
                ClearInputs();
            }
            catch (DbUpdateException)
            {
                // Hủy trạng thái "Deleted" để lần lưu sau không bị xóa lại.
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể xóa thể loại này vì đang được dữ liệu khác tham chiếu.",
                    "Không thể xóa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== TÌM KIẾM ====================
        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadDataAsync(txtTimKiem.Text);
            ClearInputs();
        }

        // ==================== LÀM MỚI ====================
        private async void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            await LoadDataAsync();
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtTL.Clear();
            txtTenTheLoai.Clear();
            txtMota.Clear();
            lblNgayTao.Text = "";

            dgvTheLoai.ClearSelection();
            dgvTheLoai.CurrentCell = null;
            txtTenTheLoai.Focus();
        }

        // ==================== ĐÓNG FORM ====================
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            context.Dispose();
        }
    }
}