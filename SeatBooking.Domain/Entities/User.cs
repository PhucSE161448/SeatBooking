using System;
using System.Collections.Generic;

namespace SeatBooking.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedDate { get; set; }
}
