using System.Text.Json.Serialization;

namespace QLNhanSu.MyModels
{
    public class ChamCongDto
    {
        //[JsonPropertyName("chamcong_id")]
        //public int? ChamcongId { get; set; }  // ✅ Đảm bảo không bị null

        [JsonPropertyName("ngay")]
        public string Ngay { get; set; } = string.Empty;

        [JsonPropertyName("giovao")]
        public string? Giovao { get; set; }

        [JsonPropertyName("giora")]
        public string? Giora { get; set; }

        [JsonPropertyName("nhanvien_id")]
        public string NhanvienId { get; set; } = string.Empty;  // ✅ Đảm bảo không bị null

        [JsonPropertyName("dimuon")]
        public string? Dimuon { get; set; }

        [JsonPropertyName("vesom")]
        public string? Vesom { get; set; }
    }
}
