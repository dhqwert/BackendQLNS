using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class KhoanTru
{
    public string TruId { get; set; } = null!;

    public string Loaikhoantru { get; set; } = null!;

    public decimal Sotien { get; set; }

    public virtual ICollection<CtNhanvienKhoantru> CtNhanvienKhoantrus { get; set; } = new List<CtNhanvienKhoantru>();
}
