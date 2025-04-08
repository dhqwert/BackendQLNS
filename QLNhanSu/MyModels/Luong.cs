using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[Table("Luong")]
public partial class Luong
{
/*    internal decimal tongluong;
*/
    [Key]
    [Column("luong_id")]
    public int LuongId { get; set; }

    [Column("thoigian")]
    public DateOnly Thoigian { get; set; }

    [Column("nhanvien_id")]
    [StringLength(9)]
    [Unicode(false)]
    public string NhanvienId { get; set; } = null!;

    [Column("tongluong", TypeName = "decimal(10, 2)")]
    public decimal Tongluong { get; set; }

    //[Column("tongluong", TypeName = "decimal(10, 2)")]
    //public decimal TongKhoantru { get; set; }

    //[Column("tongluong", TypeName = "decimal(10, 2)")]
    //public decimal TongPhucap { get; set; }

    //[Column("tongluong", TypeName = "decimal(10, 2)")]
    //public decimal TongThuong { get; set; }


    [ForeignKey("NhanvienId")]
    [InverseProperty("Luongs")]
    public virtual NhanVien Nhanvien { get; set; } = null!;
}
