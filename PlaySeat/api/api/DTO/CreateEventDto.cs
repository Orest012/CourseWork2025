using System.Reflection;

namespace api.DTO
{
    public class CreateEventDto
    {
        public int SportEventsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string VenueName { get; set; }
        public string OrganizerName { get; set; }
        public string? SportType { get; set; }
        public string? ImageUrl { get; set; }

    }
}
