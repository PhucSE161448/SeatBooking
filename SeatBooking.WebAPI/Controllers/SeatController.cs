using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeatBooking.Application.Services;
using SeatBooking.Domain.Common;
using SeatBooking.Domain.DTO.Response;

namespace SeatBooking.WebAPI.Controllers
{
    public class SeatController(ISeatService seatService) : BaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(Result<List<GetSeatResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagination([FromQuery] int page = 1, [FromQuery] int size = 100)
        {
            Result<List<GetSeatResponse>> result = await seatService.GetPagination();
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
