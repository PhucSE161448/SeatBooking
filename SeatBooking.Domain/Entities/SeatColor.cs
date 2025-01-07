using System;
using System.Collections.Generic;

namespace SeatBooking.Domain.Entities;

public partial class SeatColor
{
    public int Id { get; set; }

    public string? Color { get; set; }

    public int Price { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
