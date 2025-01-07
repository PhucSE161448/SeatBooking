using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatBooking.Domain.DTO.Request
{
    public class PaymentRequest
    {
        public List<int>? Seats { get; set; }          // List of seat IDs
        public decimal TotalAmount { get; set; }     // Total amount
        public string? StudentName { get; set; }      // Student's name
        public string? SelectedBranch { get; set; }   // Selected branch
    }
}
