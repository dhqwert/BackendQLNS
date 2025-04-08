using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class PhanQuyen
{
    public string QuyenId { get; set; } = null!;

    public string Loaiquyen { get; set; } = null!;

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
