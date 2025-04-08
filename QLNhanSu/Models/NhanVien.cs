using System;
using System.Collections.Generic;

namespace QLNhanSu.Models;

public partial class NhanVien
{
    public string NhanvienId { get; set; } = null!;

    public string Tennhanvien { get; set; } = null!;

    public string Gioitinh { get; set; } = null!;

    public DateOnly Ngaysinh { get; set; }

    public string Diachi { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public decimal Luongcoban { get; set; }

    public string PhongbanId { get; set; } = null!;

    public DateOnly Ngayvaolam { get; set; }

    public string Chucvu { get; set; } = null!;

    public string Trangthailv { get; set; } = null!;

    public virtual ICollection<ChamCong> ChamCongs { get; set; } = new List<ChamCong>();

    public virtual ICollection<CtNhanvienKhoantru> CtNhanvienKhoantrus { get; set; } = new List<CtNhanvienKhoantru>();

    public virtual ICollection<CtNhanvienPhucap> CtNhanvienPhucaps { get; set; } = new List<CtNhanvienPhucap>();

    public virtual ICollection<CtNhanvienThuong> CtNhanvienThuongs { get; set; } = new List<CtNhanvienThuong>();

    public virtual ICollection<Luong> Luongs { get; set; } = new List<Luong>();

    public virtual PhongBan Phongban { get; set; } = null!;

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
