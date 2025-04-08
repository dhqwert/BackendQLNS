using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLNhanSu.MyModels;

namespace QLNhanSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "QL")]
    public class NhanVienQLController : ControllerBase
    {
        private readonly KetNoiCSDL _context;

        public NhanVienQLController(KetNoiCSDL context)
        {
            _context = context;
        }

        // GET: api/NhanVien
        [HttpGet]
        public async Task<ActionResult> GetNhanViens()
        {
            if (_context.NhanViens == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from n in _context.NhanViens
                        join pb in _context.PhongBans on n.PhongbanId equals pb.PhongbanId
                        orderby n.Tennhanvien ascending
                        select new
                        {
                            n.NhanvienId,
                            n.Tennhanvien,
                            n.Gioitinh,
                            n.Ngaysinh,
                            n.Diachi,
                            n.Sdt,
                            n.Email,
                            n.Luongcoban,
                            n.chucvu,
                            Tenphongban = pb.Tenphongban, // Hiển thị tên phòng ban
                            n.Ngayvaolam,
                            n.trangthailv
                        };

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Không có nhân viên nào trong hệ thống!",
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
        [Route("NhanVien/Search")]
        public async Task<ActionResult> TimKiemNhanVien(string s)
        {
            if (_context.NhanViens == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from n in _context.NhanViens
                        join pb in _context.PhongBans on n.PhongbanId equals pb.PhongbanId
                        where n.Tennhanvien.Contains(s) ||
                              pb.Tenphongban.Contains(s) ||
                              n.NhanvienId.Contains(s) ||
                              n.Diachi.Contains(s) ||
                              n.Sdt.Contains(s) ||
                              n.Email.Contains(s) ||
                              n.trangthailv.Contains(s) ||
                              n.chucvu.Contains(s) 
                        orderby n.Tennhanvien ascending
                        select new
                        {
                            n.NhanvienId,
                            n.Tennhanvien,
                            n.Gioitinh,
                            n.Ngaysinh,
                            n.Diachi,
                            n.Sdt,
                            n.Email,
                            n.Luongcoban,
                            n.chucvu,
                            Tenphongban = pb.Tenphongban, // Hiển thị tên phòng ban
                            n.Ngayvaolam,
                            n.trangthailv
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

        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(string id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            return Ok(nhanVien);
        }


        // PUT: api/NhanVien/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhanVien(string id, NhanVienDto nhanVienDto)
        {
            // Kiểm tra nếu ID trong URL không khớp với ID trong dữ liệu DTO
            if (id != nhanVienDto.NhanvienId)
            {
                return BadRequest("ID trong URL không khớp với ID trong dữ liệu.");
            }

            // Tìm nhân viên trong cơ sở dữ liệu
            var nhanVien = await _context.NhanViens.FindAsync(id);

            // Nếu không tìm thấy nhân viên, trả về lỗi NotFound
            if (nhanVien == null)
            {
                return NotFound($"Nhân viên với ID {id} không tồn tại.");
            }

            // Cập nhật các thông tin của nhân viên từ DTO
            nhanVien.Tennhanvien = nhanVienDto.Tennhanvien;
            nhanVien.Gioitinh = nhanVienDto.Gioitinh;
            nhanVien.Ngaysinh = nhanVienDto.Ngaysinh;
            nhanVien.Diachi = nhanVienDto.Diachi;
            nhanVien.Sdt = nhanVienDto.Sdt;
            nhanVien.Email = nhanVienDto.Email;
            nhanVien.Luongcoban = nhanVienDto.Luongcoban;
            nhanVien.PhongbanId = nhanVienDto.PhongbanId;
            nhanVien.Ngayvaolam = nhanVienDto.Ngayvaolam;
            nhanVien.trangthailv = nhanVienDto.trangthailv;
            nhanVien.chucvu = nhanVienDto.chucvu;

            // Đánh dấu đối tượng đã được thay đổi
            _context.Entry(nhanVien).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kiểm tra lại nếu nhân viên không tồn tại
                if (!NhanVienExists(id))
                {
                    return NotFound($"Nhân viên với ID {id} không tồn tại.");
                }
                else
                {
                    // Ném ra lỗi nếu gặp phải ngoại lệ đồng thời
                    throw;
                }
            }

            // Trả về thông báo thành công khi cập nhật dữ liệu
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVienDto nhanVienDto)
        {
            if (nhanVienDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // 🔹 Kiểm tra độ dài nhanvienId (phải đúng 9 ký tự)
            if (string.IsNullOrWhiteSpace(nhanVienDto.NhanvienId) || nhanVienDto.NhanvienId.Length != 9)
            {
                return BadRequest("Mã nhân viên phải có đúng 9 ký tự.");
            }

            // Chuyển đổi từ DTO sang Entity NhanVien
            var nhanVien = new NhanVien
            {
                NhanvienId = nhanVienDto.NhanvienId.Trim(), // Loại bỏ khoảng trắng thừa
                Tennhanvien = nhanVienDto.Tennhanvien,
                Gioitinh = nhanVienDto.Gioitinh,
                Ngaysinh = nhanVienDto.Ngaysinh,
                Diachi = nhanVienDto.Diachi,
                Sdt = nhanVienDto.Sdt,
                Email = nhanVienDto.Email,
                Luongcoban = nhanVienDto.Luongcoban,
                PhongbanId = nhanVienDto.PhongbanId,
                Ngayvaolam = nhanVienDto.Ngayvaolam,
                chucvu = nhanVienDto.chucvu,
                trangthailv = nhanVienDto.trangthailv
            };

            _context.NhanViens.Add(nhanVien);

            try
            {
                // Lưu nhân viên mới vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Nếu nhân viên đã tồn tại, trả về Conflict
                if (NhanVienExists(nhanVien.NhanvienId))
                {
                    return Conflict($"Nhân viên với ID {nhanVien.NhanvienId} đã tồn tại.");
                }
                else
                {
                    throw; // Ném ngoại lệ nếu có lỗi khác
                }
            }

            // Trả về kết quả với thông tin nhân viên vừa được tạo
            return CreatedAtAction("GetNhanVien", new { id = nhanVien.NhanvienId }, nhanVien);
        }


        // DELETE: api/NhanVien/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(string id)
        {
            // Tìm nhân viên theo ID
            var nhanVien = await _context.NhanViens.FindAsync(id);

            // Nếu không tìm thấy nhân viên, trả về lỗi NotFound
            if (nhanVien == null)
            {
                return NotFound($"Nhân viên với ID {id} không tồn tại.");
            }

            // Xóa nhân viên khỏi cơ sở dữ liệu
            _context.NhanViens.Remove(nhanVien);

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Trả về lỗi 500 nếu có vấn đề trong quá trình xóa
                return StatusCode(500, $"Lỗi khi xóa nhân viên: {ex.Message}");
            }

            // Trả về thông báo thành công
            return NoContent();
        }

        private bool NhanVienExists(string id)
        {
            return _context.NhanViens.Any(e => e.NhanvienId == id);
        }
    }
}

