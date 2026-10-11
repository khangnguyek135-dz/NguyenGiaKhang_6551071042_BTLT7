-- =====================================================
-- Câu 2: Quản lý Hội viên - Phòng tập Gym "FitZone"
-- Chạy toàn bộ script trong SSMS (kết nối N-Khang\SQLEXPRESS)
-- =====================================================

-- 1. Tạo database (nếu chưa có)
IF DB_ID(N'FitZone') IS NULL
BEGIN
    CREATE DATABASE FitZone;
END
GO

USE FitZone;
GO

-- 2. Xóa bảng cũ nếu đã tồn tại (để chạy lại script nhiều lần)
IF OBJECT_ID(N'dbo.HoiVien', N'U') IS NOT NULL
    DROP TABLE dbo.HoiVien;
GO

-- 3. Tạo bảng HoiVien
CREATE TABLE dbo.HoiVien
(
    MaHV          INT IDENTITY(1,1) NOT NULL,
    HoTen         NVARCHAR(100)     NOT NULL,
    GioiTinh      BIT               NOT NULL CONSTRAINT DF_HoiVien_GioiTinh DEFAULT (1),   -- 1 = Nam, 0 = Nữ
    NgaySinh      DATE              NOT NULL,
    SDT           VARCHAR(15)       NULL,
    Email         VARCHAR(100)      NULL,
    HangThanhVien NVARCHAR(20)      NOT NULL CONSTRAINT DF_HoiVien_Hang DEFAULT (N'Basic'),
    NgayDangKy    DATETIME          NOT NULL CONSTRAINT DF_HoiVien_NgayDK DEFAULT (GETDATE()),
    TrangThai     BIT               NOT NULL CONSTRAINT DF_HoiVien_TrangThai DEFAULT (1),  -- 1 = Đang hoạt động, 0 = Tạm ngưng

    CONSTRAINT PK_HoiVien PRIMARY KEY (MaHV),
    CONSTRAINT CK_HoiVien_Hang CHECK (HangThanhVien IN (N'Basic', N'VIP', N'Premium'))
);
GO

-- 4. Dữ liệu mẫu
INSERT INTO dbo.HoiVien (HoTen, GioiTinh, NgaySinh, SDT, Email, HangThanhVien, NgayDangKy, TrangThai)
VALUES
(N'Nguyễn Văn An',     1, '1998-03-15', '0901234567', 'an.nguyen@gmail.com',     N'Basic',   '2025-01-10 08:30:00', 1),
(N'Trần Thị Bích',     0, '2001-07-22', '0912345678', 'bich.tran@gmail.com',     N'VIP',     '2025-02-14 17:45:00', 1),
(N'Lê Hoàng Cường',    1, '1995-11-05', '0987654321', 'cuong.le@yahoo.com',      N'Premium', '2024-11-20 06:15:00', 1),
(N'Phạm Thu Dung',     0, '2003-01-30', '0933445566', 'dung.pham@gmail.com',     N'Basic',   '2025-03-05 18:00:00', 0),
(N'Võ Minh Đức',       1, '1990-09-12', '0977112233', 'duc.vo@outlook.com',      N'Premium', '2024-08-01 07:00:00', 1),
(N'Đặng Ngọc Hà',      0, '1999-05-18', '0944556677', 'ha.dang@gmail.com',       N'VIP',     '2025-04-22 19:20:00', 1),
(N'Bùi Quang Huy',     1, '2008-12-02', '0966778899', 'huy.bui@gmail.com',       N'Basic',   '2025-06-01 16:30:00', 1),
(N'Hoàng Mai Lan',     0, '1993-08-25', '0922334455', 'lan.hoang@gmail.com',     N'Premium', '2024-12-12 09:10:00', 0),
(N'Ngô Thanh Phong',   1, '2000-02-14', '0955443322', 'phong.ngo@gmail.com',     N'VIP',     '2025-05-18 20:05:00', 1),
(N'Đỗ Khánh Linh',     0, '2005-10-09', '0911223344', 'linh.do@gmail.com',       N'Basic',   '2025-07-07 14:40:00', 1),
(N'Huỳnh Gia Bảo',     1, '1988-06-30', '0988990011', 'bao.huynh@gmail.com',     N'Premium', '2024-05-15 06:30:00', 1),
(N'Lý Thùy Trang',     0, '1997-04-04', '0905060708', 'trang.ly@gmail.com',      N'VIP',     '2025-08-20 18:50:00', 0);
GO

-- 5. Kiểm tra kết quả
SELECT * FROM dbo.HoiVien ORDER BY MaHV;
GO