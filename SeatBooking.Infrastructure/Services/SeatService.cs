using DishAdvisor.Infrastructure.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SeatBooking.Application.Repositories;
using SeatBooking.Application.Services;
using SeatBooking.Domain.Common;
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
        public async Task<Result<List<GetSeatResponse>>> GetPagination(int showTime)
        {
            try
            {
                var seats = await _unitOfWork.GetRepository<Seat>().GetListAsync(
                    include: x => x.Include(its => its.SeatColor));
                if (seats == null || !seats.Any())
                {
                    return Success(new List<GetSeatResponse>());
                }
                var seatResponses = seats.Adapt<List<GetSeatResponse>>();

                foreach (var seatResponse in seatResponses)
                {
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
    }
}
