using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Payment
{
    public long PaymentId { get; set; }

    public long? UserId { get; set; }

    public long? TicketId { get; set; }

    public DateTimeOffset PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public virtual Ticket? Ticket { get; set; }

    public virtual User? User { get; set; }
}
