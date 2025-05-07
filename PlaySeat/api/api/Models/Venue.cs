using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Venue
{
    public long VenueId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public int? Capacity { get; set; }

    public string? City { get; set; }

    public string Imageurl { get; set; } = null!;

    public virtual ICollection<SportEvent> SportEvents { get; set; } = new List<SportEvent>();
}
