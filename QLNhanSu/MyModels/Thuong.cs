using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[Table("Thuong")]
public partial class Thuong
{
    [Key]
    [Column("thuong_id")]
    [StringLength(4)]
    [Unicode(false)]
    public string ThuongId { get; set; } = null!;

    [Column("loaithuong")]
    [StringLength(100)]
    public string Loaithuong { get; set; } = null!;

    [Column("sotien", TypeName = "decimal(10, 2)")]
    public decimal Sotien { get; set; }

    [InverseProperty("Thuong")]
    public virtual ICollection<CtNhanvienThuong> CtNhanvienThuongs { get; set; } = new List<CtNhanvienThuong>();
}
