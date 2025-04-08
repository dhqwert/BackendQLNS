using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using QLNhanSu.Controllers;
using QLNhanSu.Settings; // <-- nhớ using
using static QLNhanSu.Controllers.ChamCongQLController;

namespace QLNhanSu.Services
{
    using Microsoft.Extensions.DependencyInjection;

    public class AttendanceSyncService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AttendanceSyncService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var attendanceService = scope.ServiceProvider.GetRequiredService<IAttendanceService>();
                        await attendanceService.SyncAttendance();
                    }

                    await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken); // 5p delay
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi đồng bộ: {ex.Message}");
                }
            }
        }
    }
}
