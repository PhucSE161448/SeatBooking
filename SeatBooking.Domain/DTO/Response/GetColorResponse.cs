using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatBooking.Domain.DTO.Response
{
    public class GetColorResponse
    {
        public int Id { get; set; }
        public string? Color { get; set; }
        public int Price { get; set; }
    }
}
