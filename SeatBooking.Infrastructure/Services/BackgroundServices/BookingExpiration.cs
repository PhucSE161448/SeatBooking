using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SeatBooking.Application.Repositories;
using SeatBooking.Domain.Entities;
using SeatBooking.Infrastructure.Database;
using SeatBooking.Infrastructure.Repositories;

namespace SeatBooking.Infrastructure.Services.BackgroundServices
{
    public class BookingExpiration : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BookingExpiration> _logger;

        public BookingExpiration(IServiceProvider serviceProvider, ILogger<BookingExpiration> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var delay = TimeSpan.FromMinutes(3);

                    _logger.LogInformation("BookingExpiryService: Scheduled to run every 3 minutes.");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<SeatBookingContext>>();
                        var expiredBookings = await unitOfWork.GetRepository<Booking>()
                            .GetListAsync(predicate: b => b.ExpiryTime <= DateTime.UtcNow && !b.Transactions.Any(),
                                include: its => its.Include(x => x.Transactions));

                        if (expiredBookings.Any())
                        {
                            foreach (var booking in expiredBookings)
                            {
                                // Lấy thông tin Seat liên quan
                                var seat = await unitOfWork.GetRepository<Seat>()
                                    .GetAsync(predicate: s => s.Id == booking.SeatId);
                              

                                if (seat != null)
                                {
                                    var updatedSeat = new Seat
                                    {
                                        Id = seat.Id, // Gán lại Id
                                        ColNumber = seat.ColNumber,
                                        Floor = seat.Floor,
                                        RowChar = seat.RowChar,
                                        SeatColorId = seat.SeatColorId,
                                        IsBookedShowTime1 = booking.BookingShow == 1 ? false : seat.IsBookedShowTime1,
                                        IsBookedShowTime2 = booking.BookingShow == 2 ? false : seat.IsBookedShowTime2
                                    };
                                    // Cập nhật Seat trong cơ sở dữ liệu
                                    unitOfWork.GetRepository<Seat>().UpdateAsync(updatedSeat);
                                }

                                // Xóa Booking đã hết hạn và không có Transaction
                                unitOfWork.GetRepository<Booking>().DeleteAsync(booking);
                            }

                            // Lưu thay đổi vào cơ sở dữ liệu
                            await unitOfWork.CommitAsync();
                        }

                    }

                    _logger.LogInformation("BookingExpiryService: Process completed at {Time}", DateTime.Now);
                    await Task.Delay(delay, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogWarning("DailyCalorieResetService: Task was canceled.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while resetting daily calories.");
                }
            }
        }
    }
}
