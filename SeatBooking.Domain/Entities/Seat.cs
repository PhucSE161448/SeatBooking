using System;
using System.Collections.Generic;

namespace SeatBooking.Domain.Entities;

public partial class Seat
{
    public int Id { get; set; }

    public string RowChar { get; set; } = null!;

    public int ColNumber { get; set; }

    public int SeatColorId { get; set; }

    public string? Floor { get; set; }

    public bool? IsBookedShowTime1 { get; set; }

    public bool? IsBookedShowTime2 { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual SeatColor SeatColor { get; set; } = null!;
}
