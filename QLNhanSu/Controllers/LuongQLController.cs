using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLNhanSu.MyModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;

namespace QLNhanSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "QL")]
    public class LuongQLController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public LuongQLController(KetNoiCSDL context)
        {
            _context = context;
        }

        //private decimal TinhLuong(NhanVien nhanVien)
        //{
        //    decimal luongCoBan = nhanVien.Luongcoban;

        //    decimal tongThuong = nhanVien.CtNhanvienThuongs?
        //        .Where(t => t.NhanvienId == nhanVien.NhanvienId)
        //        .Sum(t => t.Thuong?.Sotien ?? 0) ?? 0;

        //    decimal tongPhuCap = nhanVien.CtNhanvienPhucaps?
        //        .Where(p => p.NhanvienId == nhanVien.NhanvienId)
        //        .Sum(p => p.Phucap?.Sotien ?? 0) ?? 0;

        //    decimal tongKhoanTru = nhanVien.CtNhanvienKhoantrus?
        //        .Where(k => k.NhanvienId == nhanVien.NhanvienId)
        //        .Sum(k => k.Tru?.Sotien ?? 0) ?? 0;

        //    decimal thue = luongCoBan * 0.1m;

        //    return luongCoBan + tongThuong + tongPhuCap - thue - tongKhoanTru;
        //}

        //[HttpPost]
        //public async Task<ActionResult<LuongDto>> PostLuong()
        //{
        //    try
        //    {
        //        var nhanViens = await _context.NhanViens
        //            .Include(nv => nv.CtNhanvienThuongs).ThenInclude(ct => ct.Thuong)
        //            .Include(nv => nv.CtNhanvienPhucaps).ThenInclude(ct => ct.Phucap)
        //            .Include(nv => nv.CtNhanvienKhoantrus).ThenInclude(ct => ct.Tru)
        //            .ToListAsync();

        //        if (nhanViens == null || !nhanViens.Any())
        //        {
        //            return NotFound("Không có nhân viên nào trong hệ thống.");
        //        }

        //        foreach (var nv in nhanViens)
        //        {
        //            decimal tongLuong = TinhLuong(nv);
        //            var luong = await _context.Luongs.FirstOrDefaultAsync(l => l.NhanvienId == nv.NhanvienId);

        //            if (luong != null)
        //            {
        //                luong.Tongluong = tongLuong;
        //                luong.Thoigian = DateOnly.FromDateTime(DateTime.Now);
        //                _context.Entry(luong).State = EntityState.Modified;
        //            }
        //            else
        //            {
        //                _context.Luongs.Add(new Luong
        //                {
        //                    NhanvienId = nv.NhanvienId,
        //                    Thoigian = DateOnly.FromDateTime(DateTime.Now),
        //                    Tongluong = tongLuong
        //                });
        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //        return Ok("Lương của nhân viên đã được cập nhật thành công.");
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        return Conflict($"Lỗi khi cập nhật lương: {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Lỗi khi lưu dữ liệu: {ex.Message}");
        //    }
        //}


        [HttpGet("Luong/{month}/{year}")]
        public async Task<ActionResult> GetLuongTheoThang(int month, int year)
        {
            var today = DateTime.Today;

            if (year > today.Year || (year == today.Year && month > today.Month))
            {
                return BadRequest(new
                {
                    message = "Chưa tạo bảng lương cho tháng này!",
                    status = 400
                });
            }
            else
            {
                // 🔹 Lấy danh sách nhân viên
                var nhanviens = await _context.NhanViens.ToListAsync();

                // 🔹 Lấy danh sách lương đã có trong tháng
                var luongThang = await _context.Luongs
                    .Where(l => l.Thoigian.Month == month && l.Thoigian.Year == year)
                    .ToListAsync();

                List<Luong> newRecords = new List<Luong>(); // Danh sách lương cần thêm mới

                foreach (var nv in nhanviens)
                {
                    if (!luongThang.Any(l => l.NhanvienId == nv.NhanvienId))
                    {
                        // 🔹 Nếu nhân viên chưa có lương, thêm bản ghi mặc định (0.00)
                        var newLuong = new Luong
                        {
                            Thoigian = new DateOnly(year, month, 1),
                            NhanvienId = nv.NhanvienId,
                            Tongluong = 0.00m // Mặc định 0, sẽ tính toán lại
                        };
                        newRecords.Add(newLuong);
                    }
                }

                if (newRecords.Count > 0)
                {
                    _context.Luongs.AddRange(newRecords);
                    await _context.SaveChangesAsync();
                }

                // 🔹 Lấy danh sách lương sau khi cập nhật
                var luongs = await _context.Luongs
                    .Where(l => l.Thoigian.Month == month && l.Thoigian.Year == year)
                    .Select(l => new
                    {
                        l.NhanvienId,
                        NhanVien = l.Nhanvien.Tennhanvien,
                        PhongBan = l.Nhanvien.Phongban.Tenphongban,
                        Thoigian = l.Thoigian,
                        Luongcoban = l.Nhanvien.Luongcoban,

                        // 🔹 Tổng khoản trừ
                        TongKhoantru = _context.CtNhanvienKhoantrus
                            .Where(k => k.NhanvienId == l.NhanvienId && k.Thoigian.Month == month && k.Thoigian.Year == year)
                            .Sum(k => (decimal?)_context.KhoanTrus
                                .Where(tru => tru.TruId == k.TruId)
                                .Select(tru => tru.Sotien)
                                .FirstOrDefault() ?? 0m),

                        // 🔹 Tổng phụ cấp
                        TongPhucap = _context.CtNhanvienPhucaps
                            .Where(p => p.NhanvienId == l.NhanvienId && p.Thoigian.Month == month && p.Thoigian.Year == year)
                            .Sum(p => (decimal?)_context.PhuCaps
                                .Where(pc => pc.PhucapId == p.PhucapId)
                                .Select(pc => pc.Sotien)
                                .FirstOrDefault() ?? 0m),

                        // 🔹 Tổng thưởng
                        TongThuong = _context.CtNhanvienThuongs
                            .Where(t => t.NhanvienId == l.NhanvienId && t.Thoigian.Month == month && t.Thoigian.Year == year)
                            .Sum(t => (decimal?)_context.Thuongs
                                .Where(th => th.ThuongId == t.ThuongId)
                                .Select(th => th.Sotien)
                                .FirstOrDefault() ?? 0m),

                        // 🔹 Tổng lương tính toán
                        TongLuong = l.Nhanvien.Luongcoban
                                    + (_context.CtNhanvienPhucaps
                                        .Where(p => p.NhanvienId == l.NhanvienId && p.Thoigian.Month == month && p.Thoigian.Year == year)
                                        .Sum(p => (decimal?)_context.PhuCaps
                                            .Where(pc => pc.PhucapId == p.PhucapId)
                                            .Select(pc => pc.Sotien)
                                            .FirstOrDefault() ?? 0m))
                                    + (_context.CtNhanvienThuongs
                                        .Where(t => t.NhanvienId == l.NhanvienId && t.Thoigian.Month == month && t.Thoigian.Year == year)
                                        .Sum(t => (decimal?)_context.Thuongs
                                            .Where(th => th.ThuongId == t.ThuongId)
                                            .Select(th => th.Sotien)
                                            .FirstOrDefault() ?? 0m))
                                    - (_context.CtNhanvienKhoantrus
                                        .Where(k => k.NhanvienId == l.NhanvienId && k.Thoigian.Month == month && k.Thoigian.Year == year)
                                        .Sum(k => (decimal?)_context.KhoanTrus
                                            .Where(tru => tru.TruId == k.TruId)
                                            .Select(tru => tru.Sotien)
                                            .FirstOrDefault() ?? 0m))
                                    - (l.Nhanvien.Luongcoban * 0.1m) // ✅ Khấu trừ 10% thuế
                    })
                    .ToListAsync();
                return Ok(luongs);
            } 
        }


        [HttpGet("Luong/Search")]
        public async Task<ActionResult> SearchLuong(string? phongban_id = null, string? chucvu = null)
        {
            var query = _context.Luongs
                .Include(l => l.Nhanvien)
                    .ThenInclude(nv => nv.Phongban)
                .Include(l => l.Nhanvien.CtNhanvienKhoantrus)
                    .ThenInclude(k => k.Tru)
                .Include(l => l.Nhanvien.CtNhanvienThuongs)
                    .ThenInclude(t => t.Thuong)
                .Include(l => l.Nhanvien.CtNhanvienPhucaps)
                    .ThenInclude(p => p.Phucap)
                .Select(l => new
                {
                    l.NhanvienId,
                    Tennhanvien = l.Nhanvien.Tennhanvien,
                    Tenphongban = l.Nhanvien.Phongban != null ? l.Nhanvien.Phongban.Tenphongban : "Không có phòng ban",
                    PhongbanId = l.Nhanvien.PhongbanId,
                    Chucvu = !string.IsNullOrEmpty(l.Nhanvien.chucvu) ? l.Nhanvien.chucvu : "Không có chức vụ",
                    Thoigian = l.Thoigian,
                    Luongcoban = l.Nhanvien.Luongcoban,

                    // 🔹 Tính tổng khoản trừ
                    TongKhoantru = l.Nhanvien.CtNhanvienKhoantrus
                        .Sum(k => (decimal?)k.Tru.Sotien ?? 0m),

                    // 🔹 Tính tổng thưởng
                    TongThuong = l.Nhanvien.CtNhanvienThuongs
                        .Sum(t => (decimal?)t.Thuong.Sotien ?? 0m),

                    // 🔹 Tính tổng phụ cấp
                    TongPhucap = l.Nhanvien.CtNhanvienPhucaps
                        .Sum(p => (decimal?)p.Phucap.Sotien ?? 0m),

                    // 🔹 Tính tổng lương
                    TongLuong = l.Nhanvien.Luongcoban
                                + (l.Nhanvien.CtNhanvienPhucaps.Sum(p => (decimal?)p.Phucap.Sotien ?? 0m))
                                + (l.Nhanvien.CtNhanvienThuongs.Sum(t => (decimal?)t.Thuong.Sotien ?? 0m))
                                - (l.Nhanvien.CtNhanvienKhoantrus.Sum(k => (decimal?)k.Tru.Sotien ?? 0m))
                                - (l.Nhanvien.Luongcoban * 0.1m) // ✅ Khấu trừ thuế 10%
                });

            // 🔹 Lọc theo phòng ban
            if (!string.IsNullOrEmpty(phongban_id))
            {
                query = query.Where( l => l.PhongbanId == phongban_id);
            }


            // 🔹 Lọc theo chức vụ
            if (!string.IsNullOrEmpty(chucvu))
            {
                query = query.Where(l => l.Chucvu.Contains(chucvu));
            }

            var result = await query.ToListAsync();

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = result
            });
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLuong(int id)
        {
            var luong = await _context.Luongs.FindAsync(id);

            if (luong == null)
            {
                return NotFound($"Lương với ID {id} không tồn tại.");
            }

            _context.Luongs.Remove(luong);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Lỗi khi xóa lương: {ex.Message}");
            }

            return NoContent();
        }

    }
}
