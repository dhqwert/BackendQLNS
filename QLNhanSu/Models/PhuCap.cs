using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class PhuCap
{
    public string PhucapId { get; set; } = null!;

    public string Loaiphucap { get; set; } = null!;

    public decimal Sotien { get; set; }

    public virtual ICollection<CtNhanvienPhucap> CtNhanvienPhucaps { get; set; } = new List<CtNhanvienPhucap>();
}
