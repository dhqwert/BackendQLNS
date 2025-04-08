using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class CtNhanvienKhoantru
{
    public string NhanvienId { get; set; } = null!;

    public string TruId { get; set; } = null!;

    public DateOnly Thoigian { get; set; }

    public virtual NhanVien Nhanvien { get; set; } = null!;

    public virtual KhoanTru Tru { get; set; } = null!;
}
