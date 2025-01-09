using DishAdvisor.Infrastructure.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SeatBooking.Application.Repositories;
using SeatBooking.Application.Services;
using SeatBooking.Domain.Common;
using SeatBooking.Domain.DTO.Request;
using SeatBooking.Domain.DTO.Response;
using SeatBooking.Domain.Entities;
using SeatBooking.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatBooking.Infrastructure.Services
{
    public class SeatService(IUnitOfWork<SeatBookingContext> unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : BaseService<SeatService>(unitOfWork, mapper, httpContextAccessor), ISeatService
    {
        public async Task<List<int>> CreateBooking(PaymentRequest paymentRequest)
        {
            try
            {
                var bookingIds = new List<int>();
                if (paymentRequest.Seats == null || !paymentRequest.Seats.Any())
                {
                    throw new ArgumentException("Seats list cannot be null or empty.");
                }

                if (string.IsNullOrEmpty(paymentRequest.StudentName))
                {
                    throw new ArgumentException("StudentName cannot be null or empty.");
                }

                var seatsToUpdate = await _unitOfWork.GetRepository<Seat>()
                    .GetListAsync(predicate: seat => paymentRequest.Seats.Contains(seat.Id));

                if (seatsToUpdate.Count != paymentRequest.Seats.Count)
                {
                    throw new InvalidOperationException("Some seats do not exist or cannot be booked.");
                }

                foreach (var seat in seatsToUpdate)
                {
                    /*if (seat.IsBookedShowTime1 == true)
                    {
                        throw new InvalidOperationException($"Seat with ID {seat.Id} is already booked.");
                    }*/
                    if (paymentRequest.BookingShow == 1)
                    {
                        seat.IsBookedShowTime1 = true;
                    }
                    else
                    {
                        seat.IsBookedShowTime2 = true;
                    }

                    var booking = new Booking
                    {
                        SeatId = seat.Id,
                        StudentName = paymentRequest.StudentName!,
                        BookingTime = DateTime.UtcNow,
                        ExpiryTime = DateTime.UtcNow.AddMinutes(10),
                        Description = $"Booking for seat {seat.Id} at branch {paymentRequest.SelectedBranch}",
                        BookingShow = paymentRequest.BookingShow,
                        IsCash = paymentRequest.IsCash
                    };

                    // Thêm từng đối tượng Booking
                    await _unitOfWork.GetRepository<Booking>().InsertAsync(booking);
                    bookingIds.Add(booking.Id);
                }

                // Cập nhật trạng thái ghế
                foreach (var seat in seatsToUpdate)
                {
                    _unitOfWork.GetRepository<Seat>().UpdateAsync(seat);
                }

                var success = await _unitOfWork.CommitAsync() > 0;
                if (success)
                {
                    return bookingIds; // Trả về danh sách BookingId
                }
                return new List<int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<int>();
            }
        }


        public async Task<Result<List<GetSeatResponse>>> GetPagination(int showTime, List<int>? seatIds = null)
        {
            try
            {
                var seats = await _unitOfWork.GetRepository<Seat>().GetListAsync(predicate: seat => seatIds == null || !seatIds.Any() || seatIds.Contains(seat.Id),
                    include: x => x.Include(its => its.SeatColor)
                        .Include(x => x.Bookings));
                if (seats == null || !seats.Any())
                {
                    return Success(new List<GetSeatResponse>());
                }
                var seatResponses = seats.Adapt<List<GetSeatResponse>>();

                foreach (var seatResponse in seatResponses)
                {
                    var seat = seats.FirstOrDefault(s => s.Id == seatResponse.Id);

                    // Kiểm tra điều kiện nếu ghế có thông tin đặt chỗ
                    if (seat?.Bookings != null && seat.Bookings.Any())
                    {
                        var booking = seat.Bookings.FirstOrDefault(b => b.BookingShow == showTime && b.SeatId == seat.Id);

                        if (booking != null)
                        {
                            seatResponse.StudentName = booking.StudentName;
                            seatResponse.IsBooked = true;
                        }
                    }
                    // Nếu showTime = 1, lấy giá trị từ IsBookedShowTime1
                    if (showTime == 1)
                    {
                        seatResponse.IsBooked = seatResponse.IsBookedShowTime1;
                    }
                    else if (showTime == 2)
                    {
                        seatResponse.IsBooked = seatResponse.IsBookedShowTime2;
                    }
                }

                // Trả về kết quả thành công với danh sách ghế đã được xử lý
                return Success(seatResponses);
            }
            catch (Exception ex)
            {

            }
            return null!;
        }
        public async Task<bool> CreateTransactionsFromNumbers(string numbers, decimal totalAmount)
        {
            try
            {
                // Tách chuỗi 'numbers' thành danh sách các BookingId
                var bookingIds = numbers.Split(',')
                    .Select(id => int.Parse(id))
                    .ToList();

                foreach (var bookingId in bookingIds)
                {
                    var transaction = new Transaction
                    {
                        BookingId = bookingId,
                        TotalAmount = totalAmount,
                        PaymentType = 1,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
                }
                var success = await _unitOfWork.CommitAsync() > 0;
                if (success)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating transactions: {ex.Message}");
                return false;
            }
        }
    }
}
