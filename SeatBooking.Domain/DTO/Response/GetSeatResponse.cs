using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatBooking.Domain.DTO.Response
{
    public class GetSeatResponse
    {
        public int Id { get; set; }
        public string? RowChar { get; set; }
        public int ColNumber { get; set; }
        public GetColorResponse? SeatColor { get; set; }
        public int ShowTimeId { get; set; }
        public string? Floor { get; set; }
        public string? StudentName { get; set; }
        public string SeatInfo => $"{RowChar}{ColNumber} - {SeatColor.Color} - {Floor}";
        public bool? IsBooked { get; set; }
        public bool? IsBookedShowTime1 { get; set;}
        public bool? IsBookedShowTime2 { get; set;}
    }
}
