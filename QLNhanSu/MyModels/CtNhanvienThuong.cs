using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[PrimaryKey("NhanvienId", "ThuongId")]
[Table("CT_nhanvien_thuong")]
public partial class CtNhanvienThuong
{
    [Key]
    [Column("nhanvien_id")]
    [StringLength(9)]
    [Unicode(false)]
    public string NhanvienId { get; set; } = null!;

    [Key]
    [Column("thuong_id")]
    [StringLength(4)]
    [Unicode(false)]
    public string ThuongId { get; set; } = null!;

    [Column("thoigian")]
    public DateOnly Thoigian { get; set; }

    [ForeignKey("NhanvienId")]
    [InverseProperty("CtNhanvienThuongs")]
    public virtual NhanVien Nhanvien { get; set; } = null!;

    [ForeignKey("ThuongId")]
    [InverseProperty("CtNhanvienThuongs")]
    public virtual Thuong Thuong { get; set; } = null!;
}
