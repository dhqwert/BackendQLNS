using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[Table("NhanVien")]
[Index("Email", Name = "UQ__NhanVien__AB6E61643500110F", IsUnique = true)]
[Index("Sdt", Name = "UQ__NhanVien__DDDFB48352849C26", IsUnique = true)]
public partial class NhanVien
{
    [Key]
    [Column("nhanvien_id")]
    [StringLength(9)]
    [Unicode(false)]
    public string NhanvienId { get; set; } = null!;

    [Column("tennhanvien")]
    [StringLength(50)]
    public string Tennhanvien { get; set; } = null!;

    [Column("gioitinh")]
    [StringLength(10)]
    public string Gioitinh { get; set; } = null!;

    [Column("ngaysinh")]
    public DateOnly Ngaysinh { get; set; }

    [Column("diachi")]
    [StringLength(80)]
    [Unicode(false)]
    public string Diachi { get; set; } = null!;

    [Column("sdt")]
    [StringLength(10)]
    [Unicode(false)]
    public string Sdt { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("trangthailv")]
    [StringLength(50)]
    public string trangthailv { get; set;} = null!;

    [Column("chucvu")]
    [StringLength(50)]
    public string chucvu { get; set; } = null!;

    [Column("luongcoban", TypeName = "decimal(10, 2)")]
    public decimal Luongcoban { get; set; }

    [Column("phongban_id")]
    [StringLength(3)]
    [Unicode(false)]
    public string PhongbanId { get; set; } = null!;

    [Column("ngayvaolam")]
    public DateOnly Ngayvaolam { get; set; }

    [InverseProperty("Nhanvien")]
    public virtual ICollection<CtNhanvienKhoantru> CtNhanvienKhoantrus { get; set; } = new List<CtNhanvienKhoantru>();

    [InverseProperty("Nhanvien")]
    public virtual ICollection<CtNhanvienPhucap> CtNhanvienPhucaps { get; set; } = new List<CtNhanvienPhucap>();

    [InverseProperty("Nhanvien")]
    public virtual ICollection<CtNhanvienThuong> CtNhanvienThuongs { get; set; } = new List<CtNhanvienThuong>();

    [InverseProperty("Nhanvien")]
    public virtual ICollection<Luong> Luongs { get; set; } = new List<Luong>();

    [ForeignKey("PhongbanId")]
    [InverseProperty("NhanViens")]
    public virtual PhongBan Phongban { get; set; } = null!;

    [InverseProperty("Nhanvien")]
    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
