using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading;
using QLNhanSu.Controllers;
using QLNhanSu.MyModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using QLNhanSu.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;



namespace QLNhanSu.Services
{
    public class AttendanceService : IAttendanceService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string ApiBaseUrl = "http://127.0.0.1:5001/api/attendance";
        private readonly KetNoiCSDL _context;

        public AttendanceService(KetNoiCSDL context)
        {
            _context = context;
        }
        static AttendanceService()
        {
            client.Timeout = TimeSpan.FromSeconds(30); // Timeout cho các request
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private async Task CapNhatKhoanTru()
        {
            var danhSachChamCong = await _context.ChamCongs.ToListAsync(); // Lấy toàn bộ chấm công

            foreach (var chamCong in danhSachChamCong)
            {
                var nhanvienId = chamCong.NhanvienId;
                if (!chamCong.Ngay.HasValue) continue; // ✅ Bỏ qua nếu ngày null
                var ngayChamCong = chamCong.Ngay.Value;

                // ✅ Xác định có đi muộn / về sớm không
                bool diMuon = chamCong.Giovao.HasValue && chamCong.Giovao.Value > new TimeOnly(8, 0, 0);
                bool veSom = chamCong.Giora.HasValue && chamCong.Giora.Value < new TimeOnly(17, 0, 0);
                bool nghikhongphep = !chamCong.Giovao.HasValue; // ✅ Kiểm tra không chấm công vào


                // 🔹 Kiểm tra & cập nhật khoản trừ đi muộn (TruId = "0003")
                var truDiMuon = await _context.CtNhanvienKhoantrus
                    .FirstOrDefaultAsync(k => k.NhanvienId == nhanvienId && k.Thoigian == ngayChamCong && k.TruId == "K001");

                if (diMuon)
                {
                    if (truDiMuon == null) // Chưa có khoản trừ -> thêm mới
                    {
                        _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
                        {
                            NhanvienId = nhanvienId,
                            TruId = "K001",
                            Thoigian = ngayChamCong
                        });
                    }
                }
                else
                {
                    if (truDiMuon != null) // Đã có khoản trừ nhưng không đi muộn nữa -> Xóa
                    {
                        _context.CtNhanvienKhoantrus.Remove(truDiMuon);
                    }
                }

                // 🔹 Kiểm tra & cập nhật khoản trừ về sớm (TruId = "0004")
                var truVeSom = await _context.CtNhanvienKhoantrus
                    .FirstOrDefaultAsync(k => k.NhanvienId == nhanvienId && k.Thoigian == ngayChamCong && k.TruId == "K002");

                if (veSom)
                {
                    if (truVeSom == null) // Chưa có khoản trừ -> thêm mới
                    {
                        _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
                        {
                            NhanvienId = nhanvienId,
                            TruId = "K002",
                            Thoigian = ngayChamCong
                        });
                    }
                }
                else
                {
                    if (truVeSom != null) // Đã có khoản trừ nhưng không về sớm nữa -> Xóa
                    {
                        _context.CtNhanvienKhoantrus.Remove(truVeSom);
                    }
                }

                // 🔹 Kiểm tra & cập nhật khoản trừ KHÔNG CHẤM CÔNG VÀO (TruId = "0005")
                var truKhongChamCong = await _context.CtNhanvienKhoantrus
                    .FirstOrDefaultAsync(k => k.NhanvienId == nhanvienId && k.Thoigian == ngayChamCong && k.TruId == "K005");

                if (nghikhongphep)
                {
                    if (truKhongChamCong == null) // Chưa có khoản trừ -> thêm mới
                    {
                        _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
                        {
                            NhanvienId = nhanvienId,
                            TruId = "K005",
                            Thoigian = ngayChamCong
                        });
                    }
                }
                else
                {
                    if (truKhongChamCong != null) // Đã có khoản trừ nhưng sau đó sửa lại -> Xóa
                    {
                        _context.CtNhanvienKhoantrus.Remove(truKhongChamCong);
                    }
                }

