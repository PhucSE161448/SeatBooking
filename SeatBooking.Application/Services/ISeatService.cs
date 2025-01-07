using SeatBooking.Domain.Common;
using SeatBooking.Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatBooking.Application.Services
{
    public interface ISeatService
    {
        Task<Result<List<GetSeatResponse>>> GetPagination();
    }
}
