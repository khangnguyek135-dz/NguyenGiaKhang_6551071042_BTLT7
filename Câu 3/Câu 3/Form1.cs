using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Câu_3.Models;

namespace Câu_3
{
    public partial class Form1 : Form
    {
        private readonly SunriseHomestayContext context;

        // Danh sách phòng đang hiển thị trên lưới (kèm LoaiPhong nhờ Include).
        private List<Phong> dsHienTai = new();

        // Mã phòng đang chọn (0 = chưa chọn). Form không có ô Mã phòng nên lưu ở biến này.
        private int maPhongDangChon = 0;

        // TÊN FILE ảnh đang chọn (không phải full path) - sẽ lưu vào cột HinhAnh.
        private string? tenAnhDangChon = null;

        // Chặn SelectionChanged chạy khi đang nạp/xóa chọn lưới bằng code.
        private bool dangNapDuLieu = false;

        private const int ThumbW = 80;
        private const int ThumbH = 60;

        // Thư mục Images nằm cạnh file .exe của ứng dụng.
        private static string ThuMucAnh => Path.Combine(Application.StartupPath, "Images");

        // Giữ các thumbnail đang hiển thị trên lưới để giải phóng khi nạp lại.
        private readonly List<Image> anhThumbnail = new();

        // Item cho ComboBox loại phòng (MaLoai = 0 dành cho mục "Tất cả" ở ComboBox lọc).
        private class LoaiItem
        {
            public int MaLoai { get; set; }
            public string TenLoai { get; set; } = "";
        }

        // Dòng dữ liệu hiển thị trên lưới. Thuộc tính kiểu Image sẽ tự thành DataGridViewImageColumn.
        private class PhongRow
        {
            public int MaPhong { get; set; }
            public Image? Anh { get; set; }
            public string SoPhong { get; set; } = "";
            public string TenLoai { get; set; } = "";
            public int TangSo { get; set; }
            public string GiaMoiDem { get; set; } = "";
            public string TinhTrang { get; set; } = "";
        }

        public Form1()
        {
            InitializeComponent();

            var options = new DbContextOptionsBuilder<SunriseHomestayContext>()
                .UseSqlServer("Server=N-Khang\\SQLEXPRESS;Database=SunriseHomestay;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            context = new SunriseHomestayContext(options);

            // ComboBox Tình trạng (nạp cứng 3 giá trị đúng với CHECK trong CSDL).
            cboxTinhTrang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxTinhTrang.Items.Clear();
            cboxTinhTrang.Items.AddRange(new object[] { "Trống", "Đang ở", "Đang dọn" });

            cboxFilterTinhTrang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxFilterTinhTrang.Items.Clear();
            cboxFilterTinhTrang.Items.AddRange(new object[] { "Tất cả", "Trống", "Đang ở", "Đang dọn" });
            cboxFilterTinhTrang.SelectedIndex = 0;

            cboxLoaiPhong.DropDownStyle = ComboBoxStyle.DropDownList;
            cboxFilterLoaiPhong.DropDownStyle = ComboBoxStyle.DropDownList;

            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // Cấu hình lưới (bỏ qua nếu đã đặt trong Designer).
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.RowTemplate.Height = ThumbH + 6;

            // Gắn sự kiện bằng code.
            dataGridView.SelectionChanged += dataGridView_SelectionChanged;
            this.Load += Form1_Load;
            this.FormClosed += Form1_FormClosed;
        }

        // ==================== NẠP DỮ LIỆU ====================
        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadLoaiPhongAsync();
            await LoadDataAsync();
            ClearInputs();
        }

