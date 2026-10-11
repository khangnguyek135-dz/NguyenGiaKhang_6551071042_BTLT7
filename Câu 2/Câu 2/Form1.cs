using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Câu_2.Models;

namespace Câu_2
{
    public partial class Form1 : Form
    {
        private readonly FitZoneContext context;

        // Danh sách hội viên đang hiển thị trên lưới (dùng để đổ dữ liệu lên control khi chọn dòng).
        private List<HoiVien> dsHienTai = new();

        // Mã hội viên đang chọn (0 = chưa chọn). Form không có ô Mã HV nên lưu ở biến này.
        private int maHVDangChon = 0;

        // Chặn SelectionChanged chạy khi đang nạp/xóa chọn lưới bằng code.
        private bool dangNapDuLieu = false;

        // Dòng dữ liệu hiển thị trên lưới (đã đổi bool -> chữ, ngày -> chuỗi cho dễ đọc).
        private class HoiVienRow
        {
            public int MaHV { get; set; }
            public string HoTen { get; set; } = "";
            public string GioiTinh { get; set; } = "";
            public string NgaySinh { get; set; } = "";
            public string SDT { get; set; } = "";
            public string Email { get; set; } = "";
            public string HangThanhVien { get; set; } = "";
            public string NgayDangKy { get; set; } = "";
            public string TrangThai { get; set; } = "";
        }

        public Form1()
        {
            InitializeComponent();

            var options = new DbContextOptionsBuilder<FitZoneContext>()
                .UseSqlServer("Server=N-Khang\\SQLEXPRESS;Database=FitZone;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            context = new FitZoneContext(options);

            // ComboBox Hạng thành viên (nạp cứng 3 giá trị).
            cboxHangThanhVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxHangThanhVien.Items.Clear();
            cboxHangThanhVien.Items.AddRange(new object[] { "Basic", "VIP", "Premium" });

            // ComboBox lọc: thêm "Tất cả" để có thể tìm chỉ theo họ tên.
            cboxTimKiem.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxTimKiem.Items.Clear();
            cboxTimKiem.Items.AddRange(new object[] { "Tất cả", "Basic", "VIP", "Premium" });
            cboxTimKiem.SelectedIndex = 0;

            dateTimePicker.Format = DateTimePickerFormat.Short;

            // Cấu hình lưới (bỏ qua nếu đã đặt trong Designer).
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;

            // Gắn sự kiện bằng code.
            dataGridView.SelectionChanged += dataGridView_SelectionChanged;
            this.Load += Form1_Load;
            this.FormClosed += Form1_FormClosed;
        }

        // ==================== CHUYỂN ĐỔI NGÀY ====================
        // NgaySinh trong Model là DateOnly (EF Core 8+ với cột kiểu date).
        // Nếu Model của bạn là DateTime thì thay 2 hàm này bằng:
        //   private static DateTime ToDateTime(DateTime d) => d;
        //   private static DateTime ToModelDate(DateTime d) => d.Date;
        private static DateTime ToDateTime(DateOnly d) => d.ToDateTime(TimeOnly.MinValue);
        private static DateOnly ToModelDate(DateTime d) => DateOnly.FromDateTime(d);

        // ==================== HIỂN THỊ DỮ LIỆU ====================
        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
            ClearInputs();
        }

        /// <summary>
        /// Nạp danh sách, có áp dụng bộ lọc hiện tại: Họ tên (Contains) VÀ Hạng thành viên.
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim();
                string hangLoc = cboxTimKiem.SelectedIndex > 0 ? cboxTimKiem.Text : "";

                bool coTen = tuKhoa != "";
                bool coHang = hangLoc != "";

                // Nhiều điều kiện nối bằng && ; điều kiện nào không nhập thì bỏ qua.
                dsHienTai = await context.HoiViens
                    .AsNoTracking()
                    .Where(x => (!coTen || x.HoTen.Contains(tuKhoa))
                             && (!coHang || x.HangThanhVien == hangLoc))
                    .OrderBy(x => x.MaHv)
                    .ToListAsync();

                var rows = dsHienTai.Select(x => new HoiVienRow
                {
                    MaHV = x.MaHv,
                    HoTen = x.HoTen,
                    GioiTinh = x.GioiTinh ? "Nam" : "Nữ",
                    NgaySinh = x.NgaySinh.ToString("dd/MM/yyyy"),
                    SDT = x.Sdt ?? "",
                    Email = x.Email ?? "",
                    HangThanhVien = x.HangThanhVien,
                    NgayDangKy = x.NgayDangKy.ToString("dd/MM/yyyy HH:mm"),
                    TrangThai = x.TrangThai ? "Đang hoạt động" : "Tạm ngưng"
                }).ToList();

