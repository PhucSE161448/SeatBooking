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
        public async Task<Result<List<GetSeatResponse>>> GetPagination()
        {
            try
            {
                var seat = await _unitOfWork.GetRepository<Seat>().GetListAsync(
                    include : x => x.Include(its => its.SeatColor)
                    .Include(its => its.ShowTime));
                return Success(seat.Adapt<List<GetSeatResponse>>());
            }catch (Exception ex)
            {

            }
            return null!;
        }
    }
}
