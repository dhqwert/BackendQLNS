using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[Table("PhuCap")]
public partial class PhuCap
{
    [Key]
    [Column("phucap_id")]
    [StringLength(4)]
    [Unicode(false)]
    public string PhucapId { get; set; } = null!;

    [Column("loaiphucap")]
    [StringLength(200)]
    public string Loaiphucap { get; set; } = null!;

    [Column("sotien", TypeName = "decimal(10, 2)")]
    public decimal Sotien { get; set; }

    [InverseProperty("Phucap")]
    public virtual ICollection<CtNhanvienPhucap> CtNhanvienPhucaps { get; set; } = new List<CtNhanvienPhucap>();
}
