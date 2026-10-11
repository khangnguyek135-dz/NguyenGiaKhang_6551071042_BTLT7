CREATE DATABASE QuanLyNhaSach;
GO

USE QuanLyNhaSach;
GO

CREATE TABLE TheLoaiSach
(
    MaTL INT IDENTITY(1,1) PRIMARY KEY,
    TenTheLoai NVARCHAR(100) NOT NULL UNIQUE,
    MoTa NVARCHAR(255) NULL,
    SoLuongSach INT NOT NULL DEFAULT 0,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE()
);
GO

INSERT INTO TheLoaiSach (TenTheLoai, MoTa)
VALUES
(N'Tiểu thuyết', N'Các loại sách tiểu thuyết'),
(N'Kỹ năng sống', N'Sách phát triển bản thân'),
(N'Thiếu nhi', N'Sách dành cho trẻ em'),
(N'Sách giáo khoa', N'Sách phục vụ học tập');
GO

SELECT * FROM TheLoaiSach;