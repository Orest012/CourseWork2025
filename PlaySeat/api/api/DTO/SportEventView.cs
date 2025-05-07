namespace api.DTO
{
    public class SportEventView
    {
        public long SportEventsId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? EventDate { get; set; }

        public string? SportType { get; set; }

        // Venue info
        public string? VenueName { get; set; }
        public string? VenueAddress { get; set; }
        public int? VenueCapacity { get; set; }
        public string? VenueCity { get; set; }

        // Organizer info
        public string? OrganizerName { get; set; }
        public string? OrganizerContact { get; set; }
        public bool? OrganizerVerified { get; set; }

        // Optional: Ticket statistics
        public int TotalTickets { get; set; }
        public int TicketsSold { get; set; }
        public decimal? AveragePrice { get; set; }
        public string? ImageUrl { get; set; }

    }

}