                dangNapDuLieu = true;
                try
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = rows;
                    CauHinhCot();
                }
                finally
                {
                    dangNapDuLieu = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CauHinhCot()
        {
            var tieuDe = new Dictionary<string, string>
            {
                ["MaHV"] = "Mã HV",
                ["HoTen"] = "Họ tên",
                ["GioiTinh"] = "Giới tính",
                ["NgaySinh"] = "Ngày sinh",
                ["SDT"] = "SĐT",
                ["Email"] = "Email",
                ["HangThanhVien"] = "Hạng",
                ["NgayDangKy"] = "Ngày đăng ký",
                ["TrangThai"] = "Trạng thái"
            };

            foreach (var kv in tieuDe)
            {
                if (dataGridView.Columns.Contains(kv.Key))
                    dataGridView.Columns[kv.Key].HeaderText = kv.Value;
            }

            if (dataGridView.Columns.Contains("MaHV"))
                dataGridView.Columns["MaHV"].FillWeight = 40;
            if (dataGridView.Columns.Contains("GioiTinh"))
                dataGridView.Columns["GioiTinh"].FillWeight = 50;
        }

        // ==================== CHỌN DÒNG -> ĐỔ LÊN CONTROL ====================
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dangNapDuLieu) return;
            if (dataGridView.CurrentRow?.DataBoundItem is not HoiVienRow row) return;

            var hv = dsHienTai.FirstOrDefault(x => x.MaHv == row.MaHV);
            if (hv == null) return;

            maHVDangChon = hv.MaHv;
            txtHoTen.Text = hv.HoTen;
            txtSDT.Text = hv.Sdt ?? "";
            txtEmail.Text = hv.Email ?? "";

            // RadioButton giới tính: 1 = Nam, 0 = Nữ.
            radioBtnNam.Checked = hv.GioiTinh;
            radioBtnNu.Checked = !hv.GioiTinh;

            // DateTimePicker ngày sinh.
            dateTimePicker.Value = ToDateTime(hv.NgaySinh);

            // ComboBox: chọn đúng vị trí theo hạng.
            cboxHangThanhVien.SelectedIndex = cboxHangThanhVien.FindStringExact(hv.HangThanhVien);

