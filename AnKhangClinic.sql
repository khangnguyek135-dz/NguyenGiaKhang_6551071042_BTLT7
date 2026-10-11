-- =====================================================
-- Câu 4: Quản lý Lịch khám bệnh - Phòng khám tư "An Khang Clinic"
-- Chạy toàn bộ script trong SSMS (kết nối N-Khang\SQLEXPRESS)
-- =====================================================

-- 1. Tạo database (nếu chưa có)
IF DB_ID(N'AnKhangClinic') IS NULL
BEGIN
    CREATE DATABASE AnKhangClinic;
END
GO

USE AnKhangClinic;
GO

-- 2. Xóa bảng cũ (bảng con xóa trước, bảng cha xóa sau) để chạy lại nhiều lần
IF OBJECT_ID(N'dbo.LichKham', N'U') IS NOT NULL
    DROP TABLE dbo.LichKham;
IF OBJECT_ID(N'dbo.BacSi', N'U') IS NOT NULL
    DROP TABLE dbo.BacSi;
GO

-- 3. Bảng BacSi (bảng cha - phía "1")
CREATE TABLE dbo.BacSi
(
    MaBS        INT IDENTITY(1,1) NOT NULL,
    HoTen       NVARCHAR(100)     NOT NULL,
    ChuyenKhoa  NVARCHAR(100)     NOT NULL,
    SDT         VARCHAR(15)       NULL,

    CONSTRAINT PK_BacSi PRIMARY KEY (MaBS)
);
GO

-- 4. Bảng LichKham (bảng con - phía "n")
CREATE TABLE dbo.LichKham
(
    MaLich       INT IDENTITY(1,1) NOT NULL,
    TenBenhNhan  NVARCHAR(100)     NOT NULL,
    SDT          VARCHAR(15)       NULL,
    NgayKham     DATE              NOT NULL,
    GioKham      TIME(0)           NOT NULL,           -- giờ khám (kiểu time)
    MaBS         INT               NOT NULL,
    TrangThai    NVARCHAR(20)      NOT NULL,           -- Chờ khám / Đã khám / Đã hủy

    CONSTRAINT PK_LichKham PRIMARY KEY (MaLich),
    CONSTRAINT CK_LichKham_TrangThai CHECK (TrangThai IN (N'Chờ khám', N'Đã khám', N'Đã hủy')),
    CONSTRAINT FK_LichKham_BacSi FOREIGN KEY (MaBS)
        REFERENCES dbo.BacSi (MaBS)
);
GO

-- 5. Dữ liệu mẫu: BacSi
INSERT INTO dbo.BacSi (HoTen, ChuyenKhoa, SDT)
VALUES
(N'Nguyễn Văn An',   N'Nội tổng quát', '0901112233'),
(N'Trần Thị Bình',   N'Nhi khoa',      '0912223344'),
(N'Lê Minh Cường',   N'Tim mạch',      '0923334455'),
(N'Phạm Thu Dung',   N'Da liễu',       '0934445566'),
(N'Hoàng Anh Em',    N'Tai Mũi Họng',  '0945556677');
GO

-- 6. Dữ liệu mẫu: LichKham
-- Ngày khám tính theo ngày chạy script (hôm nay = 0, quá khứ < 0, tương lai > 0)
-- để luôn có cả lịch cũ lẫn lịch sắp tới, tiện thử tìm kiếm theo khoảng ngày.
DECLARE @HomNay DATE = CAST(GETDATE() AS DATE);

INSERT INTO dbo.LichKham (TenBenhNhan, SDT, NgayKham, GioKham, MaBS, TrangThai)
VALUES
-- Lịch đã qua
(N'Nguyễn Hoàng Nam',  '0961000001', DATEADD(DAY, -7, @HomNay), '08:00', 1, N'Đã khám'),
(N'Trần Thị Mai',      '0961000002', DATEADD(DAY, -6, @HomNay), '09:30', 2, N'Đã khám'),
(N'Lê Quốc Bảo',       '0961000003', DATEADD(DAY, -5, @HomNay), '14:00', 3, N'Đã khám'),
(N'Phạm Ngọc Anh',     '0961000004', DATEADD(DAY, -3, @HomNay), '10:15', 4, N'Đã hủy'),
(N'Võ Thanh Tùng',     '0961000005', DATEADD(DAY, -2, @HomNay), '15:30', 1, N'Đã khám'),
(N'Đặng Thu Hà',       '0961000006', DATEADD(DAY, -1, @HomNay), '08:45', 5, N'Đã khám'),
-- Hôm nay và sắp tới
(N'Bùi Minh Khôi',     '0961000007', @HomNay,                   '09:00', 1, N'Chờ khám'),
(N'Hoàng Gia Hân',     '0961000008', @HomNay,                   '10:30', 2, N'Chờ khám'),
(N'Ngô Văn Lộc',       '0961000009', DATEADD(DAY,  1, @HomNay), '08:30', 3, N'Chờ khám'),
(N'Đỗ Khánh Vy',       '0961000010', DATEADD(DAY,  1, @HomNay), '13:30', 4, N'Chờ khám'),
(N'Huỳnh Thanh Sơn',   '0961000011', DATEADD(DAY,  2, @HomNay), '09:15', 5, N'Chờ khám'),
(N'Lý Mỹ Duyên',       '0961000012', DATEADD(DAY,  3, @HomNay), '14:45', 1, N'Đã hủy'),
(N'Trương Quang Vinh', '0961000013', DATEADD(DAY,  5, @HomNay), '10:00', 2, N'Chờ khám'),
(N'Phan Bảo Ngọc',     '0961000014', DATEADD(DAY,  7, @HomNay), '16:00', 3, N'Chờ khám'),
(N'Mai Anh Tuấn',      '0961000015', DATEADD(DAY, 10, @HomNay), '08:00', 1, N'Chờ khám');
GO

-- 7. Kiểm tra kết quả (JOIN 2 bảng)
SELECT lk.MaLich, lk.TenBenhNhan, lk.SDT, lk.NgayKham, lk.GioKham, lk.TrangThai,
       bs.MaBS, bs.HoTen AS TenBacSi, bs.ChuyenKhoa
FROM dbo.LichKham lk
JOIN dbo.BacSi bs ON bs.MaBS = lk.MaBS
ORDER BY lk.NgayKham, lk.GioKham;
GO