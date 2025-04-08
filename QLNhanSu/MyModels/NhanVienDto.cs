namespace QLNhanSu.MyModels
{
    public class NhanVienDto
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
        public string chucvu { get; set; } = null!;

        public string trangthailv { get; set; } = null!;
    }
}

