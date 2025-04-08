using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class ChamCong
{
    public int ChamcongId { get; set; }

    public DateOnly? Ngay { get; set; }

    public TimeOnly? Giovao { get; set; }

    public TimeOnly? Giora { get; set; }

    public string NhanvienId { get; set; } = null!;

    public string? Dimuon { get; set; }

    public string? Vesom { get; set; }

    public virtual NhanVien Nhanvien { get; set; } = null!;
}
