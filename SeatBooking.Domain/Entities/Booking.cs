using System;
using System.Collections.Generic;

namespace SeatBooking.Domain.Entities;

public partial class Booking
{
    public int Id { get; set; }

    public int SeatId { get; set; }

    public string StudentName { get; set; } = null!;

    public DateTime BookingTime { get; set; }

    public string? Description { get; set; }

    public DateTime ExpiryTime { get; set; }

    public int? BookingShow { get; set; }
    public bool IsCash { get; set; }

    public virtual Seat Seat { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
