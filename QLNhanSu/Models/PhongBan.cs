using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class PhongBan
{
    public string PhongbanId { get; set; } = null!;

    public string Tenphongban { get; set; } = null!;

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
