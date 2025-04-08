using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class TaiKhoan
{
    public int TaikhoanId { get; set; }

    public string Tendangnhap { get; set; } = null!;

    public string Matkhau { get; set; } = null!;

    public string QuyenId { get; set; } = null!;

    public string? NhanvienId { get; set; }

    public virtual NhanVien? Nhanvien { get; set; }

    public virtual PhanQuyen Quyen { get; set; } = null!;
}
