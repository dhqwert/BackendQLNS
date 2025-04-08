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
    [Authorize(Roles = "QTV")]
    [Route("api/[controller]")]
    [ApiController]
    public class PhanQuyenQTVController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public PhanQuyenQTVController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/PhanQuyen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhanQuyenDto>>> GetPhanQuyens()
        {
            var phanQuyens = await _context.PhanQuyens.ToListAsync();

            if (phanQuyens == null || phanQuyens.Count == 0)
            {
                return NotFound("Không có quyền nào trong hệ thống.");
            }

            var phanQuyenDtos = phanQuyens.Select(p => new PhanQuyenDto
            {
                QuyenId = p.QuyenId,
                Loaiquyen = p.Loaiquyen
            }).ToList();

            return Ok(phanQuyenDtos);
        }

        // GET: api/PhanQuyen/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhanQuyenDto>> GetPhanQuyen(string id)
        {
            var phanQuyen = await _context.PhanQuyens.FindAsync(id);

            if (phanQuyen == null)
            {
                return NotFound($"Quyền với ID {id} không tồn tại.");
            }

            var phanQuyenDto = new PhanQuyenDto
            {
                QuyenId = phanQuyen.QuyenId,
                Loaiquyen = phanQuyen.Loaiquyen
            };

            return Ok(phanQuyenDto);
        }


        // PUT: api/PhanQuyen/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhanQuyen(string id, PhanQuyenDto phanQuyenDto)
        {
            if (id != phanQuyenDto.QuyenId)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            // Chuyển đổi từ DTO sang entity PhanQuyen
            var phanQuyen = await _context.PhanQuyens.FindAsync(id);

            if (phanQuyen == null)
            {
                return NotFound($"Không tìm thấy quyền với ID {id}.");
            }

            // Cập nhật thông tin của entity từ DTO
            phanQuyen.Loaiquyen = phanQuyenDto.Loaiquyen;

            _context.Entry(phanQuyen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhanQuyenExists(id))
                {
                    return NotFound($"Quyền với ID {id} không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Trả về trạng thái 204 nếu thành công
        }


        // POST: api/PhanQuyen
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhanQuyen>> PostPhanQuyen(PhanQuyenDto phanQuyenDto)
        {
            // Kiểm tra nếu dữ liệu không hợp lệ
            if (phanQuyenDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Chuyển từ DTO sang entity PhanQuyen
            var phanQuyen = new PhanQuyen
            {
                QuyenId = phanQuyenDto.QuyenId,
                Loaiquyen = phanQuyenDto.Loaiquyen
            };

            _context.PhanQuyens.Add(phanQuyen);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Nếu quyền đã tồn tại, trả về lỗi Conflict
                if (PhanQuyenExists(phanQuyen.QuyenId))
                {
                    return Conflict($"Quyền với ID {phanQuyen.QuyenId} đã tồn tại.");
                }
                else
                {
                    // Xử lý các lỗi khác (nếu có)
                    throw;
                }
            }

            // Trả về thông tin quyền đã tạo, với URL để truy cập thông tin quyền vừa tạo
            return CreatedAtAction("GetPhanQuyen", new { id = phanQuyen.QuyenId }, phanQuyen);
        }


        // DELETE: api/PhanQuyen/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhanQuyen(string id)
        {
            // Tìm kiếm quyền theo ID
            var phanQuyen = await _context.PhanQuyens.FindAsync(id);

            // Nếu quyền không tồn tại, trả về lỗi NotFound
            if (phanQuyen == null)
            {
                return NotFound($"Quyền với ID {id} không tồn tại.");
            }

            // Xóa quyền khỏi cơ sở dữ liệu
            _context.PhanQuyens.Remove(phanQuyen);
            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Trường hợp gặp lỗi trong quá trình xóa
                return StatusCode(500, $"Lỗi khi xóa quyền: {ex.Message}");
            }

            // Trả về thông báo thành công
            return NoContent();
        }

        private bool PhanQuyenExists(string id)
        {
            return _context.PhanQuyens.Any(e => e.QuyenId == id);
        }
    }
}
