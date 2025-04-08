using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[Table("PhanQuyen")]
[Index("Loaiquyen", Name = "UQ__PhanQuye__D86804DA9AEBE0E2", IsUnique = true)]
public partial class PhanQuyen
{
    [Key]
    [Column("quyen_id")]
    [StringLength(4)]
    [Unicode(false)]
    public string QuyenId { get; set; } = null!;

    [Column("loaiquyen")]
    [StringLength(50)]
    public string Loaiquyen { get; set; } = null!;

    [InverseProperty("Quyen")]
    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
