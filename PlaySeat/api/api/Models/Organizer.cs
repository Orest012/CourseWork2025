using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Organizer
{
    public long OrganizerId { get; set; }

    public long? UserId { get; set; }

    public string? OrganizationName { get; set; }

    public string? ContactInfo { get; set; }

    public bool? Verified { get; set; }

    public virtual ICollection<SportEvent> SportEvents { get; set; } = new List<SportEvent>();

    public virtual User? User { get; set; }
}
