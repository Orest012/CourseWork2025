using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class TopEventResult
    {
        [Key]
        public long Id { get; set; }  // Додано поле Id
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public string VenueName { get; set; }
        public long TicketsSold { get; set; }
    }
}