        /// <summary>
        /// Nạp ComboBox Loại phòng (form nhập + form lọc) từ bảng LoaiPhong bằng LINQ.
        /// Để public để Form phụ "Quản lý loại phòng" có thể gọi lại sau khi thêm/sửa/xóa.
        /// </summary>
        public async Task LoadLoaiPhongAsync()
        {
            try
            {
                int? maChonForm = cboxLoaiPhong.SelectedValue as int?;
                int? maChonLoc = cboxFilterLoaiPhong.SelectedValue as int?;

                var ds = await context.LoaiPhongs
                    .AsNoTracking()
                    .OrderBy(x => x.TenLoai)
                    .Select(x => new LoaiItem { MaLoai = x.MaLoai, TenLoai = x.TenLoai })
                    .ToListAsync();

                var dsLoc = new List<LoaiItem> { new LoaiItem { MaLoai = 0, TenLoai = "Tất cả" } };
                dsLoc.AddRange(ds);

                cboxLoaiPhong.DisplayMember = "TenLoai";
                cboxLoaiPhong.ValueMember = "MaLoai";
                cboxLoaiPhong.DataSource = ds;

                cboxFilterLoaiPhong.DisplayMember = "TenLoai";
                cboxFilterLoaiPhong.ValueMember = "MaLoai";
                cboxFilterLoaiPhong.DataSource = dsLoc;

                // Khôi phục lựa chọn trước đó (nếu còn tồn tại).
                if (maChonForm.HasValue) cboxLoaiPhong.SelectedValue = maChonForm.Value;
                else cboxLoaiPhong.SelectedIndex = -1;

                if (maChonLoc.HasValue) cboxFilterLoaiPhong.SelectedValue = maChonLoc.Value;
                else cboxFilterLoaiPhong.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách loại phòng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Nạp lại cả ComboBox lẫn lưới (Form phụ có thể gọi sau khi đóng).</summary>
        public async Task NapLaiTatCaAsync()
        {
            await LoadLoaiPhongAsync();
            await LoadDataAsync();
        }

        /// <summary>
        /// Nạp lưới Phòng: Include loại phòng + Where theo Tình trạng VÀ Loại phòng.
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                string tinhTrangLoc = cboxFilterTinhTrang.SelectedIndex > 0 ? cboxFilterTinhTrang.Text : "";
                int maLoaiLoc = cboxFilterLoaiPhong.SelectedValue is int ma ? ma : 0;

                bool coTinhTrang = tinhTrangLoc != "";
                bool coLoai = maLoaiLoc > 0;

                dsHienTai = await context.Phongs
                    .AsNoTracking()
                    .Include(x => x.MaLoaiNavigation)           // Eager Loading
                    .Where(x => (!coTinhTrang || x.TinhTrang == tinhTrangLoc)
                             && (!coLoai || x.MaLoai == maLoaiLoc))
                    .OrderBy(x => x.MaPhong)
                    .ToListAsync();

                var thumbMoi = new List<Image>();

                var rows = dsHienTai.Select(x =>
                {
                    var thumb = TaoThumbnail(x.HinhAnh);
                    if (thumb != null) thumbMoi.Add(thumb);

                    return new PhongRow
                    {
                        MaPhong = x.MaPhong,
                        Anh = thumb,
                        SoPhong = x.SoPhong,
                        TenLoai = x.MaLoaiNavigation?.TenLoai ?? "",
                        TangSo = x.TangSo,
                        GiaMoiDem = x.MaLoaiNavigation != null ? $"{x.MaLoaiNavigation.GiaMoiDem:N0} đ" : "",
                        TinhTrang = x.TinhTrang
                    };
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

                // Giải phóng thumbnail cũ sau khi lưới đã dùng ảnh mới.
                foreach (var cu in anhThumbnail) cu.Dispose();
                anhThumbnail.Clear();
                anhThumbnail.AddRange(thumbMoi);
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
                ["MaPhong"] = "Mã phòng",
                ["Anh"] = "Ảnh",
                ["SoPhong"] = "Số phòng",
                ["TenLoai"] = "Loại phòng",
                ["TangSo"] = "Tầng",
                ["GiaMoiDem"] = "Giá/đêm",
                ["TinhTrang"] = "Tình trạng"
            };

            foreach (var kv in tieuDe)
            {
                if (dataGridView.Columns.Contains(kv.Key))
                    dataGridView.Columns[kv.Key].HeaderText = kv.Value;
            }

            if (dataGridView.Columns["Anh"] is DataGridViewImageColumn colAnh)
            {
                colAnh.ImageLayout = DataGridViewImageCellLayout.Zoom;
                colAnh.DefaultCellStyle.NullValue = null;   // phòng chưa có ảnh: để trống, không hiện dấu X đỏ
                colAnh.FillWeight = 70;
            }

            if (dataGridView.Columns.Contains("MaPhong"))
                dataGridView.Columns["MaPhong"].FillWeight = 50;
            if (dataGridView.Columns.Contains("TangSo"))
                dataGridView.Columns["TangSo"].FillWeight = 40;
        }

        // ==================== XỬ LÝ ẢNH ====================
        /// <summary>Đọc ảnh từ thư mục Images mà không khóa file. Trả về null nếu không có/không đọc được.</summary>
        private static Bitmap? LoadImage(string? tenFile)
        {
            if (string.IsNullOrWhiteSpace(tenFile)) return null;

            try
            {
                string path = Path.Combine(ThuMucAnh, tenFile);
                if (!File.Exists(path)) return null;

                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var img = Image.FromStream(fs);
                return new Bitmap(img);   // bản sao trong bộ nhớ, file gốc không bị giữ khóa
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Tạo ảnh thumbnail vừa khung ThumbW x ThumbH (giữ tỉ lệ).</summary>
        private static Image? TaoThumbnail(string? tenFile)
        {
            using var anh = LoadImage(tenFile);
            if (anh == null) return null;

            var thumb = new Bitmap(ThumbW, ThumbH);
            using var g = Graphics.FromImage(thumb);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            float tiLe = Math.Min((float)ThumbW / anh.Width, (float)ThumbH / anh.Height);
            int w = Math.Max(1, (int)(anh.Width * tiLe));
            int h = Math.Max(1, (int)(anh.Height * tiLe));
            g.DrawImage(anh, (ThumbW - w) / 2, (ThumbH - h) / 2, w, h);

            return thumb;
        }

        /// <summary>Hiển thị ảnh lên PictureBox (giải phóng ảnh cũ).</summary>
        private void HienThiAnh(string? tenFile)
        {
            var cu = pictureBox.Image;
            pictureBox.Image = LoadImage(tenFile);
            cu?.Dispose();
        }

        // ==================== CHỌN ẢNH ====================
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Chọn ảnh phòng",
                Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Multiselect = false
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                Directory.CreateDirectory(ThuMucAnh);

                string nguon = ofd.FileName;
                string tenFile = Path.GetFileName(nguon);
                string dich = Path.Combine(ThuMucAnh, tenFile);

                // Chỉ copy khi file chưa nằm sẵn trong thư mục Images.
                bool cungFile = string.Equals(Path.GetFullPath(nguon), Path.GetFullPath(dich),
                    StringComparison.OrdinalIgnoreCase);

                if (!cungFile)
                {
                    // Trùng tên với ảnh khác -> đổi tên để không ghi đè ảnh của phòng khác.
                    if (File.Exists(dich))
                    {
                        tenFile = $"{Path.GetFileNameWithoutExtension(nguon)}_{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(nguon)}";
                        dich = Path.Combine(ThuMucAnh, tenFile);
                    }

                    File.Copy(nguon, dich);
                }

                tenAnhDangChon = tenFile;   // chỉ lưu TÊN FILE
                HienThiAnh(tenFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể chọn ảnh: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== CHỌN DÒNG -> ĐỔ LÊN CONTROL ====================
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dangNapDuLieu) return;
            if (dataGridView.CurrentRow?.DataBoundItem is not PhongRow row) return;

            var phong = dsHienTai.FirstOrDefault(x => x.MaPhong == row.MaPhong);
            if (phong == null) return;

            maPhongDangChon = phong.MaPhong;
            txtSoPhong.Text = phong.SoPhong;
            DatTang(phong.TangSo);
            cboxLoaiPhong.SelectedValue = phong.MaLoai;
            cboxTinhTrang.SelectedIndex = cboxTinhTrang.FindStringExact(phong.TinhTrang);

            tenAnhDangChon = phong.HinhAnh;
            HienThiAnh(phong.HinhAnh);
        }

        private void DatTang(int tang)
        {
            numericUpDown.Value = Math.Clamp(tang, (int)numericUpDown.Minimum, (int)numericUpDown.Maximum);
        }

        // ==================== VALIDATE ====================
        private bool ValidateInput()
        {
            string soPhong = txtSoPhong.Text.Trim();

            if (soPhong == "")
            {
                BaoLoi("Vui lòng nhập số phòng!", txtSoPhong);
                return false;
            }

            if (soPhong.Length > 10)
            {
                BaoLoi("Số phòng không được dài quá 10 ký tự!", txtSoPhong);
                return false;
            }

            bool hopLe = soPhong.All(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z')
                                       || (c >= 'a' && c <= 'z') || c == '-');
            if (!hopLe)
            {
                BaoLoi("Số phòng chỉ gồm chữ cái không dấu, chữ số hoặc dấu '-' (ví dụ: 101, A12)!", txtSoPhong);
                return false;
            }

            if (cboxLoaiPhong.SelectedValue is not int)
            {
                BaoLoi("Vui lòng chọn loại phòng!", cboxLoaiPhong);
                return false;
            }

            if (cboxTinhTrang.SelectedIndex < 0)
            {
                BaoLoi("Vui lòng chọn tình trạng phòng!", cboxTinhTrang);
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
            if (!ValidateInput()) return;

            string soPhong = txtSoPhong.Text.Trim();

            try
            {
                bool daTonTai = await context.Phongs.AnyAsync(x => x.SoPhong == soPhong);
                if (daTonTai)
                {
                    BaoLoi($"Số phòng \"{soPhong}\" đã tồn tại!", txtSoPhong);
                    return;
                }

                var phong = new Phong
                {
                    SoPhong = soPhong,
                    TangSo = (int)numericUpDown.Value,
                    TinhTrang = cboxTinhTrang.Text,
                    HinhAnh = tenAnhDangChon,                       // chỉ lưu tên file
                    MaLoai = (int)cboxLoaiPhong.SelectedValue!
                };

                context.Phongs.Add(phong);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                MessageBox.Show($"Thêm phòng {soPhong} thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (DbUpdateException ex)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể thêm phòng. Vui lòng kiểm tra lại dữ liệu.\n\n" +
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
            if (maPhongDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng cần sửa trên danh sách!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            string soPhong = txtSoPhong.Text.Trim();

            try
            {
                bool trung = await context.Phongs
                    .AnyAsync(x => x.SoPhong == soPhong && x.MaPhong != maPhongDangChon);
                if (trung)
                {
                    BaoLoi($"Số phòng \"{soPhong}\" đã được dùng cho phòng khác!", txtSoPhong);
                    return;
                }

                var phong = await context.Phongs.FindAsync(maPhongDangChon);
                if (phong == null)
                {
                    MessageBox.Show("Phòng này không còn tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await LoadDataAsync();
                    ClearInputs();
                    return;
                }

                phong.SoPhong = soPhong;
                phong.TangSo = (int)numericUpDown.Value;
                phong.TinhTrang = cboxTinhTrang.Text;
                phong.HinhAnh = tenAnhDangChon;
                phong.MaLoai = (int)cboxLoaiPhong.SelectedValue!;

                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                MessageBox.Show("Cập nhật phòng thành công!", "Thông báo",
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
            if (maPhongDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng cần xóa trên danh sách!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phòng {txtSoPhong.Text} không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var phong = await context.Phongs.FindAsync(maPhongDangChon);
                if (phong == null)
                {
                    MessageBox.Show("Phòng này không còn tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    await LoadDataAsync();
                    ClearInputs();
                    return;
                }

                // Chỉ xóa bản ghi; không xóa file ảnh vì ảnh có thể đang dùng chung với phòng khác.
                context.Phongs.Remove(phong);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();

                MessageBox.Show("Xóa phòng thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataAsync();
                ClearInputs();
            }
            catch (DbUpdateException)
            {
                context.ChangeTracker.Clear();
                MessageBox.Show(
                    "Không thể xóa phòng này vì đang được dữ liệu khác tham chiếu.",
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
            cboxFilterTinhTrang.SelectedIndex = 0;
            if (cboxFilterLoaiPhong.Items.Count > 0) cboxFilterLoaiPhong.SelectedIndex = 0;

            await LoadDataAsync();
            ClearInputs();
        }

        private void ClearInputs()
        {
            maPhongDangChon = 0;
            tenAnhDangChon = null;

            txtSoPhong.Clear();
            DatTang(1);
            cboxLoaiPhong.SelectedIndex = -1;
            cboxTinhTrang.SelectedIndex = -1;
            HienThiAnh(null);

            dangNapDuLieu = true;
            dataGridView.ClearSelection();
            dataGridView.CurrentCell = null;
            dangNapDuLieu = false;

            txtSoPhong.Focus();
        }

        // ==================== TÌM KIẾM (Tình trạng VÀ Loại phòng) ====================
        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
            ClearInputs();

            if (dsHienTai.Count == 0)
            {
                MessageBox.Show("Không tìm thấy phòng nào phù hợp với điều kiện tìm kiếm.",
                    "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ==================== ĐÓNG FORM ====================
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            pictureBox.Image?.Dispose();
            foreach (var anh in anhThumbnail) anh.Dispose();
            anhThumbnail.Clear();
            context.Dispose();
        }

        // ==================== CÁC SỰ KIỆN TRỐNG ====================
        // Giữ lại vì Form1.Designer.cs đã gắn các sự kiện này; xóa sẽ gây lỗi CS0103.
        private void txtSoPhong_TextChanged(object sender, EventArgs e) { }
        private void numericUpDown_ValueChanged(object sender, EventArgs e) { }
        private void cboxLoaiPhong_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cboxTinhTrang_SelectedIndexChanged(object sender, EventArgs e) { }
        private void pictureBox_Click(object sender, EventArgs e) { }
        private void cboxFilterLoaiPhong_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cboxFilterTinhTrang_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}