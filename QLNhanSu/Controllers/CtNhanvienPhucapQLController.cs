using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhanSu.MyModels;

namespace QLNhanSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "QL")]
    public class CtNhanvienPhucapQLController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public CtNhanvienPhucapQLController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/CtNhanvienPhucap
        [HttpGet]
        public async Task<ActionResult> GetCtNhanvienPhucaps()
        {
            if (_context.CtNhanvienPhucaps == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienPhucaps
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join pc in _context.PhuCaps on ct.PhucapId equals pc.PhucapId
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            pc.PhucapId,
                            pc.Loaiphucap, // Hiển thị tên phụ cấp thay vì ID
                            ct.Thoigian
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không có phụ cấp nào cho nhân viên!",
                    status = 404
                });
            }

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            });
        }

        [HttpGet]
        [Route("Search")]
        public async Task<ActionResult> TimKiemCtNhanvienPhucap(string s)
        {
            if (_context.CtNhanvienPhucaps == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienPhucaps
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join t in _context.PhuCaps on ct.PhucapId equals t.PhucapId
                        where nv.Tennhanvien.Contains(s) ||
                              t.Loaiphucap.Contains(s) ||
                              ct.NhanvienId.Contains(s) ||
                              ct.Thoigian.ToString().Contains(s) ||
                              t.PhucapId.ToString().Contains(s)
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            t.PhucapId,
                            t.Loaiphucap, // Hiển thị tên loại thưởng thay vì ID
                            ct.Thoigian
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không tìm thấy kết quả phù hợp!",
                    status = 404
                });
            }

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            });
        }


        [HttpPost]
        public async Task<ActionResult> PostCtNhanvienPhucap(CtNhanvienPhucapDto ctNhanvienPhucapDto)
        {
            if (ctNhanvienPhucapDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var month = ctNhanvienPhucapDto.Thoigian.Month;
            var year = ctNhanvienPhucapDto.Thoigian.Year;

            try
            {
                var existingLuong = await _context.Luongs
                    .FirstOrDefaultAsync(l => l.NhanvienId == ctNhanvienPhucapDto.NhanvienId
                                              && l.Thoigian.Month == month
                                              && l.Thoigian.Year == year);

                if (existingLuong == null)
                {
                    _context.Luongs.Add(new Luong
                    {
                        NhanvienId = ctNhanvienPhucapDto.NhanvienId,
                        Thoigian = new DateOnly(year, month, 1),
                        Tongluong = 0
                    });
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException)
            {
                Console.WriteLine($"⚠️ Lương đã tồn tại, bỏ qua tạo mới.");
            }

            var ctNhanvienPhucap = new CtNhanvienPhucap
            {
                NhanvienId = ctNhanvienPhucapDto.NhanvienId,
                PhucapId = ctNhanvienPhucapDto.PhucapId,
                Thoigian = ctNhanvienPhucapDto.Thoigian
            };

            _context.CtNhanvienPhucaps.Add(ctNhanvienPhucap);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm phụ cấp thành công!" });
        }


        [HttpPost("AddByPhongBan/{phongbanId}")]
        public async Task<ActionResult> PostPhuCapByPhongBan(string phongbanId, string phucapId, DateOnly thoigian)
        {
            try
            {
                var nhanviens = await _context.NhanViens.Where(nv => nv.PhongbanId == phongbanId).ToListAsync();
                if (!nhanviens.Any())
                {
                    return NotFound("Không tìm thấy nhân viên nào trong phòng ban này.");
                }

                foreach (var nv in nhanviens)
                {
                    if (!CtNhanvienPhucapExists(nv.NhanvienId, thoigian))
                    {
                        _context.CtNhanvienPhucaps.Add(new CtNhanvienPhucap { NhanvienId = nv.NhanvienId, PhucapId = phucapId, Thoigian = thoigian });
                    }
                }
                await _context.SaveChangesAsync();
                return Ok(new { message = "Thêm phụ cấp theo phòng ban thành công!", status = 201 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm phụ cấp theo phòng ban", error = ex.Message });
            }
        }


        //[HttpPut("{id}/{thoigian}")]
        //public async Task<IActionResult> PutCtNhanvienPhucap(string id, DateOnly thoigian, CtNhanvienPhucapDto ctNhanvienPhucapDto)
        //{
        //    if (id != ctNhanvienPhucapDto.NhanvienId)
        //    {
        //        return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
        //    }

        //    var entity = await _context.CtNhanvienPhucaps.FirstOrDefaultAsync(ct => ct.NhanvienId == id && ct.Thoigian == thoigian);
        //    if (entity == null)
        //    {
        //        return NotFound($"Không tìm thấy phụ cấp của nhân viên {id} vào ngày {thoigian}.");
        //    }

        //    entity.PhucapId = ctNhanvienPhucapDto.PhucapId;
        //    _context.Entry(entity).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CtNhanvienPhucapExists(id, thoigian))
        //        {
        //            return NotFound($"Khoản phụ cấp của nhân viên với ID {id} không tồn tại.");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPut("{nhanvienId}/{thoigian}")]
        public async Task<IActionResult> PutCtNhanvienPhucap(
            string nhanvienId,
            DateOnly thoigian,
            [FromBody] CtNhanvienPhucapDto ctNhanvienPhucapDto)
        {
            if (ctNhanvienPhucapDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // 🔹 Tìm bản ghi cần cập nhật
            var existingRecord = await _context.CtNhanvienPhucaps
                .FirstOrDefaultAsync(ct => ct.NhanvienId == nhanvienId && ct.Thoigian == thoigian);

            if (existingRecord == null)
            {
                return NotFound($"Không tìm thấy phụ cấp của nhân viên {nhanvienId} vào ngày {thoigian}.");
            }

            // 🔥 Bước 1: Xóa bản ghi cũ
            _context.CtNhanvienPhucaps.Remove(existingRecord);
            await _context.SaveChangesAsync();

            // 🔥 Bước 2: Thêm bản ghi mới với `PhucapId` được cập nhật
            var newRecord = new CtNhanvienPhucap
            {
                NhanvienId = nhanvienId,
                Thoigian = thoigian,
                PhucapId = ctNhanvienPhucapDto.PhucapId  // ✅ PhucapId mới
            };

            _context.CtNhanvienPhucaps.Add(newRecord);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật phụ cấp thành công!" });
        }


        //[HttpDelete("{id}/{thoigian}")]
        //public async Task<IActionResult> DeleteCtNhanvienPhucap(string id, DateOnly thoigian)
        //{
        //    var ctNhanvienPhucap = await _context.CtNhanvienPhucaps.FirstOrDefaultAsync(e => e.NhanvienId == id && e.Thoigian == thoigian);
        //    if (ctNhanvienPhucap == null)
        //    {
        //        return NotFound($"Khoản phụ cấp của nhân viên với ID {id} vào ngày {thoigian} không tồn tại.");
        //    }

        //    _context.CtNhanvienPhucaps.Remove(ctNhanvienPhucap);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        return StatusCode(500, $"Lỗi khi xóa khoản phụ cấp: {ex.Message}");
        //    }

        //    return NoContent();
        //}

        [HttpDelete("{nhanvienId}/{thoigian}")]
        public async Task<IActionResult> DeleteCtNhanvienPhucap(string nhanvienId, DateOnly thoigian)
        {
            if (string.IsNullOrWhiteSpace(nhanvienId))
            {
                return BadRequest("Mã nhân viên không được để trống!");
            }

            // 🔹 Tìm bản ghi cần xóa
            var ctNhanvienPhucap = await _context.CtNhanvienPhucaps
                .FirstOrDefaultAsync(e => e.NhanvienId == nhanvienId && e.Thoigian == thoigian);

            if (ctNhanvienPhucap == null)
            {
                return NotFound($"Không tìm thấy khoản phụ cấp của nhân viên {nhanvienId} vào ngày {thoigian}.");
            }

            // 🔥 Xóa bản ghi
            _context.CtNhanvienPhucaps.Remove(ctNhanvienPhucap);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa khoản phụ cấp thành công!" });
        }


        private bool CtNhanvienPhucapExists(string nhanvienid, DateOnly thoigian)
        {
            return _context.CtNhanvienPhucaps.Any(e => e.NhanvienId == nhanvienid && e.Thoigian == thoigian);
        }
    }
}


