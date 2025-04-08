using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhanSu.MyModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QLNhanSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "NV")]
    public class NVController : ControllerBase
    {
        private readonly KetNoiCSDL _context;
        private static readonly string PYTHON_API_URL = "http://localhost:5001/api/attendance";

        public NVController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/ChamCongNV?ngay=yyyy-MM-dd
        [HttpGet("Attendance")]
        public async Task<ActionResult<IEnumerable<ChamCongDto>>> GetChamCong([FromQuery] string ngay)
        {
            if (!DateOnly.TryParse(ngay, out DateOnly parsedNgay))
            {
                return BadRequest(new { message = "Ngày không hợp lệ." });
            }

            // Lấy NhanvienId từ token
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;
            if (string.IsNullOrEmpty(nhanvienId))
            {
                return Unauthorized(new { message = "Không xác định được nhân viên." });
            }

            var chamCong = await _context.ChamCongs
                .Where(c => c.NhanvienId == nhanvienId && c.Ngay == parsedNgay)
                .Select(ch => new ChamCongDto
                {
                    Ngay = ch.Ngay.HasValue ? ch.Ngay.Value.ToString("yyyy-MM-dd") : string.Empty,
                    Giovao = ch.Giovao.HasValue ? ch.Giovao.Value.ToString("HH:mm:ss") : string.Empty,
                    Giora = ch.Giora.HasValue ? ch.Giora.Value.ToString("HH:mm:ss") : string.Empty,
                    NhanvienId = ch.NhanvienId,
                    Dimuon = ch.Dimuon,
                    Vesom = ch.Vesom
                })
                .ToListAsync();

            if (!chamCong.Any())
            {
                return NotFound(new { message = "Không tìm thấy dữ liệu chấm công cho ngày này." });
            }

            return Ok(chamCong);
        }

        [HttpGet("MyProfile")]
        public async Task<IActionResult> GetMyProfile()
        {
            // 🔹 Lấy `NhanvienId` từ token
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;

            if (string.IsNullOrEmpty(nhanvienId))
            {
                return Unauthorized(new { message = "Không xác định được nhân viên từ token." });
            }

            Console.WriteLine($"🔍 Kiểm tra nhân viên: {nhanvienId}");

            // 🔹 Truy vấn nhân viên từ database
            var nhanVien = await _context.NhanViens
                .Where(nv => nv.NhanvienId == nhanvienId)
                .Select(nv => new
                {
                    nv.NhanvienId,
                    nv.Tennhanvien,
                    nv.Phongban.Tenphongban,
                    nv.chucvu,
                    nv.Ngaysinh,
                    nv.Gioitinh,
                    nv.Diachi,
                    nv.Sdt,
                    nv.Email,
                    nv.trangthailv,
                    nv.Luongcoban
                })
                .FirstOrDefaultAsync();

            if (nhanVien == null)
            {
                return NotFound("Không tìm thấy thông tin nhân viên.");
            }

            return Ok(nhanVien);
        }


        [HttpGet("MyAccount")]
        public async Task<IActionResult> GetMyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            var account = await _context.TaiKhoans
                .Where(tk => tk.TaikhoanId.ToString() == userId)
                .Select(tk => new
                {
                    tk.Tendangnhap,
                    tk.NhanvienId,
                    tk.Matkhau
                })
                .FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound(new { message = "Không tìm thấy thông tin tài khoản." });
            }

            return Ok(account);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            // 🛑 Lấy `userId` từ token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Không xác định được người dùng." });
            }

            // 🛠 Lấy `NhanvienId` từ token (có thể NULL với quản lý & quản trị)
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;

            // 🔍 Tìm tài khoản dựa trên `userId`
            var account = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.TaikhoanId.ToString() == userId);

            if (account == null)
            {
                return NotFound(new { message = "Không tìm thấy tài khoản để đổi mật khẩu." });
            }

            // 🔐 Kiểm tra mật khẩu cũ
            if (account.Matkhau != model.OldPassword)
            {
                return BadRequest(new { message = "Mật khẩu cũ không đúng." });
            }

            // ✅ Cập nhật mật khẩu mới
            account.Matkhau = model.NewPassword;
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mật khẩu đã được thay đổi thành công!" });
        }



        // 🆕 API dành cho nhân viên lấy dữ liệu của chính mình
        //[HttpGet("sync-self")]
        //public async Task<IActionResult> SyncSelfAttendance()
        //{
        //    // ✅ Lấy `NhanvienId` từ token
        //    string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;
        //    if (string.IsNullOrEmpty(nhanvienId))
        //    {
        //        return Unauthorized(new { error = "Không tìm thấy ID nhân viên trong token." });
        //    }

        //    Console.WriteLine($"🔍 Nhân viên đang đăng nhập: {nhanvienId}");

        //    using HttpClient client = new();
        //    HttpResponseMessage response = await client.GetAsync(PYTHON_API_URL);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return BadRequest(new { error = "Không thể lấy dữ liệu từ API Python", status = response.StatusCode });
        //    }

        //    string jsonData = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine("📥 JSON từ API Python:");
        //    Console.WriteLine(jsonData);

        //    try
        //    {
        //        var data = System.Text.Json.JsonSerializer.Deserialize<List<ChamCongDto>>(jsonData, new System.Text.Json.JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        });

        //        if (data == null || data.Count == 0)
        //        {
        //            return BadRequest(new { error = "Dữ liệu nhận được từ API Python trống hoặc không hợp lệ." });
        //        }

        //        // 🛑 Chỉ lấy dữ liệu của nhân viên đang đăng nhập
        //        var filteredData = data.Where(x => x.NhanvienId == nhanvienId).ToList();

        //        if (!filteredData.Any())
        //        {
        //            return NotFound(new { message = "Không có dữ liệu chấm công cho nhân viên này." });
        //        }

        //        return Ok(filteredData);
        //    }
        //    catch (System.Text.Json.JsonException ex)
        //    {
        //        Console.WriteLine($"❌ Lỗi JSON: {ex.Message}");
        //        return BadRequest(new { error = "Lỗi khi parse JSON từ API Python", details = ex.Message });
        //    }
        //}


        // Lấy danh sách tất cả khoản trừ của bản thân nhân viên
        [HttpGet("all-khoantru")]
        public async Task<ActionResult> GetAllKhoanTru()
        {
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;
            if (string.IsNullOrEmpty(nhanvienId))
            {
                return Unauthorized(new { message = "Không xác định được nhân viên." });
            }


            var _data = from ct in _context.CtNhanvienKhoantrus
                        join tru in _context.KhoanTrus on ct.TruId equals tru.TruId
                        where ct.NhanvienId == nhanvienId
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            tru.Loaikhoantru,
                            tru.Sotien,
                            ct.Thoigian
                        };

            return Ok(new { message = "Lấy dữ liệu thành công!", status = 200, data = _data });
        }

        // Lấy danh sách khoản trừ theo tháng của bản thân nhân viên
        [HttpGet("byMonth-khoantru/{month}/{year}")]
        public async Task<ActionResult> GetKhoanTruByMonth(int month, int year)
        {
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;

            if (string.IsNullOrEmpty(nhanvienId))
            {
                return Unauthorized(new { message = "Không xác định được nhân viên." });
            }

            var _data = from ct in _context.CtNhanvienKhoantrus
                        join tru in _context.KhoanTrus on ct.TruId equals tru.TruId
                        where ct.NhanvienId == nhanvienId && ct.Thoigian.Month == month && ct.Thoigian.Year == year
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            tru.Loaikhoantru,
                            tru.Sotien,
                            ct.Thoigian
                        };

            return Ok(new { message = "Lấy dữ liệu thành công!", status = 200, data = _data });
        }

        // Lấy danh sách tất cả phụ cấp của bản thân nhân viên
        [HttpGet("all-phucap")]
        public async Task<ActionResult> GetAllPhuCap()
        {
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;
            if (string.IsNullOrEmpty(nhanvienId))
            {
                return Unauthorized(new { message = "Không xác định được nhân viên." });
            }

            var _data = from ct in _context.CtNhanvienPhucaps
                        join pc in _context.PhuCaps on ct.PhucapId equals pc.PhucapId
                        where ct.NhanvienId == nhanvienId
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            pc.Loaiphucap,
                            pc.Sotien,
                            ct.Thoigian
                        };
            return Ok(new { message = "Lấy dữ liệu thành công!", status = 200, data = _data });
        }

        // Lấy danh sách phụ cấp theo tháng của bản thân nhân viên
        [HttpGet("byMonth-phucap/{month}/{year}")]
        public async Task<ActionResult> GetPhuCapByMonth(int month, int year)
        {
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;
            if (string.IsNullOrEmpty(nhanvienId))
            {
                return Unauthorized(new { message = "Không xác định được nhân viên." });
            }

            var _data = from ct in _context.CtNhanvienPhucaps
                        join pc in _context.PhuCaps on ct.PhucapId equals pc.PhucapId
                        where ct.NhanvienId == nhanvienId && ct.Thoigian.Month == month && ct.Thoigian.Year == year
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            pc.Loaiphucap,
                            pc.Sotien,
                            ct.Thoigian
                        };
            return Ok(new { message = "Lấy dữ liệu thành công!", status = 200, data = _data });
        }

        [HttpGet("all-thuong")]
        public async Task<ActionResult> GetAllThuong()
        {
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;
            if (string.IsNullOrEmpty(nhanvienId))
            {
                return Unauthorized(new { message = "Không xác định được nhân viên." });
            }

            var _data = from ct in _context.CtNhanvienThuongs
                        join t in _context.Thuongs on ct.ThuongId equals t.ThuongId
                        where ct.NhanvienId == nhanvienId
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            t.Loaithuong,
                            t.Sotien,
                            ct.Thoigian
                        };
            return Ok(new { message = "Lấy dữ liệu thành công!", status = 200, data = _data });
        }

        // Lấy danh sách phụ cấp theo tháng của bản thân nhân viên
        [HttpGet("byMonth-thuong/{month}/{year}")]
        public async Task<ActionResult> GetThuongByMonth(int month, int year)
        {
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;
            if (string.IsNullOrEmpty(nhanvienId))
            {
                return Unauthorized(new { message = "Không xác định được nhân viên." });
            }

            var _data = from ct in _context.CtNhanvienThuongs
                        join t in _context.Thuongs on ct.ThuongId equals t.ThuongId
                        where ct.NhanvienId == nhanvienId && ct.Thoigian.Month == month && ct.Thoigian.Year == year
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            t.Loaithuong,
                            t.Sotien,
                            ct.Thoigian
                        };
            return Ok(new { message = "Lấy dữ liệu thành công!", status = 200, data = _data });
        }

        [HttpGet("XemLuong")]
        public async Task<ActionResult> GetLuongNhanVien([FromQuery] int month, [FromQuery] int year)
        {
            // 🔹 Lấy NhanvienId từ token
            string? nhanvienId = User.Claims.FirstOrDefault(c => c.Type == "NhanvienId")?.Value;
            if (string.IsNullOrEmpty(nhanvienId))
            {
                return Unauthorized(new { message = "Không xác định được nhân viên." });
            }

            // 🔹 Kiểm tra xem nhân viên có tồn tại không
            var nhanVien = await _context.NhanViens.FirstOrDefaultAsync(nv => nv.NhanvienId == nhanvienId);
            if (nhanVien == null)
            {
                return NotFound(new { message = "Không tìm thấy thông tin nhân viên." });
            }

            // 🔹 Lấy thông tin lương của nhân viên đó theo tháng/năm
            var luong = await _context.Luongs
                .Where(l => l.NhanvienId == nhanvienId && l.Thoigian.Month == month && l.Thoigian.Year == year)
                .Select(l => new
                {
                    l.NhanvienId,
                    nhanVien.Tennhanvien,
                    l.Thoigian,
                    LuongCoBan = nhanVien.Luongcoban,

                    // 🔹 Tính tổng khoản trừ
                    TongKhoanTru = _context.CtNhanvienKhoantrus
                        .Where(k => k.NhanvienId == nhanvienId && k.Thoigian.Month == month && k.Thoigian.Year == year)
                        .Sum(k => (decimal?)_context.KhoanTrus
                            .Where(tru => tru.TruId == k.TruId)
                            .Select(tru => tru.Sotien)
                            .FirstOrDefault() ?? 0m),

                    // 🔹 Tính tổng phụ cấp
                    TongPhuCap = _context.CtNhanvienPhucaps
                        .Where(p => p.NhanvienId == nhanvienId && p.Thoigian.Month == month && p.Thoigian.Year == year)
                        .Sum(p => (decimal?)_context.PhuCaps
                            .Where(pc => pc.PhucapId == p.PhucapId)
                            .Select(pc => pc.Sotien)
                            .FirstOrDefault() ?? 0m),

                    // 🔹 Tính tổng thưởng
                    TongThuong = _context.CtNhanvienThuongs
                        .Where(t => t.NhanvienId == nhanvienId && t.Thoigian.Month == month && t.Thoigian.Year == year)
                        .Sum(t => (decimal?)_context.Thuongs
                            .Where(th => th.ThuongId == t.ThuongId)
                            .Select(th => th.Sotien)
                            .FirstOrDefault() ?? 0m),

                    // 🔹 Tính tổng lương cuối cùng
                    TongLuong = nhanVien.Luongcoban
                                + (_context.CtNhanvienPhucaps
                                    .Where(p => p.NhanvienId == nhanvienId && p.Thoigian.Month == month && p.Thoigian.Year == year)
                                    .Sum(p => (decimal?)_context.PhuCaps
                                        .Where(pc => pc.PhucapId == p.PhucapId)
                                        .Select(pc => pc.Sotien)
                                        .FirstOrDefault() ?? 0m))
                                + (_context.CtNhanvienThuongs
                                    .Where(t => t.NhanvienId == nhanvienId && t.Thoigian.Month == month && t.Thoigian.Year == year)
                                    .Sum(t => (decimal?)_context.Thuongs
                                        .Where(th => th.ThuongId == t.ThuongId)
                                        .Select(th => th.Sotien)
                                        .FirstOrDefault() ?? 0m))
                                - (_context.CtNhanvienKhoantrus
                                    .Where(k => k.NhanvienId == nhanvienId && k.Thoigian.Month == month && k.Thoigian.Year == year)
                                    .Sum(k => (decimal?)_context.KhoanTrus
                                        .Where(tru => tru.TruId == k.TruId)
                                        .Select(tru => tru.Sotien)
                                        .FirstOrDefault() ?? 0m))
                                - (nhanVien.Luongcoban * 0.1m) // 🔹 Trừ bảo hiểm 10%
                })
                .FirstOrDefaultAsync();

            if (luong == null)
            {
                return NotFound(new { message = $"Không có dữ liệu lương tháng {month}/{year}." });
            }

            return Ok(luong);
        }



        public class ChangePasswordDto
        {
            public string? OldPassword { get; set; }
            public string? NewPassword { get; set; }
        }
    }
}
