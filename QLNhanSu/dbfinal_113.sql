CREATE DATABASE QLNhanSu	
USE [QLNhanSu]
GO
/****** Object:  Table [dbo].[ChamCong]    Script Date: 3/11/2025 4:40:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChamCong](
	[chamcong_id] [int] IDENTITY(1,1) NOT NULL,
	[ngay] [date] NULL,
	[giovao] [time](7) NULL,
	[giora] [time](7) NULL,
	[nhanvien_id] [char](9) NOT NULL,
	[dimuon] [nvarchar](10) NULL,
	[vesom] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[chamcong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CT_nhanvien_khoantru]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_nhanvien_khoantru](
	[nhanvien_id] [char](9) NOT NULL,
	[tru_id] [char](4) NOT NULL,
	[thoigian] [date] NOT NULL,
 CONSTRAINT [PK_CT_nhanvien_khoantru] PRIMARY KEY CLUSTERED 
(
	[nhanvien_id] ASC,
	[tru_id] ASC,
	[thoigian] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CT_nhanvien_phucap]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_nhanvien_phucap](
	[nhanvien_id] [char](9) NOT NULL,
	[phucap_id] [char](4) NOT NULL,
	[thoigian] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nhanvien_id] ASC,
	[phucap_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CT_nhanvien_thuong]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_nhanvien_thuong](
	[nhanvien_id] [char](9) NOT NULL,
	[thuong_id] [char](4) NOT NULL,
	[thoigian] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[nhanvien_id] ASC,
	[thuong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhoanTru]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhoanTru](
	[tru_id] [char](4) NOT NULL,
	[loaikhoantru] [nvarchar](200) NOT NULL,
	[sotien] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[tru_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Luong]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Luong](
	[luong_id] [int] IDENTITY(1,1) NOT NULL,
	[thoigian] [date] NOT NULL,
	[nhanvien_id] [char](9) NOT NULL,
	[tongluong] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[luong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[nhanvien_id] [char](9) NOT NULL,
	[tennhanvien] [nvarchar](50) NOT NULL,
	[gioitinh] [nvarchar](10) NOT NULL,
	[ngaysinh] [date] NOT NULL,
	[diachi] [varchar](80) NOT NULL,
	[sdt] [varchar](10) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[luongcoban] [decimal](10, 2) NOT NULL,
	[phongban_id] [char](3) NOT NULL,
	[ngayvaolam] [date] NOT NULL,
	[chucvu] [nvarchar](50) NOT NULL,
	[trangthailv] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__NhanVien__4B5DC987A248DF01] PRIMARY KEY CLUSTERED 
(
	[nhanvien_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__NhanVien__AB6E61643500110F] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__NhanVien__DDDFB48352849C26] UNIQUE NONCLUSTERED 
(
	[sdt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhanQuyen]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhanQuyen](
	[quyen_id] [char](4) NOT NULL,
	[loaiquyen] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[quyen_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[loaiquyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhongBan]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhongBan](
	[phongban_id] [char](3) NOT NULL,
	[tenphongban] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[phongban_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[tenphongban] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhuCap]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhuCap](
	[phucap_id] [char](4) NOT NULL,
	[loaiphucap] [nvarchar](200) NOT NULL,
	[sotien] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[phucap_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[taikhoan_id] [int] IDENTITY(1,1) NOT NULL,
	[tendangnhap] [varchar](50) NOT NULL,
	[matkhau] [varchar](80) NOT NULL,
	[quyen_id] [char](4) NOT NULL,
	[nhanvien_id] [char](9) NULL,
PRIMARY KEY CLUSTERED 
(
	[taikhoan_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[matkhau] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[tendangnhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Thuong]    Script Date: 3/11/2025 4:40:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Thuong](
	[thuong_id] [char](4) NOT NULL,
	[loaithuong] [nvarchar](100) NOT NULL,
	[sotien] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[thuong_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CT_nhanvien_khoantru]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_khoantru_khoantru] FOREIGN KEY([tru_id])
REFERENCES [dbo].[KhoanTru] ([tru_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_khoantru] CHECK CONSTRAINT [FK_CT_nhanvien_khoantru_khoantru]
GO
ALTER TABLE [dbo].[CT_nhanvien_khoantru]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_khoantru_nhanvien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_khoantru] CHECK CONSTRAINT [FK_CT_nhanvien_khoantru_nhanvien]
GO
ALTER TABLE [dbo].[CT_nhanvien_phucap]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_phucap_nhanvien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_phucap] CHECK CONSTRAINT [FK_CT_nhanvien_phucap_nhanvien]
GO
ALTER TABLE [dbo].[CT_nhanvien_phucap]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_phucap_phucap] FOREIGN KEY([phucap_id])
REFERENCES [dbo].[PhuCap] ([phucap_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_phucap] CHECK CONSTRAINT [FK_CT_nhanvien_phucap_phucap]
GO
ALTER TABLE [dbo].[CT_nhanvien_thuong]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_thuong_nhanvien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_thuong] CHECK CONSTRAINT [FK_CT_nhanvien_thuong_nhanvien]
GO
ALTER TABLE [dbo].[CT_nhanvien_thuong]  WITH CHECK ADD  CONSTRAINT [FK_CT_nhanvien_thuong_thuong] FOREIGN KEY([thuong_id])
REFERENCES [dbo].[Thuong] ([thuong_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CT_nhanvien_thuong] CHECK CONSTRAINT [FK_CT_nhanvien_thuong_thuong]
GO
ALTER TABLE [dbo].[Luong]  WITH CHECK ADD  CONSTRAINT [FK_Luong_NhanVien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Luong] CHECK CONSTRAINT [FK_Luong_NhanVien]
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_TaiKhoan_NhanVien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [FK_TaiKhoan_NhanVien]
GO
ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK_TaiKhoan_PhanQuyen] FOREIGN KEY([quyen_id])
REFERENCES [dbo].[PhanQuyen] ([quyen_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [FK_TaiKhoan_PhanQuyen]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [CK__NhanVien__gioiti__6EF57B66] CHECK  (([gioitinh]=N'Khác' OR [gioitinh]=N'Nữ' OR [gioitinh]=N'Nam'))
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [CK__NhanVien__gioiti__6EF57B66]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [FK_NhanVien_PhongBan] FOREIGN KEY([phongban_id])
REFERENCES [dbo].[PhongBan] ([phongban_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [FK_NhanVien_PhongBan]
GO
ALTER TABLE [dbo].[ChamCong]  WITH CHECK ADD  CONSTRAINT [FK_ChamCong_NhanVien] FOREIGN KEY([nhanvien_id])
REFERENCES [dbo].[NhanVien] ([nhanvien_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChamCong] CHECK CONSTRAINT [FK_ChamCong_NhanVien]
GO

-- bang khoan tru, phucap, thuong
insert into KhoanTru values ('K001', 'Di muon', 10000);
insert into KhoanTru values ('K002', 'Ve som', 10000);
insert into KhoanTru values ('K003', 'Vang buoi sang', 150000);
insert into KhoanTru values ('K004', 'Vang buoi chieu', 150000);
insert into KhoanTru values ('K005', 'Nghi khong phep', 300000);
insert into KhoanTru values ('K006', 'Nghi co phep', 200000);
insert into KhoanTru values ('K007', 'Lam hu hong tai san cong ty', 200000);
insert into KhoanTru values ('K008', 'Do lam viec khong hieu qua', 500000);
insert into KhoanTru values ('K009', 'Tre han cong viec', 500000);
insert into KhoanTru values ('K010', 'Vi pham an toan lao dong', 600000);
insert into KhoanTru values ('K011', 'Gay roi trat tu cong ty', 700000);

insert into Thuong values ('T001', 'Thuong Tet', 500000);
insert into Thuong values ('T002', 'Thuong gioi thieu', 1000000);
insert into Thuong values ('T003', 'Thuong hieu suat', 1000000);
insert into Thuong values ('T004', 'Thuong du an', 1000000);
insert into Thuong values ('T005', 'Thuong dao tao', 800000);
insert into Thuong values ('T006', 'Thuong sang kien', 1500000);

insert into PhuCap values ('P001', 'Phu cap an trua', 1000000);
insert into PhuCap values ('P002', 'Phu cap tham nien', 1000000);
insert into PhuCap values ('P003', 'Phu cap di lai', 600000);
insert into PhuCap values ('P004', 'Phu cap cong tac', 1200000);
insert into PhuCap values ('P005', 'Phu cap ngoai ngu', 700000);
insert into PhuCap values ('P006', 'Phu cap trang phuc', 500000);


insert into PhanQuyen values ('QTV0', 'Quan tri vien');
insert into PhanQuyen values ('QL00', 'Quan ly');
insert into PhanQuyen values ('NV00', 'Nhan vien');

insert into TaiKhoan values ('admin', 'admin@cmc', 'QTV0', null);
insert into TaiKhoan values ('ql', 'ql@cmc', 'QL00', null);
insert into TaiKhoan values ('bcs230070', 'bcs230070', 'NV00', 'BCS230070');

insert into PhongBan values ('BCS', 'Khoa hoc may tinh');
insert into PhongBan values ('BIT', 'Cong nghe thong tin');
insert into PhongBan values ('BA', 'Quan tri kinh doanh');

select * from NhanVien

insert into NhanVien values ('BCS230070', 'Doan Hoang Quan', 'Nam', '2005-12-22', 'Ha Noi', '0123456789', 'toga@cmc', 50000000, 'BCS', '2025-02-02', 'Giam doc', 'Dang lam viec');
insert into NhanVien values ('BCS230008', 'Nguyen Tuan Anh', 'Nam', '2005-12-22', 'Ha Noi', '0123456799', 'toga1@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230011', 'Nguyen Duc Anh', 'Nam', '2005-12-22', 'Ha Noi', '0123456999', 'toga2@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230021', 'Pham Tien Dat', 'Nam', '2005-12-22', 'Ha Noi', '0123459999', 'toga3@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230024', 'Pham Khac Do', 'Nam', '2005-12-22', 'Ha Noi', '0123499999', 'toga4@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230035', 'Lai Hoang Duy', 'Nam', '2005-12-22', 'Ha Noi', '0123999999', 'toga5@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230039', 'Vu Van Huan', 'Nam', '2005-12-22', 'Ha Noi', '0129999999', 'toga6@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230064', 'Nguyen Thi Ngoc', N'Nữ', '2005-12-22', 'Ha Noi', '019999999', 'toga7@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230073', 'Nguyen Ba Minh Quang', 'Nam', '2005-12-22', 'Ha Noi', '0999999999', 'toga8@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230074', 'Vu Trong Quy', 'Nam', '2005-12-22', 'Ha Noi', '0123456788', 'toga9@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230085', 'Pham Viet Trinh', N'Nữ', '2005-12-22', 'Ha Noi', '0123456888', 'toga10@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230093', 'Nguyen Dinh Quang Vinh', 'Nam', '2005-12-22', 'Ha Noi', '0123458888', 'toga11@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230099', 'Nguyen Viet Duc', 'Nam', '2005-12-22', 'Ha Noi', '0123488888', 'toga12@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230116', 'Nguyen Thanh Vinh', 'Nam', '2005-12-22', 'Ha Noi', '0123888888', 'toga13@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230119', 'Cao Nguyen Minh Quan', 'Nam', '2005-12-22', 'Ha Noi', '0128888888', 'toga14@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');
insert into NhanVien values ('BCS230123', 'Quach Ngoc Nguyen', 'Nam', '2005-12-22', 'Ha Noi', '0188888888', 'toga15@cmc', 20000000, 'BCS', '2025-02-02', 'Nhan vien', 'Dang lam viec');

delete from NhanVien 