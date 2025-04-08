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
    public class PhongBanQLController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public PhongBanQLController(KetNoiCSDL context)
        {
            _context = context;
        }

        //// GET: api/PhongBan 
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<PhongBanDto>>> GetPhongBans()
        //{
        //    try
        //    {
        //        var phongBans = await _context.PhongBans
        //            .Select(p => new PhongBanDto
        //            {
        //                PhongbanId = p.PhongbanId,
        //                Tenphongban = p.Tenphongban,
        //                SoLuongNhanVien = _context.NhanViens.Count(nv => nv.PhongbanId == p.PhongbanId)
        //            }).ToListAsync();

        //        if (!phongBans.Any())
        //        {
        //            return NotFound("Không có phòng ban nào trong hệ thống.");
        //        }

        //        return Ok(phongBans);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Lỗi khi truy xuất dữ liệu: {ex.Message}");
        //    }
        //}

        // GET: api/PhongBan 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhongBanDto>>> GetPhongBans()
        {
            try
            {
                var phongBans = await _context.PhongBans
                    .Select(p => new PhongBanDto
                    {
                        PhongbanId = p.PhongbanId,
                        Tenphongban = p.Tenphongban,
                        SoLuongNhanVien = _context.NhanViens
                            .Count(nv => nv.PhongbanId == p.PhongbanId && nv.trangthailv == "Dang lam") // 🔹 Chỉ đếm nhân viên đang làm
                    }).ToListAsync();

                if (!phongBans.Any())
                {
                    return NotFound("Không có phòng ban nào trong hệ thống.");
                }

                return Ok(phongBans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi truy xuất dữ liệu: {ex.Message}");
            }
        }

        // GET: api/PhongBan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhongBanDto>> GetPhongBan(string id)
        {
            try
            {
                // Lấy phòng ban theo ID từ cơ sở dữ liệu và chuyển thành DTO
                var phongBan = await _context.PhongBans
                    .Where(p => p.PhongbanId == id)
                    .Select(p => new PhongBanDto
                    {
                        PhongbanId = p.PhongbanId,
                        Tenphongban = p.Tenphongban,
                        SoLuongNhanVien = _context.NhanViens
                           .Count(nv => nv.PhongbanId == p.PhongbanId && nv.trangthailv == "Dang lam") // 🔹 Chỉ đếm nhân viên đang làm
                    })
                    .FirstOrDefaultAsync();

                // Nếu không tìm thấy phòng ban với ID cho trước, trả về NotFound
                if (phongBan == null)
                {
                    return NotFound($"Không tìm thấy phòng ban với ID: {id}");
                }

                return Ok(phongBan); // Trả về phòng ban dưới dạng DTO
            }
            catch (Exception ex)
            {
                // Trả về lỗi 500 khi có sự cố khi truy vấn dữ liệu
                return StatusCode(500, $"Lỗi khi truy xuất dữ liệu: {ex.Message}");
            }
        }

        // GET: api/PhongBan/Suggest/{keyword}
        [HttpGet("Suggest/{keyword}")]
        public async Task<ActionResult> SuggestPhongBan(string keyword)
        {
            if (_context.PhongBans == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = _context.PhongBans
                .Where(pb => pb.Tenphongban.Contains(keyword)) // Lọc tên có chứa từ khóa
                .Select(pb => new
                {
                    pb.PhongbanId,
                    pb.Tenphongban,
                    SoLuongNhanVien = _context.NhanViens.Count(nv => nv.PhongbanId == pb.PhongbanId)
                })
                .Take(10) // Giới hạn số lượng kết quả
                .ToList();

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = $"Không tìm thấy phòng ban nào với từ khóa '{keyword}'!",
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


        // PUT: api/PhongBan/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhongBan(string id, PhongBanDto phongBanDto)
        {
            // Kiểm tra ID trong URL có khớp với ID trong DTO không
            if (id != phongBanDto.PhongbanId)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            // Tìm phòng ban cần cập nhật
            var phongBan = await _context.PhongBans.FindAsync(id);
            if (phongBan == null)
            {
                return NotFound($"Không tìm thấy phòng ban với ID: {id}");
            }

            // Cập nhật thông tin phòng ban từ DTO
            phongBan.Tenphongban = phongBanDto.Tenphongban;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Nếu có lỗi trong quá trình cập nhật (do concurrency), kiểm tra lại xem phòng ban có tồn tại không
                if (!PhongBanExists(id))
                {
                    return NotFound($"Phòng ban với ID {id} không tồn tại.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khác (ví dụ lỗi cơ sở dữ liệu)
                return StatusCode(500, $"Lỗi khi cập nhật dữ liệu: {ex.Message}");
            }

            // Trả về NoContent khi cập nhật thành công (HTTP 204)
            return NoContent();
        }


        // POST: api/PhongBan
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhongBan>> PostPhongBan(PhongBanDto phongBanDto)
        {
            // Chuyển đổi từ DTO sang entity PhongBan
            var phongBan = new PhongBan
            {
                PhongbanId = phongBanDto.PhongbanId,
                Tenphongban = phongBanDto.Tenphongban
            };

            _context.PhongBans.Add(phongBan);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Nếu phòng ban đã tồn tại thì trả về Conflict
                if (PhongBanExists(phongBan.PhongbanId))
                {
                    return Conflict($"Phòng ban với ID {phongBan.PhongbanId} đã tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            // Trả về thông tin phòng ban đã tạo mới với mã ID
            return CreatedAtAction("GetPhongBan", new { id = phongBan.PhongbanId }, phongBan);
        }

        // DELETE: api/PhongBan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhongBan(string id)
        {
            var phongBan = await _context.PhongBans.FindAsync(id);

            // Kiểm tra nếu phòng ban không tồn tại
            if (phongBan == null)
            {
                return NotFound($"Phòng ban với ID {id} không tồn tại.");
            }

            _context.PhongBans.Remove(phongBan);

            try
            {
                // Xóa phòng ban khỏi cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác khi xóa dữ liệu
                return StatusCode(500, $"Có lỗi khi xóa phòng ban: {ex.Message}");
            }

            // Trả về NoContent khi xóa thành công
            return NoContent();
        }

        private bool PhongBanExists(string id)
        {
            return _context.PhongBans.Any(e => e.PhongbanId == id);
        }
    }
}
