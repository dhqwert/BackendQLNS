using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class CtNhanvienThuong
{
    public string NhanvienId { get; set; } = null!;

    public string ThuongId { get; set; } = null!;

    public DateOnly Thoigian { get; set; }

    public virtual NhanVien Nhanvien { get; set; } = null!;

    public virtual Thuong Thuong { get; set; } = null!;
}
