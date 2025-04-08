namespace QLNhanSu.MyModels

{
    public class CtNhanvienPhucapDto
    {
        public string NhanvienId { get; set; } = null!;
        public string PhucapId { get; set; } = null!;
        public DateOnly Thoigian { get; set; }
    }
}

