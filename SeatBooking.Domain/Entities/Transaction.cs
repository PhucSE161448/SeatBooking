using System;
using System.Collections.Generic;

namespace SeatBooking.Domain.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public decimal TotalAmount { get; set; }

    public int PaymentType { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
