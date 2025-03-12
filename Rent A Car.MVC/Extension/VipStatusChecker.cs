using Microsoft.EntityFrameworkCore;
using Rent_A_Car.CORE.Enums;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.Extension
{
    public class VipStatusChecker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public VipStatusChecker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var expiredAds = await _context.Advertisements
                        .Where(ad => ad.Status == AdvertisementStatus.VIP && ad.VipEnded < DateTime.UtcNow)
                        .ToListAsync();

                    foreach (var ad in expiredAds)
                    {
                        ad.Status = AdvertisementStatus.Normal;
                    }

                    await _context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }

}
