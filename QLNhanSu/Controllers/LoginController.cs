//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using QLNhanSu.MyModels;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.EntityFrameworkCore;

//// phải đăng nhập thì mới có thể thực hiện các thao tác khác
//namespace QLNhanSu.Controllers
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class LoginController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;
//        private readonly KetNoiCSDL _context;

//        public LoginController(IConfiguration configuration, KetNoiCSDL context)
//        {
//            _configuration = configuration;
//            _context = context;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] Login user)
//        {
//            if (user == null || string.IsNullOrEmpty(user.tenDangNhap) || string.IsNullOrEmpty(user.matKhau))
//            {
//                return BadRequest("Thông tin đăng nhập không hợp lệ.");
//            }

//            var dbUser = await _context.TaiKhoans
//                    .Where(tk => tk.Tendangnhap == user.tenDangNhap && tk.Matkhau == user.matKhau)  // Dùng user thay vì dbUser
//                    .Select(tk => new
//                    {
//                        tk.TaikhoanId,
//                        tk.Tendangnhap,
//                        tk.QuyenId,
//                        NhanvienId = tk.NhanvienId == null ? "Không áp dụng" : tk.NhanvienId.Trim()
//                    })
//                    .FirstOrDefaultAsync();

//            if (dbUser == null)
//            {
//                return Unauthorized("Tên đăng nhập hoặc mật khẩu không chính xác.");
//            }

//            // Xác định vai trò
//            string role = dbUser.QuyenId switch
//            {
//                "QTV0" => "QTV",
//                "QL00" => "QL",
//                "NV00" => "NV",
//                _ => "Unknown"
//            };

//            // Nếu là Quản trị viên, không cần `nhanvienId`
//            string nhanvienId = dbUser.QuyenId == "QTV0" ? "Không áp dụng" : dbUser.NhanvienId;

//            // Gọi hàm tạo token
//            var token = GenerateJwtToken(dbUser.TaikhoanId, dbUser.Tendangnhap, dbUser.QuyenId);

//            return Ok(new
//            {
//                message = "Đăng nhập thành công!",
//                user = new
//                {
//                    taikhoanId = dbUser.TaikhoanId,
//                    tendangnhap = dbUser.Tendangnhap,
//                    quyenId = dbUser.QuyenId,
//                    role,
//                    nhanvienId
//                },
//                token
//            });
//        }

//        private string GenerateJwtToken(int userId, string username, string quyenId)
//        {
//            var jwtSettings = _configuration.GetSection("Jwt");
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            // Chuyển QuyenId thành Role hợp lệ
//            string role = quyenId switch
//            {
//                "QTV0" => "QTV",
//                "QL00" => "QL",
//                "NV00" => "NV",
//                _ => "Unknown"
//            };

//            var claims = new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
//                new Claim(ClaimTypes.Name, username),
//                new Claim(ClaimTypes.Role, role)  // Gán quyền đúng cách
//            };