            // CheckBox trạng thái.
            checkBox.Checked = hv.TrangThai;
        }

        // ==================== VALIDATE ====================
        private bool ValidateInput()
        {
            string hoTen = txtHoTen.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (hoTen == "")
            {
                BaoLoi("Vui lòng nhập họ tên!", txtHoTen);
                return false;
            }

            if (hoTen.Length > 100)
            {
                BaoLoi("Họ tên không được dài quá 100 ký tự!", txtHoTen);
                return false;
            }

            if (sdt == "")
            {
                BaoLoi("Vui lòng nhập số điện thoại!", txtSDT);
                return false;
            }

            if (!sdt.All(c => c >= '0' && c <= '9'))
            {
                BaoLoi("Số điện thoại chỉ được chứa chữ số (không có khoảng trắng hay ký tự khác)!", txtSDT);
                return false;
            }

            if (sdt.Length < 9 || sdt.Length > 11)
            {
                BaoLoi($"Số điện thoại phải có từ 9 đến 11 chữ số (hiện có {sdt.Length} chữ số)!", txtSDT);
                return false;
            }

            if (email == "")
            {
                BaoLoi("Vui lòng nhập email!", txtEmail);
                return false;
            }

            if (!email.Contains('@'))
            {
                BaoLoi("Email không hợp lệ: phải chứa ký tự '@'!", txtEmail);
                return false;
            }

            if (email.Length > 100)
            {
                BaoLoi("Email không được dài quá 100 ký tự!", txtEmail);
                return false;
            }

            if (!radioBtnNam.Checked && !radioBtnNu.Checked)
            {
                BaoLoi("Vui lòng chọn giới tính (Nam hoặc Nữ)!", radioBtnNam);
                return false;
            }

            DateTime ngaySinh = dateTimePicker.Value.Date;

            if (ngaySinh > DateTime.Today)
            {
                BaoLoi("Ngày sinh không được lớn hơn ngày hiện tại!", dateTimePicker);
                return false;
            }

            int tuoi = TinhTuoi(ngaySinh);
            if (tuoi < 15)
            {
                BaoLoi($"Hội viên phải từ 15 tuổi trở lên mới được đăng ký (hiện {tuoi} tuổi)!", dateTimePicker);
                return false;
            }

            if (cboxHangThanhVien.SelectedIndex < 0)
            {
                BaoLoi("Vui lòng chọn hạng thành viên!", cboxHangThanhVien);
                return false;
            }

            return true;
        }

        private static int TinhTuoi(DateTime ngaySinh)
        {
            DateTime homNay = DateTime.Today;
            int tuoi = homNay.Year - ngaySinh.Year;
            if (ngaySinh.Date > homNay.AddYears(-tuoi))
                tuoi--;
            return tuoi;
        }

        private void BaoLoi(string noiDung, Control control)
        {
            MessageBox.Show(noiDung, "Dữ liệu không hợp lệ",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        // ==================== THÊM ====================
        private async void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var hv = new HoiVien
                {
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = radioBtnNam.Checked,
                    NgaySinh = ToModelDate(dateTimePicker.Value),
                    Sdt = txtSDT.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    HangThanhVien = cboxHangThanhVien.Text,
                    NgayDangKy = DateTime.Now,
                    TrangThai = checkBox.Checked
                };

                context.HoiViens.Add(hv);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                MessageBox.Show($"Thêm hội viên \"{hv.HoTen}\" thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (DbUpdateException ex)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể thêm hội viên. Vui lòng kiểm tra lại dữ liệu.\n\n" +
                    (ex.InnerException?.Message ?? ex.Message),
                    "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (maHVDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn hội viên cần sửa trên danh sách!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                var hv = await context.HoiViens.FindAsync(maHVDangChon);

                if (hv == null)
                {
                    MessageBox.Show("Hội viên này không còn tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await LoadDataAsync();
                    ClearInputs();
                    return;
                }

                hv.HoTen = txtHoTen.Text.Trim();
                hv.GioiTinh = radioBtnNam.Checked;
                hv.NgaySinh = ToModelDate(dateTimePicker.Value);
                hv.Sdt = txtSDT.Text.Trim();
                hv.Email = txtEmail.Text.Trim();
                hv.HangThanhVien = cboxHangThanhVien.Text;
                hv.TrangThai = checkBox.Checked;
                // Không đổi NgayDangKy khi sửa.

                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                MessageBox.Show("Cập nhật hội viên thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (DbUpdateException ex)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể cập nhật. Vui lòng kiểm tra lại dữ liệu.\n\n" +
                    (ex.InnerException?.Message ?? ex.Message),
                    "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (maHVDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn hội viên cần xóa trên danh sách!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa hội viên \"{txtHoTen.Text}\" không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var hv = await context.HoiViens.FindAsync(maHVDangChon);

                if (hv == null)
                {
                    MessageBox.Show("Hội viên này không còn tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await LoadDataAsync();
                    ClearInputs();
                    return;
                }

                context.HoiViens.Remove(hv);
                int soDong = await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                MessageBox.Show(
                    soDong > 0 ? "Xóa hội viên thành công!" : "Không có hội viên nào được xóa.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (DbUpdateException)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể xóa hội viên này vì đang được dữ liệu khác tham chiếu.",
                    "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== LÀM MỚI ====================
        private async void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboxTimKiem.SelectedIndex = 0;
            await LoadDataAsync();
            ClearInputs();
        }

        private void ClearInputs()
        {
            maHVDangChon = 0;

            txtHoTen.Clear();
            txtSDT.Clear();
            txtEmail.Clear();

            radioBtnNam.Checked = false;
            radioBtnNu.Checked = false;

            dateTimePicker.Value = DateTime.Today.AddYears(-18);
            cboxHangThanhVien.SelectedIndex = -1;
            checkBox.Checked = true;

            dangNapDuLieu = true;
            dataGridView.ClearSelection();
            dataGridView.CurrentCell = null;
            dangNapDuLieu = false;

            txtHoTen.Focus();
        }

        // ==================== TÌM KIẾM (Họ tên VÀ Hạng) ====================
        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
            ClearInputs();

            if (dsHienTai.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hội viên nào phù hợp với điều kiện tìm kiếm.",
                    "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ==================== ĐÓNG FORM ====================
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            context.Dispose();
        }

        // ==================== CÁC SỰ KIỆN TRỐNG ====================
        // Giữ lại vì Form1.Designer.cs đã gắn các sự kiện này; xóa sẽ gây lỗi CS0103.
        private void txtHoTen_TextChanged(object sender, EventArgs e) { }
        private void txtSDT_TextChanged(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker_ValueChanged(object sender, EventArgs e) { }
        private void cboxHangThanhVien_SelectedIndexChanged(object sender, EventArgs e) { }
        private void checkBox_CheckedChanged(object sender, EventArgs e) { }
        private void radioBtnNam_CheckedChanged(object sender, EventArgs e) { }
        private void radioBtnNu_CheckedChanged(object sender, EventArgs e) { }
        private void txtTimKiem_TextChanged(object sender, EventArgs e) { }
        private void cboxTimKiem_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}