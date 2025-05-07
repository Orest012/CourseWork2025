namespace api.DTO
{
    public class EventView
    {
        public long SportEventsId { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? EventDate { get; set; }

        public string? SportType { get; set; }
        public long? VenueId { get; set; }
        public string? City { get; set; }
        public long? OrganizerId { get; set; }
        public string? Organizer { get; set; }
        public string? VenueName { get; set; }
        public string? ImageUrl { get; set; }

    }
}
