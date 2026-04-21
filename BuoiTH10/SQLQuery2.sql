Use QLNV

Create table NhanVien (
MaNV  NVARCHAR(5) PRIMARY KEY,
HoLot NVARCHAR(50),
TenNV NVARCHAR(10),
Phai NVARCHAR(3),
NgaySinh SMALLDATETIME,
MaCV NVARCHAR(5),
)

Create table ChucVu (
MaCV NVARCHAR(5) PRIMARY KEY ,
TenCV NVARCHAR(10) NOT NULL,
HSPhuCap int,
)

Create table QuaTrinhLuong (
MaNV  NVARCHAR(5),
Ngay_BD SMALLDATETIME,
HSLuong int,
GhiChu NVARCHAR(50),
FOREIGN KEY (MaNV)
REFERENCES NhanVien(MaNV),

)




