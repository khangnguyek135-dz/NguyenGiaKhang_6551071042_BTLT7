using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Câu_4.Models;

namespace Câu_4
{
    public partial class Form1 : Form
    {
        private readonly AnKhangClinicContext context;

        // Danh sách lịch khám đang hiển thị trên lưới (kèm BacSi nhờ Include).
        private List<LichKham> dsHienTai = new();

        // Mã lịch đang chọn (0 = chưa chọn). Form không có ô Mã lịch nên lưu ở biến này.
        private int maLichDangChon = 0;

        // Chặn SelectionChanged chạy khi đang nạp/xóa chọn lưới bằng code.
        private bool dangNapDuLieu = false;

        // Bộ lọc tìm kiếm đã áp dụng (chốt tại thời điểm bấm "Tìm kiếm"),
        // để sau mỗi Thêm/Sửa/Xóa lưới vẫn giữ đúng kết quả lọc.
        private bool dangLoc = false;
        private DateTime locTuNgay = DateTime.Today;
        private DateTime locDenNgay = DateTime.Today;
        private int locMaBS = 0;

        // Item cho ComboBox bác sĩ (MaBS = 0 dành cho mục "Tất cả" ở ComboBox lọc).
        private class BacSiItem
        {
            public int MaBS { get; set; }
            public string HienThi { get; set; } = "";
        }

        // Dòng dữ liệu hiển thị trên lưới.
        private class LichRow
        {
            public int MaLich { get; set; }
            public string TenBenhNhan { get; set; } = "";
            public string SDT { get; set; } = "";
            public string NgayKham { get; set; } = "";
            public string GioKham { get; set; } = "";
            public string TenBacSi { get; set; } = "";
            public string ChuyenKhoa { get; set; } = "";
            public string TrangThai { get; set; } = "";
        }

        public Form1()
        {
            InitializeComponent();

            var options = new DbContextOptionsBuilder<AnKhangClinicContext>()
                .UseSqlServer("Server=N-Khang\\SQLEXPRESS;Database=AnKhangClinic;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            context = new AnKhangClinicContext(options);

            // DateTimePicker: Ngày = Short, Giờ = Time (dùng nút tăng/giảm để chỉnh giờ dễ hơn).
            dtpNgayKham.Format = DateTimePickerFormat.Short;
            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Format = DateTimePickerFormat.Short;
            dtpGioKham.Format = DateTimePickerFormat.Time;
            dtpGioKham.ShowUpDown = true;

            // Khoảng ngày mặc định của bộ lọc.
            dtpTuNgay.Value = DateTime.Today.AddDays(-7);
            dtpDenNgay.Value = DateTime.Today.AddDays(14);

            // ComboBox Trạng thái (nạp cứng 3 giá trị đúng với CHECK trong CSDL).
            cboxTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxTrangThai.Items.Clear();
            cboxTrangThai.Items.AddRange(new object[] { "Chờ khám", "Đã khám", "Đã hủy" });

            cboxBacSi.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxTimBacSi.DropDownStyle = ComboBoxStyle.DropDownList;

            // Cấu hình lưới (bỏ qua nếu đã đặt trong Designer).
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            // Gắn sự kiện bằng code.
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            this.Load += Form1_Load;
            this.FormClosed += Form1_FormClosed;
        }

        // ==================== CHUYỂN ĐỔI NGÀY / GIỜ ====================
        // Với EF Core 8+: cột date -> DateOnly, cột time -> TimeOnly.
        // Nếu Model của bạn dùng kiểu khác thì chỉ cần sửa 4 hàm này (và ToString ở LoadDataAsync):
        //   NgayKham là DateTime : ToDateTime(DateTime d) => d;  ToModelDate(DateTime d) => d.Date;
        //   GioKham  là TimeSpan : ToDateTime(TimeSpan t) => DateTime.Today.Add(t);
        //                          ToModelTime(DateTime d) => new TimeSpan(d.Hour, d.Minute, 0);
        private static DateTime ToDateTime(DateOnly d) => d.ToDateTime(TimeOnly.MinValue);
        private static DateOnly ToModelDate(DateTime d) => DateOnly.FromDateTime(d);
        private static DateTime ToDateTime(TimeOnly t) => DateTime.Today.Add(t.ToTimeSpan());
        private static TimeOnly ToModelTime(DateTime d) => new TimeOnly(d.Hour, d.Minute);

        // ==================== NẠP DỮ LIỆU ====================
        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadBacSiAsync();
            await LoadDataAsync();
            ClearInputs();
        }

