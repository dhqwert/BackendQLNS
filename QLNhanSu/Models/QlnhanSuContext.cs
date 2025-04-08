using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.Models;

public partial class QlnhanSuContext : DbContext
{
    public QlnhanSuContext()
    {
    }

    public QlnhanSuContext(DbContextOptions<QlnhanSuContext> options)
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=dhq;Initial Catalog=QLNhanSu;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChamCong>(entity =>
        {
            entity.HasKey(e => e.ChamcongId).HasName("PK__ChamCong__62DB2F1BFFD44342");

            entity.ToTable("ChamCong");

            entity.Property(e => e.ChamcongId).HasColumnName("chamcong_id");
            entity.Property(e => e.Dimuon)
                .HasMaxLength(10)
                .HasColumnName("dimuon");
            entity.Property(e => e.Giora).HasColumnName("giora");
            entity.Property(e => e.Giovao).HasColumnName("giovao");
            entity.Property(e => e.Ngay).HasColumnName("ngay");
            entity.Property(e => e.NhanvienId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nhanvien_id");
            entity.Property(e => e.Vesom)
                .HasMaxLength(10)
                .HasColumnName("vesom");

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.ChamCongs)
                .HasForeignKey(d => d.NhanvienId)
                .HasConstraintName("FK_ChamCong_NhanVien");
        });

