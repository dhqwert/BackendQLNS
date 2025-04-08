using System.Threading.Tasks;


namespace QLNhanSu.Services
{
    public interface IAttendanceService
    {
        Task SyncAttendance();
        Task<string> GetAttendanceDataAsync();
        Task<(string NhanVienId, string Ngay)> GetNhanVienIdAndNgayByChamCongIdAsync(string chamCongId);
    }
}

