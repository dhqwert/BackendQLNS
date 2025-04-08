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
    [Authorize(Roles = "QL")]
    [Route("api/[controller]")]
    [ApiController]
    public class CtNhanvienKhoantruQLController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public CtNhanvienKhoantruQLController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/CtNhanvienKhoantru
        [HttpGet]
        public async Task<ActionResult> GetCtNhanvienKhoantrus()
        {
            if (_context.CtNhanvienKhoantrus == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienKhoantrus
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join tru in _context.KhoanTrus on ct.TruId equals tru.TruId
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            tru.TruId,
                            tru.Loaikhoantru, // Hiển thị tên khoản trừ thay vì ID
                            ct.Thoigian
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không có khoản trừ nào cho nhân viên!",
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
        public async Task<ActionResult> TimKiemCtNhanvienKhoantru(string s)
        {
            if (_context.CtNhanvienKhoantrus == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from ct in _context.CtNhanvienKhoantrus
                        join nv in _context.NhanViens on ct.NhanvienId equals nv.NhanvienId
                        join tru in _context.KhoanTrus on ct.TruId equals tru.TruId
                        where nv.Tennhanvien.Contains(s) ||
                              tru.Loaikhoantru.Contains(s) ||
                              ct.NhanvienId.Contains(s) ||
                              ct.Thoigian.ToString().Contains(s) ||
                              tru.TruId.ToString().Contains(s)
                        orderby ct.Thoigian descending
                        select new
                        {
                            ct.NhanvienId,
                            nv.Tennhanvien,
                            tru.TruId,
                            tru.Loaikhoantru, 
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

        //// PUT: api/CtNhanvienKhoantru/5
        //[HttpPut("{id}/{thoigian}/{truid}")]
        //public async Task<IActionResult> PutCtNhanvienKhoantru(string id, DateOnly thoigian, string truid, CtNhanvienKhoantruDto ctNhanvienKhoantruDto)
        //{
        //    // Kiểm tra nếu ID trong URL không khớp với ID trong dữ liệu
        //    if (id != ctNhanvienKhoantruDto.NhanvienId)
        //    {
        //        return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
        //    }

        //    //if (!DateOnly.TryParse(thoigian, out DateOnly date))
        //    //{
        //    //    return BadRequest("Ngày tháng không hợp lệ.");
        //    //}

        //    // Tìm khoản trừ nhân viên theo ID + thoi gian + truid
        //    var ctNhanvienKhoantru = await _context.CtNhanvienKhoantrus.FirstOrDefaultAsync(ct => ct.NhanvienId == id && ct.Thoigian == thoigian && ct.TruId == truid);

        //    // Nếu không tìm thấy, trả về lỗi NotFound
        //    if (ctNhanvienKhoantru == null)
        //    {
        //        return NotFound($"Khoản trừ {truid} cua nhân viên với ID {id} vào ngày {thoigian} không tồn tại");
        //    }

        //    // Cập nhật thông tin khoản trừ từ DTO
        //    ctNhanvienKhoantru.TruId = ctNhanvienKhoantruDto.TruId;
        //    ctNhanvienKhoantru.Thoigian = ctNhanvienKhoantruDto.Thoigian;

        //    // Đánh dấu trạng thái của entity là Modified để cập nhật
        //    _context.Entry(ctNhanvienKhoantru).State = EntityState.Modified;

        //    try
        //    {
        //        // Lưu thay đổi vào cơ sở dữ liệu
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        // Kiểm tra xem có bị lỗi khi đồng bộ dữ liệu không
        //        if (!CtNhanvienKhoantruExists(id))
        //        {
        //            return NotFound($"Khoản trừ của nhân viên với ID {id} không tồn tại.");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    // Trả về thành công mà không có nội dung (NoContent)
        //    return NoContent();
        //}

        [HttpPut("{nhanvienId}/{thoigian}/{truid}")]
        public async Task<IActionResult> PutCtNhanvienKhoantru(
            string nhanvienId,
            DateOnly thoigian,
            string truid,
            [FromBody] CtNhanvienKhoantruDto ctNhanvienKhoantruDto)
        {
            if (ctNhanvienKhoantruDto == null)
            {
                return BadRequest("Dữ liệu khoản trừ không hợp lệ.");
            }

            // 🔹 Tìm bản ghi cần cập nhật
            var existingRecord = await _context.CtNhanvienKhoantrus
                .FirstOrDefaultAsync(ct => ct.NhanvienId == nhanvienId && ct.Thoigian == thoigian && ct.TruId == truid);

            if (existingRecord == null)
            {
                return NotFound($"Không tìm thấy khoản trừ của nhân viên {nhanvienId} vào ngày {thoigian}.");
            }

            // 🔥 Bước 1: Xóa bản ghi cũ
            _context.CtNhanvienKhoantrus.Remove(existingRecord);
            await _context.SaveChangesAsync();

            // 🔥 Bước 2: Thêm bản ghi mới với `TruId` được cập nhật
            var newRecord = new CtNhanvienKhoantru
            {
                NhanvienId = nhanvienId,
                Thoigian = thoigian,
                TruId = ctNhanvienKhoantruDto.TruId  // ✅ Truid mới
            };

            _context.CtNhanvienKhoantrus.Add(newRecord);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật khoản trừ thành công!" });
        }



        // ✅ Kiểm tra khoản trừ có tồn tại không
        //private bool CtNhanvienKhoantruExists(string nhanvienId, DateOnly thoigian, string truid)
        //{
        //    return _context.CtNhanvienKhoantrus.Any(ct => ct.NhanvienId == nhanvienId && ct.Thoigian == thoigian && ct.TruId == truid);
        //}


        [HttpPost]
        public async Task<ActionResult> PostCtNhanvienKhoantru(CtNhanvienKhoantruDto ctNhanvienKhoantruDto)
        {
            if (ctNhanvienKhoantruDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var ngay = ctNhanvienKhoantruDto.Thoigian; // Lưu cả ngày
            var month = ngay.Month;
            var year = ngay.Year;

            try
            {
                var existingLuong = await _context.Luongs
                    .FirstOrDefaultAsync(l => l.NhanvienId == ctNhanvienKhoantruDto.NhanvienId
                                              && l.Thoigian.Month == month
                                              && l.Thoigian.Year == year);

                if (existingLuong == null)
                {
                    _context.Luongs.Add(new Luong
                    {
                        NhanvienId = ctNhanvienKhoantruDto.NhanvienId,
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

            var ctNhanvienKhoantru = new CtNhanvienKhoantru
            {
                NhanvienId = ctNhanvienKhoantruDto.NhanvienId,
                TruId = ctNhanvienKhoantruDto.TruId,
                Thoigian = ngay
            };

            _context.CtNhanvienKhoantrus.Add(ctNhanvienKhoantru);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm khoản trừ thành công!" });
        }



        //[HttpDelete("{id}/{thoigian}/{truid}")]
        //public async Task<IActionResult> DeleteCtNhanvienKhoantru(string id, DateOnly thoigian, string truid)
        //{
        //    //if (!DateOnly.TryParse(thoigian, out DateOnly date))
        //    //{
        //    //    return BadRequest("Ngày tháng không hợp lệ.");
        //    //}
        //    var ctNhanvienKhoantru = await _context.CtNhanvienKhoantrus.FirstOrDefaultAsync(e => e.NhanvienId == id && e.Thoigian == thoigian && e.TruId == truid);
        //    if (ctNhanvienKhoantru == null)
        //    {
        //        return NotFound($"Khoản trừ của nhân viên với ID {id}, ngày {thoigian} và mã khoản trừ {truid} không tồn tại.");
        //    }

        //    _context.CtNhanvienKhoantrus.Remove(ctNhanvienKhoantru);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi xóa khoản trừ: {ex.Message}");
        //    }

        //    return NoContent();
        //}

        // DELETE: api/CtNhanvienKhoantru/NV123/2025-03-22/0003
        [HttpDelete("{nhanvienId}/{thoigian}/{truid}")]
        public async Task<IActionResult> DeleteCtNhanvienKhoantru(string nhanvienId, string thoigian, string truid)
        {
            // ✅ Kiểm tra định dạng ngày
            if (!DateOnly.TryParse(thoigian, out DateOnly parsedThoigian))
            {
                return BadRequest("Ngày tháng không hợp lệ.");
            }

            // ✅ Tìm khoản trừ nhân viên theo ID + thời gian + TruId
            var ctNhanvienKhoantru = await _context.CtNhanvienKhoantrus
                .FirstOrDefaultAsync(e => e.NhanvienId == nhanvienId && e.Thoigian == parsedThoigian && e.TruId == truid);

            // 🔹 Nếu không tìm thấy, trả về lỗi NotFound
            if (ctNhanvienKhoantru == null)
            {
                return NotFound($"Khoản trừ {truid} của nhân viên {nhanvienId} vào ngày {thoigian} không tồn tại.");
            }

            // ✅ Xóa khoản trừ
            _context.CtNhanvienKhoantrus.Remove(ctNhanvienKhoantru);

            try
            {
                // 🔹 Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Lỗi khi xóa khoản trừ: {ex.Message}");
            }

            return NoContent();
        }


        //[HttpPost("AutoUpdateKhoanTru")]
        //public async Task<ActionResult> AutoPostCtNhanvienKhoantru(ChamCongDto chamCongDto)
        //{
        //    DateOnly ngay = DateOnly.Parse(chamCongDto.Ngay);

        //    if (chamCongDto.Dimuon == "Y")
        //    {
        //        var exist = await _context.CtNhanvienKhoantrus.AnyAsync(ct => ct.NhanvienId == chamCongDto.NhanvienId && ct.TruId == "0001" && ct.Thoigian == ngay);
        //        if (!exist)
        //        {
        //            _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
        //            {
        //                NhanvienId = chamCongDto.NhanvienId,
        //                TruId = "0001", // Mã khoản trừ cho đi muộn
        //                Thoigian = ngay
        //            });
        //        }
        //    }
        //    else
        //    {
        //        var khoanTru = await _context.CtNhanvienKhoantrus.FirstOrDefaultAsync(ct => ct.NhanvienId == chamCongDto.NhanvienId && ct.TruId == "0001" && ct.Thoigian == ngay);
        //        if (khoanTru != null)
        //        {
        //            _context.CtNhanvienKhoantrus.Remove(khoanTru);
        //        }
        //    }

        //    if (chamCongDto.Vesom == "Y")
        //    {
        //        var exist = await _context.CtNhanvienKhoantrus.AnyAsync(ct => ct.NhanvienId == chamCongDto.NhanvienId && ct.TruId == "0002" && ct.Thoigian == ngay);
        //        if (!exist)
        //        {
        //            _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
        //            {
        //                NhanvienId = chamCongDto.NhanvienId,
        //                TruId = "0002", // Mã khoản trừ cho về sớm
        //                Thoigian = ngay
        //            });
        //        }
        //    }
        //    else
        //    {
        //        var khoanTru = await _context.CtNhanvienKhoantrus.FirstOrDefaultAsync(ct => ct.NhanvienId == chamCongDto.NhanvienId && ct.TruId == "0002" && ct.Thoigian == ngay);
        //        if (khoanTru != null)
        //        {
        //            _context.CtNhanvienKhoantrus.Remove(khoanTru);
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok(new { message = "Khoản trừ đã được cập nhật tự động!" });
        //}

        private bool CtNhanvienKhoantruExists(string nhanvienId, DateOnly thoigian, string truid)
        {
            return _context.CtNhanvienKhoantrus.Any(ct => ct.NhanvienId == nhanvienId && ct.Thoigian == thoigian && ct.TruId == truid);
        }

    }
}
