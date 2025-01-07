using System;
using System.Collections.Generic;

namespace SeatBooking.Domain.Entities;

public partial class Seat
{
    public int Id { get; set; }

    public string RowChar { get; set; }

    public int ColNumber { get; set; }

    public int SeatColorId { get; set; }

    public int ShowTimeId { get; set; }

    public string Floor { get; set; }

    public bool? IsBooked { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual SeatColor SeatColor { get; set; }

    public virtual ShowTime ShowTime { get; set; }
}
