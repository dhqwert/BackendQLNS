using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhanSu.MyModels;
using QLNhanSu.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QLNhanSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "QL")] // Chỉ quản lý mới có quyền thao tác
    public class ChamCongQLController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory; // Khai báo trường này
        private readonly KetNoiCSDL _context;
        //private readonly AttendanceService _attendanceService;
        private readonly IAttendanceService _attendanceService;

        // Constructor tiêm IHttpClientFactory
        public ChamCongQLController(KetNoiCSDL context, IAttendanceService attendanceService, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _attendanceService = attendanceService ?? throw new ArgumentNullException(nameof(attendanceService));
            _httpClientFactory = httpClientFactory; // Gán _httpClientFactory
        }
        //private async Task CapNhatKhoanTru()
        //{
        //    var danhSachChamCong = await _context.ChamCongs.ToListAsync(); // Lấy toàn bộ chấm công

        //    foreach (var chamCong in danhSachChamCong)
        //    {
        //        var nhanvienId = chamCong.NhanvienId;
        //        if (!chamCong.Ngay.HasValue) continue; // ✅ Bỏ qua nếu ngày null
        //        var ngayChamCong = chamCong.Ngay.Value;

        //        // ✅ Xác định có đi muộn / về sớm không
        //        bool diMuon = chamCong.Giovao.HasValue && chamCong.Giovao.Value > new TimeOnly(8, 0, 0);
        //        bool veSom = chamCong.Giora.HasValue && chamCong.Giora.Value < new TimeOnly(17, 0, 0);
        //        bool nghikhongphep = !chamCong.Giovao.HasValue; // ✅ Kiểm tra không chấm công vào


        //        // 🔹 Kiểm tra & cập nhật khoản trừ đi muộn (TruId = "0003")
        //        var truDiMuon = await _context.CtNhanvienKhoantrus
        //            .FirstOrDefaultAsync(k => k.NhanvienId == nhanvienId && k.Thoigian == ngayChamCong && k.TruId == "0003");

        //        if (diMuon)
        //        {
        //            if (truDiMuon == null) // Chưa có khoản trừ -> thêm mới
        //            {
        //                _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
        //                {
        //                    NhanvienId = nhanvienId,
        //                    TruId = "0003",
        //                    Thoigian = ngayChamCong
        //                });
        //            }
        //        }
        //        else
        //        {
        //            if (truDiMuon != null) // Đã có khoản trừ nhưng không đi muộn nữa -> Xóa
        //            {
        //                _context.CtNhanvienKhoantrus.Remove(truDiMuon);
        //            }
        //        }

        //        // 🔹 Kiểm tra & cập nhật khoản trừ về sớm (TruId = "0004")
        //        var truVeSom = await _context.CtNhanvienKhoantrus
        //            .FirstOrDefaultAsync(k => k.NhanvienId == nhanvienId && k.Thoigian == ngayChamCong && k.TruId == "0004");

        //        if (veSom)
        //        {
        //            if (truVeSom == null) // Chưa có khoản trừ -> thêm mới
        //            {
        //                _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
        //                {
        //                    NhanvienId = nhanvienId,
        //                    TruId = "0004",
        //                    Thoigian = ngayChamCong
        //                });
        //            }
        //        }
        //        else
        //        {
        //            if (truVeSom != null) // Đã có khoản trừ nhưng không về sớm nữa -> Xóa
        //            {
        //                _context.CtNhanvienKhoantrus.Remove(truVeSom);
        //            }
        //        }

        //        // 🔹 Kiểm tra & cập nhật khoản trừ KHÔNG CHẤM CÔNG VÀO (TruId = "0005")
        //        var truKhongChamCong = await _context.CtNhanvienKhoantrus
        //            .FirstOrDefaultAsync(k => k.NhanvienId == nhanvienId && k.Thoigian == ngayChamCong && k.TruId == "0005");

        //        if (nghikhongphep)
        //        {
        //            if (truKhongChamCong == null) // Chưa có khoản trừ -> thêm mới
        //            {
        //                _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
        //                {
        //                    NhanvienId = nhanvienId,
        //                    TruId = "0005",
        //                    Thoigian = ngayChamCong
        //                });
        //            }
        //        }
        //        else
        //        {
        //            if (truKhongChamCong != null) // Đã có khoản trừ nhưng sau đó sửa lại -> Xóa
        //            {
        //                _context.CtNhanvienKhoantrus.Remove(truKhongChamCong);
        //            }
        //        }
        //    }

        //    await _context.SaveChangesAsync(); // Lưu lại thay đổi vào database
        //}

        ////[HttpGet("sync")]
        //public async Task<IActionResult> SyncAttendance()
        //{
        //    try
        //    {
        //        // Lấy dữ liệu từ API Python
        //        string jsonData = await _attendanceService.GetAttendanceDataAsync();
        //        Console.WriteLine("📥 JSON từ API Python:");
        //        Console.WriteLine(jsonData);

        //        var options = new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true,
        //            WriteIndented = true,
        //            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        //        };

        //        var data = JsonSerializer.Deserialize<List<ChamCongDto>>(jsonData, options);

        //        if (data == null || data.Count == 0)
        //        {
        //            return BadRequest(new { error = "Dữ liệu nhận được từ API Python trống hoặc không hợp lệ." });
        //        }

        //        // Xóa toàn bộ dữ liệu hiện tại trong cơ sở dữ liệu
        //        _context.ChamCongs.RemoveRange(_context.ChamCongs);
        //        await _context.SaveChangesAsync();
        //        Console.WriteLine("🧹 Đã xóa toàn bộ dữ liệu ChamCong trong DB");

        //        // Thêm mới tất cả các bản ghi từ API
        //        foreach (var item in data)
        //        {
        //            if (string.IsNullOrEmpty(item.NhanvienId))
        //            {
        //                Console.WriteLine("❌ Thiếu NhanvienId");
        //                continue;
        //            }

        //            if (!DateOnly.TryParse(item.Ngay, out var ngay))
        //            {
        //                Console.WriteLine($"❌ Ngày không hợp lệ: {item.Ngay}");
        //                continue;
        //            }

        //            TimeOnly? giovaoTime = null;
        //            TimeOnly? gioraTime = null;

        //            if (!string.IsNullOrWhiteSpace(item.Giovao) && TimeOnly.TryParse(item.Giovao, out var gv))
        //                giovaoTime = gv;
        //            if (!string.IsNullOrWhiteSpace(item.Giora) && TimeOnly.TryParse(item.Giora, out var gr))
        //                gioraTime = gr;

        //            // Kiểm tra Dimuon và Vesom
        //            string dimuon = (giovaoTime.HasValue && giovaoTime.Value > new TimeOnly(8, 0, 0)) ? "Y" : "N";
        //            string vesom = (gioraTime.HasValue && gioraTime.Value < new TimeOnly(17, 0, 0)) ? "Y" : "N";

        //            var newRecord = new ChamCong
        //            {
        //                NhanvienId = item.NhanvienId,
        //                Ngay = ngay,
        //                Giovao = giovaoTime,
        //                Giora = gioraTime,
        //                Dimuon = dimuon,
        //                Vesom = vesom
        //            };

        //            // Thêm bản ghi mới vào DB
        //            await _context.ChamCongs.AddAsync(newRecord);
        //        }

        //        // Lưu lại các thay đổi vào DB
        //        await _context.SaveChangesAsync();
        //        await CapNhatKhoanTru(); // Cập nhật các khoản trừ

        //        return Ok(new { message = "Đã đồng bộ dữ liệu thành công!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"❌ Lỗi: {ex.Message}");
        //        if (ex.InnerException != null)
        //            Console.WriteLine($"🔎 Inner Exception: {ex.InnerException.Message}");

        //        return StatusCode(500, new
        //        {
        //            error = "Lỗi hệ thống",
        //            message = ex.Message,
        //            inner = ex.InnerException?.Message,
        //            stack = ex.StackTrace
        //        });
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetChamCongs()
        {
            var chamCongs = await _context.ChamCongs
                .Select(ch => new
                {
                    ChamcongId = ch.ChamcongId,
                    Ngay = ch.Ngay.HasValue ? ch.Ngay.Value.ToString("yyyy-MM-dd") : null, // ✅ Kiểm tra null trước khi gọi .ToString()
                    Giovao = ch.Giovao.HasValue ? ch.Giovao.Value.ToString("HH:mm:ss") : null,
                    Giora = ch.Giora.HasValue ? ch.Giora.Value.ToString("HH:mm:ss") : null,
                    NhanvienId = ch.NhanvienId,
                    Dimuon = ch.Dimuon,
                    Vesom = ch.Vesom
                })
                .ToListAsync();

            return Ok(chamCongs);
        }


        // GET: api/ChamCongQL/search?nhanvienId=NV123&ngay=2025-03-22
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ChamCong>>> SearchChamCong(
            [FromQuery] string? nhanvienId,
            [FromQuery] string? ngay,
            [FromQuery] string? thangNam,
            [FromQuery] string? phongbanId)
        {
            var query = _context.ChamCongs.AsQueryable();

            if (!string.IsNullOrEmpty(nhanvienId))
            {
                query = query.Where(cc => cc.NhanvienId == nhanvienId);
            }

            if (DateOnly.TryParse(ngay, out DateOnly parsedNgay))
            {
                query = query.Where(cc => cc.Ngay == parsedNgay);
            }

            if (!string.IsNullOrEmpty(thangNam) && thangNam.Contains("-"))
            {
                var parts = thangNam.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[0], out int thang) && int.TryParse(parts[1], out int nam))
                {
                    query = query.Where(cc => cc.Ngay.HasValue && cc.Ngay.Value.Month == thang && cc.Ngay.Value.Year == nam);
                }
            }

            if (!string.IsNullOrEmpty(phongbanId))
            {
                query = query.Where(cc => _context.NhanViens.Any(nv => nv.NhanvienId == cc.NhanvienId && nv.PhongbanId == phongbanId));
            }

            //return await query.ToListAsync();
            var result = await query.Select(cc => new
            {
                cc.ChamcongId,  // ✅ Hiển thị ID của bản ghi chấm công
                cc.NhanvienId,
                cc.Ngay,
                Giovao = cc.Giovao.HasValue ? cc.Giovao.Value.ToString("HH:mm:ss") : string.Empty,  // 🛠 Xử lý null thành chuỗi rỗng
                Giora = cc.Giora.HasValue ? cc.Giora.Value.ToString("HH:mm:ss") : string.Empty,  // 🛠 Xử lý null thành chuỗi rỗng
                cc.Dimuon,
                cc.Vesom
            }).ToListAsync();


            return Ok(result);

        }

        [HttpGet("search-report")]
        public async Task<IActionResult> GetSearch(string? nhanvienId, string? phongbanId, string thang)
        {
            if (string.IsNullOrWhiteSpace(thang) || !DateTime.TryParseExact(thang + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest("Tháng phải có định dạng yyyy-MM (ví dụ: 2024-03).");
            }

            int month = parsedDate.Month;
            int year = parsedDate.Year;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            var nhanVienQuery = _context.NhanViens.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nhanvienId))
                nhanVienQuery = nhanVienQuery.Where(nv => nv.NhanvienId == nhanvienId);
            if (!string.IsNullOrWhiteSpace(phongbanId))
                nhanVienQuery = nhanVienQuery.Where(nv => nv.PhongbanId == phongbanId);

            var chamCongTrongThang = await _context.ChamCongs
                .Where(cc => cc.Ngay.HasValue &&
                             cc.Ngay.Value.Month == month &&
                             cc.Ngay.Value.Year == year)
                .ToListAsync(); // 👈 Truy vấn riêng

            var nhanViens = await nhanVienQuery.ToListAsync(); // 👈 Tách ra ngoài để xử lý LINQ-to-Objects

            var result = nhanViens
                .Select(nv =>
                {
                    var chamCongCuaNv = chamCongTrongThang
                        .Where(cc => cc.NhanvienId == nv.NhanvienId)
                        .GroupBy(cc => cc.Ngay.Value) // Group theo ngày duy nhất
                        .Select(g =>
                        {
                            var vao = g.FirstOrDefault(x => x.Giovao.HasValue)?.Giovao;
                            var ra = g.FirstOrDefault(x => x.Giora.HasValue)?.Giora;

                            return new
                            {
                                Ngay = g.Key,
                                DiMuon = vao.HasValue && vao.Value > new TimeOnly(8, 0, 0),
                                VeSom = ra.HasValue && ra.Value < new TimeOnly(17, 0, 0),
                            };
                        }).ToList();

                    return new
                    {
                        nv.NhanvienId,
                        nv.PhongbanId,
                        Thang = $"{year}-{month:D2}",
                        DiMuon = chamCongCuaNv.Count(x => x.DiMuon),
                        VeSom = chamCongCuaNv.Count(x => x.VeSom),
                        Nghi = daysInMonth - chamCongCuaNv.Count
                    };
                })
                .ToList();

            if (!result.Any())
                return NotFound("Không có dữ liệu thống kê cho yêu cầu này.");

            return Ok(result);
        }







        //// POST: api/ChamCongQL
        //[HttpPost]
        //public async Task<ActionResult<ChamCong>> PostChamCong([FromBody] ChamCongDto chamCongDto)
        //{
        //    if (chamCongDto == null)
        //    {
        //        return BadRequest(new { message = "Dữ liệu không hợp lệ." });
        //    }

        //    if (!DateOnly.TryParse(chamCongDto.Ngay, out DateOnly ngay))
        //    {
        //        return BadRequest(new { message = "Ngày không hợp lệ." });
        //    }

        //    chamCongDto.NhanvienId = chamCongDto.NhanvienId?.Trim();

        //    if (string.IsNullOrEmpty(chamCongDto.NhanvienId))
        //    {
        //        return BadRequest(new { message = "NhanvienId không được để trống!" });
        //    }

        //    bool nhanvienExists = await _context.NhanViens
        //        .AnyAsync(nv => nv.NhanvienId == chamCongDto.NhanvienId);

        //    if (!nhanvienExists)
        //    {
        //        return BadRequest(new { message = $"Nhân viên với ID {chamCongDto.NhanvienId} không tồn tại!" });
        //    }

        //    bool exists = await _context.ChamCongs
        //        .AnyAsync(cc => cc.NhanvienId == chamCongDto.NhanvienId && cc.Ngay == ngay);

        //    if (exists)
        //    {
        //        return BadRequest(new { message = $"Nhân viên {chamCongDto.NhanvienId} đã có dữ liệu chấm công ngày {ngay}!" });
        //    }

        //    // Kiểm tra giờ vào
        //    if (string.IsNullOrWhiteSpace(chamCongDto.Giovao))
        //    {
        //        return BadRequest(new { message = "Giờ vào không được để trống." });
        //    }

        //    if (!TimeOnly.TryParse(chamCongDto.Giovao, out TimeOnly giovao))
        //    {
        //        return BadRequest(new { message = "Giờ vào không hợp lệ." });
        //    }

        //    // Kiểm tra giờ ra
        //    if (string.IsNullOrWhiteSpace(chamCongDto.Giora))
        //    {
        //        return BadRequest(new { message = "Giờ ra không được để trống." });
        //    }

        //    if (!TimeOnly.TryParse(chamCongDto.Giora, out TimeOnly giora))
        //    {
        //        return BadRequest(new { message = "Giờ ra không hợp lệ." });
        //    }

        //    string dimuon = giovao > new TimeOnly(8, 0, 0) ? "Y" : "N";
        //    string vesom = giora < new TimeOnly(17, 0, 0) ? "Y" : "N";

        //    var chamCong = new ChamCong
        //    {
        //        NhanvienId = chamCongDto.NhanvienId,
        //        Ngay = ngay,
        //        Giovao = giovao,
        //        Giora = giora,
        //        Dimuon = dimuon,
        //        Vesom = vesom
        //    };

        //    _context.ChamCongs.Add(chamCong);
        //    await _context.SaveChangesAsync();
        //    await CapNhatKhoanTru();

        //    // Đồng bộ với API Python
        //    var pythonJsonData = new
        //    {
        //        NhanvienId = chamCong.NhanvienId,
        //        Ngay = chamCong.Ngay.HasValue
        //            ? chamCong.Ngay.Value.ToDateTime(new TimeOnly(0, 0, 0))
        //            : (DateTime?)null,
        //        Giovao = chamCong.Giovao?.ToString("HH:mm:ss"),
        //        Giora = chamCong.Giora?.ToString("HH:mm:ss")
        //    };

        //    var jsonContent = new StringContent(
        //        JsonSerializer.Serialize(pythonJsonData),
        //        Encoding.UTF8,
        //        "application/json"
        //    );

        //    using (var client = new HttpClient())
        //    {
        //        var response = await client.PostAsync("http://127.0.0.1:5001/api/attendance", jsonContent);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("Dữ liệu đã được đồng bộ với API Python.");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Đồng bộ với API Python không thành công.");
        //        }
        //    }

        //    return CreatedAtAction(nameof(GetChamCongs), new { id = chamCong.ChamcongId }, chamCong);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutChamCong(int id, [FromBody] ChamCongDto chamCongDto)
        //{
        //    if (chamCongDto == null)
        //    {
        //        return BadRequest(new { message = "Dữ liệu không hợp lệ." });
        //    }

        //    // Tìm bản ghi trong CSDL bằng ChamcongId
        //    var existingChamCong = await _context.ChamCongs
        //        .FirstOrDefaultAsync(x => x.ChamcongId == id);

        //    if (existingChamCong == null)
        //    {
        //        return NotFound(new { message = "Không tìm thấy bản ghi với ID này." });
        //    }

        //    // Kiểm tra và cập nhật giờ vào nếu có giá trị hợp lệ
        //    if (!string.IsNullOrEmpty(chamCongDto.Giovao) && TimeOnly.TryParse(chamCongDto.Giovao, out var giovao))
        //    {
        //        existingChamCong.Giovao = giovao;
        //    }

        //    // Kiểm tra và cập nhật giờ ra nếu có giá trị hợp lệ
        //    if (!string.IsNullOrEmpty(chamCongDto.Giora) && TimeOnly.TryParse(chamCongDto.Giora, out var giora))
        //    {
        //        existingChamCong.Giora = giora;
        //    }

        //    // Cập nhật đi muộn và về sớm nếu giờ vào/ra có thay đổi
        //    existingChamCong.Dimuon = existingChamCong.Giovao.HasValue && existingChamCong.Giovao.Value > new TimeOnly(8, 0, 0) ? "Y" : "N";
        //    existingChamCong.Vesom = existingChamCong.Giora.HasValue && existingChamCong.Giora.Value < new TimeOnly(17, 0, 0) ? "Y" : "N";

        //    try
        //    {
        //        // Cập nhật bản ghi vào cơ sở dữ liệu
        //        await _context.SaveChangesAsync();
        //        await CapNhatKhoanTru();  // Cập nhật các khoản trừ

        //        // Gửi dữ liệu cập nhật đến Python API
        //        var pythonJsonData = new
        //        {
        //            NhanvienId = existingChamCong.NhanvienId,
        //            Ngay = existingChamCong.Ngay.Value.ToDateTime(new TimeOnly(0, 0, 0)).ToString("yyyy-MM-dd"),
        //            Giovao = existingChamCong.Giovao?.ToString("HH:mm:ss"),
        //            Giora = existingChamCong.Giora?.ToString("HH:mm:ss")
        //        };

        //        var jsonContent = new StringContent(
        //            JsonSerializer.Serialize(pythonJsonData),
        //            Encoding.UTF8,
        //            "application/json"
        //        );

        //        using var client = new HttpClient();
        //        var response = await client.PostAsync("http://127.0.0.1:5001/api/attendance", jsonContent);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("Đồng bộ với API Python không thành công.");
        //            return StatusCode(500, new { message = "Đồng bộ với API Python không thành công." });
        //        }

        //        return Ok(new { message = "Cập nhật thành công và đồng bộ với API Python!", data = existingChamCong });
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return StatusCode(500, new { message = "Lỗi khi cập nhật dữ liệu." });
        //    }
        //}
    }
}

