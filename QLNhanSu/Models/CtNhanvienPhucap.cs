using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class CtNhanvienPhucap
{
    public string NhanvienId { get; set; } = null!;

    public string PhucapId { get; set; } = null!;

    public DateOnly Thoigian { get; set; }

    public virtual NhanVien Nhanvien { get; set; } = null!;

    public virtual PhuCap Phucap { get; set; } = null!;
}