                // 🔹 Kiểm tra & cập nhật vắng buổi sáng (TruId = "0006") nếu đi muộn hơn 2 tiếng
                var truVangBuoiSang = await _context.CtNhanvienKhoantrus
                    .FirstOrDefaultAsync(k => k.NhanvienId == nhanvienId && k.Thoigian == ngayChamCong && k.TruId == "K003");

                if (chamCong.Giovao.HasValue && chamCong.Giovao.Value > new TimeOnly(10, 0, 0)) // Đi muộn hơn 2 tiếng
                {
                    if (truVangBuoiSang == null) // Chưa có khoản trừ -> thêm mới
                    {
                        _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
                        {
                            NhanvienId = nhanvienId,
                            TruId = "K003", // Trường hợp vắng buổi sáng
                            Thoigian = ngayChamCong
                        });
                    }
                }
                else
                {
                    if (truVangBuoiSang != null) // Đã có khoản trừ nhưng không vắng buổi sáng nữa -> Xóa
                    {
                        _context.CtNhanvienKhoantrus.Remove(truVangBuoiSang);
                    }
                }

                // 🔹 Kiểm tra & cập nhật vắng buổi chiều (TruId = "0007") nếu ra sớm hơn 2 tiếng
                var truVangBuoiChieu = await _context.CtNhanvienKhoantrus
                    .FirstOrDefaultAsync(k => k.NhanvienId == nhanvienId && k.Thoigian == ngayChamCong && k.TruId == "K004");

                if (chamCong.Giora.HasValue && chamCong.Giora.Value < new TimeOnly(15, 0, 0)) // Ra sớm hơn 2 tiếng
                {
                    if (truVangBuoiChieu == null) // Chưa có khoản trừ -> thêm mới
                    {
                        _context.CtNhanvienKhoantrus.Add(new CtNhanvienKhoantru
                        {
                            NhanvienId = nhanvienId,
                            TruId = "K004", // Trường hợp vắng buổi chiều
                            Thoigian = ngayChamCong
                        });
                    }
                }
                else
                {
                    if (truVangBuoiChieu != null) // Đã có khoản trừ nhưng không vắng buổi chiều nữa -> Xóa
                    {
                        _context.CtNhanvienKhoantrus.Remove(truVangBuoiChieu);
                    }
                }
            }

            await _context.SaveChangesAsync(); // Lưu lại thay đổi vào database
        }

        public async Task SyncAttendance()
        {
            try
            {
                string jsonData = await GetAttendanceDataAsync();
                Console.WriteLine("📥 JSON từ API Python:");
                Console.WriteLine(jsonData);

                // Chuyển đổi dữ liệu JSON thành danh sách các đối tượng
                var data = System.Text.Json.JsonSerializer.Deserialize<List<ChamCongDto>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data == null || data.Count == 0)
                {
                    throw new Exception("Dữ liệu nhận được từ API không hợp lệ.");
                }

                // Xóa dữ liệu cũ và thêm mới dữ liệu từ API
                _context.ChamCongs.RemoveRange(_context.ChamCongs);
                await _context.SaveChangesAsync();

                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item.NhanvienId))
                    {
                        Console.WriteLine("❌ Thiếu NhanvienId");
                        continue;
                    }

                    if (!DateOnly.TryParse(item.Ngay, out var ngay))
                    {
                        Console.WriteLine($"❌ Ngày không hợp lệ: {item.Ngay}");
                        continue;
                    }
                    TimeOnly? giovaoTime = null;
                    TimeOnly? gioraTime = null;

                    if (!string.IsNullOrWhiteSpace(item.Giovao) && TimeOnly.TryParse(item.Giovao, out var gv))
                        giovaoTime = gv;
                    if (!string.IsNullOrWhiteSpace(item.Giora) && TimeOnly.TryParse(item.Giora, out var gr))
                        gioraTime = gr;

                    string dimuon = (giovaoTime.HasValue && giovaoTime.Value > new TimeOnly(8, 0, 0)) ? "Y" : "N";
                    string vesom = (gioraTime.HasValue && gioraTime.Value < new TimeOnly(17, 0, 0)) ? "Y" : "N";

                    var newRecord = new ChamCong
                    {
                        NhanvienId = item.NhanvienId,
                        Ngay = ngay,
                        Giovao = giovaoTime,
                        Giora = gioraTime,
                        Dimuon = dimuon,
                        Vesom = vesom
                    };

                    await _context.ChamCongs.AddAsync(newRecord);
                }

                await _context.SaveChangesAsync();
                await CapNhatKhoanTru(); // Cập nhật khoản trừ
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi: {ex.Message}");
                throw;
            }
        }

        public async Task<(string NhanVienId, string Ngay)> GetNhanVienIdAndNgayByChamCongIdAsync(string chamCongId)
        {
            try
            {
                var response = await SendRequestWithRetryAsync(() => client.GetAsync($"{ApiBaseUrl}/{chamCongId}"), 3);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                dynamic data = JsonConvert.DeserializeObject(content);
                if (data == null)
                {
                    throw new Exception("Dữ liệu trả về null khi deserialize");
                }

                string nhanvienId = data.nhanvien_id;
                string ngay = data.ngay;

                return (nhanvienId, ngay);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy dữ liệu từ ChamCongId: {ex.Message}");
                throw;
            }
        }

        // Hàm GET dữ liệu chấm công từ API
        public async Task<string> GetAttendanceDataAsync()
        {
            try
            {
                var response = await SendRequestWithRetryAsync(() => client.GetAsync(ApiBaseUrl), 3); // Retry 3 lần
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (nếu có)
                Console.WriteLine($"Lỗi khi lấy dữ liệu: {ex.Message}");
                throw;
            }
        }

        // Hàm để thực hiện retry cho request
        private async Task<HttpResponseMessage> SendRequestWithRetryAsync(Func<Task<HttpResponseMessage>> sendRequest, int maxRetries)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    return await sendRequest();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Lỗi kết nối hoặc HTTP, thử lại lần {retries + 1}/{maxRetries}: {ex.Message}");
                }
                catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Timeout, thử lại lần {retries + 1}/{maxRetries}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi không xác định: {ex.Message}");
                    throw;
                }

                retries++;
                if (retries < maxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2)); // Delay trước khi retry
                }
            }

            throw new Exception("Lỗi khi kết nối API sau nhiều lần thử.");
        }


    }
}



