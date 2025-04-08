using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[Table("ChamCong")]
public partial class ChamCong
{
    [Key]
    [Column("chamcong_id")]
    public int ChamcongId { get; set; }

    [Column("ngay")]
    public DateOnly? Ngay { get; set; }

    [Column("giovao")]
    public TimeOnly? Giovao { get; set; }

    [Column("giora")]
    public TimeOnly? Giora { get; set; }

    [Column("nhanvien_id")]
    [StringLength(9)]
    [Unicode(false)]
    public string NhanvienId { get; set; } = null!;

    [Column("dimuon")]
    [StringLength(10)]
    public string? Dimuon { get; set; }

    [Column("vesom")]
    [StringLength(10)]
    public string? Vesom { get; set; }
}
