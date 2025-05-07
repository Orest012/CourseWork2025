using System;
using System.Collections.Generic;

namespace api.Models;

public partial class SportEvent
{
    public long SportEventsId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? EventDate { get; set; }

    public long? VenueId { get; set; }

    public long? OrganizerId { get; set; }

    public string? SportType { get; set; }

    public virtual Organizer? Organizer { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Venue? Venue { get; set; }
}