// Hàm POST/PUT dữ liệu lên API
//public async Task<bool> PostAttendanceAsync(string nhanvienId, string ngay, string giovao, string giora)
//{
//    var data = new
//    {
//        nhanvien_id = nhanvienId,
//        ngay = ngay,
//        giovao = giovao,
//        giora = giora
//    };

//    var jsonContent = JsonConvert.SerializeObject(data);
//    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

//    try
//    {
//        var response = await SendRequestWithRetryAsync(() => client.PostAsync(ApiBaseUrl, content), 3); // Retry 3 lần
//        response.EnsureSuccessStatusCode();
//        return true;
//    }
//    catch (Exception ex)
//    {
//        // Xử lý lỗi (nếu có)
//        Console.WriteLine($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
//        return false;
//    }
//}

//public async Task<bool> UpdateChamCongByChamCongIdAsync(string chamCongId, string gioVao, string gioRa)
//{
//    var (nhanvienId, ngay) = await GetNhanVienIdAndNgayByChamCongIdAsync(chamCongId);

//    var data = new
//    {
//        nhanvien_id = nhanvienId,
//        ngay = ngay,
//        giovao = gioVao,
//        giora = gioRa
//    };

//    var jsonContent = JsonConvert.SerializeObject(data);
//    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

//    try
//    {
//        var response = await SendRequestWithRetryAsync(() => client.PutAsync($"{ApiBaseUrl}/{chamCongId}", content), 3);
//        response.EnsureSuccessStatusCode();
//        return true;
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Lỗi khi PUT dữ liệu: {ex.Message}");
//        return false;
//    }
//}
