using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class Thuong
{
    public string ThuongId { get; set; } = null!;

    public string Loaithuong { get; set; } = null!;

    public decimal Sotien { get; set; }

    public virtual ICollection<CtNhanvienThuong> CtNhanvienThuongs { get; set; } = new List<CtNhanvienThuong>();
}