//            var token = new JwtSecurityToken(
//                issuer: jwtSettings["Issuer"],
//                audience: jwtSettings["Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QLNhanSu.MyModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace QLNhanSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly KetNoiCSDL _context;

        public LoginController(IConfiguration configuration, KetNoiCSDL context)
        {
            _configuration = configuration;
            _context = context;
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] Login user)
        //{
        //    if (user == null || string.IsNullOrEmpty(user.tenDangNhap) || string.IsNullOrEmpty(user.matKhau))
        //    {
        //        return BadRequest("Thông tin đăng nhập không hợp lệ.");
        //    }

        //    var dbUser = await _context.TaiKhoans
        //            .Where(tk => tk.Tendangnhap == user.tenDangNhap && tk.Matkhau == user.matKhau)
        //            .Select(tk => new
        //            {
        //                tk.TaikhoanId,
        //                tk.Tendangnhap,
        //                tk.QuyenId,
        //                NhanvienId = tk.NhanvienId == null ? "Không áp dụng" : tk.NhanvienId.Trim()
        //            })
        //            .FirstOrDefaultAsync();

        //    if (dbUser == null)
        //    {
        //        return Unauthorized("Tên đăng nhập hoặc mật khẩu không chính xác.");
        //    }

        //    string role = dbUser.QuyenId switch
        //    {
        //        "QTV0" => "QTV",
        //        "QL00" => "QL",
        //        "NV00" => "NV",
        //        _ => "Unknown"
        //    };

        //    var token = GenerateJwtToken(dbUser.TaikhoanId, dbUser.Tendangnhap, role, NhanvienId);

        //    return Ok(new
        //    {
        //        message = "Đăng nhập thành công!",
        //        user = new
        //        {
        //            taikhoanId = dbUser.TaikhoanId,
        //            tendangnhap = dbUser.Tendangnhap,
        //            quyenId = dbUser.QuyenId,
        //            role,
        //            nhanvienId = dbUser.QuyenId == "QTV0" ? "Không áp dụng" : dbUser.NhanvienId
        //        },
        //        token
        //    });
        //}

        //private string GenerateJwtToken(int userId, string username, string role, )
        //{
        //    var jwtSettings = _configuration.GetSection("Jwt");
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        //        new Claim(ClaimTypes.Name, username),
        //        new Claim(ClaimTypes.Role, role)  // ✅ Sử dụng chuẩn ClaimTypes.Role
        //    };

        //    var token = new JwtSecurityToken(
        //        issuer: jwtSettings["Issuer"],
        //        audience: jwtSettings["Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login user)
        {
            if (user == null || string.IsNullOrEmpty(user.tenDangNhap) || string.IsNullOrEmpty(user.matKhau))
            {
                return BadRequest("Thông tin đăng nhập không hợp lệ.");
            }

            var dbUser = await _context.TaiKhoans
                .Where(tk => tk.Tendangnhap == user.tenDangNhap && tk.Matkhau == user.matKhau)
                .Select(tk => new
                {
                    tk.TaikhoanId,
                    tk.Tendangnhap,
                    tk.QuyenId,
                    NhanvienId = tk.NhanvienId == null ? null : tk.NhanvienId.Trim() // ✅ Xử lý NULL
                })
                .FirstOrDefaultAsync();


            if (dbUser == null)
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
            // 🔹 Xác định role từ `QuyenId`
            string role = dbUser.QuyenId switch
            {
                "QTV0" => "QTV",
                "QL00" => "QL",
                "NV00" => "NV",
                _ => "Unknown"
            };

            // 🔹 Kiểm tra `NhanvienId`, tránh lỗi `NULL`
            string? nhanvienId = dbUser.NhanvienId; // Giữ nguyên giá trị từ DB

            // 🔹 Nếu không phải QTV, kiểm tra `NhanvienId`
            if (role == "NV")
            {
                if (string.IsNullOrEmpty(nhanvienId))
                {
                    return BadRequest("Không xác định nhân viên cho tài khoản này.");
                }
                nhanvienId = nhanvienId.Trim(); // ✅ Chỉ `Trim()` nếu không NULL
            }

            // 🔹 Gọi `GenerateJwtToken` với `NhanvienId`
            var token = GenerateJwtToken(dbUser.TaikhoanId, dbUser.Tendangnhap, role, nhanvienId);

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                user = new
                {
                    taikhoanId = dbUser.TaikhoanId,
                    tendangnhap = dbUser.Tendangnhap,
                    quyenId = dbUser.QuyenId,
                    role,
                    nhanvienId = role == "QTV" ? "Không áp dụng" : nhanvienId // ✅ Không báo lỗi khi `QTV`
                },
                token
            });
        }

        private string GenerateJwtToken(int userId, string username, string role, string? nhanvienId = null)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            if (!string.IsNullOrEmpty(nhanvienId))
            {
                claims.Add(new Claim("NhanvienId", nhanvienId)); // ✅ Chỉ thêm nếu không NULL
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

