using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[Table("KhoanTru")]
public partial class KhoanTru
{
    [Key]
    [Column("tru_id")]
    [StringLength(4)]
    [Unicode(false)]
    public string TruId { get; set; } = null!;

    [Column("loaikhoantru")]
    [StringLength(200)]
    public string Loaikhoantru { get; set; } = null!;

    [Column("sotien", TypeName = "decimal(10, 2)")]
    public decimal Sotien { get; set; }

    [InverseProperty("Tru")]
    public virtual ICollection<CtNhanvienKhoantru> CtNhanvienKhoantrus { get; set; } = new List<CtNhanvienKhoantru>();
}
