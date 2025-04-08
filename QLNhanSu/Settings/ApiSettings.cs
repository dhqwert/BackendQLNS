namespace QLNhanSu.Settings
{
    public class ApiSettings
    {
        public string ApiBaseUrl { get; set; }
        public int MaxRetries { get; set; }
        public int TimeoutSeconds { get; set; }
        public int SyncIntervalSeconds { get; set; } // <-- thêm field này
    }
}
