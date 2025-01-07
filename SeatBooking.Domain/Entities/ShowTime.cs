using System;
using System.Collections.Generic;

namespace SeatBooking.Domain.Entities;

public partial class ShowTime
{
    public int Id { get; set; }

    public string ShowName { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
