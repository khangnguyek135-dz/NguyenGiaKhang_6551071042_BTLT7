-- =====================================================
-- Câu 3: Quản lý Loại phòng - Phòng ở Homestay "Sunrise Homestay"
-- Chạy toàn bộ script trong SSMS (kết nối N-Khang\SQLEXPRESS)
-- =====================================================

-- 1. Tạo database (nếu chưa có)
IF DB_ID(N'SunriseHomestay') IS NULL
BEGIN
    CREATE DATABASE SunriseHomestay;
END
GO

USE SunriseHomestay;
GO

-- 2. Xóa bảng cũ (bảng con xóa trước, bảng cha xóa sau) để chạy lại nhiều lần
IF OBJECT_ID(N'dbo.Phong', N'U') IS NOT NULL
    DROP TABLE dbo.Phong;
IF OBJECT_ID(N'dbo.LoaiPhong', N'U') IS NOT NULL
    DROP TABLE dbo.LoaiPhong;
GO

-- 3. Bảng LoaiPhong (bảng cha - phía "1")
CREATE TABLE dbo.LoaiPhong
(
    MaLoai     INT IDENTITY(1,1) NOT NULL,
    TenLoai    NVARCHAR(100)     NOT NULL,
    GiaMoiDem  DECIMAL(18,2)     NOT NULL,
    MoTa       NVARCHAR(255)     NULL,

    CONSTRAINT PK_LoaiPhong PRIMARY KEY (MaLoai),
    CONSTRAINT CK_LoaiPhong_Gia CHECK (GiaMoiDem >= 0)
);
GO

-- 4. Bảng Phong (bảng con - phía "n")
CREATE TABLE dbo.Phong
(
    MaPhong    INT IDENTITY(1,1) NOT NULL,
    SoPhong    VARCHAR(10)       NOT NULL,
    TangSo     INT               NOT NULL,
    TinhTrang  NVARCHAR(20)      NOT NULL,           -- Trống / Đang ở / Đang dọn
    HinhAnh    NVARCHAR(255)     NULL,               -- chỉ lưu TÊN FILE ảnh, không lưu đường dẫn đầy đủ
    MaLoai     INT               NOT NULL,

    CONSTRAINT PK_Phong PRIMARY KEY (MaPhong),
    CONSTRAINT UQ_Phong_SoPhong UNIQUE (SoPhong),
    CONSTRAINT CK_Phong_TinhTrang CHECK (TinhTrang IN (N'Trống', N'Đang ở', N'Đang dọn')),
    CONSTRAINT CK_Phong_TangSo CHECK (TangSo >= 0),
    CONSTRAINT FK_Phong_LoaiPhong FOREIGN KEY (MaLoai)
        REFERENCES dbo.LoaiPhong (MaLoai)
);
GO

-- 5. Dữ liệu mẫu: LoaiPhong
INSERT INTO dbo.LoaiPhong (TenLoai, GiaMoiDem, MoTa)
VALUES
(N'Phòng đơn',     350000.00,  N'Phòng 1 giường đơn, phù hợp khách đi một mình, có điều hòa và WiFi.'),
(N'Phòng đôi',     550000.00,  N'Phòng 1 giường đôi hoặc 2 giường đơn, có ban công và máy nước nóng.'),
(N'Phòng VIP',     1200000.00, N'Phòng rộng, view đẹp, bồn tắm riêng, bữa sáng miễn phí.'),
(N'Phòng gia đình', 900000.00, N'Phòng 2 giường lớn, phù hợp gia đình 4 người, có bếp nhỏ.');
GO

-- 6. Dữ liệu mẫu: Phong
-- Cột HinhAnh chỉ là TÊN FILE. Hãy chép các file ảnh cùng tên vào thư mục Images của ứng dụng
-- (hoặc để NULL / dùng nút "Chọn ảnh" trong chương trình để thêm ảnh).
INSERT INTO dbo.Phong (SoPhong, TangSo, TinhTrang, HinhAnh, MaLoai)
VALUES
('101', 1, N'Trống',    'phong_don.jpg',      1),
('102', 1, N'Đang ở',   'phong_don.jpg',      1),
('103', 1, N'Đang dọn', NULL,                 1),
('201', 2, N'Trống',    'phong_doi.jpg',      2),
('202', 2, N'Đang ở',   'phong_doi.jpg',      2),
('203', 2, N'Trống',    'phong_doi.jpg',      2),
('204', 2, N'Đang dọn', NULL,                 2),
('301', 3, N'Đang ở',   'phong_vip.jpg',      3),
('302', 3, N'Trống',    'phong_vip.jpg',      3),
('303', 3, N'Trống',    'phong_giadinh.jpg',  4),
('304', 3, N'Đang ở',   'phong_giadinh.jpg',  4),
('305', 3, N'Đang dọn', 'phong_giadinh.jpg',  4);
GO

-- 7. Kiểm tra kết quả (JOIN 2 bảng)
SELECT p.MaPhong, p.SoPhong, p.TangSo, p.TinhTrang, p.HinhAnh,
       lp.MaLoai, lp.TenLoai, lp.GiaMoiDem
FROM dbo.Phong p
JOIN dbo.LoaiPhong lp ON lp.MaLoai = p.MaLoai
ORDER BY p.MaPhong;
GO