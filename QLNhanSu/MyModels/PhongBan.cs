using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

[Table("PhongBan")]
[Index("Tenphongban", Name = "UQ__PhongBan__75B59B4A47785422", IsUnique = true)]
public partial class PhongBan
{
    [Key]
    [Column("phongban_id")]
    [StringLength(3)]
    [Unicode(false)]
    public string PhongbanId { get; set; } = null!;

    [Column("tenphongban")]
    [StringLength(100)]
    public string Tenphongban { get; set; } = null!;

    [InverseProperty("Phongban")]
    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
