using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhanSu.MyModels;

//khác với KhoanTru (mỗi tháng có thể 1 nhân viên có nhiều hơn 1 khoản trừ), thì thưởng và phụ cấp chỉ 1 lần/1 tháng/1 nhân viên
namespace QLNhanSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "QL")]
    public class CtNhanvienThuongQLController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public CtNhanvienThuongQLController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/CtNhanvienThuong
        [HttpGet]
        public async Task<ActionResult> GetCtNhanvienThuongs()
        {
            if (_context.CtNhanvienThuongs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienThuongs
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join t in _context.Thuongs on ct.ThuongId equals t.ThuongId
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            t.ThuongId,
                            t.Loaithuong, // Hiển thị tên loại thưởng thay vì ID
                            ct.Thoigian
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không có khoản thưởng nào cho nhân viên!",
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
        public async Task<ActionResult> TimKiemCtNhanvienThuong(string s)
        {
            if (_context.CtNhanvienThuongs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienThuongs
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join t in _context.Thuongs on ct.ThuongId equals t.ThuongId
                        where nv.Tennhanvien.Contains(s) ||
                              t.Loaithuong.Contains(s) ||
                              ct.NhanvienId.Contains(s) ||
                              ct.Thoigian.ToString().Contains(s) ||
                              t.ThuongId.ToString().Contains(s)
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            t.ThuongId,
                            t.Loaithuong, // Hiển thị tên loại thưởng thay vì ID
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
        //// PUT: api/CtNhanvienThuong/5
        //[HttpPut("{id}/{thoigian}")]
        //public async Task<IActionResult> PutCtNhanvienThuong(string id, DateOnly thoigian, CtNhanvienThuongDto ctNhanvienThuongDto)
        //{
        //    if (id != ctNhanvienThuongDto.NhanvienId)
        //    {
        //        return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
        //    }

        //    // Tìm khoản thưởng trong cơ sở dữ liệu
        //    var ctNhanvienThuong = await _context.CtNhanvienThuongs
        //        .FirstOrDefaultAsync(ct => ct.NhanvienId == id && ct.Thoigian == thoigian);

        //    if (ctNhanvienThuong == null)
        //    {
        //        return NotFound($"Khoản thưởng của nhân viên với ID {id} vào ngày {thoigian} không tồn tại.");
        //    }

        //    // Cập nhật dữ liệu
        //    ctNhanvienThuong.ThuongId = ctNhanvienThuongDto.ThuongId;

        //    // Đánh dấu entity là đã thay đổi
        //    _context.Entry(ctNhanvienThuong).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CtNhanvienThuongExists(id, thoigian))
        //        {
        //            return NotFound($"Khoản thưởng của nhân viên với ID {id} vào ngày {thoigian} không tồn tại.");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        private bool CtNhanvienThuongExists(string nhanvienid, DateOnly thoigian)
        {
            return _context.CtNhanvienThuongs.Any(e => e.NhanvienId == nhanvienid && e.Thoigian == thoigian);
        }



        // POST: api/CtNhanvienThuong
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostCtNhanvienThuong(CtNhanvienThuongDto ctNhanvienThuongDto)
        {
            if (ctNhanvienThuongDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var month = ctNhanvienThuongDto.Thoigian.Month;
            var year = ctNhanvienThuongDto.Thoigian.Year;

            try
            {
                var existingLuong = await _context.Luongs
                    .FirstOrDefaultAsync(l => l.NhanvienId == ctNhanvienThuongDto.NhanvienId
                                              && l.Thoigian.Month == month
                                              && l.Thoigian.Year == year);

                if (existingLuong == null)
                {
                    _context.Luongs.Add(new Luong
                    {
                        NhanvienId = ctNhanvienThuongDto.NhanvienId,
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

            var ctNhanvienThuong = new CtNhanvienThuong
            {
                NhanvienId = ctNhanvienThuongDto.NhanvienId,
                ThuongId = ctNhanvienThuongDto.ThuongId,
                Thoigian = ctNhanvienThuongDto.Thoigian
            };

            _context.CtNhanvienThuongs.Add(ctNhanvienThuong);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm thưởng thành công!" });
        }


        [HttpPost("AddByPhongBan/{phongbanId}")]
        public async Task<ActionResult> PostThuongByPhongBan(string phongbanId, string thuongId, DateOnly thoigian)
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
                    if (_context.CtNhanvienThuongs.Any(e => e.NhanvienId == nv.NhanvienId && e.Thoigian == thoigian))
                    {
                        return Conflict($"Khoản thưởng của nhân viên {nv.NhanvienId} vào ngày {thoigian} đã tồn tại.");
                    }
                    _context.CtNhanvienThuongs.Add(new CtNhanvienThuong { NhanvienId = nv.NhanvienId, ThuongId = thuongId, Thoigian = thoigian });
                }
                await _context.SaveChangesAsync();
                return Ok(new { message = "Thêm thưởng theo phòng ban thành công!", status = 201 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm thưởng theo phòng ban", error = ex.Message });
            }
        }

        //// DELETE: api/CtNhanvienThuong/5
        //[HttpDelete("{id}/{thoigian}")]
        //public async Task<IActionResult> DeleteCtNhanvienThuong(string id, DateOnly thoigian)
        //{
        //    var ctNhanvienThuong = await _context.CtNhanvienThuongs.FirstOrDefaultAsync(e => e.NhanvienId == id && e.Thoigian == thoigian);
        //    if (ctNhanvienThuong == null)
        //    {
        //        return NotFound($"Khoản thưởng của nhân viên với ID {id} vào ngày {thoigian} không tồn tại.");
        //    }

        //    _context.CtNhanvienThuongs.Remove(ctNhanvienThuong);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        return StatusCode(500, $"Lỗi khi xóa khoản thưởng: {ex.Message}");
        //    }

        //    return NoContent();
        //}

        [HttpPut("{nhanvienId}/{thoigian}")]
        public async Task<IActionResult> PutCtNhanvienThuong(
            string nhanvienId,
            DateOnly thoigian,
            [FromBody] CtNhanvienThuongDto ctNhanvienThuongDto)
        {
            if (ctNhanvienThuongDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // 🔹 Tìm bản ghi cần cập nhật
            var existingRecord = await _context.CtNhanvienThuongs
                .FirstOrDefaultAsync(ct => ct.NhanvienId == nhanvienId && ct.Thoigian == thoigian);

            if (existingRecord == null)
            {
                return NotFound($"Không tìm thấy thưởng của nhân viên {nhanvienId} vào ngày {thoigian}.");
            }

            // 🔥 Bước 1: Xóa bản ghi cũ
            _context.CtNhanvienThuongs.Remove(existingRecord);
            await _context.SaveChangesAsync();

            // 🔥 Bước 2: Thêm bản ghi mới với `ThuongId` được cập nhật
            var newRecord = new CtNhanvienThuong
            {
                NhanvienId = nhanvienId,
                Thoigian = thoigian,
                ThuongId = ctNhanvienThuongDto.ThuongId  // ✅ ThuongId mới
            };

            _context.CtNhanvienThuongs.Add(newRecord);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thưởng thành công!" });
        }

        [HttpDelete("{nhanvienId}/{thoigian}")]
        public async Task<IActionResult> DeleteCtNhanvienThuong(string nhanvienId, DateOnly thoigian)
        {
            if (string.IsNullOrWhiteSpace(nhanvienId))
            {
                return BadRequest("Mã nhân viên không được để trống!");
            }

            // 🔹 Tìm bản ghi cần xóa
            var ctNhanvienThuong = await _context.CtNhanvienThuongs
                .FirstOrDefaultAsync(e => e.NhanvienId == nhanvienId && e.Thoigian == thoigian);

            if (ctNhanvienThuong == null)
            {
                return NotFound($"Không tìm thấy khoản thưởng của nhân viên {nhanvienId} vào ngày {thoigian}.");
            }

            // 🔥 Xóa bản ghi
            _context.CtNhanvienThuongs.Remove(ctNhanvienThuong);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa khoản thưởng thành công!" });
        }
    }
}
