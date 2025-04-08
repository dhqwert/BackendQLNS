using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[PrimaryKey("NhanvienId", "TruId", "Thoigian")]
[Table("CT_nhanvien_khoantru")]
public partial class CtNhanvienKhoantru
{
    [Key]
    [Column("nhanvien_id")]
    [StringLength(9)]
    [Unicode(false)]
    public string NhanvienId { get; set; } = null!;

    [Key]
    [Column("tru_id")]
    [StringLength(4)]
    [Unicode(false)]
    public string TruId { get; set; } = null!;

    [Key]
    [Column("thoigian")]
    public DateOnly Thoigian { get; set; }

    [ForeignKey("NhanvienId")]
    [InverseProperty("CtNhanvienKhoantrus")]
    public virtual NhanVien Nhanvien { get; set; } = null!;

    [ForeignKey("TruId")]
    [InverseProperty("CtNhanvienKhoantrus")]
    public virtual KhoanTru Tru { get; set; } = null!;
}
