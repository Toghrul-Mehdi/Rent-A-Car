using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rent_A_Car.CORE.Enums;
using Rent_A_Car.DAL.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class BookingStatusUpdater : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BookingStatusUpdater(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var currentTime = DateTime.UtcNow;

                // **Started Status**
                var startedBookings = await dbContext.Bookings
                    .Where(b => b.EndDate > currentTime && b.StartDate <= currentTime)
                    .ToListAsync();

                foreach (var booking in startedBookings)
                {
                    booking.Status = BookingStatus.Started;
                }

                // **Completed Status**
                var completedBookings = await dbContext.Bookings
                    .Where(b => b.EndDate <= currentTime)
                    .ToListAsync();

                foreach (var booking in completedBookings)
                {
                    booking.Status = BookingStatus.Completed;
                }

                await dbContext.SaveChangesAsync();
            }

            // **Hər 5 dəqiqədən bir yoxla**
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
