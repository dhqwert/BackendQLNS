using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[Table("TaiKhoan")]
[Index("Matkhau", Name = "UQ__TaiKhoan__17FFF615C043869F", IsUnique = true)]
[Index("Tendangnhap", Name = "UQ__TaiKhoan__CE900A1E186F1594", IsUnique = true)]
public partial class TaiKhoan
{
    [Key]
    [Column("taikhoan_id")]
    public int TaikhoanId { get; set; }

    [Column("tendangnhap")]
    [StringLength(50)]
    [Unicode(false)]
    public string Tendangnhap { get; set; } = null!;

    [Column("matkhau")]
    [StringLength(80)]
    [Unicode(false)]
    public string Matkhau { get; set; } = null!;

    [Column("quyen_id")]
    [StringLength(4)]
    [Unicode(false)]
    public string QuyenId { get; set; } = null!;

    [Column("nhanvien_id")]
    [StringLength(9)]
    [Unicode(false)]
    public string NhanvienId { get; set; } = null!;

    [ForeignKey("NhanvienId")]
    [InverseProperty("TaiKhoans")]
    public virtual NhanVien Nhanvien { get; set; } = null!;

    [ForeignKey("QuyenId")]
    [InverseProperty("TaiKhoans")]
    public virtual PhanQuyen Quyen { get; set; } = null!;
}
