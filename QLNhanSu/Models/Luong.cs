using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class Luong
{
    public int LuongId { get; set; }

    public DateOnly Thoigian { get; set; }

    public string NhanvienId { get; set; } = null!;

    public decimal Tongluong { get; set; }

    public virtual NhanVien Nhanvien { get; set; } = null!;
}
