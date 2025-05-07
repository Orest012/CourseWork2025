using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Ticket
{
    public long TicketId { get; set; }

    public long? EventId { get; set; }

    public long? UserId { get; set; }

    public string? SeatNumber { get; set; }

    public string? Section { get; set; }

    public decimal? Price { get; set; }

    public bool? IsSold { get; set; }

    public DateTime? PurchasedAt { get; set; }

    public virtual SportEvent? Event { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