        modelBuilder.Entity<CtNhanvienKhoantru>(entity =>
        {
            entity.HasKey(e => new { e.NhanvienId, e.TruId, e.Thoigian });

            entity.ToTable("CT_nhanvien_khoantru");

            entity.Property(e => e.NhanvienId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nhanvien_id");
            entity.Property(e => e.TruId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tru_id");
            entity.Property(e => e.Thoigian).HasColumnName("thoigian");

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.CtNhanvienKhoantrus)
                .HasForeignKey(d => d.NhanvienId)
                .HasConstraintName("FK_CT_nhanvien_khoantru_nhanvien");

            entity.HasOne(d => d.Tru).WithMany(p => p.CtNhanvienKhoantrus)
                .HasForeignKey(d => d.TruId)
                .HasConstraintName("FK_CT_nhanvien_khoantru_khoantru");
        });

        modelBuilder.Entity<CtNhanvienPhucap>(entity =>
        {
            entity.HasKey(e => new { e.NhanvienId, e.PhucapId }).HasName("PK__CT_nhanv__EEB2E75AAA09B8DC");

            entity.ToTable("CT_nhanvien_phucap");

            entity.Property(e => e.NhanvienId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nhanvien_id");
            entity.Property(e => e.PhucapId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phucap_id");
            entity.Property(e => e.Thoigian).HasColumnName("thoigian");

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.CtNhanvienPhucaps)
                .HasForeignKey(d => d.NhanvienId)
                .HasConstraintName("FK_CT_nhanvien_phucap_nhanvien");

            entity.HasOne(d => d.Phucap).WithMany(p => p.CtNhanvienPhucaps)
                .HasForeignKey(d => d.PhucapId)
                .HasConstraintName("FK_CT_nhanvien_phucap_phucap");
        });

        modelBuilder.Entity<CtNhanvienThuong>(entity =>
        {
            entity.HasKey(e => new { e.NhanvienId, e.ThuongId }).HasName("PK__CT_nhanv__EBC3A4352607B499");

            entity.ToTable("CT_nhanvien_thuong");

            entity.Property(e => e.NhanvienId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nhanvien_id");
            entity.Property(e => e.ThuongId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("thuong_id");
            entity.Property(e => e.Thoigian).HasColumnName("thoigian");

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.CtNhanvienThuongs)
                .HasForeignKey(d => d.NhanvienId)
                .HasConstraintName("FK_CT_nhanvien_thuong_nhanvien");

            entity.HasOne(d => d.Thuong).WithMany(p => p.CtNhanvienThuongs)
                .HasForeignKey(d => d.ThuongId)
                .HasConstraintName("FK_CT_nhanvien_thuong_thuong");
        });

        modelBuilder.Entity<KhoanTru>(entity =>
        {
            entity.HasKey(e => e.TruId).HasName("PK__KhoanTru__BF81BF9269907217");

            entity.ToTable("KhoanTru");

            entity.Property(e => e.TruId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tru_id");
            entity.Property(e => e.Loaikhoantru)
                .HasMaxLength(200)
                .HasColumnName("loaikhoantru");
            entity.Property(e => e.Sotien)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sotien");
        });

        modelBuilder.Entity<Luong>(entity =>
        {
            entity.HasKey(e => e.LuongId).HasName("PK__Luong__25818DA010699009");

            entity.ToTable("Luong");

            entity.Property(e => e.LuongId).HasColumnName("luong_id");
            entity.Property(e => e.NhanvienId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nhanvien_id");
            entity.Property(e => e.Thoigian).HasColumnName("thoigian");
            entity.Property(e => e.Tongluong)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("tongluong");

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.Luongs)
                .HasForeignKey(d => d.NhanvienId)
                .HasConstraintName("FK_Luong_NhanVien");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.NhanvienId).HasName("PK__NhanVien__4B5DC987A248DF01");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.Email, "UQ__NhanVien__AB6E61643500110F").IsUnique();

            entity.HasIndex(e => e.Sdt, "UQ__NhanVien__DDDFB48352849C26").IsUnique();

            entity.Property(e => e.NhanvienId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nhanvien_id");
            entity.Property(e => e.Chucvu)
                .HasMaxLength(50)
                .HasColumnName("chucvu");
            entity.Property(e => e.Diachi)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("diachi");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gioitinh)
                .HasMaxLength(10)
                .HasColumnName("gioitinh");
            entity.Property(e => e.Luongcoban)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("luongcoban");
            entity.Property(e => e.Ngaysinh).HasColumnName("ngaysinh");
            entity.Property(e => e.Ngayvaolam).HasColumnName("ngayvaolam");
            entity.Property(e => e.PhongbanId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phongban_id");
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sdt");
            entity.Property(e => e.Tennhanvien)
                .HasMaxLength(50)
                .HasColumnName("tennhanvien");
            entity.Property(e => e.Trangthailv)
                .HasMaxLength(50)
                .HasColumnName("trangthailv");

            entity.HasOne(d => d.Phongban).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.PhongbanId)
                .HasConstraintName("FK_NhanVien_PhongBan");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.QuyenId).HasName("PK__PhanQuye__11B6D60A9F471908");

            entity.ToTable("PhanQuyen");

            entity.HasIndex(e => e.Loaiquyen, "UQ__PhanQuye__D86804DA0DBDC644").IsUnique();

            entity.Property(e => e.QuyenId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("quyen_id");
            entity.Property(e => e.Loaiquyen)
                .HasMaxLength(50)
                .HasColumnName("loaiquyen");
        });

        modelBuilder.Entity<PhongBan>(entity =>
        {
            entity.HasKey(e => e.PhongbanId).HasName("PK__PhongBan__7642E319E7CA6597");

            entity.ToTable("PhongBan");

            entity.HasIndex(e => e.Tenphongban, "UQ__PhongBan__75B59B4A4C459CCF").IsUnique();

            entity.Property(e => e.PhongbanId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phongban_id");
            entity.Property(e => e.Tenphongban)
                .HasMaxLength(100)
                .HasColumnName("tenphongban");
        });

        modelBuilder.Entity<PhuCap>(entity =>
        {
            entity.HasKey(e => e.PhucapId).HasName("PK__PhuCap__5EF2EDDB725EF30E");

            entity.ToTable("PhuCap");

            entity.Property(e => e.PhucapId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phucap_id");
            entity.Property(e => e.Loaiphucap)
                .HasMaxLength(200)
                .HasColumnName("loaiphucap");
            entity.Property(e => e.Sotien)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sotien");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.TaikhoanId).HasName("PK__TaiKhoan__3FA9BB9C9388F0CB");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.Matkhau, "UQ__TaiKhoan__17FFF6154EADA3AE").IsUnique();

            entity.HasIndex(e => e.Tendangnhap, "UQ__TaiKhoan__CE900A1E851541C8").IsUnique();

            entity.Property(e => e.TaikhoanId).HasColumnName("taikhoan_id");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("matkhau");
            entity.Property(e => e.NhanvienId)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("nhanvien_id");
            entity.Property(e => e.QuyenId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("quyen_id");
            entity.Property(e => e.Tendangnhap)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tendangnhap");

            entity.HasOne(d => d.Nhanvien).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.NhanvienId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TaiKhoan_NhanVien");

            entity.HasOne(d => d.Quyen).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.QuyenId)
                .HasConstraintName("FK_TaiKhoan_PhanQuyen");
        });

        modelBuilder.Entity<Thuong>(entity =>
        {
            entity.HasKey(e => e.ThuongId).HasName("PK__Thuong__09E6DB2B67A2339C");

            entity.ToTable("Thuong");

            entity.Property(e => e.ThuongId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("thuong_id");
            entity.Property(e => e.Loaithuong)
                .HasMaxLength(100)
                .HasColumnName("loaithuong");
            entity.Property(e => e.Sotien)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sotien");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
