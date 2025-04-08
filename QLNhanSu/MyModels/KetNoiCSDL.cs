using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.MyModels;

public partial class KetNoiCSDL : DbContext
{
    public KetNoiCSDL()
    {
    }

    public KetNoiCSDL(DbContextOptions<KetNoiCSDL> options)
        : base(options)
    {
    }

    public virtual DbSet<ChamCong> ChamCongs { get; set; }

    public virtual DbSet<CtNhanvienKhoantru> CtNhanvienKhoantrus { get; set; }

    public virtual DbSet<CtNhanvienPhucap> CtNhanvienPhucaps { get; set; }

    public virtual DbSet<CtNhanvienThuong> CtNhanvienThuongs { get; set; }

    public virtual DbSet<KhoanTru> KhoanTrus { get; set; }

    public virtual DbSet<Luong> Luongs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<PhongBan> PhongBans { get; set; }

    public virtual DbSet<PhuCap> PhuCaps { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<Thuong> Thuongs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChamCong>(entity =>
        {
            entity.HasKey(e => e.ChamcongId).HasName("PK__ChamCong__62DB2F1BCE9D7502");

            entity.Property(e => e.NhanvienId).IsFixedLength();
        });

        modelBuilder.Entity<CtNhanvienKhoantru>(entity =>
        {
            entity.Property(e => e.NhanvienId).IsFixedLength();
            entity.Property(e => e.TruId).IsFixedLength();

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.CtNhanvienKhoantrus).HasConstraintName("FK_CT_nhanvien_khoantru_nhanvien");

            entity.HasOne(d => d.Tru).WithMany(p => p.CtNhanvienKhoantrus).HasConstraintName("FK_CT_nhanvien_khoantru_khoantru");
        });

        modelBuilder.Entity<CtNhanvienPhucap>(entity =>
        {
            entity.HasKey(e => new { e.NhanvienId, e.PhucapId }).HasName("PK__CT_nhanv__EEB2E75A83031A52");

            entity.Property(e => e.NhanvienId).IsFixedLength();
            entity.Property(e => e.PhucapId).IsFixedLength();

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.CtNhanvienPhucaps).HasConstraintName("FK_CT_nhanvien_phucap_nhanvien");

            entity.HasOne(d => d.Phucap).WithMany(p => p.CtNhanvienPhucaps).HasConstraintName("FK_CT_nhanvien_phucap_phucap");
        });

        modelBuilder.Entity<CtNhanvienThuong>(entity =>
        {
            entity.HasKey(e => new { e.NhanvienId, e.ThuongId }).HasName("PK__CT_nhanv__EBC3A435678038DF");

            entity.Property(e => e.NhanvienId).IsFixedLength();
            entity.Property(e => e.ThuongId).IsFixedLength();

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.CtNhanvienThuongs).HasConstraintName("FK_CT_nhanvien_thuong_nhanvien");

            entity.HasOne(d => d.Thuong).WithMany(p => p.CtNhanvienThuongs).HasConstraintName("FK_CT_nhanvien_thuong_thuong");
        });

        modelBuilder.Entity<KhoanTru>(entity =>
        {
            entity.HasKey(e => e.TruId).HasName("PK__KhoanTru__BF81BF9247CD09B0");

            entity.Property(e => e.TruId).IsFixedLength();
        });

        modelBuilder.Entity<Luong>(entity =>
        {
            entity.HasKey(e => e.LuongId).HasName("PK__Luong__25818DA000B3B4D7");

            entity.Property(e => e.NhanvienId).IsFixedLength();

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.Luongs).HasConstraintName("FK_Luong_NhanVien");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.NhanvienId).HasName("PK__NhanVien__4B5DC987A248DF01");

            entity.Property(e => e.NhanvienId).IsFixedLength();
            entity.Property(e => e.PhongbanId).IsFixedLength();

            entity.HasOne(d => d.Phongban).WithMany(p => p.NhanViens).HasConstraintName("FK_NhanVien_PhongBan");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.QuyenId).HasName("PK__PhanQuye__11B6D60AA440D52A");

            entity.Property(e => e.QuyenId).IsFixedLength();
        });

        modelBuilder.Entity<PhongBan>(entity =>
        {
            entity.HasKey(e => e.PhongbanId).HasName("PK__PhongBan__7642E319DB59D37C");

            entity.Property(e => e.PhongbanId).IsFixedLength();
        });

        modelBuilder.Entity<PhuCap>(entity =>
        {
            entity.HasKey(e => e.PhucapId).HasName("PK__PhuCap__5EF2EDDBFB80EA82");

            entity.Property(e => e.PhucapId).IsFixedLength();
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.TaikhoanId).HasName("PK__TaiKhoan__3FA9BB9C90DD9B65");

            entity.Property(e => e.NhanvienId).IsFixedLength();
            entity.Property(e => e.QuyenId).IsFixedLength();

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.TaiKhoans).HasConstraintName("FK_TaiKhoan_NhanVien");

            entity.HasOne(d => d.Quyen).WithMany(p => p.TaiKhoans).HasConstraintName("FK_TaiKhoan_PhanQuyen");
        });

        modelBuilder.Entity<Thuong>(entity =>
        {
            entity.HasKey(e => e.ThuongId).HasName("PK__Thuong__09E6DB2BBE57845F");

            entity.Property(e => e.ThuongId).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
