using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[PrimaryKey("NhanvienId", "PhucapId")]
[Table("CT_nhanvien_phucap")]
public partial class CtNhanvienPhucap
{
    [Key]
    [Column("nhanvien_id")]
    [StringLength(9)]
    [Unicode(false)]
    public string NhanvienId { get; set; } = null!;

    [Key]
    [Column("phucap_id")]
    [StringLength(4)]
    [Unicode(false)]
    public string PhucapId { get; set; } = null!;

    [Column("thoigian")]
    public DateOnly Thoigian { get; set; }

    [ForeignKey("NhanvienId")]
    [InverseProperty("CtNhanvienPhucaps")]
    public virtual NhanVien Nhanvien { get; set; } = null!;

    [ForeignKey("PhucapId")]
    [InverseProperty("CtNhanvienPhucaps")]
    public virtual PhuCap Phucap { get; set; } = null!;
}