        /// <summary>
        /// Nạp ComboBox Bác sĩ (form nhập + form lọc) từ bảng BacSi bằng LINQ,
        /// hiển thị dạng "BS. Họ tên - Chuyên khoa".
        /// Để public để Form phụ "Quản lý bác sĩ" có thể gọi lại sau khi thêm/sửa/xóa.
        /// </summary>
        public async Task LoadBacSiAsync()
        {
            try
            {
                int? maChonForm = cboxBacSi.SelectedValue as int?;
                int? maChonLoc = cboxTimBacSi.SelectedValue as int?;

                var ds = await context.BacSis
                    .AsNoTracking()
                    .OrderBy(x => x.HoTen)
                    .Select(x => new BacSiItem
                    {
                        MaBS = x.MaBs,
                        HienThi = "BS. " + x.HoTen + " - " + x.ChuyenKhoa
                    })
                    .ToListAsync();

                var dsLoc = new List<BacSiItem> { new BacSiItem { MaBS = 0, HienThi = "Tất cả bác sĩ" } };
                dsLoc.AddRange(ds);

                cboxBacSi.DisplayMember = "HienThi";
                cboxBacSi.ValueMember = "MaBS";
                cboxBacSi.DataSource = ds;

                cboxTimBacSi.DisplayMember = "HienThi";
                cboxTimBacSi.ValueMember = "MaBS";
                cboxTimBacSi.DataSource = dsLoc;

                // Khôi phục lựa chọn trước đó (nếu còn tồn tại).
                if (maChonForm.HasValue) cboxBacSi.SelectedValue = maChonForm.Value;
                else cboxBacSi.SelectedIndex = -1;

                if (maChonLoc.HasValue) cboxTimBacSi.SelectedValue = maChonLoc.Value;
                else cboxTimBacSi.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách bác sĩ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Nạp lại cả ComboBox lẫn lưới (Form phụ có thể gọi sau khi đóng).</summary>
        public async Task NapLaiTatCaAsync()
        {
            await LoadBacSiAsync();
            await LoadDataAsync();
        }

        /// <summary>
        /// Nạp lưới Lịch khám: Include bác sĩ (Eager Loading) để hiển thị Tên bác sĩ + Chuyên khoa.
        /// Nếu đang lọc: Where NgayKham >= từ ngày VÀ NgayKham <= đến ngày VÀ MaBS == bác sĩ chọn.
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                IQueryable<LichKham> query = context.LichKhams
                    .AsNoTracking()
                    .Include(x => x.MaBsNavigation);

                if (dangLoc)
                {
                    var tu = ToModelDate(locTuNgay);
                    var den = ToModelDate(locDenNgay);
                    int maBS = locMaBS;
                    bool coBacSi = maBS > 0;

                    query = query.Where(x => x.NgayKham >= tu
                                          && x.NgayKham <= den
                                          && (!coBacSi || x.MaBs == maBS));
                }

                dsHienTai = await query
                    .OrderBy(x => x.NgayKham)
                    .ThenBy(x => x.GioKham)
                    .ToListAsync();

                var rows = dsHienTai.Select(x => new LichRow
                {
                    MaLich = x.MaLich,
                    TenBenhNhan = x.TenBenhNhan,
                    SDT = x.Sdt ?? "",
                    NgayKham = x.NgayKham.ToString("dd/MM/yyyy"),
                    GioKham = x.GioKham.ToString("HH:mm"),
                    // Dùng ?. để lịch khám nào lỡ không có bác sĩ cũng không gây NullReferenceException.
                    TenBacSi = x.MaBsNavigation?.HoTen ?? "(không rõ)",
                    ChuyenKhoa = x.MaBsNavigation?.ChuyenKhoa ?? "",
                    TrangThai = x.TrangThai
                }).ToList();

                dangNapDuLieu = true;
                try
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = rows;
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
                ["MaLich"] = "Mã lịch",
                ["TenBenhNhan"] = "Bệnh nhân",
                ["SDT"] = "SĐT",
                ["NgayKham"] = "Ngày khám",
                ["GioKham"] = "Giờ khám",
                ["TenBacSi"] = "Bác sĩ",
                ["ChuyenKhoa"] = "Chuyên khoa",
                ["TrangThai"] = "Trạng thái"
            };

            foreach (var kv in tieuDe)
            {
                if (dataGridView1.Columns.Contains(kv.Key))
                    dataGridView1.Columns[kv.Key].HeaderText = kv.Value;
            }

            if (dataGridView1.Columns.Contains("MaLich"))
                dataGridView1.Columns["MaLich"].FillWeight = 45;
            if (dataGridView1.Columns.Contains("GioKham"))
                dataGridView1.Columns["GioKham"].FillWeight = 55;
        }

        // ==================== CHỌN DÒNG -> ĐỔ LÊN CONTROL ====================
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dangNapDuLieu) return;
            if (dataGridView1.CurrentRow?.DataBoundItem is not LichRow row) return;

            var lich = dsHienTai.FirstOrDefault(x => x.MaLich == row.MaLich);
            if (lich == null) return;

            maLichDangChon = lich.MaLich;
            txtTenBN.Text = lich.TenBenhNhan;
            txtSDT.Text = lich.Sdt ?? "";
            dtpNgayKham.Value = ToDateTime(lich.NgayKham);
            dtpGioKham.Value = ToDateTime(lich.GioKham);
            cboxBacSi.SelectedValue = lich.MaBs;
            cboxTrangThai.SelectedIndex = cboxTrangThai.FindStringExact(lich.TrangThai);
        }

        // ==================== VALIDATE ====================
        /// <param name="kiemTraNgayQuaKhu">
        /// true: không cho chọn ngày khám trong quá khứ (Thêm, hoặc Sửa mà có đổi ngày).
        /// </param>
        private bool ValidateInput(bool kiemTraNgayQuaKhu)
        {
            string tenBN = txtTenBN.Text.Trim();
            string sdt = txtSDT.Text.Trim();

            if (tenBN == "")
            {
                BaoLoi("Vui lòng nhập tên bệnh nhân!", txtTenBN);
                return false;
            }

            if (tenBN.Length > 100)
            {
                BaoLoi("Tên bệnh nhân không được dài quá 100 ký tự!", txtTenBN);
                return false;
            }

            // SĐT không bắt buộc, nhưng nếu nhập thì phải đúng định dạng.
            if (sdt != "")
            {
                if (!sdt.All(c => c >= '0' && c <= '9'))
                {
                    BaoLoi("Số điện thoại chỉ được chứa chữ số!", txtSDT);
                    return false;
                }

                if (sdt.Length < 9 || sdt.Length > 11)
                {
                    BaoLoi($"Số điện thoại phải có từ 9 đến 11 chữ số (hiện có {sdt.Length} chữ số)!", txtSDT);
                    return false;
                }
            }

            if (cboxBacSi.SelectedValue is not int)
            {
                BaoLoi("Vui lòng chọn bác sĩ khám!", cboxBacSi);
                return false;
            }

            if (kiemTraNgayQuaKhu && dtpNgayKham.Value.Date < DateTime.Today)
            {
                BaoLoi($"Không thể đặt lịch khám vào ngày trong quá khứ (hôm nay là {DateTime.Today:dd/MM/yyyy})!",
                    dtpNgayKham);
                return false;
            }

            if (cboxTrangThai.SelectedIndex < 0)
            {
                BaoLoi("Vui lòng chọn trạng thái lịch khám!", cboxTrangThai);
                return false;
            }

            return true;
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
            if (!ValidateInput(kiemTraNgayQuaKhu: true)) return;

            try
            {
                var lich = new LichKham
                {
                    TenBenhNhan = txtTenBN.Text.Trim(),
                    Sdt = string.IsNullOrWhiteSpace(txtSDT.Text) ? null : txtSDT.Text.Trim(),
                    NgayKham = ToModelDate(dtpNgayKham.Value),
                    GioKham = ToModelTime(dtpGioKham.Value),
                    MaBs = (int)cboxBacSi.SelectedValue!,
                    TrangThai = cboxTrangThai.Text
                };

                context.LichKhams.Add(lich);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                MessageBox.Show("Thêm lịch khám thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (DbUpdateException ex)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể thêm lịch khám. Vui lòng kiểm tra lại dữ liệu.\n\n" +
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
            if (maLichDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn lịch khám cần sửa trên danh sách!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chỉ chặn ngày quá khứ khi người dùng ĐỔI sang ngày khác; giữ nguyên ngày cũ
            // thì vẫn sửa được (ví dụ cập nhật trạng thái "Đã khám" cho lịch hôm qua).
            var goc = dsHienTai.FirstOrDefault(x => x.MaLich == maLichDangChon);
            bool doiNgay = goc == null || !ToModelDate(dtpNgayKham.Value).Equals(goc.NgayKham);

            if (!ValidateInput(kiemTraNgayQuaKhu: doiNgay)) return;

            try
            {
                var lich = await context.LichKhams.FindAsync(maLichDangChon);

                if (lich == null)
                {
                    MessageBox.Show("Lịch khám này không còn tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await LoadDataAsync();
                    ClearInputs();
                    return;
                }

                lich.TenBenhNhan = txtTenBN.Text.Trim();
                lich.Sdt = string.IsNullOrWhiteSpace(txtSDT.Text) ? null : txtSDT.Text.Trim();
                lich.NgayKham = ToModelDate(dtpNgayKham.Value);
                lich.GioKham = ToModelTime(dtpGioKham.Value);
                lich.MaBs = (int)cboxBacSi.SelectedValue!;
                lich.TrangThai = cboxTrangThai.Text;

                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                MessageBox.Show("Cập nhật lịch khám thành công!", "Thông báo",
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
            if (maLichDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn lịch khám cần xóa trên danh sách!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Bắt buộc xác nhận YesNo TRƯỚC khi gọi SaveChangesAsync.
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa lịch khám của bệnh nhân \"{txtTenBN.Text}\" " +
                $"ngày {dtpNgayKham.Value:dd/MM/yyyy} không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var lich = await context.LichKhams.FindAsync(maLichDangChon);

                if (lich == null)
                {
                    MessageBox.Show("Lịch khám này không còn tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await LoadDataAsync();
                    ClearInputs();
                    return;
                }

                context.LichKhams.Remove(lich);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                MessageBox.Show("Xóa lịch khám thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (DbUpdateException)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể xóa lịch khám này vì đang được dữ liệu khác tham chiếu.",
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
            // Bỏ bộ lọc, trả khoảng ngày về mặc định và hiển thị lại toàn bộ lịch khám.
            dangLoc = false;
            dtpTuNgay.Value = DateTime.Today.AddDays(-7);
            dtpDenNgay.Value = DateTime.Today.AddDays(14);
            if (cboxTimBacSi.Items.Count > 0) cboxTimBacSi.SelectedIndex = 0;

            await LoadDataAsync();
            ClearInputs();
        }

        private void ClearInputs()
        {
            maLichDangChon = 0;

            txtTenBN.Clear();
            txtSDT.Clear();
            dtpNgayKham.Value = DateTime.Today;
            dtpGioKham.Value = DateTime.Today.AddHours(8);
            cboxBacSi.SelectedIndex = -1;
            cboxTrangThai.SelectedIndex = 0;   // lịch mới mặc định là "Chờ khám"

            dangNapDuLieu = true;
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;
            dangNapDuLieu = false;

            txtTenBN.Focus();
        }

        // ==================== TÌM KIẾM (Khoảng ngày VÀ Bác sĩ) ====================
        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                BaoLoi("\"Từ ngày\" không được lớn hơn \"Đến ngày\"!", dtpTuNgay);
                return;
            }

            // Chốt bộ lọc; các lần nạp lại sau Thêm/Sửa/Xóa sẽ dùng lại bộ lọc này.
            locTuNgay = tuNgay;
            locDenNgay = denNgay;
            locMaBS = cboxTimBacSi.SelectedValue is int ma ? ma : 0;
            dangLoc = true;

            await LoadDataAsync();
            ClearInputs();

            if (dsHienTai.Count == 0)
            {
                MessageBox.Show("Không tìm thấy lịch khám nào phù hợp với điều kiện tìm kiếm.",
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
        private void txtTenBN_TextChanged(object sender, EventArgs e) { }
        private void txtSDT_TextChanged(object sender, EventArgs e) { }
        private void dtpNgayKham_ValueChanged(object sender, EventArgs e) { }
        private void dtpGioKham_ValueChanged(object sender, EventArgs e) { }
        private void cboxBacSi_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cboxTrangThai_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dtpTuNgay_ValueChanged(object sender, EventArgs e) { }
        private void dtpDenNgay_ValueChanged(object sender, EventArgs e) { }
        private void cboxTimBacSi_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}